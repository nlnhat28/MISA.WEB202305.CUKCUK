<template>
    <div
        class="pagination"
        v-if="true"
    >
        <div class="pagination__property">
            <div class="pagination__switch">
                <!-- Trở về trang đầu -->
                <div
                    class="d-flex"
                    :class="{ 'disabled': page.pageNumber <= 1 }"
                >
                    <m-icon-container
                        icon="cukcuk-double-left"
                        :isBtn="true"
                        @click="onFirst()"
                        v-tooltip="this.$resources['vn'].firstPage"
                    >
                    </m-icon-container>
                </div>
                <!-- Trang trước -->
                <div
                    class="d-flex"
                    :class="{ 'disabled': page.pageNumber <= 1 }"
                >
                    <m-icon-container
                        icon="cukcuk-angle-left"
                        :isBtn="true"
                        @click="onPrevious()"
                        v-tooltip="this.$resources['vn'].previousPage"
                    >
                    </m-icon-container>
                </div>
            </div>
            <m-separator></m-separator>
            <div class="pagination__property">
                {{ this.$resources['vn'].page }}
                <!-- Số trang hiện tại -->
                <m-textfield
                    v-model="page.pageNumber"
                    :format="formatInput"
                    :isReadOnly="pageLength == 0"
                    @emitInput="inputChange()"
                    ref="input"
                ></m-textfield>
                {{ this.$resources['vn'].on }}
                {{ pageLength }}
            </div>
            <m-separator></m-separator>
            <div class="pagination__switch">
                <!-- Trang sau -->
                <div
                    class="d-flex"
                    :class="{ 'disabled': page.pageNumber >= pageLength }"
                >
                    <m-icon-container
                        icon="cukcuk-angle-right"
                        :isBtn="true"
                        @click="onNext()"
                        v-tooltip="this.$resources['vn'].nextPage"
                    >
                    </m-icon-container>
                </div>
                <!-- Trang cuối -->
                <div
                    class="d-flex"
                    :class="{ 'disabled': page.pageNumber >= pageLength }"
                >
                    <m-icon-container
                        icon="cukcuk-double-right"
                        :isBtn="true"
                        @click="onLast()"
                        v-tooltip="this.$resources['vn'].lastPage"
                    >
                    </m-icon-container>
                </div>
            </div>
        </div>
        <m-separator></m-separator>
        <!-- refresh -->
        <div class="pagination__property">
            <m-icon-container
                icon="refresh"
                :isBtn="true"
                @click="this.$emit('emitReload')"
                v-if="true"
                v-tooltip="this.$resources['vn'].reloadData"
            >
            </m-icon-container>
        </div>
        <m-separator></m-separator>
        <div class="pagination__property">
            <m-combobox
                :theSelects="pageSizesSelects"
                v-model:id="page.pageSize"
                v-model:name="page.pageSize"
                ref="refPageSize"
                :isDisabled="pageLength == 0"
                :isReadOnly="true"
                :canSearch="false"
                :canUnselect="false"
            ></m-combobox>
        </div>
    </div>
    <div class="pagination__property pagination__property--onlytext">
        <div v-if="totalRecord > 0">
            {{ this.$resources['vn'].display }}
            {{ displayComputed }}
            {{ totalRecord }}
            {{ this.$resources['vn'].result }}
        </div>
        <div v-else>
            {{ this.$resources['vn'].noContent }}
        </div>
    </div>
</template>
<script>
import { cleanNotDigitChar } from '@/js/utils/clean-format.js';
import { isNullOrEmpty } from '@/js/utils/string.js';
import { debounce } from 'lodash';

export default {
    name: 'MISAPagination',
    props: {
        /**
         * (Props) Total record
         */
        totalRecord: {
            type: Number,
            default: 0,
        },
        /**
         * (Props) All record
         */
        allRecord: {
            type: Number,
            default: 0,
        },
        /**
         * (Props) Page object (includes pageNumber and pageSize 
         */
        pageModel: {
            type: Object,
            default: {
                pageNumber: 1,
                pageSize: 25,
            }
        },
        /**
         * Can access to any page number or access each page one by one
         */
        canAccessRandom: {
            type: Boolean,
            default: true,
        },
    },
    data() {
        return {
            /**
             * (Data) Page properties (pageNumber, pageSize)
             */
            page: this.pageModel,
            /**
             * Selects for records/page 
             */
            pageSizesSelects: [
                {
                    id: 10,
                    name: 10
                },
                {
                    id: 25,
                    name: 25
                },
                {
                    id: 50,
                    name: 50
                },
                {
                    id: 100,
                    name: 100
                }
            ],
            /**
             * Percentage totalRecord/allRecord
             */
            percentage: 0,
        }
    },
    mounted() {
        // Add shortkey event
        document.addEventListener('keydown', this.handleShortKey);
    },
    beforeUnmount() {
        // Remove shortkey event
        document.removeEventListener('keydown', this.handleShortKey);
    },
    computed: {
        /**
         * Return length of total pages
         * 
         * Author: nlnhat (22/06/2023)
         */
        pageLength() {
            const length = Math.ceil(this.totalRecord / this.page.pageSize)
            return length
        },
        /**
         * Return index of first record in a page
         * 
         * Author: nlnhat (22/06/2023)
         */
        fromRecord() {
            return (this.page.pageSize * (this.page.pageNumber - 1) + 1)
        },
        /**
         * Return index of last record in a page
         * 
         * Author: nlnhat (22/06/2023)
         */
        toRecord() {
            return Math.min(this.fromRecord + this.page.pageSize - 1, this.totalRecord)
        },
        /**
         * Percentage totalRecord/allRecord
         * 
         * Author: nlnhat (22/06/2023)
         */
        percentageComputed() {
            this.percentage = (this.totalRecord / this.allRecord) * 100;
            this.percentage = this.percentage % 1 == 0 ? this.percentage.toFixed(0) : this.percentage.toFixed(1);
            return `${this.percentage}%`
        },
        /**
         * Display from record to record
         * 
         * Author: nlnhat (22/06/2023)
         */
        displayComputed() {
            if (this.fromRecord >= this.toRecord)
                return `${this.$resources['vn'].result} ${this.toRecord} ${this.$resources['vn'].on}`;
            else
                return `${this.fromRecord} - ${this.toRecord} ${this.$resources['vn'].on}`;
        }
    },
    emits: ['emitUpdatePage', 'emitReload'],
    watch: {
        /**
         * Handler when page model changes
         * 
         * Author: nlnhat (22/06/2023)
         */
        pageModel: {
            handler() {
                this.page = this.pageModel
            },
            deep: true
        },
        /**
         * Handle when page size changes
         * 
         * Author: nlnhat (22/06/2023)
         */
        "page.pageSize": function () {
            this.page.pageNumber = 1
            this.updatePage();
        },
    },
    methods: {
        /**
         * Jump to previous page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onPrevious() {
            this.page.pageNumber = Math.max(1, --this.page.pageNumber);
            this.updatePage();
        },
        /**
         * Jump to next page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onNext() {
            this.page.pageNumber = Math.min(this.pageLength, ++this.page.pageNumber);
            this.updatePage();
        },
        /**
         * Jump to first page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onFirst() {
            this.page.pageNumber = 1;
            this.updatePage();
        },
        /**
         * Jump to last page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onLast() {
            this.page.pageNumber = this.pageLength;
            this.updatePage();
        },
        /**
         * Handle shortcut keys
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            const code = this.$enums.keyCode;
            // Left: Trang trước
            if (event.ctrlKey && event.keyCode == code.left) {
                event.preventDefault();
                event.stopPropagation();
                this.onPrevious();
            }
            // Right: Trang tiếp
            else if (event.ctrlKey && event.keyCode == code.right) {
                event.preventDefault();
                event.stopPropagation();
                this.onNext();
            }
            // Ctrl + up : Trang đầu
            else if (event.ctrlKey && event.keyCode == code.up) {
                event.preventDefault();
                this.onFirst();
            }
            // Ctrl + up: Trang cuối
            else if (event.ctrlKey && event.keyCode == code.down) {
                event.preventDefault();
                event.stopPropagation();
                this.onLast();
            }
        },
        /**
         * Update page property
         
         * Author: nlnhat (18/08/2023)
         */
        updatePage() {
            this.$emit('emitUpdatePage', this.page)
        },
        /**
         * Select input
         
         * Author: nlnhat (18/08/2023)
         */
        select() {
            if (this.$refs.input)
                this.$refs.input.select();
        },
        /**
         * Handle input change
         
         * Author: nlnhat (18/08/2023)
         */
        inputChange() {
            this.debounceUpdate();
        },
        /**
         * Handle input change
         * 
         * Author: nlnhat (12/07/2023)
         */
        debounceUpdate: debounce(function () {
            this.select();
            this.updatePage();
        }, 600),
        /**
         * Format input
         * 
         * Author: nlnhat (18/08/2023)
         * @param {*} value 
         */
        formatInput(value) {
            if (this.pageLength == 0)
                return 0;
            let newValue = Math.max(1, Math.min(
                this.pageLength, parseInt(
                    value.toString().replace(/\D/g, ""))) || 1);
            return newValue;
        },
        /**
         * Imported methods
         */
        cleanNotDigitChar,
        isNullOrEmpty,
    }
}
</script>
<style>
input[type="number"]::-webkit-inner-spin-button,
input[type="number"]::-webkit-outer-spin-button {
    display: none;
}
</style>