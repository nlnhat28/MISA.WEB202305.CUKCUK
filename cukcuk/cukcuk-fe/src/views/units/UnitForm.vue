<template>
    <m-popup
        :title="title"
        class="popup--sm"
        @emitClose="onClickCloseForm()"
        @keydown="handleShortKey"
        tabindex="0"
    >
        <template #mask>
            <m-loading v-if="isLoading"></m-loading>
        </template>
        <template #body>
            <m-form-body>
                <!-- Row 1: Tên -->
                <m-form-row>
                    <m-form-item
                        :label="fields.unitName.label"
                        :isRequired="true"
                    >
                        <m-textfield
                            v-model="unit.UnitName"
                            :validate="validateUnitName"
                            :label="fields.unitName.label"
                            :maxLength="fields.unitName.maxLength"
                            ref="UnitName"
                        >
                        </m-textfield>
                    </m-form-item>
                </m-form-row>
                <!-- Row 2: Ghi chú -->
                <m-form-row>
                    <m-form-item :label="fields.description.label">
                        <m-textfield
                            v-model="unit.Description"
                            :label="fields.note.label"
                            :maxLength="fields.note.maxLength"
                            :isTextarea="true"
                            ref="Description"
                        >
                        </m-textfield>
                    </m-form-item>
                </m-form-row>
            </m-form-body>
        </template>
        <!-- footer's form -->
        <template #footer>
            <div class="button-container">
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].save"
                    :click="onClickSave"
                    iconLeft="cukcuk-save"
                    tooltipContent="Ctrl + S"
                ></m-button>
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].cancel"
                    :click="onClickCloseForm"
                    iconLeft="cukcuk-cancel"
                    tooltipContent="Esc"
                ></m-button>
            </div>
            <div class="button-container">
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].help"
                    :click="onClickHelp"
                    iconLeft="cukcuk-help"
                    tooltipContent="F1"
                    @keydown="handleShortKeyLastButton"
                ></m-button>
            </div>
        </template>
    </m-popup>
    <!-- dialogs -->
    <m-dialog
        :type="this.noticeDialog.type"
        :title="this.noticeDialog.title"
        :content="this.noticeDialog.content"
        @emitCloseDialog="closeNoticeDialog()"
        v-if="this.noticeDialog.isShowed"
    >
        <template #footer>
            <m-button
                :type="this.$enums.buttonType.primary"
                :text="this.$resources['vn'].gotIt"
                :click="closeNoticeDialog"
                ref="buttonFocus"
            ></m-button>
        </template>
    </m-dialog>
    <m-dialog
        :type="this.confirmDialog.type"
        :title="this.confirmDialog.title"
        :content="this.confirmDialog.content"
        @emitCloseDialog="closeConfirmDialog()"
        v-if="this.confirmDialog.isShowed"
    >
        <template #footer>
            <div class="button-container">
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].save"
                    :click="onClickSaveConfirm"
                    ref="buttonFocus"
                ></m-button>
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].no"
                    :click="hideForm"
                ></m-button>
            </div>
            <m-button
                :type="this.$enums.buttonType.primary"
                :text="this.$resources['vn'].cancel"
                :click="closeConfirmDialog"
            ></m-button>
        </template>
    </m-dialog>
</template>

<script>
import enums from "@/constants/enums.js";
const formMode = enums.formMode;
import { fields } from "@/js/form/form.js";
import { validateUnitName } from "@/js/form/validate.js";
import { copyObject, sameObject } from "@/js/utils/json.js";
import { openUrl } from "@/js/utils/window.js";
import { unitService } from '@/services/services.js';
import { useUnitStore } from "@/stores/stores.js";
import { mapStores } from 'pinia';

export default {
    name: "UnitForm",
    props: {
        /**
         * Function to hide this form
         */
        hideForm: {
            type: Function,
        },
        /**
         * (Props) Form mode {create | update}
         */
        formMode: {
            type: Number,
            default: formMode.create,
            validator: (value) => {
                return [
                    formMode.create,
                    formMode.update,
                    formMode.duplicate
                ].includes(
                    value
                );
            },
        },
    },
    data() {
        return {
            /**
             * Mode of form
             */
            mode: this.formMode,
            /**
             * Fields info
             */
            title: this.formTitle,
            /**
             * Unit show on form
             */
            unit: {},
            /**
             * Fields info
             */
            fields: fields,
            /**
             * Dialog notice info
             */
            noticeDialog: {
                type: this.$enums.dialogType.error,
                title: this.$resources["vn"].error,
                content: "",
                isShowed: false,
            },
            /**
             * Dialog confirm info
             */
            confirmDialog: {
                type: this.$enums.dialogType.question,
                title: this.$resources["vn"].saveChange,
                content: "",
                isShowed: false,
            },
            /**
             * Flag check success response
             */
            isSuccessResponseFlag: true,
            /**
             * Message to show on dialog if invalid form
             */
            messageValidate: null,
            /**
             * Focused input
             */
            refFocus: null,
            /**
             * Form item refs
             */
            refs: [],
            /**
             * Flag loading
             */
            isLoading: false,
            /**
             * Original object
             */
            originalUnit: {},
        };
    },
    created() {
        this.title = this.formTitle;
        this.$emitter.on("focusFormItem", (ref) => {
            this.focusErrorItem(ref);
        });
        this.$emitter.on("setMessageFormItem", (ref, errorMessage) => {
            this.setErrorMessage(ref, errorMessage);
        });
    },
    mounted() {
        this.refs = [
            this.$refs.Description,
            this.$refs.UnitName,
        ];
        this.focusFirstInput();
    },
    emits: [
        "emitUpdateUnit",
    ],
    watch: {
        /**
         * Theo dõi chế độ form để đổi title
         */
        mode() {
            this.title = this.formTitle;
        },
    },
    computed: {
        /**
         * Check form is valid or invalid
         *
         * Author: nlnhat (02/07/2023)
         * @return {*} True if valid, false if invalid
         */
        isValidForm() {
            try {
                let isValid = true;
                this.messageValidate = null;
                this.refs.forEach((ref) => {
                    const message = ref.checkValidate();
                    if (message) {
                        this.messageValidate = message;
                        isValid = false;
                        this.refFocus = ref;
                    }
                });
                return isValid;
            } catch (error) {
                console.error(error);
                return false;
            }
        },
        /**
         * Change title when change mode
         *
         * Author: nlnhat (02/07/2023)
         * @return {*} New title update or create
         */
        formTitle() {
            switch (this.mode) {
                case this.$enums.formMode.create:
                    return this.$resources["vn"].createUnit;
                case this.$enums.formMode.update:
                    return this.$resources["vn"].updateUnit;
                case this.$enums.formMode.duplicate:
                    return this.$resources["vn"].duplicateUnit;
                default:
                    return this.$resources["vn"].createUnit;
            }
        },
        /**
         * Message of unit on toast
         *
         * Author: nlnhat (02/07/2023)
         * @return {*} New title update or create
         */
        messageOnToast() {
            let title = null;
            switch (this.mode) {
                case this.$enums.formMode.create:
                    title = this.$resources["vn"].created;
                    break;
                case this.$enums.formMode.update:
                    title = this.$resources["vn"].updated;
                    break;
                case this.$enums.formMode.duplicate:
                    title = this.$resources["vn"].duplicated;
                    break;
                default:
                    break;
            }
            const content = `${title} ${this.$resources["vn"].unit} <${this.unit.UnitName}>`;
            return content;
        },
        /**
         * Map unit store
         */
        ...mapStores(useUnitStore)
    },
    methods: {
        /**
         * Make loading effect
         *
         * Author: nlnhat (05/07/2023)
         * @param {function} func Function executes in loading process
         */
        async makeLoadingEffect(func) {
            try {
                this.isLoading = true;
                await func();
            } catch (error) {
                console.error(error);
            } finally {
                this.isLoading = false;
            }
        },
        /**
         * Show notice dialog
         *
         * Author: nlnhat (04/07/2023)
         * @param {string} message Content to show on dialog
         */
        showNoticeDialog(message) {
            this.noticeDialog.content = message;
            this.noticeDialog.isShowed = true;
            this.focusOnButton();
        },
        /**
         * Close notice dialog
         *
         * Author: nlnhat (10/07/2023)
         */
        closeNoticeDialog() {
            try {
                this.noticeDialog.isShowed = false;
                if (this.refFocus) this.refFocus.focus();
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Close notice dialog
         *
         * Author: nlnhat (10/07/2023)
         */
        closeConfirmDialog() {
            this.confirmDialog.isShowed = false;
            this.focusFirstInput();
        },
        /**
         * Focus on button
         *
         * Author: nlnhat (25/07/2023)
         */
        focusOnButton() {
            this.$nextTick(() => {
                this.$refs.buttonFocus.focus();
            });
        },
        /**
         * Create new unit
         *
         * Author: nlnhat (02/07/2023)
         */
        async createUnit() {
            try {
                const response = await unitService.post(this.unit);
                if (response?.status == this.$enums.status.created) {
                    this.unit.UnitId = response.data;
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                }
            } catch (error) {
                console.error(error);
                this.isSuccessResponseFlag = false;
            }
        },
        /**
         * Update unit
         *
         * Author: nlnhat (02/07/2023)
         */
        async updateUnit() {
            try {
                const response = await unitService.put(
                    this.unit.UnitId,
                    this.reformatUnit
                );
                if (response?.status == this.$enums.status.ok) {
                    this.isSuccessResponseFlag = true;
                } else {
                    this.isSuccessResponseFlag = false;
                }
            } catch (error) {
                console.error(error);
                this.isSuccessResponseFlag = false;
            }
        },
        /**
         * Save when create or update
         *
         * Author: nlnhat (02/07/2023)
         */
        async onSave() {
            await this.makeLoadingEffect(async () => {
                switch (this.mode) {
                    case this.$enums.formMode.create:
                        await this.createUnit();
                        break;
                    case this.$enums.formMode.update:
                        await this.updateUnit();
                        break;
                    case this.$enums.formMode.duplicate:
                        await this.createUnit();
                        break;
                    default:
                        break;
                };
                if (this.isSuccessResponseFlag == true) {
                    await this.unitStore.getUnits();
                }
            });
        },
        /**
         * On click button save (Cất)
         *
         * Author: nlnhat (02/07/2023)
         */
        async onClickSave() {
            try {
                if (this.isValidForm) {
                    await this.onSave();
                    if (this.isSuccessResponseFlag == true) {
                        this.$emit("emitUpdateUnit", this.unit.UnitId);
                        this.showToastSuccess();
                        this.hideForm();
                    }
                } else {
                    this.showNoticeDialog(this.messageValidate ?? this.$resources["vn"].userMsg500);
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * On click button save and add (Cất và thêm)
         *
         * Author: nlnhat (02/07/2023)
         */
        async onClickSaveAdd() {
            try {
                if (this.isValidForm) {
                    await this.onSave();
                    if (this.isSuccessResponseFlag == true) {
                        this.resetForm();
                    }
                } else {
                    this.showNoticeDialog(this.messageValidate ?? this.$resources["vn"].userMsg500);
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Reset form
         * 
         * Author: nlnhat (28/08/2023)
         */
        resetForm() {
            this.$emit("emitReRenderForm");
        },
        /**
         * Show toast message after created, updated, duplicated success
         *
         * Author: nlnhat (02/07/2023)
         */
        showToastSuccess() {
            this.$emitter.emit("showToastSuccess", this.messageOnToast);
        },
        /**
         * Show confirm dialog
         *
         * Author: nlnhat (06/07/2023)
         * @param {string} message Content to show on dialog
         */
        showSaveConfirmDialog(message) {
            this.confirmDialog.content = message;
            this.confirmDialog.isShowed = true;
            this.focusOnButton();
        },
        /**
         * Handle when click save on confirm dialog
         *
         * Author: nlnhat (06/07/2023)
         */
        async onClickSaveConfirm() {
            this.confirmDialog.isShowed = false;
            await this.onClickSave();
        },
        /**
         * Handle click close this form
         *
         * Author: nlnhat (26/06/2023)
         */
        onClickCloseForm() {
            if (!this.sameObject(this.originalUnit, this.unit))
                this.showSaveConfirmDialog(this.$resources["vn"].saveChangeConfirm);
            else
                this.hideForm();
        },
        /**
         * Handle shortcut keys
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            const code = this.$enums.keyCode;

            // Handle shortcut keys on confirm dialog
            if (this.confirmDialog.isShowed) {
                this.handleShortKeyConfirmDialog(event);
            }
            // Handle shortcut keys on notice dialog
            else if (this.noticeDialog.isShowed) {
                this.handleShortKeyNoticeDialog(event);
            }

            // Ctrl + S || Ctrl + F8: Cất
            else if (
                ((event.ctrlKey && event.keyCode == code.s) ||
                    (event.ctrlKey && event.keyCode == code.f8)) &&
                !event.shiftKey
            ) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickSave();
            }
            // Ctrl + Shift + N: Cất và thêm
            else if (event.ctrlKey && event.shiftKey && event.keyCode == code.s) {
                // event.preventDefault();
                // this.onClickSaveAdd();
            }
            // Esc: Đóng form
            else if (event.keyCode == code.esc) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickCloseForm();
            }
            // F1: Help
            else if (event.keyCode == code.f1) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickHelp();
            }
        },
        /**
         * Handle shortcut keys on confirm dialog
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event on confirm dialog
         */
        handleShortKeyConfirmDialog(event) {
            const code = this.$enums.keyCode;
            switch (event.keyCode) {
                // Enter: Cất
                case code.enter:
                    event.stopPropagation();
                    event.preventDefault();
                    this.onClickSaveConfirm();
                    break;
                // N: Không
                case code.n:
                    event.stopPropagation();
                    event.preventDefault();
                    this.hideForm();
                    break;
                // Esc: Đóng
                case code.esc:
                    event.stopPropagation();
                    event.preventDefault();
                    this.closeConfirmDialog();
                    break;
            }
        },
        /**
         * Handle shortcut keys on notice dialog
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event on notice dialog
         */
        handleShortKeyNoticeDialog(event) {
            const code = this.$enums.keyCode;
            // Enter || Esc: Đã hiểu
            if (event.keyCode == code.enter || event.keyCode == code.enter) {
                event.stopPropagation();
                event.preventDefault();
                this.closeNoticeDialog();
            }
        },
        /**
         * Handle shortcut keys on last button
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event on last button
         */
        handleShortKeyLastButton(event) {
            const code = this.$enums.keyCode;
            // Tab: Focus on first input
            if (event.keyCode == code.tab) {
                event.stopPropagation();
                event.preventDefault();
                this.focusFirstInput();
            }
        },
        /**
         * Focus on first input
         *
         * Author: nlnhat (25/07/2023)
         */
        focusFirstInput() {
            this.$refs.UnitName.focus();
        },
        /**
         * Focus on error item
         *
         * Author: nlnhat (04/08/2023)
         * @param {*} ref Ref name of error item
         */
        focusErrorItem(ref) {
            if (this.$refs[ref]) {
                this.$refs[ref].focus();
            }
        },
        /**
         * Set message for error item
         *
         * Author: nlnhat (04/08/2023)
         * @param {*} ref Ref name of error item
         * @param {*} errorMessage Error message
         */
        setErrorMessage(ref, errorMessage) {
            if (this.$refs[ref]) {
                this.$refs[ref].errorMessage = errorMessage;
            }
        },
        /**
         * On click help
         * 
         * Author: nlnhat (26/06/2023)
         */
        onClickHelp() {
            this.openUrl('https://helpv2.cukcuk.com/vi/kb/enter-unit/');
        },
        /**
         * Validate methods
         */
        validateUnitName,
        /**
         * Utils
         */
        copyObject,
        sameObject,
        openUrl,
    },
};
</script>
<style></style>
