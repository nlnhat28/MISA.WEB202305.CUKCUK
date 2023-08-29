<template>
    <div class="textfield">
        <input
            type="text"
            ref="input"
            :placeholder="placeholder"
            v-model="this.inputValue"
            @keydown="onKeyDown"
        />
    </div>
</template>
<script>
export default {
    name: 'MISAPageNumber',
    props: {
        pageNumber: {
            /**
             * Page number 
             */
            number: {
                type: Number,
            },
            /**
             * Is active or not
             */
            isActive: {
                type: Boolean,
                default: false
            }
        }
    },
    methods: {
        /**
         * Handle click this page
         * 
         * Author: nlnhat (22/06/2023)
         */
        onActive() {
            this.$emit('emitPageActive')
        }
    }
}
</script>