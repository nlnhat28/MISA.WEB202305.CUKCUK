<template>
    <div class="d-flex justify-content--center">
        <label :class="{'checkbox-wrapper': true, 'disabled': isReadOnly}">
            <input
                type="checkbox"
                :checked="isChecked"
                @change="onChange()"
                
            >
            <span class="checkmark"></span>
        </label>
    </div>
</template>

<script>
export default {
    name: 'MISACheckbox',
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
         * (Props) Bind checked state
         */
        checked: {
            type: Boolean,
        },
        /**
         * Read only
         */
        isReadOnly: {
            type: Boolean,
            default: false
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
         * Handle check change
         * 
         * Author: nlnhat (05/07/2023)
         */
        onChange() {
            if (this.isIncluded) {
                this.$emit('update:model', this.model.filter((item) => item !== this.id));
            } else {
                this.$emit('update:model', [...this.model, this.id]);
            }
        }
    }
};
</script>