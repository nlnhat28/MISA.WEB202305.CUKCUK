<template>
    <div
        class="textfield"
        v-tooltip:bottom="errorMessage"
    >
        <div class="input-group input-group--right">
            <VueDatePicker
                v-model="date"
                text-input
                auto-apply
                keep-action-row
                clearable
                arrow-navigation
                model-type="yyyy-MM-dd"
                month-name-format="long"
                locale="vi"
                :action-row="{ showNow: true, showCancel: false, showSelect: true, showPreview: false }"
                :day-names="['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN']"
                :enable-time-picker="false"
                ref="datePicker"
                :input-class-name="errorMessage ? 'input--error' : ''"
            >
                <template #dp-input>
                    <input
                        type="text"
                        ref="input"
                        v-model="inputValue"
                        :placeholder="dateFormat"
                        :class="{ 'dp__input': true, 'input--error': errorMessage }"
                        @keydown="handleKeydownInput"
                        @focus="onFocusInput"
                    />
                </template>
                <template #input-icon>
                    <div
                        class="input-icon input-icon--right icon-container"
                        @click="focus()"
                    >
                        <m-icon icon="calendar"></m-icon>
                    </div>
                </template>
                <template #action-row>
                    <div class="action-row">
                        <m-button
                            :type="this.$enums.buttonType.link"
                            :text="this.$resources['vn'].today"
                            :click="selectToday"
                        >
                        </m-button>
                        <m-button
                            :type="this.$enums.buttonType.linkDanger"
                            :text="this.$resources['vn'].delete"
                            :click="removeInputValue"
                        >
                        </m-button>
                    </div>
                </template>
            </VueDatePicker>
        </div>
    </div>
</template>

<script>
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import { formatDateDefault, formatDate } from '@/js/utils/format.js'
import { isValidDate } from '@/js/form/validate.js'

export default {
    components: { VueDatePicker },
    props: {
        /**
         * Date model 
         */
        dateModel: {
            type: String,
            default: null
        },
        /**
         * Validate function
         */
        validate: {
            type: Function,
            default: null
        },
        /**
         * Date format
         */
        dateFormat: {
            type: String,
            default: 'dd/MM/yyyy',
            validator: (value) => {
                return [
                    'dd/MM/yyyy',
                    'yyyy/MM/dd'
                ].includes(value);
            },
        },
        /**
         * Label name of field
         */
        label: {
            type: String,
        }
    },
    data() {
        return {
            /**
             * Date data
             */
            date: null,
            /**
             * Error message in error validate case
             */
            errorMessage: null,
            /**
             * Input value
             */
            inputValue: null,
            /**
             * Old input value
             */
            oldInputValue: null,
        };
    },
    expose: ['checkValidate', 'focus', 'select', 'errorMessage', 'clearErrorMessage', 'setErrorMessage'],
    created() {
        if (typeof this.dateModel != 'number')
            this.date = this.formatDateDefault(this.dateModel);
        this.inputValue = this.formatDateComputed;
    },
    watch: {
        date() {
            this.inputValue = this.formatDateComputed;
            this.focus();
        },
        inputValue(newValue, oldValue) {
            this.oldInputValue = oldValue;
            this.inputValue = this.formatInputComputed;
            this.onChangeValue();
        }
    },
    computed: {
        /**
         * Validate value
         * 
         * Author: nlnhat (01/07/2023)
         * @return Error message if invalid, null if valid
         */
        validateComputed() {
            try {
                // Validate format
                if (this.inputValue != null && this.inputValue != '') {
                    switch (this.dateFormat) {
                        case 'dd/MM/yyyy':
                            var pattern = /^\d{2}\/\d{2}\/\d{4}$/;
                            if (!pattern.test(this.inputValue)) {
                                return `${this.label} ${this.$resources['vn'].invalidDateddMMyyyy}`;
                            }
                            break;
                        case 'yyyy/MM/dd':
                            var pattern = /^\d{4}\/\d{2}\/\d{2}$/;
                            if (!pattern.test(this.inputValue)) {
                                return `${this.label} ${this.$resources['vn'].invalidDateyyyyMMdd}`;
                            }
                            break;
                        default:
                            break;
                    }
                }

                // Validate valid date
                if (!this.isValidDate(this.convertDateComputed)) {
                    return `${this.label} ${this.$resources['vn'].invalidDate}`;
                }

                this.date = this.convertDateComputed;

                // Custom validate
                if (this.validate) {
                    return this.validate(this.label, this.convertDateComputed);
                }
            } catch (error) {
                console.error(error);
            }
            return null;
        },
        /**
         * Convert date to yyyy-MM-dd 
         *
         * Author: nlnhat (01/07/2023)
         * @return Date format yyyy-MM-dd
         */
        convertDateComputed() {
            try {
                if (this.inputValue == null || this.inputValue == '') return null;

                switch (this.dateFormat) {
                    case 'dd/MM/yyyy':
                        var pattern = /^\d{2}\/\d{2}\/\d{4}$/;
                        if (pattern.test(this.inputValue)) {
                            var newDate = this.inputValue?.replace(/(\d{2})\/(\d{2})\/(\d{4})/, '$3-$2-$1');
                            return newDate;
                        }
                    case 'yyyy/MM/dd':
                        var pattern = /^\d{4}\/\d{2}\/\d{2}$/;
                        if (pattern.test(this.inputValue)) {
                            var newDate = this.inputValue?.replace(/(\d{4})\/(\d{2})\/(\d{2})/, '$1-$2-$3');
                            return newDate;
                        }
                    default:
                        break;
                }
                return "/"
            } catch (error) {
                console.error(error);
            }
            return null;
        },
        /**
         * Format date 
         *
         * Author: nlnhat (01/07/2023)
         * @return Date format yyyy/MM/dd
         */
        formatDateComputed() {
            return this.formatDate(this.date, this.dateFormat);
        },
        /**
         * Format input 
         *
         * Author: nlnhat (01/07/2023)
         * @return Date format yyyy/MM/dd
         */
        formatInputComputed() {
            if (this.inputValue == null || this.inputValue == '') return this.inputValue;
            switch (this.dateFormat) {
                case 'dd/MM/yyyy':
                    // Nếu không có dạng dd/ thì thay dd -> dd/
                    var newInputValue = this.inputValue;

                    var pattern = /^\d{2}\/$/;
                    if (!pattern.test(this.oldInputValue))
                        newInputValue = newInputValue?.replace(/^(\d{2})$/g, '$1/');

                    // Nếu không có dạng dd/MM/ thì thay dd/MM -> dd/MM/
                    var pattern = /^\d{2}\/\d{2}\/$/;
                    if (!pattern.test(this.oldInputValue))
                        newInputValue = newInputValue?.replace(/^(\d{2}\/\d{2})$/g, '$1/')

                    return newInputValue;

                case 'yyyy/MM/dd':
                    // Nếu không có dạng yyyy/ thì thay yyyy -> yyyy/
                    var newInputValue = this.inputValue;

                    var pattern = /^\d{4}\/$/;
                    if (!pattern.test(this.oldInputValue))
                        newInputValue = newInputValue?.replace(/^(\d{4})$/g, '$1/');

                    // Nếu không có dạng yyyy/MM/ thì thay yyyy/MM -> yyyy/MM/
                    var pattern = /^\d{4}\/\d{2}\/$/;
                    if (!pattern.test(this.oldInputValue))
                        newInputValue = newInputValue?.replace(/^(\d{4}\/\d{2})$/g, '$1/')

                    return newInputValue;

                default:
                    return this.inputValue;
            }
        }
    },
    methods: {
        /**
         * Open menu
         * 
         * Author: nlnhat (02/08/2023)
         */
        openMenu() {
            if (this.$refs.datePicker)
                this.$refs.datePicker.openMenu();
        },
        /**
         * Close menu
         * 
         * Author: nlnhat (02/08/2023)
         */
        closeMenu() {
            if (this.$refs.datePicker)
                this.$refs.datePicker.closeMenu();
        },
        /**
         * Handle keydown on input
         * 
         * Author: nlnhat (02/08/2023)
         */
        handleKeydownInput(event) {
            // Space: Open menu
            if (event.keyCode == this.$enums.keyCode.space) {
                event.preventDefault();
                event.stopPropagation();
                this.openMenu();
            }
            // Enter: Close menu
            else if (event.keyCode == this.$enums.keyCode.enter) {
                event.preventDefault();
                event.stopPropagation();
                this.$emit('emitEnterInput');
                this.closeMenu();
            }
        },
        /**
         * Handle focus on input
         * 
         * Author: nlnhat (02/08/2023)
         */
        onFocusInput() {
            // this.openMenu();
        },
        /**
         * Select today
         * 
         * Author: nlnhat (02/08/2023)
         */
        selectToday() {
            try {
                this.date = this.formatDateDefault(new Date());
                this.closeMenu();
                this.focus();
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Remove input value
         * 
         * Author: nlnhat (02/08/2023)
         */
        removeInputValue() {
            try {
                this.inputValue = null;
                this.closeMenu();
                this.focus();
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Handle selected complete
         * 
         * Author: nlnhat (02/08/2023)
         */
        selected() {
            this.$emit('emitSelected');
        },
        /**
         * Focus on input
         * 
         * Author: nlnhat (01/07/2023)
         */
        focus() {
            if (this.$refs.input) {
                this.$refs.input.focus();
            }
        },
        /**
         * Blur input
         * 
         * Author: nlnhat (01/07/2023)
         */
        blur() {
            if (this.$refs.input)
                this.$refs.input.blur();
        },
        /**
         * Select all text input
         * 
         * Author: nlnhat (30/07/2023)
         */
        select() {
            if (this.$refs.input)
                this.$refs.input.select();
        },
        /**
         * Handle input value change
         * 
         * Author: nlnhat (01/07/2023)
         */
        onChangeValue() {
            try {
                this.checkValidate();
                this.$emit('update:dateModel', this.convertDateComputed)
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Check validate value
         * 
         * Author: nlnhat (02/07/2023)
         */
        checkValidate() {
            try {
                return this.errorMessage = this.validateComputed;
            } catch (error) {
                console.error(error);
            }
            return null
        },
        /**
         * Set error message
         * 
         * Author: nlnhat (23/07/2023)
         * @param {*} message Message to set for error message
         */
        setErrorMessage(message) {
            this.errorMessage = message;
        },
        /**
         * Clear error message
         * 
         * Author: nlnhat (23/07/2023)
         */
        clearErrorMessage() {
            this.errorMessage = null;
        },
        /**
         * Imported methods
         */
        formatDateDefault,
        formatDate,
        isValidDate,
    }
}
</script>
<style>@import './date-picker.css';</style>