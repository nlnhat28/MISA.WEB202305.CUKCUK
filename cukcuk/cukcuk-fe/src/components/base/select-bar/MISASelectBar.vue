<template>
    <div
        class="select-bar"
        v-show="selectLength > 0"
    >
        <div class="selected-count">
            {{ this.$resources['vn'].selectedCount }}:
            <span class="selected-count__count">
                {{ selectLength }}
            </span>
        </div>
        <m-button
            :type="this.$enums.buttonType.linkDanger"
            :text="this.$resources['vn'].unSelect"
            :click="onClickUnselect"
        >
        </m-button>
        <m-button
            :type="this.$enums.buttonType.danger"
            :tooltipContent="`${this.$resources['vn'].delete} ${selectLength} ${recordName} ${this.$resources['vn'].selected}`"
            :click="onClickDelete"
            :text="this.$resources['vn'].delete"
            iconLeft="trash--light"
        >
        </m-button>
    </div>
</template>
<script>
export default {
    name: 'MISASelectBar',
    props: {
        /**
         * Number of selected items
         */
        selectLength: {
            type: Number,
        },
        /**
         * Name of records
         */
        recordName: {
            type: String
        }
    },
    methods: {
        /**
         * Unselect records
         * 
         * Author: nlnhat (07/07/2023)
         */
        onClickUnselect() {
            this.$emit('emitUnselect')
        },
        /**
         * Delete selected records
         * 
         * Author: nlnhat (07/07/2023)
         */
        onClickDelete() {
            this.$emit('emitDelete')
        }
    }
}
</script>