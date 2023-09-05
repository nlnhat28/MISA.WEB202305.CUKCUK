<template>
    <tr
        tabindex="0"
        :class="{ 'tr--selected': isSelected, 'tr--dark': index % 2 != 0 && applyDark, 'tr--focused': focusedId == id }"
        @keydown="onKeyDown"
        @dblclick="onUpdate"
        @focus="this.$emit('update:focusedId', this.id);"
        ref="tr"
    >
        <slot name="content"></slot>
    </tr>
</template>
<script>
export default {
    name: 'MISATr',
    props: {
        /**
         * This record in row is selected or not
         */
        isSelected: {
            type: Boolean,
            default: false,
        },
        /**
         * Index of row
         */
        index: {
            type: Number,
        },
        /**
         * Id of row
         */
        id: {
            type: [String, Number]
        },
        /**
         * Focused id
         */
        focusedId: {
            type: String,
            default: null,
        },
        /**
         * Apply dark
         */
        applyDark: {
            type: Boolean,
            default: true,
        },
        /**
         * Apply focus
         */
        applyFocus: {
            type: Boolean,
            default: true,
        }
    },
    data() {
        return {
            /**
             * Focused state
             */
            isFocused: false,
        }
    },
    mounted() {
        this.focusById();
    },
    expose: ['focus', 'index', 'id', 'vanish', 'setFocus', 'clearFocus'],
    watch: {
        // focusedId() {
        //     this.focusById();
        // }
    },
    methods: {
        /**
         * Handle short key
         * 
         * Author: nlnhat (29/07/2023)
         * @param {*} event Keydown event on tr
         */
        onKeyDown(event) {
            const code = this.$enums.keyCode;
            // Space || F2: Update
            if (event.keyCode == code.space || event.keyCode == code.f2) {
                event.preventDefault();
                event.stopPropagation();
                this.onUpdate();
            }
            // Ctrl + D || Delete: Delete
            else if ((event.ctrlKey && event.keyCode == code.d) || event.keyCode == code.delete) {
                event.preventDefault();
                event.stopPropagation();
                this.onDelete();
            }
            // Ctrl + V: Duplicate
            else if (event.ctrlKey && event.keyCode == code.v) {
                event.preventDefault();
                event.stopPropagation();
                this.onDuplicate();
            }
            // Arrow down: Focus on next row
            else if (event.keyCode == code.down) {
                event.preventDefault();
                event.stopPropagation();
                this.focusNext();
            }
            // Arrow up: Focus on previous row
            else if (event.keyCode == code.up) {
                event.preventDefault();
                event.stopPropagation();
                this.focusPrevious();
            }
        },
        /**
         * Update record
         * 
         * Author: nlnhat (29/07/2023)
         */
        onUpdate() {
            this.$emit('emitUpdate');
        },
        /**
         * Delete record
         * 
         * Author: nlnhat (29/07/2023)
         */
        onDelete() {
            this.$emit('emitDelete');
        },
        /**
         * Duplicate record
         * 
         * Author: nlnhat (29/07/2023)
         */
        onDuplicate() {
            this.$emit('emitDuplicate');
        },
        /**
         * Duplicate record
         * 
         * Author: nlnhat (29/07/2023)
         */
        focusNext() {
            this.$emit('emitFocusNext', this.index);
        },
        /**
         * Duplicate record
         * 
         * Author: nlnhat (29/07/2023)
         */
        focusPrevious() {
            this.$emit('emitFocusPrevious', this.index);
        },
        /**
         * Focus on tr
         * 
         * Author: nlnhat (29/07/2023)
         */
        focus() {
            this.$nextTick(() => {
                if (this.$refs.tr)
                    this.$refs.tr.focus();
            });
        },
        /**
         * Focus on tr if id equals focus id
         * 
         * Author: nlnhat (29/07/2023)
         */
        focusById() {
            if (this.focusedId == this.id) {
                this.focus();
                // this.$emit('update:focusedId', null);
            }
        },
        /**
         * Make vanish effect when delete 
         * 
         * Author: nlnhat (09/08/2023)
         */
        vanish() {
            this.$nextTick(() => {
                if (this.$refs.tr)
                    this.$refs.tr.style.transition = 'opacity 400ms';
                this.$refs.tr.style.opacity = '0';
            });
        },
        /**
         * Set class focus
         * 
         * Author: nlnhat (09/08/2023)
         */
        setFocus() {
            this.isFocused = true;
        },
        /**
         * Clear class focus
         * 
         * Author: nlnhat (09/08/2023)
         */
        clearFocus() {
            this.isFocused = false;
        }
    }

}
</script>