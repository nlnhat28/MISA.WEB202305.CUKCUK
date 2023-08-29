<template>
    <td
        :style="`text-align: ${textAlign};`"
        :class="{ 'td--resizable': content }"
        ref="td"
        @mouseover="onMouseOver"
    >
        <!-- default slot content -->
        <slot></slot>
        <span
            class="tooltip-wrapper"
            v-show="isShowTooltip && content"
            :style="`width: ${widthTooltip}px`"
        >
            <span
                v-tooltip="content"
                style="position: absolute; left: 50%"
            ></span>
        </span>
        {{ content }}
    </td>
</template>
<script>
export default {
    name: 'MISATd',
    props: {
        /**
         * Text align of cell
         */
        textAlign: {
            type: String,
            default: 'left',
            validator: (value) => {
                return [
                    'left',
                    'center',
                    'right',
                ].includes(value);
            },
        },
        /**
         * Content of cell
         */
        content: {
            type: [String, Number],
            default: null
        }
    },
    data() {
        return {
            /**
             * Show tooltip or not
             */
            isShowTooltip: false,
            /**
             * Width of tooltip
             */
            widthTooltip: 0,
        }
    },
    methods: {
        /**
         * Handle mouse over on td
         * 
         * Author: nlnhat (09/08/2023)
         */
        onMouseOver() {
            const refTd = this.$refs.td;
            if (refTd) {
                this.widthTooltip = refTd.offsetWidth - 24;
                this.isShowTooltip = refTd.clientWidth < refTd.scrollWidth
            }
        }
    }
}
</script>
<style scoped>
.table td:hover [tooltip]::before,
.table td:hover [tooltip]::after {
    display: block;
    opacity: 1;
    transition: opacity 0.6s;
}
</style>
