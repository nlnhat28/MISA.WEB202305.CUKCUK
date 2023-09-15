<template>
    <div class="tab-menu">
        <div class="tab-menu__header">
            <div class="tab-item tab-item--active">
                {{ this.$resources['vn'].conversionUnit }}
            </div>
        </div>
        <div class="tab-menu__body">
            <m-table @emitScroll="resetComboboxes()">
                <template #thead>
                    <!-- table head -->
                    <th
                        v-for="(head, index) in table.heads"
                        :key="index"
                        :style="`width: ${head.widthCell}px;`"
                        v-tooltip:right="head.fullTitle"
                    >
                        {{ head.title }}
                    </th>
                </template>
                <template #tbody>
                    <!-- table body -->
                    <ConversionUnit
                        v-for="(conversionUnit, index) in conversionUnitsComputed"
                        v-model:focusedId="focusedId"
                        :key="conversionUnit.ConversionUnitId"
                        :index="index"
                        :conversionUnitModel="conversionUnit"
                        :material="material"
                        @emitCreate="addRow"
                        @emitDelete="deleteRow"
                        ref="ConversionUnit"
                    >
                    </ConversionUnit>
                </template>
            </m-table>
        </div>
        <div class="tab-menu__footer">
            <div class="button-container">
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].addRow"
                    :click="addRow"
                    title="Ctrl + Insert"
                    iconLeft="cukcuk-new"
                ></m-button>
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].deleteRow"
                    :click="deleteRow"
                    :isDisabled="focusedId == null"
                    title="Ctrl + Delete"
                    iconLeft="cukcuk-delete"
                ></m-button>
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].detail"
                    :click="showConversionUnitDetail"
                    :isDisabled="conversionUnitsComputed.length <= 0 || !material.UnitId"
                    :title=null
                    iconLeft="cukcuk-detail"
                ></m-button>
            </div>
        </div>
        <Teleport to=".main-content">
            <ConversionUnitDetail
                v-if="isShowConversionUnitDetail"
                :closeThis="closeConversionUnitDetail"
                :title="this.$resources['vn'].detail"
                :dataModel="sortedConversionUnits"
                :unitName="this.material.UnitName"
            >
            </ConversionUnitDetail>
        </Teleport>
    </div>
</template>
<script>
import ConversionUnit from './ConversionUnit.vue';
import ConversionUnitDetail from './ConversionUnitDetail.vue';
import { v4 as uuidv4 } from 'uuid';
import { reserveArray } from '@/js/utils/array.js';
import { reformatDecimal } from "@/js/utils/clean-format.js";
import { formatDecimalLocale } from "@/js/utils/format.js";

export default {
    name: 'ConversionUnitTab',
    components: {
        ConversionUnit,
        ConversionUnitDetail,
    },
    props: {
        /**
         * Material object
         */
        material: {
            type: Object,
            default: null
        },
    },
    data() {
        return {
            /**
             * Table data
             */
            table: {
                heads: [
                    {
                        title: this.$resources['vn'].rowIndex,
                        fullTitle: this.$resources['vn'].rowIndexFullTitle,
                        textAlign: 'center',
                        widthCell: 50,
                    },
                    {
                        title: this.$resources['vn'].conversionUnit,
                        textAlign: 'center',
                        widthCell: 160,
                    },
                    {
                        title: this.$resources['vn'].conversionRate,
                        textAlign: 'center',
                        widthCell: 160,
                    },
                    {
                        title: this.$resources['vn'].operator,
                        textAlign: 'center',
                        widthCell: 160,
                    },
                    {
                        title: this.$resources['vn'].description,
                        textAlign: 'center',
                        widthCell: 200,
                    },
                ]
            },
            /**
             * Conversion unit data
             */
            conversionUnits: null,
            /**
             * Id is focused
             */
            focusedId: null,
            /**
             * Focused input
             */
            refFocus: null,
            /**
             * Error message
             */
            errorMessage: null,
            /**
             * Show or hide conversion unit detail
             */
            isShowConversionUnitDetail: false,
        }
    },
    created() {
        this.updateConversionUnits();
    },
    expose: [
        'checkValidate',
        'focus',
        'resetComboboxes',
        'assignEditMode',
        'addRow',
        'deleteRow'
    ],
    watch: {
        "material.ConversionUnits": {
            handler() {
                this.updateConversionUnits();
            },
            deep: true,
        },
        conversionUnits: {
            handler() {
                this.focusedId = this.conversionUnitsComputed.length > 0 ? this.focusedId : null;
            },
            deep: true,
        },
    },
    computed: {
        /**
         * Lấy đơn vị chuyển đổi không bị xoá
         * 
         * Author: nlnhat (26/08/2023)
         */
        conversionUnitsComputed() {
            return this.conversionUnits.filter(unit => unit.EditMode != this.$enums.editMode.delete)
        },
        /**
         * Validate các đơn vị chuyển đổi
         * 
         * Author: nlnhat (26/08/2023)
         */
        validateComputed() {
            try {
                let errorMessage = null;
                if (this.$refs.ConversionUnit) {
                    for (const ref of this.reserveArray(this.$refs.ConversionUnit)) {
                        const message = ref.checkValidate();
                        if (message) {
                            errorMessage = message;
                            this.refFocus = ref;
                        }
                    };
                };
                return errorMessage;
            } catch (error) {
                console.error(error);
                return null
            }
        },
        /**
         * Sorted convertion units
         * 
         * Author: nlnhat (26/08/2023)
         */
        sortedConversionUnits() {
            const dataUnits = this.conversionUnitsComputed
                .filter(unit => unit.DestinationUnitId)
                .map(unit => ({
                    name: unit.DestinationUnitName,
                    rate: (() => {
                        let rate = this.reformatDecimal(unit.Rate);
                        if (unit.Operator === this.$enums.operator.divide) {
                            rate = 1 / rate;
                        }
                        rate = this.formatDecimalLocale(rate, "en-US");
                        return rate;
                    })()
                }));
            const sortedUnits = dataUnits.sort((a, b) => a.rate - b.rate);
            return sortedUnits;
        }
    },
    methods: {
        /**
         * Xử lý khi click thêm dòng mới
         * 
         * Author: nlnhat (22/08/2023)
         */
        addRow() {
            const newId = uuidv4();
            this.conversionUnits.push({
                EditMode: this.$enums.editMode.create,
                ConversionUnitId: newId,
                Operator: this.$common.selects.operator[0].id,
                OperatorName: this.$common.selects.operator[0].name,
            });
            this.focusedId = newId;
        },
        /**
         * Xử lý khi click xoá dòng
         * 
         * Author: nlnhat (22/08/2023)
         */
        deleteRow() {
            if (this.conversionUnitsComputed.length > 0) {
                const conversionUnit = this.conversionUnits.find(unit => unit.ConversionUnitId == this.focusedId);
                const conversionUnitIndex = this.conversionUnits.indexOf(conversionUnit);
                const oldIndex = this.conversionUnitsComputed.indexOf(conversionUnit);

                // Nếu edit là create thì xoá luôn
                if (conversionUnit.EditMode == this.$enums.editMode.create) {
                    if (conversionUnitIndex !== -1) {
                        this.conversionUnits.splice(conversionUnitIndex, 1);
                    }
                }
                // TH khác thì chỉnh edit mode thành delete
                else {
                    conversionUnit.EditMode = this.$enums.editMode.delete;
                }
                // Cập nhật lại focused id
                const length = this.conversionUnitsComputed.length;
                if (length > 0) {
                    const indexFocus = oldIndex < length ? oldIndex : length - 1;
                    this.$nextTick(() => {
                        this.$refs.ConversionUnit[indexFocus].focusOnFirst();
                    })
                }
            }
        },
        /**
         * Update conversion units
         * 
         * Author: nlnhat (23/08/2023)
         */
        updateConversionUnits() {
            if (this.material.ConversionUnits == null)
                this.material.ConversionUnits = [];
            this.conversionUnits = this.material.ConversionUnits;
        },
        /**
         * Update focused id
         * 
         * Author: nlnhat (08/08/2023) 
         * @param {*} id Focused id
         */
        updateFocusedId(id) {
            this.focusedId = id;
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
            if (this.$refs.ConversionUnit) {
                for (const ref of this.$refs.ConversionUnit) {
                    ref.assignEditMode();
                };
            };
        },
        /**
         * Focus on error
         */
        focus() {
            if (this.refFocus)
                this.refFocus.focus();
        },
        /**
         * Reset style comboboxes
         *
         * Author: nlnhat (26/06/2023)
         */
        resetComboboxes() {
            if (this.$refs.ConversionUnit) {
                for (const ref of this.$refs.ConversionUnit) {
                    ref.resetComboboxes();
                };
            };
        },
        /**
         * Show ConversionUnitDetail
         *
         * Author: nlnhat (08/09/2023)
         */
        showConversionUnitDetail() {
            this.isShowConversionUnitDetail = true;
        },
        /**
         * Close ConversionUnitDetail
         *
         * Author: nlnhat (08/09/2023)
         */
        closeConversionUnitDetail() {
            this.isShowConversionUnitDetail = false;
        },
        /**
         * Imported methods
         */
        reserveArray,
        reformatDecimal,
        formatDecimalLocale,
    }
}
</script>
<style></style>