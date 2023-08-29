<template>
    <div
        class="checkbox-item"
        @click="onClickItem()"
        @mousedown="onMouseDown"
        @keydown="onKeyDown"
    >
        <label
            class="checkbox-wrapper"
            @click="onClickWrapper"
        >
            <input
                type="checkbox"
                v-model="isChecked"
                ref="input"
            >
            <span class="checkmark"></span>
        </label>
        <label :class="{ 'checkbox-item__label': true, 'checkbox--selected': isChecked }">
            {{ name }}
        </label>
    </div>
</template>
<script>
export default {
    name: 'MISACheckboxItem',
    props: {
        /**
         * (Props) Array to bind with this checkbox, if this checkbox checked, array will push, otherwise will remove
         */
        model: {
            type: Array,
            default: () => []
        },
        /**
         * (Props) Id of object to identify object
         */
        id: {
            type: [String, Number]
        },
        /**
         * (Props) Name of item
         */
        name: {
            type: [String, Number]
        },
        /**
         * (Props) Bind checked state
         */
        checked: {
            type: Boolean,
        }
    },
    data() {
        return {
            isChecked: this.checked
        }
    },
    watch: {
        checked: function () {
            this.isChecked = this.checked
        },
        isChecked: function () {
            this.$emit('update:checked', this.isChecked)
        }
    },
    computed: {
        /**
         * Check model value includes id or not
         * 
         * @return True if includes
         */
        isIncluded() {
            return this.model.includes(this.id);
        }
    },
    methods: {
        /**
         * Handle click checkbox item
         * 
         * Author: nlnhat (05/07/2023)
         */
        onClickItem() {
            this.isChecked = !this.isChecked;
            if (this.$refs.input)
                this.$refs.input.focus();
        },
        /**
         * Handle click checkbox wrapper
         * 
         * Author: nlnhat (05/07/2023)
         * @param {*} event Click event
         */
        onClickWrapper(event) {
            event.stopPropagation();
        },
        /**
         * Handle mouse down checkbox item
         * 
         * Author: nlnhat (05/07/2023)
         * @param {*} event Mouse down event
         */
        onMouseDown(event) {
            event.stopPropagation();
        },
        /**
         * Handle keydown on checkbox item
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
};
</script>
<style scoped>.checkbox-wrapper {
    height: fit-content;
    width: fit-content;
    margin: 12px;
    display: flex;
}</style>