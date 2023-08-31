<template>
    <div class="textfield">
        <div
            :class="{ 'input-group': true, 'input-group--right': action }"
            @click="focus()"
        >
            <input
                v-if="!isTextarea"
                :type="type"
                :class="{ 'input--error': errorMessage }"
                :placeholder="placeholder"
                :readonly="isReadOnly"
                :style="`text-align: ${textAlign};`"
                ref="input"
                v-model="inputValue"
                @input="this.$emit('emitInput')"
                @blur="this.$emit('emitBlur')"
                @keydown="onKeyDown"
            />
            <textarea
                v-if="isTextarea"
                :type="type"
                :class="{ 'input--error': errorMessage }"
                :placeholder="placeholder"
                :readonly="isReadOnly"
                :style="`text-align: ${textAlign};`"
                ref="input"
                v-model="inputValue"
                @input="this.$emit('emitInput')"
                @blur="this.$emit('emitBlur')"
                @keydown="onKeyDown"
            />
            <div
                v-if="action"
                class="input-icon input-icon--right icon-container"
                @click="onAction"
                v-tooltip="isLoading ? null : action.title"
            >
                <m-icon
                    :icon="action.icon"
                    :isBtn="true"
                    v-if="!isLoading && !isSuccess"
                ></m-icon>
                <m-icon-container
                    icon="check--blue"
                    v-if="isSuccess"
                ></m-icon-container>
                <m-spinner v-if="isLoading && !isSuccess"></m-spinner>
            </div>
        </div>
        <m-icon-container
            v-if="errorMessage"
            :icon="'error'"
            class="cursor--default tooltip--light"
            v-tooltip="`⛔ ${errorMessage}`"
        ></m-icon-container>
    </div>
</template>
<script>
import { debounce } from 'lodash';

export default {
    name: 'MISATextfield',
    props: {
        /**
         * Type of input
         */
        type: {
            type: String,
            default: 'text'
        },
        /**
         * Focused or not
         */
        isFocused: {
            type: Boolean,
            default: false,
        },
        /**
         * Readonly or not
         */
        isReadOnly: {
            type: Boolean,
            default: false,
        },
        /**
         * Model binding
         */
        modelValue: {
            type: [String, Number]
        },
        /**
         * Tên nhãn
         */
        label: {
            type: String,
        },
        /**
         * Độ dài tối đa
         */
        maxLength: {
            type: Number,
        },
        /**
         * Placeholder
         */
        placeholder: {
            type: String,
        },
        /**
         * Function to validate
         */
        validate: {
            type: Function,
        },
        /**
         * Function to format
         */
        format: {
            type: Function,
        },
        /**
         * Hành động đi kèm
         */
        action: {
            type: Object,
            default: null
        },
        /**
         * Tag name input or textarea
         */
        isTextarea: {
            type: Boolean,
            default: false,
        },
        /**
         * Align text
         */
        textAlign: {
            type: String,
            validator: (value) => {
                return [
                    'left',
                    'center',
                    'right',
                ].includes(value)
            }
        },
        /**
         * Set lại con trỏ select về vị trí trước khi text thay đổi
         */
        canSetSelection: {
            type: Boolean,
            default: false,
        },
        /**
         * Debounce when update or not
         */
        hasDebounce: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            /**
             * Giá trị text fields
             */
            inputValue: this.modelValue ?? null,
            /**
             * Thông báo lỗi nếu có
             */
            errorMessage: null,
            /**
             * Trạng thái thực hiện hành động
             */
            isLoading: {
                type: Boolean,
                default: false
            },
            /**
             * Show icon after success action
             */
            isSuccess: false,
        }
    },
    created() {
        if (this.type == 'date') {
            this.inputValue = this.formatDateComputed;
        };
        this.inputValue = this.formatComputed;
        this.isLoading = false;
        this.isSuccess = false;
    },
    mounted() {
        if (this.isFocused) this.focus();
    },
    beforeUnmount() {
        this.blur();
    },
    expose: [
        'checkValidate',
        'focus',
        'select',
        'errorMessage',
        'clearErrorMessage',
        'setErrorMessage',
        'onAction'],
    watch: {
        modelValue() {
            this.inputValue = this.modelValue
        },
        inputValue() {
            this.onChangeValue();
        },
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
                // Validate length
                if (this.maxLength && this.inputValue?.length > this.maxLength)
                    return `${this.label} ${this.$resources['vn'].mustLessThan} ${this.maxLength + 1} ${this.$resources['vn'].char}`;

                // Custom validate
                if (this.validate) {
                    return this.validate(this.label, this.inputValue);
                }
            } catch (error) {
                console.error(error);
            }
            return null;
        },
        /**
         * Format value
         * 
         * Author: nlnhat (01/07/2023)
         * @return Formated value
         */
        formatComputed() {
            try {
                if (this.format)
                    return this.format(this.inputValue)
            } catch (error) {
                console.error(error);
            }
            return this.inputValue;
        },
        /**
         * Format date 
         *
         * Author: nlnhat (01/07/2023)
         * @return Date format yyyy/MM/dd
         */
        formatDateComputed() {
            try {
                if (this.inputValue != null) {
                    let date = new Date(this.inputValue);
                    const dateOfMonth = date.getDate().toString().padStart(2, "0");
                    const month = (date.getMonth() + 1).toString().padStart(2, "0");
                    const year = date.getFullYear();
                    return `${year}-${month}-${dateOfMonth}`;
                }
            } catch (error) {
                console.error(error);
            }
            return null;
        }
    },
    methods: {
        /**
         * Focus on input
         * 
         * Author: nlnhat (01/07/2023)
         */
        focus() {
            if (this.$refs.input)
                this.$refs.input.focus();
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
                // Set lại con trỏ select về vị trí trước khi text thay đổi
                if (this.canSetSelection && this.$refs.input) {
                    var position = this.$refs.input.selectionStart;
                    this.inputValue = this.formatComputed;
                    this.$nextTick(() => {
                        this.$refs.input.setSelectionRange(position, position);
                    });
                }
                else this.inputValue = this.formatComputed;

                if (this.hasDebounce)
                    this.debounceUpdate();
                else 
                    this.$emit('update:modelValue', this.inputValue);
            
                this.checkValidate();
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
         * Make loading effect
         * 
         * Author: nlnhat (05/07/2023)
         * @param {function} func Function executes in loading process
         */
        async makeLoadingEffect(func) {
            try {
                this.isLoading = true;
                if (await func()) {
                    this.isSuccess = true;
                    await new Promise((resolve) => setTimeout(resolve, 1500));
                    this.isSuccess = false;
                }
            } catch (error) {
                console.error(error);
            } finally {
                this.isLoading = false;
            }
        },
        /**
         * Handle when click action
         * 
         * Author: nlnhat (23/07/2023)
         */
        onAction() {
            this.makeLoadingEffect(this.action.method)
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
         * Handle key down on input
         * 
         * Author: nlnhat (29/07/2023)
         */
        onKeyDown(event) {
            switch (event.keyCode) {
                case this.$enums.keyCode.enter:
                    event.preventDefault();
                    event.stopPropagation();
                    this.$emit('emitEnter');
                    break;
                case this.$enums.keyCode.space:
                    event.stopPropagation();
                    this.$emit('emitSpace');
                    break;
                case this.$enums.keyCode.tab:
                    this.$emit('emitTab');
                    break;
                default:
                    break;
            }
        },
        /**
         * Debounce update
         * 
         * Author: nlnhat (12/07/2023)
         */
        debounceUpdate: debounce(function () {
            this.$emit('update:modelValue', this.inputValue)
        }, 1000),
    }
}
</script>
<style scoped>
.input-group .icon-container:hover .icon-btn {
    filter: invert(0.4)
}

.input-group .icon-container:active .icon-btn {
    filter: brightness(0.6)
}

.input-group .icon-container {
    position: absolute;
}

.input-icon {
    display: none;
}

.input-group:hover .input-icon,
input:focus~.input-icon {
    display: flex;
}

.loading-spinner {
    position: absolute;
    scale: 0.24;
}

.icon--xmark {
    scale: 0.7;
}
</style>
