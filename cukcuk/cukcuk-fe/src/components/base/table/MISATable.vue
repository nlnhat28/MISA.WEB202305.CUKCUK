<template>
    <div class="table-container">
        <div class="table-container__top d-flex--column flex-1">
            <!-- table toolbar -->
            <div class="table-toolbar">
                <div class="table-toolbar__left">
                    <slot name="toolbarLeft"></slot>
                </div>
                <div class="table-toolbar__right">
                    <slot name="toolbarRight"></slot>
                </div>
            </div>
            <!-- table content -->
            <div
                class="table-content"
                @scroll=showHideTheadShadow()
                ref="tableContent"
            >
                <table :class="{ 'table': true, '.disabled': isLoading }">
                    <thead
                        class="table__head"
                        ref="tableHead"
                    >
                        <slot name="thead"></slot>
                    </thead>
                    <tbody class="table__body">
                        <slot name="tbody"></slot>
                    </tbody>
                </table>
                <m-loading v-if="isLoading"></m-loading>
                <m-no-content v-if="(!isLoading && totalRecord == 0)"></m-no-content>
            </div>
        </div>
        <!-- table footer -->
        <div class="table__footer">
            <slot name="tfooter"></slot>
        </div>
    </div>
</template>
<script>
export default {
    name: 'MISATable',
    props: {
        /**
         * List of records
         */
        records: {
            type: Array
        },
        /**
         * List of selected records
         */
        recordsSelect: {
            type: Array
        },
        /**
         * Total record in database
         */
        totalRecord: {
            type: Number
        },
        /**
         * Loading state
         */
        isLoading: {
            type: Boolean
        }
    },
    methods: {
        /**
         * Remove thead shadow when scroll at top
         *
         * Author: nlnhat (26/06/2023)
         */
        showHideTheadShadow() {
            try {
                if (this.$refs.tableContent.scrollTop === 0) {
                    this.$refs.tableHead.style.boxShadow = "none";
                } else {
                    this.$refs.tableHead.style.boxShadow = "0px 2px 10px rgba(23, 27, 42, 0.2)";
                }
            } catch { }
        }
    }
}
</script>