<template>
    <div
        class="textfield search-box"
        v-tooltip="tooltipContent"
    >
        <div
            :class="{ 'input-group input-group--left': true, 'input-group--right': this.inputValue?.length }"
            @click="focus()"
        >
            <div class="input-icon input-icon--left">
                <m-icon icon="magnifying-glass"></m-icon>
            </div>
            <div
                class="input-icon input-icon--right input-clear icon-container"
                @click="onClear()"
                v-show="this.inputValue?.length"
            >
                <m-icon-x></m-icon-x>
            </div>
            <input
                type="text"
                ref="input"
                :placeholder="placeholder"
                v-model="this.inputValue"
                @keydown="onKeyDown"
            />
        </div>
    </div>
</template>
<script>
import { debounce } from 'lodash';

export default {
    name: 'MISASearchBox',
    props: {
        /**
         * Value to bind
         */
        modelValue: {
            type: String,
        },
        /**
         * Tooltip string
         */
        tooltipContent: {
            type: String
        },
        /**
         * Placeholder input
         */
        placeholder: {
            type: String
        }
    },
    data() {
        return {
            inputValue: this.modelValue
        }
    },
    beforeUnmount() {
        this.blur();
    },
    watch: {
        inputValue() {
            this.onInput()
        },
    },
    methods: {
        /**
         * Handle input change
         * 
         * Author: nlnhat (12/07/2023)
         */
        onInput: debounce(function () {
            this.$emit('update:modelValue', this.inputValue)
        }, 1000),
        /**
         * Clear content in textfield
         * 
         * Author: nlnhat (12/07/2023)
         */
        onClear() {
            this.inputValue = ''
        },
        focus() {
            this.$refs.input.focus();
        },
        blur() {
            this.$refs.input.blur();
        },
        /**
         * Handle keydown on input
         * 
         * Author: nlnhat (29/07/2023)
         */
        onKeyDown(event) {
            if (event.keyCode == this.$enums.keyCode.down) {
                event.preventDefault();
                event.stopPropagation();
                this.$emit('emitFocusFirst');
            }
        }
    }
}
</script>