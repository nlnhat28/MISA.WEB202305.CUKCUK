<template>
    <tr
        :class="{ 'conversion-unit': true, 'tr--focused': focusedId == conversionUnit.ConversionUnitId }"
        tabindex="0"
        @click="updateFocusedId"
        ref="tr"
    >
        <td
            class="td--readonly"
            style="text-align: center"
        >
            {{ index + 1 }}
        </td>
        <td class="td--nopadding">
            <!-- Destination unit -->
            <div
                ref="DesUnitWrapper"
                style="height: 30px; width: 100%;"
                tabindex="-1"
                v-click-outside="resetDesUnitCombobox"
                @click="onClickDesUnit()"
            >
                <m-combobox
                    v-model:id="conversionUnit.DestinationUnitId"
                    v-model:name="conversionUnit.DestinationUnitName"
                    :validate="validateDestinationUnit"
                    :theSelects="unitsComputed"
                    :label="fields.conversionUnit.label"
                    :maxLength="fields.conversionUnit.maxLength"
                    :action="actionCreateUnit"
                    :style="`position: ${styleDesUnit.position}; width: ${styleDesUnit.width}px; top: ${styleDesUnit.top}px; left: ${styleDesUnit.left}px;`"
                    class="fixed"
                    ref="DestinationUnit"
                >
                </m-combobox>
            </div>
        </td>
        <td
            class="td--nopadding"
            style="text-align: right"
        >
            <!-- Rate -->
            <m-textfield
                v-model="conversionUnit.Rate"
                :format="formatRateInput"
                :canSetSelection="true"
                textAlign="right"
                ref="Rate"
            >
            </m-textfield>
        </td>
        <td class="td--nopadding">
            <div
                ref="OperatorWrapper"
                style="height: 30px; width: 100%"
                v-click-outside="resetOperatorCombobox"
                tabindex="-1"
                @click="onClickOperator()"
            >
                <!-- Operator -->
                <m-combobox
                    v-model:id="conversionUnit.Operator"
                    v-model:name="conversionUnit.OperatorName"
                    :theSelects="operatorSelects"
                    :label="fields.operator.label"
                    :maxLength="fields.operator.maxLength"
                    :isReadOnly="true"
                    :canUnselect="false"
                    :style="`position: ${styleOperator.position}; width: ${styleOperator.width}px; top: ${styleOperator.top}px; left: ${styleOperator.left}px;`"
                    class="fixed"
                    ref="Operator"
                >
                </m-combobox>
            </div>
        </td>
        <td class="td--readonly">
            <!-- Description -->
            {{ descriptionComputed }}
        </td>
    </tr>
    <Teleport to=".main-content">
        <UnitForm
            v-if="isShowedUnitForm"
            :hideForm="hideUnitForm"
            :formMode="unitFormMode"
            @emitUpdateUnit="updateUnit"
        >
        </UnitForm>
    </Teleport>
</template>
<script>
import { fields } from "@/js/form/form.js";
import { validateUnit } from "@/js/form/validate.js";
import { isNullOrEmpty } from "@/js/utils/string.js";
import { formatDecimalInput, formatDecimal } from "@/js/utils/format.js";
import { copyObject, sameObject } from "@/js/utils/object.js";
import { duplicatedItem } from "@/js/utils/array.js";
import { useUnitStore } from "@/stores/stores.js";
import { mapStores, mapState } from 'pinia';
import UnitForm from '@/views/units/UnitForm.vue';

export default {
    name: 'ConversionUnit',
    components: {
        UnitForm,
    },
    props: {
        /**
         * (Props) Conversion unit
         */
        conversionUnitModel: {
            type: Object,
            default: null
        },
        /**
         * Material object
         */
        material: {
            type: Object,
            default: null,
        },
        /**
         * Số thứ tự
         */
        index: {
            type: Number,
        },
        /**
         * Focused id
         */
        focusedId: {
            type: String,
            default: null,
        },
    },
    data() {
        return {
            /**
             * (Data) Conversion unit data
             */
            conversionUnit: null,
            /**
             * Fields info
             */
            fields: fields,
            /**
             * Operator list
             */
            operatorSelects: this.$common.selects.operator,
            /**
             * Style of DestinationUnit combobox
             */
            styleDesUnit: {
                width: null,
                top: null,
                left: null,
                position: null,
            },
            /**
             * Style of Operator combobox
             */
            styleOperator: {
                width: null,
                top: null,
                left: null,
                position: null,
            },
            /**
             * Original conversion unit
             */
            originalModel: {},
            /**
             * Focused input
             */
            refFocus: null,
            /**
             * Input refs
             */
            refs: [],
            /**
             * Error message
             */
            errorMessage: null,
            /**
             * Hành động tạo mới đơn vị tính
             */
            actionCreateUnit: {
                icon: "cukcuk-create",
                title: this.$resources['vn'].createNew,
                method: this.showEmptyUnitForm,
            },
            /**
             * Mode of unit form 
             */
            unitFormMode: null,
            /**
             * Show unit form or not 
             */
            isShowedUnitForm: false,
        }
    },
    created() {
        this.conversionUnit = this.conversionUnitModel;

        if (this.conversionUnit.Rate) {
            this.conversionUnit.Rate = this.formatDecimal(this.conversionUnit?.Rate);
        }

        this.conversionUnit.RowIndex ??= this.index + 1;
        
        this.originalModel = this.copyObject(this.conversionUnit);
    },
    mounted() {
        this.setWidthDesUnitCombobox();
        this.setWidthOperatorCombobox();

        if (this.focusedId) {
            this.focusOnFirst();
        }

        this.refs = [
            this.$refs.DestinationUnit,
        ];
    },
    expose: [
        'checkValidate',
        'assignEditMode',
        'focus',
        'focusOnFirst',
        'resetComboboxes',
        'errorMessage'],
    watch: {
        /**
         * Gán edit mode khi conversion unit thay đổi
         * 
         * Author: nlnhat (26/08/2023)
         */
        // conversionUnit: {
        //     handler() {
        //         if (this.conversionUnit.EditMode != this.$enums.editMode.create &&
        //             this.conversionUnit.EditMode != this.$enums.editMode.delete) {

        //             if (this.sameObject(this.conversionUnit, this.originalModel)) {
        //                 this.originalModel.EditMode = this.$enums.editMode.noEdit;
        //                 this.conversionUnit.EditMode = this.$enums.editMode.noEdit;
        //             }
        //             else {
        //                 this.originalModel.EditMode = this.$enums.editMode.update;
        //                 this.conversionUnit.EditMode = this.$enums.editMode.update;
        //             }
        //         }
        //     },
        //     deep: true,
        // },
        /**
         * Gán số thứ tự khi row index thay đổi
         * 
         * Author: nlnhat (26/08/2023)
         */
        index() {
            this.conversionUnit.RowIndex = this.index + 1;
        },
        /**
         * Check validate khi Unit Name hoặc các Conversion units khác thay đổi
         * 
         * Author: nlnhat (26/08/2023)
         */
        "material.UnitName": function () {
            if (this.conversionUnit.DestinationUnitId)
                this.checkValidate();
        },
        "material.ConversionUnits": {
            handler() {
                if (this.conversionUnit.DestinationUnitId)
                    this.checkValidate();
            },
            deep: true,
        }
    },
    computed: {
        /**
         * Tạo mô tả
         * 
         * Author: nlnhat (22/08/2023)
         */
        descriptionComputed() {
            const destinationUnit = this.conversionUnit.DestinationUnitId;
            const destinationUnitName = this.conversionUnit.DestinationUnitName;
            const rate = this.conversionUnit.Rate;
            const operator = this.conversionUnit.OperatorName;
            const unitName = this.material.UnitName;

            if (unitName != null && destinationUnit != null && rate != null && operator != null) {
                switch (this.conversionUnit.Operator) {
                    // Phép nhân
                    case this.$enums.operator.multiple: {
                        const description = `1 ${destinationUnitName} = ${rate} ${operator} ${unitName}`;
                        return description;
                    };
                    // Phép chia
                    case this.$enums.operator.divide: {
                        const description = `1 ${destinationUnitName} = 1 ${operator} ${rate} ${unitName}`;
                        return description;
                    }
                    default: return null
                };
            }
            return null;
        },
        /**
         * DestinationUnits
         * 
         * Author: nlnhat (26/08/2023)
         */
        destinationUnits() {
            return this.material.ConversionUnits.filter(unit => unit.EditMode != this.$enums.editMode.delete)
                .map(unit => unit.DestinationUnitName);
        },
        /**
         * Validate các input
         * 
         * Author: nlnhat (26/08/2023)
         */
        validateComputed() {
            try {
                let errorMessage = null;
                this.refs.forEach((ref) => {
                    const message = ref.checkValidate();
                    if (message) {
                        errorMessage = message;
                        this.refFocus = ref;
                        ref.focus();
                    };
                });
                return errorMessage;
            } catch (error) {
                console.error(error);
                return null
            }
        },
        /**
         * Map unit store
         */
        ...mapStores(useUnitStore),
        ...mapState(useUnitStore, {
            unitsComputed: 'unitSelects'
        }),
    },
    methods: {
        /**
         * Set width for destination unit combobox
         * 
         * Author: nlnhat (22/08/2023)
         */
        setWidthDesUnitCombobox() {
            const wrapper = this.$refs.DesUnitWrapper;
            this.styleDesUnit.width = wrapper.offsetWidth;
        },
        /**
         * Set position for destination unit combobox
         * 
         * Author: nlnhat (22/08/2023)
         */
        setPositionDesUnitCombobox() {
            const wrapper = this.$refs.DesUnitWrapper;
            this.styleDesUnit.top = wrapper.getBoundingClientRect().top;
            this.styleDesUnit.left = wrapper.getBoundingClientRect().left;
            this.styleDesUnit.position = "fixed";
        },
        /**
         * Set width for operator unit combobox
         * 
         * Author: nlnhat (22/08/2023)
         */
        setWidthOperatorCombobox() {
            const wrapper = this.$refs.OperatorWrapper;
            this.styleOperator.width = wrapper.offsetWidth;
        },

        /**
         * Set position for operator unit combobox
         * 
         * Author: nlnhat (22/08/2023)
         */
        setPositionOperatorCombobox() {
            const wrapper = this.$refs.OperatorWrapper;
            // console.log(wrapper.getBoundingClientRect());
            this.styleOperator.top = wrapper.getBoundingClientRect().top;
            this.styleOperator.left = wrapper.getBoundingClientRect().left;
            this.styleOperator.position = "fixed";
        },
        /**
         * Handle click wrapper combobox destination unit
         * 
         * Author: nlnhat (26/08/2023)
         */
        onClickDesUnit() {
            // this.setWidthDesUnitCombobox();
            this.setPositionDesUnitCombobox();
        },
        /**
         * Handle click wrapper combobox operator
         * 
         * Author: nlnhat (26/08/2023)
         */
        onClickOperator() {
            // this.setWidthOperatorCombobox();
            this.setPositionOperatorCombobox();
        },
        /**
         * Reset combobox destination unit
         * 
         * Author: nlnhat (26/08/2023)
         */
        resetDesUnitCombobox() {
            if (this.$refs.DestinationUnit)
                this.$refs.DestinationUnit.hideSelects();
            this.setWidthDesUnitCombobox();
            this.styleDesUnit.position = "relative";
            this.styleDesUnit.top = 0;
            this.styleDesUnit.left = 0;
        },
        /**
         * Reset combobox operator
         * 
         * Author: nlnhat (26/08/2023)
         */
        resetOperatorCombobox() {
            if (this.$refs.Operator)
                this.$refs.Operator.hideSelects();
            this.setWidthOperatorCombobox();
            this.styleOperator.position = "relative";
            this.styleOperator.top = 0;
            this.styleOperator.left = 0;
        },
        /**
         * Reset comboboxes
         * 
         * Author: nlnhat (26/08/2023)
         */
        resetComboboxes() {
            this.resetDesUnitCombobox();
            this.resetOperatorCombobox();
        },
        /**
         * Format rate input
         * 
         * Author: nlnhat (22/08/2023)
         */
        formatRateInput(value) {
            let rate = this.formatDecimalInput(value);
            if (isNullOrEmpty(rate))
                rate = '0';
            const numericValue = parseFloat(rate?.replace(',', '.'));
            if (numericValue == 0) {
                rate = 1;
            }
            rate = formatDecimal(rate);
            return rate;
        },
        /**
         * Focus on error
         */
        focus() {
            if (this.refFocus)
                this.refFocus.focus();
        },
        /**
         * Focus on first field
         */
        focusOnFirst() {
            if (this.$refs.DestinationUnit)
                this.$refs.DestinationUnit.focus();
            this.updateFocusedId();
        },
        /**
         * Update focused id
         *
         * Author: nlnhat (22/08/2023)
         */
        updateFocusedId() {
            this.$emit('update:focusedId', this.conversionUnit.ConversionUnitId);
        },
        /**
         * Validate DestinationUnit
         * 
         * Author: nlnhat (22/08/2023)
         * @param {*} label Nhãn hiển thị
         * @param {*} value Id unit
         * @param {*} selects Danh sách lựa chọn
         */
        validateDestinationUnit(label, value, selects) {
            try {
                let errorMessage = this.validateUnit(label, value, selects);

                if (!errorMessage) {
                    // Check trùng với đơn vị tính
                    if (this.material.UnitId != null && this.material.UnitName == value) {
                        errorMessage = `${label} <${value}> ${this.$resources['vn'].duplicatedWith} ${this.fields.unit.label}`;
                    }
                    // Check trùng với đơn vị chuyển đổi khác
                    if (!errorMessage && this.destinationUnits.filter(item => item == value).length > 1) {
                        errorMessage = `${label} <${value}> ${this.$resources['vn'].used}`;
                    }
                }
                return errorMessage;
            } catch (error) {
                console.error(error);
                return null;
            }
        },
        /**
         * Check validate value
         * 
         * Author: nlnhat (21/08/2023)
         * @param {*} value Value to validate 
         */
        checkValidate() {
            try {
                return this.errorMessage = this.validateComputed;
            } catch (error) {
                console.error(error);
                return null;
            }
        },
        /**
         * Gán edit mode
         * 
         * Author: nlnhat (26/08/2023)
         */
        assignEditMode() {
            if (this.conversionUnit.EditMode != this.$enums.editMode.create &&
                this.conversionUnit.EditMode != this.$enums.editMode.delete) {

                if (this.sameObject(this.conversionUnit, this.originalModel)) {
                    this.originalModel.EditMode = this.$enums.editMode.noEdit;
                    this.conversionUnit.EditMode = this.$enums.editMode.noEdit;
                }
                else {
                    this.originalModel.EditMode = this.$enums.editMode.update;
                    this.conversionUnit.EditMode = this.$enums.editMode.update;
                }
            }
        },
        /**
         * Show empty unit form to add new 
         * 
         * Author: nlnhat (26/08/2023)
         */
        showEmptyUnitForm() {
            this.unitFormMode = this.$enums.formMode.create;
            this.isShowedUnitForm = true;
        },
        /**
         * Hide unit form
         * 
         * Author: nlnhat (26/06/2023)
         */
        hideUnitForm() {
            this.isShowedUnitForm = false;
        },
        /**
         * Update new unit id
         * 
         * Author: nlnhat (26/06/2023)
         */
        updateUnit(id) {
            this.conversionUnit.DestinationUnitId = id;
        },
        /**
         * Imported methods
         */
        validateUnit,
        formatDecimalInput,
        formatDecimal,
        isNullOrEmpty,
        copyObject,
        sameObject,
        duplicatedItem,
    }
}
</script>
<style></style>