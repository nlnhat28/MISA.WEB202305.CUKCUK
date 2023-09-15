<template>
    <m-popup
        :title="title"
        id="material-import"
        class="popup--md"
        @emitClose="onClickCloseForm()"
        @keydown="handleShortKey"
        tabindex="0"
    >
        <template #mask>
            <m-loading v-if="isLoading"></m-loading>
        </template>
        <template #body>
            <div class="import-header">
                {{ stepTitle }}
            </div>
            <div class="import-body">
                <div class="import-sidebar">
                    <div
                        v-for="(step, index) in steps"
                        :key="index"
                        :class="{ 'import-sidebar__item': true, 'import-sidebar__item--active': step.number == this.step }"
                        tabindex="0"
                    >
                        {{ step.number }}. {{ step.title }}
                    </div>
                </div>
                <div class="import-body__content">
                    <!-- Step 1 -->
                    <div
                        v-if="step == 1"
                        class="import-body__page-step"
                    >
                        {{ this.$resources['vn'].chooseImportData }}
                        <div style="display: flex; column-gap: 8px;">
                            <m-textfield
                                :isReadOnly="true"
                                v-model="selectedFileName"
                                id="textfield--file-name"
                                ref="SelectedFileName"
                            >
                            </m-textfield>
                            <m-button
                                :text="this.$resources['vn'].chooseFile"
                                :type="this.$enums.buttonType.primary"
                                :click="onClickChooseFile"
                                ref="chooseFile"
                            >
                            </m-button>
                        </div>
                        <div style="display: flex; column-gap: 4px;">
                            {{ this.$resources['vn'].downloadTemplateFileTitle }}
                            <div
                                class="link-url"
                                tabindex="0"
                                @click="getImportTemplate()"
                                ref="DownloadLink"
                            >{{ this.$resources['vn'].here }}</div>
                        </div>
                    </div>
                    <!-- Step 2 -->
                    <div
                        v-if="step == 2"
                        class="import-body__page-step"
                    >
                        <div class="import-body__result-text">
                            <div
                                style="display: flex; column-gap: 16px;"
                                v-if="materialsLength > 0"
                            >
                                <!-- Số lượng hợp lệ -->
                                <div
                                    :class="{
                                        'import-body__button color--success': true,
                                        'line-through': this.showState == this.$enums.showState.showInvalid
                                    }"
                                    v-if="validMaterialsLength > 0"
                                    @click="onClickValidMaterials()"
                                >
                                    {{ this.$resources['vn'].isValid }}: {{ validMaterialsLength }}/{{ materialsLength }}
                                </div>
                                <!-- Số lượng không hợp lệ -->
                                <div
                                    :class="{
                                        'import-body__button color--error': true,
                                        'line-through': this.showState == this.$enums.showState.showValid
                                    }"
                                    v-if="invalidMaterialsLength > 0"
                                    @click="onClickInvalidMaterials()"
                                >
                                    {{ this.$resources['vn'].notValid }}: {{ invalidMaterialsLength }}/{{ materialsLength
                                    }}
                                </div>
                            </div>
                            <div v-if="materialsLength == 0">
                                {{ this.$resources['vn'].noContent }}
                            </div>
                        </div>
                        <m-table
                            class="import-body__table"
                            :totalRecord="materialsLength"
                        >
                            <template #thead>
                                <!-- table head -->
                                <m-th
                                    v-for="(head, index) in table.heads"
                                    :key="index"
                                    :widthCell="head.widthCell"
                                    :fullTitle="head.fullTitle"
                                >
                                    {{ head.title }}
                                </m-th>
                            </template>
                            <template #tbody>
                                <m-tr
                                    v-for="(material, index) in materialsDisplay"
                                    :key="material.MaterialId"
                                    :index="index"
                                    :id="material.MaterialId"
                                    ref="tr"
                                    @emitFocusNext="focusNextRow"
                                    @emitFocusPrevious="focusPreviousRow"
                                >
                                    <template #content>
                                        <!-- Hợp lệ-->
                                        <m-td textAlign="center">
                                            <m-icon-valid :isValid="material.IsValid">
                                            </m-icon-valid>
                                        </m-td>
                                        <!-- Lý do không hợp lệ -->
                                        <m-td
                                            textAlign="left"
                                            :content="material.ValidateDescription"
                                        >
                                        </m-td>
                                        <!-- Mã nguyên vật liệu -->
                                        <m-td
                                            textAlign="left"
                                            :content="material.MaterialCode"
                                        >
                                        </m-td>
                                        <!-- Tên nguyên vật liệu -->
                                        <m-td
                                            textAlign="left"
                                            :content="material.MaterialName"
                                        ></m-td>
                                        <!-- Số lượng tồn tối thiểu -->
                                        <m-td
                                            textAlign="right"
                                            :content="formatDecimal(material.MinimumInventory)"
                                        ></m-td>
                                        <!-- Đơn vị tính -->
                                        <m-td
                                            textAlign="left"
                                            :content="material.UnitName"
                                        ></m-td>
                                        <!-- Kho ngầm định -->
                                        <m-td
                                            textAlign="left"
                                            :content="material.WarehouseCode"
                                        ></m-td>
                                        <!-- Ghi chú -->
                                        <m-td
                                            textAlign="left"
                                            :content="material.Note"
                                        ></m-td>
                                    </template>
                                </m-tr>
                            </template>
                            <template #tfooter>
                                {{ `${this.$resources['vn'].amount}: ${materialsLength}` }}
                            </template>
                        </m-table>
                        <m-checkbox-item
                            v-if="isShowCheckboxImportValid"
                            v-model:checked="canImportValidRecords"
                            :name="this.$resources['vn'].allowImportValidRecords(this.validMaterialsLength)"
                            ref="checkboxImportValid"
                        >
                        </m-checkbox-item>
                    </div>
                    <!-- Step 3 -->
                    <div
                        v-if="step == 3"
                        class="import-body__page-step"
                    >
                        <div class="import-body__result-text">
                            <div style="display: flex; column-gap: 16px;">
                                <div
                                    class="color--success"
                                    v-if="successImportCount > 0"
                                >
                                    {{ this.$resources['vn'].success }}: {{ successImportCount }}/{{ validMaterialsLength }}
                                </div>
                                <div
                                    class="color--error"
                                    v-if="failImportCount > 0"
                                >
                                    {{ this.$resources['vn'].fail }}: {{ failImportCount }}/{{ validMaterialsLength
                                    }}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </template>
        <!-- footer's form -->
        <template #footer>
            <div class="button-container">
                <m-button
                    v-if="isShowBack"
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].back"
                    :click="onClickBack"
                    :isDisabled="isDisabledBack"
                    iconLeft="cukcuk-arrow-left"
                    ref="backButton"
                ></m-button>
                <m-button
                    v-if="isShowCountinue"
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].continue"
                    :click="onClickContinue"
                    :hasLoading="true"
                    :isDisabled="isDisabledCountinue"
                    iconLeft="cukcuk-arrow-right"
                    ref="continueButton"
                ></m-button>
                <m-button
                    v-if="isShowFinish"
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].finish"
                    :click="onClickFinish"
                    iconLeft="check--blue"
                    ref="finishButton"
                ></m-button>
                <m-button
                    v-else
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].cancel"
                    :click="onClickCloseForm"
                    iconLeft="cukcuk-cancel"
                    title="Esc"
                ></m-button>
            </div>
        </template>
    </m-popup>
</template>

<script>
import { openUrl } from "@/js/utils/window.js";
import { percentageFormatter, gradientBackground } from "@/js/utils/chart.js";
import { materialService } from '@/services/services.js';
import { download, chooseFile } from '@/js/utils/file.js'
import { generateColorsByNumbers } from '@/js/utils/color.js';
import { formatDecimal } from "@/js/utils/format.js";
export default {
    name: "MaterialImport",
    components: {
    },
    props: {
        /**
         * Function to hide this form
         */
        closeThis: {
            type: Function,
        },
        /**
         * Title of window
         */
        title: {
            type: String,
        },
        /**
         * (Props) Id of material to focus
         */
        focusedId: {
            type: String,
            default: null,
        },

    },
    data() {
        return {
            /**
             * Loading effect
             */
            isLoading: false,
            /**
             * Danh sách các bước
             */
            steps: [
                {
                    number: 1,
                    title: this.$resources['vn'].chooseSourceFile
                },
                {
                    number: 2,
                    title: this.$resources['vn'].checkData
                },
                {
                    number: 3,
                    title: this.$resources['vn'].importResult
                },
            ],
            /**
             * Bước hiện tại
             */
            step: 1,
            /**
             * Số bản ghi nhập khẩu thành công
             */
            successImportCount: 0,
            /**
             * Số bản ghi nhập khẩu thất bại
             */
            failImportCount: 0,
            /**
             * Các ids được import thành công
             */
            successIds: 0,
            /**
             * File được tải lên
             */
            selectedFile: null,
            /**
             * Tên file được tải lên
             */
            selectedFileName: null,
            /**
             * Danh sách nguyên vật liệu tải lên từ file
             */
            materials: [],
            /**
             * 
             */
            table: {
                heads: [
                    {
                        title: this.$resources['vn'].isValid,
                        name: "IsValid",
                        widthCell: 68,
                    },
                    {
                        title: this.$resources['vn'].validateDescription,
                        name: "ValidateDescription",
                        widthCell: 180,
                    },
                    {
                        title: this.$resources['vn'].materialCode,
                        name: "MaterialCode",
                        widthCell: 140,
                    },
                    {
                        title: this.$resources['vn'].materialName,
                        widthCell: 180,
                        name: "MaterialName",
                    },
                    {
                        title: this.$resources['vn'].minimumInventory,
                        fullTitle: this.$resources['vn'].minimumInventoryFullTitle,
                        widthCell: 140,
                        name: "MinimumInventory",
                    },
                    {
                        title: this.$resources['vn'].unit,
                        widthCell: 140,
                        name: "UnitName",
                    },
                    {
                        title: this.$resources['vn'].warehouseCode,
                        widthCell: 140,
                        name: "WarehouseCode",
                    },
                    {
                        title: this.$resources['vn'].note,
                        widthCell: 200,
                        name: "Note",
                    },
                ]
            },
            /**
             * Có thể nhập những bản ghi hợp lệ nếu có cả không hợp lệ
             */
            canImportValidRecords: false,
            /**
             * Trạng thái show bản ghi
             */
            showState: this.$enums.showState.showAll,
        }
    },
    async created() {
        this.$emitter.on("focusFormItem", (ref) => {
            this.focusErrorItem(ref);
        });
        this.$emitter.on("setMessageFormItem", (ref, errorMessage) => {
            this.setErrorMessage(ref, errorMessage);
        });
    },
    mounted() {
        this.focus('chooseFile');
    },
    unmounted() {
        this.$emitter.off("focusFormItem")
        this.$emitter.off("setMessageFormItem")
    },
    emits: [
        'emitReloadData',
        'emitUpdateFocusedId',
        'emitUpdateFocusedIds',
    ],
    watch: {
        /**
         * Theo dõi file nhập khẩu
         */
        selectedFile() {
            this.selectedFileName = this.selectedFile?.name;
        },
        /**
         * Theo dõi checkbox
         */
        canImportValidRecords() {
            if (this.canImportValidRecords)
                this.focus('continueButton');
        },
        /**
         * Theo dõi step
         */
        step() {
            switch (this.step) {
                case 2:
                    if (this.isShowCheckboxImportValid) {
                        this.$nextTick(() => {
                            this.focus('checkboxImportValid');
                        });
                    }
                    else {
                        this.focus('continueButton');
                    }
                    break;
                case 3:
                    if (this.isShowFinish) {
                        this.$nextTick(() => {
                            this.focus('finishButton');
                        });
                    }
                    else {
                        this.focus('backButton');
                    }
                    break;
                default:
                    break;
            }
        }
    },
    computed: {
        /**
         * Step title
         * 
         * Author: nlnhat (08/09/2023)
         */
        stepTitle() {
            try {
                const currentStep = this.steps.find(step => step.number == this.step);
                if (currentStep)
                    return `${this.$resources['vn'].step} ${currentStep.number}: ${currentStep.title}`
            } catch (error) {
                console.error(error);
            }
            return null;
        },
        /**
         * Disable button tiếp tục
         * 
         * Author: nlnhat (08/09/2023)
         */
        isDisabledCountinue() {
            switch (this.step) {
                // Bước 1
                case 1:
                    return this.selectedFile == null;
                // Bước 2
                case 2:
                    if (this.validMaterialsLength <= 0)
                        return true;
                    if (this.validMaterialsLength == this.materialsLength)
                        return false;
                    return !this.canImportValidRecords;
                case 3:
                    return true;
                default:
                    return true;
            }
        },
        /**
         * Disable button quay lại
         * 
         * Author: nlnhat (08/09/2023)
         */
        isDisabledBack() {
            switch (this.step) {
                // Bước 1
                case 1:
                    return true;
                // Bước 2
                case 2:
                    return false;
                case 3:
                    return false;
                default:
                    return true;
            }
        },
        /**
         * Show button tiếp tục
         * 
         * Author: nlnhat (08/09/2023)
         */
        isShowCountinue() {
            return this.step < this.steps.length;
        },
        /**
         * Show button quay lại
         * 
         * Author: nlnhat (08/09/2023)
         */
        isShowBack() {
            return !this.isShowFinish && this.step > 1;
        },
        /**
         * Show button kết thúc
         * 
         * Author: nlnhat (08/09/2023)
         */
        isShowFinish() {
            // Nếu số bản ghi nhập khẩu thành công > 0 và bằng tổng số bản ghi hợp lệ
            return this.successImportCount > 0 && this.successImportCount == this.validMaterialsLength;
        },
        /**
         * Show checkbox cho phép nhập những bản ghi hợp lệ nếu có bản ghi không hợp lệ
         */
        isShowCheckboxImportValid() {
            return this.validMaterialsLength > 0 && this.invalidMaterialsLength > 0;
        },
        /**
         * Số lượng material
         * 
         * Author: nlnhat (12/09/2023)
         */
        materialsLength() {
            return this.materials ? this.materials.length : 0;
        },
        /**
         * Dữ liệu hợp lệ
         * 
         * Author: nlnhat (12/09/2023)
         */
        validMaterials() {
            let data = []
            data = this.materials.filter(material => material.IsValid);
            return data;
        },
        /**
         * Số dữ liệu hợp lệ
         * 
         * Author: nlnhat (12/09/2023)
         */
        validMaterialsLength() {
            return this.validMaterials ? this.validMaterials.length : 0;
        },
        /**
         * Dữ liệu không hợp lệ
         * 
         * Author: nlnhat (12/09/2023)
         */
        invalidMaterials() {
            let data = []
            data = this.materials.filter(material => !material.IsValid);
            return data;
        },
        /**
         * Số dữ liệu không hợp lệ
         * 
         * Author: nlnhat (12/09/2023)
         */
        invalidMaterialsLength() {
            return this.invalidMaterials ? this.invalidMaterials.length : 0;
        },
        /**
         * Danh sách nguyên vật liệu hiển thị lên bảng kiểm tra
         */
        materialsDisplay() {
            switch (this.showState) {
                case this.$enums.showState.showAll:
                    return this.materials;
                case this.$enums.showState.showValid:
                    return this.validMaterials;
                case this.$enums.showState.showInvalid:
                    return this.invalidMaterials;
                default:
                    return [];
            }
        },
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
         * Lấy toàn bộ dữ liệu thống kê
         * 
         * Author: nlnhat (09/09/2023)
         */
        async getData() {
            await Promise.all([
                this.getCountByYear(),
                this.getCountByWarehouse(),
                this.getCountByFollow(),
            ]);
            this.dataFilterCount = this.filterCount;
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
         * Handle click close this form
         *
         * Author: nlnhat (26/06/2023)
         */
        onClickCloseForm() {
            this.closeThis();
        },
        /**
         * Handle click finish
         *
         * Author: nlnhat (26/06/2023)
         */
        onClickFinish() {
            this.reloadData();
            this.closeThis();
        },
        /**
         * Handle shortcut keys
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            const code = this.$enums.keyCode;

            // Esc: Đóng form
            if (event.keyCode == code.esc) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickCloseForm();
            }
        },
        /**
         * Focus on ref
         * 
         * Author: nlnhat (08/09/2023)
         * @param {*} ref Ref name
         */
        focus(ref) {
            if (this.$refs[ref]) {
                this.$refs[ref].focus();
            }
        },
        /**
         * Focus on error item
         *
         * Author: nlnhat (04/08/2023)
         * @param {*} ref Ref name of error item
         */
        focusErrorItem(ref) {
            this.focus(ref);
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
         * Chọn tệp để nhập khẩu
         * 
         * Author: nlnhat (11/09/2023)
         */
        onClickChooseFile() {
            try {
                const input = document.createElement("input");
                input.type = "file";
                input.style.display = "none";
                input.addEventListener("change", (event) => {
                    const selectedFile = event.target.files[0];
                    if (selectedFile) {
                        this.selectedFile = selectedFile;
                        this.focus('continueButton');
                    } else {
                        this.selectedFile = null;
                    }
                });
                input.click();
            } catch (error) {
                console.error(error);
                this.selectedFile = null;
            }
        },
        /**
         * Get excel import template
         * 
         * Author: nlnhat (22/08/2023)
         */
        async getImportTemplate() {
            try {
                const response = await materialService.getImportTemplate();
                if (response?.status == this.$enums.status.ok) {
                    const blob = new Blob(
                        [response.data],
                        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
                    );
                    const fileName = "materials-import-template.xlsx";
                    await this.download(blob, fileName);
                    return true;
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Xử lý sự kiến nhấn tiếp tục
         * 
         * Author: nlnhat (11/09/2023)
         */
        async onClickContinue() {
            switch (this.step) {
                // Bước 1
                case 1:
                    await this.handleSelectedFile();
                    break;
                // Bước 2
                case 2:
                    await this.importData();
                    break;
                default:
                    break;
            }
        },
        /**
         * Xử lý file excel được chọn
         * 
         * Author: nlnhat (12/09/2023)
         */
        async handleSelectedFile() {
            const formData = new FormData();
            formData.append('file', this.selectedFile);
            const response = await materialService.mapImport(formData);
            if (response?.status == this.$enums.status.ok) {
                this.materials = response.data;
                this.showState = this.$enums.showState.showAll;
                this.step = 2;
            }
        },
        /**
         * Nhập các dữ liệu hợp lệ
         * 
         * Author: nlnhat (12/09/2023)
         */
        async importData() {
            const response = await materialService.import(this.validMaterials);
            if (response?.status == this.$enums.status.ok) {
                this.successIds = response.data;
                this.successImportCount = this.successIds ? this.successIds.length : 0;
                this.failImportCount = this.validMaterialsLength - this.successImportCount;
                this.step = 3;
            }
        },
        /**
         * Reload data sau khi nhập khẩu
         * 
         * Author: nlnhat (12/09/2023)
         */
        reloadData() {
            if (this.successIds.length > 0) {
                this.$emit("emitReloadData");
                this.$emit("emitUpdateFocusedId", this.successIds[0]);
                this.$emit("emitUpdateFocusedIds", this.successIds);
            }
        },
        /**
         * Xử lý sự kiến nhấn quay lại
         * 
         * Author: nlnhat (11/09/2023)
         */
        async onClickBack() {
            switch (this.step) {
                // Bước 2
                case 2:
                    this.step = 1;
                    break;
                // Bước 3
                case 3:
                    this.step = 2;
                    break;
                default:
                    break;
            }
        },
        /**
         * Click on valid materials button
         * 
         * Author: nlnhat (25/08/2023)
         */
        onClickValidMaterials() {
            if (this.showState == this.$enums.showState.showAll) {
                this.showState = this.$enums.showState.showValid
            }
            else {
                this.showState = this.$enums.showState.showAll
            }
        },
        /**
         * Click on invalid materials button
         * 
         * Author: nlnhat (25/08/2023)
         */
        onClickInvalidMaterials() {
            if (this.showState == this.$enums.showState.showAll) {
                this.showState = this.$enums.showState.showInvalid
            }
            else {
                this.showState = this.$enums.showState.showAll
            }
        },
        /**
         * Focus on next row in table
         * 
         * Author: nlnhat (25/08/2023)
         * @param {number} index Index of row 
         */
        focusNextRow(index) {
            let newIndex = index + 1;
            if (newIndex >= this.materialsLength)
                newIndex = 0;
            this.focusRow(newIndex);
        },
        /**
         * Focus on previous row in table
         * 
         * Author: nlnhat (25/08/2023)
         * @param {number} index Index of row 
         */
        focusPreviousRow(index) {
            let newIndex = index - 1;
            if (newIndex < 0)
                newIndex = this.materialsLength - 1;
            this.focusRow(newIndex);
        },
        /**
         * Focus on a row by index
         * 
         * Author: nlnhat (08/08/2023)
         * @param {number} index Index of row 
         */
        focusRow(index) {
            const refFocus = this.$refs.tr.find(tr => tr.index == index);
            if (refFocus)
                refFocus.focus();
        },
        /**
         * Utils
         */
        openUrl,
        generateColorsByNumbers,
        percentageFormatter,
        gradientBackground,
        chooseFile,
        download,
        formatDecimal,
    },
};
</script>
<style>
#material-import .popup {
    width: 960px;
}

#material-import .popup__body {
    height: 540px;
    padding: 12px;
    font-size: 13px;
}

#material-import .popup__footer {
    padding: 8px 12px;
}

.import-header {
    font-weight: 700;
}

.import-body {
    display: flex;
    flex: 1;
    column-gap: 8px;
}

.import-sidebar {
    display: flex;
    flex-direction: column;
    row-gap: 4px;
    width: 200px;
    height: 100%;
    border-radius: 2px;
    background-color: var(--grey-200);
}

.import-sidebar__item {
    display: flex;
    align-items: center;
    border: 1px solid var(--border-color);
    border-radius: 2px;
    padding: 0 12px;
    width: 100%;
    height: 36px;
    font-weight: 700;
    /* cursor: pointer; */
}

.import-sidebar__item:hover {
    /* background-color: var(--orange-100); */
}

.import-sidebar__item:active {
    /* background-color: var(--orange-200); */
}

.import-sidebar__item:focus {
    /* border-color: var(--blue-100); */
}

.import-sidebar__item--active {
    background-color: var(--blue-100);
    pointer-events: none;
}

.import-body__content {
    display: flex;
    flex: 1;
    height: 100%;
    border: 1px solid var(--border-color);
    border-radius: 2px;
    padding: 8px;
}

.import-body__page-step {
    display: flex;
    flex-direction: column;
    row-gap: 8px;
    width: 100%;
}

.import-body__page-step .checkbox-item {
    width: fit-content;
}

.import-body__result-text {
    padding-bottom: 4px;
    font-weight: 700;
    caret-color: transparent;
}

.import-body__button {
    cursor: pointer;
}

.line-through {
    text-decoration: line-through;
}

#textfield--file-name,
#textfield--file-name .input-group,
#textfield--file-name input {
    height: 24px
}

.import-body__table {
    caret-color: transparent;
}

.import-body__table .table th:first-child .th__resize {
    display: none;
}

.import-body__table .table th:first-child,
.import-body__table .table td:first-child {
    position: sticky;
    left: 0;
    z-index: 1;
}
.import-body__table .table th:nth-child(2),
.import-body__table .table td:nth-child(2) {
    /* position: sticky;
    left: 68px;
    z-index: 1; */
}
.import-body__table .table td:first-child .icon-container {
    cursor: default;
}
</style>
