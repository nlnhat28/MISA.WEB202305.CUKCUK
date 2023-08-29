<template>
    <label class="checkbox-wrapper">
        <input
            type="checkbox"
            :checked="checked"
            @change="onChange()"
            :disabled="disabled"
        >
        <span :class="{ 'checkmark': true, 'checkmark--checksome': isCheckSome }"></span>
    </label>
</template>

<script>
export default {
    name: 'MISACheckboxAll',
    props: {
        /**
         * (Props) Disabled state
         */
        disabled: {
            type: Boolean,
        },
        /**
         * (Props) Bind checked state
         */
        checked: {
            type: Boolean,
        },
        /**
         * Check some (Has check but not check all)
         */
        isCheckSome: {
            type: Boolean,
            default: false,
        }
    },
    methods: {
        /**
         * Handle check change
         * 
         * Author: nlnhat (12/07/2023)
         */
        onChange() {
            this.$emit('emitOnChange');
        }
    }
};
</script>