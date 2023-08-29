<template>
    <div
        class="radio-item"
        @click="onClickItem()"
        @keydown="onKeyDown"
    >
        <label
            class="radio-wrapper"
            @click="onClickWrapper"
        >
            <input
                type="radio"
                :checked="checked"
                ref="input"
                @input="onChangeValue"
                :name="name"
            />
            <span class="checkmark"></span>
        </label>
        <label class="radio-item__label"> {{ radio.name }}</label>
    </div>
</template>
<script>
export default {
    name: 'MISARadioItem',
    props: {
        /**
         * Object contains value of radio (id, name)
         */
        radio: {
            type: Object
        },
        /**
         * Checked or not
         */
        checked: {
            type: Boolean
        },
        /**
         * Name of radio group
         */
        name: {
            type: String
        },
    },
    expose: ['focus'],
    methods: {
        /** 
         * Handle checked radio
         * 
         * Author: nlnhat (01/07/2023)
         * @param {*} event Radio input
         * @param {*} radio This radio
         */
        onChangeValue(event) {
            if (event.target.checked) {
                this.$emit('emitUpdateValue', this.radio)
            }
        },
        /** 
         * Handle click radio item
         * 
         * Author: nlnhat (01/07/2023)
         */
        onClickItem() {
            if (this.$refs.input) {
                this.$refs.input.checked = true;
                this.$emit('emitUpdateValue', this.radio)
                this.$refs.input.focus();
            }
        },
        /**
         * Handle click radio wrapper
         * 
         * Author: nlnhat (05/07/2023)
         * @param {*} event Click event
         */
        onClickWrapper(event) {
            event.stopPropagation();
        },
        /**
         * Focus on input
         * 
         * Author: nlnhat (05/07/2023)
         */
        focus() {
            if (this.$refs.input)
                this.$refs.input.focus();
        },
        /**
         * Handle keydown on radio item
         * 
         * Author: nlnhat (05/07/2023)
         */
        onKeyDown(event) {
            if (event.keyCode == this.$enums.keyCode.enter) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickItem();
            }
        },
    }

}
</script>