<template>
    <th
        :style="`text-align: ${textAlign}; width: ${width}px; min-width: ${width}px;`"
        ref="th"
    >
        <div class="th-wrapper">
            <div class="th-wrapper__content">
                <div
                    class="p-relative"
                    style="width: fit-content;"
                >
                    <!-- default slot content -->
                    <span v-tooltip:bottom="fullTitle">
                        <slot></slot>

                    </span>
                    <!-- sort -->
                    <m-icon-container
                        v-if="sortType != this.$enums.sortType.disabled"
                        :icon="sortIcon"
                        :isBtn="true"
                        class="th__sort"
                        @click="onClickSortType()"
                        :title="this.$resources['vn'].sort"
                    >
                    </m-icon-container>
                </div>
            </div>
            <div
                class="th-wrapper__filter"
                v-if="filterConfig"
            >
                <m-filter
                    :filterConfig="filterConfig"
                    :filterModel="filterModel"
                    :name="name"
                    :title="fullTitle ?? title"
                    @emitUpdateFilterModel="updateFilterModel"
                    ref="filter"
                >
                </m-filter>
            </div>
        </div>
        <span
            class="th__resize"
            @mousedown="startResize"
        ></span>
    </th>
</template>
<script>
import enums from '@/constants/enums.js'
const sortType = enums.sortType;

export default {
    name: 'MISATh',
    props: {
        /**
         * Width of cell
         */
        widthCell: {
            type: [String, Number],
            default: 160,
        },
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
         * Name of column
         */
        name: {
            type: String
        },
        /**
         * Title of column
         */
        title: {
            type: String
        },
        /**
         * Tooltip string
         */
        fullTitle: {
            type: String,
            default: null
        },
        /**
         * Kiểu sắp xếp
         */
        sortType: {
            type: Number,
            default: null,
            validator: (value) => {
                return [
                    sortType.disabled,
                    sortType.asc,
                    sortType.desc,
                    sortType.blur,
                ].includes(value);
            },
        },
        /**
         * (Prop) Sort object
         */
        sortModel: {
            type: Object
        },
        /**
         * Filter config
         */
        filterConfig: {
            type: Object,
            default: null,
        },
        /**
         * Filter model output
         */
        filterModel: {
            type: Object
        },
        /**
         * Resizable or not
         */
        isResizable: {
            type: Boolean,
            default: true,
        }
    },
    data() {
        return {
            /**
             * Width of cell
             */
            width: parseInt(this.widthCell),
            /**
             * Is resizing or not
             */
            isResizing: false,
            /**
             * Position X before resize
             */
            startX: 0,
            /**
             * Sort state
             */
            sort: this.sortType,
            /**
             * Sort icon
             */
            sortIcon: 'arrow-blur',
            /**
             * Filter data
             */
            filterData: null,
            /**
             * Show filter or not
             */
            isShowFilter: false,
        }
    },
    watch: {
        /**
         * Watch sort data
         */
        sort() {
            this.sortIcon = this.sortIconComputed;
            this.$emit('update:sortModel', this.sortModelComputed)
        },
        /**
         * Watch fitlter item to update filter data
         */
        filterModel: {
            handler() {
                this.filterData = this.filterModel;
            },
            deep: true
        }
    },
    computed: {
        /**
         * Đổi icon sort
         * 
         * Author: nlnhat (20/07/2023)
         */
        sortIconComputed() {
            switch (this.sort) {
                case this.$enums.sortType.asc:
                    return 'arrow-up'
                case this.$enums.sortType.desc:
                    return 'arrow-down'
                default:
                    return 'arrow-blur';
            }
        },
        /**
         * Trả về thuộc tính sắp xếp
         * 
         * Author: nlnhat (20/07/2023)
         */
        sortModelComputed() {
            if (this.sort == this.$enums.sortType.asc || this.sort == this.$enums.sortType.desc)
                return {
                    sortType: this.sort,
                    columnName: this.name
                };
            else return null;
        },
        /**
         * Show filter textfield or not
         * 
         * Author: nlnhat (20/07/2023)
         */
        isShowTextField() {
            return (
                this.filterConfig.filterType == this.$enums.filterType.text ||
                this.filterConfig.filterType == this.$enums.filterType.date)
        },
    },
    methods: {
        /**
         * Start resize when mouse down
         * 
         * Author: nlnhat (13/07/2023)
         * @param {*} event Mouse event
         */
        startResize(event) {
            this.isResizing = true;
            this.startX = event.clientX;
            document.addEventListener('mousemove', this.resize);
            document.addEventListener('mouseup', this.endResize);
        },
        /**
         * Resize when mouse move
         * 
         * Author: nlnhat (13/07/2023)
         * @param {*} event Mouse event
         */
        resize(event) {
            if (this.isResizing) {
                const deltaX = event.clientX - this.startX;
                this.width = Math.max(100, this.width + deltaX);
                this.startX = event.clientX;
            }
        },
        /**
         * End resize when mouse up
         * 
         * Author: nlnhat (13/07/2023)
         * @param {*} event Mouse event
         */
        endResize() {
            this.isResizing = false;
            document.removeEventListener('mousemove', this.resize);
            document.removeEventListener('mouseup', this.endResize);
        },
        /**
         * Return width of th 
         */
        getWidth() {
            return this.$refs.th.offsetWidth;
        },
        /**
         * Change sort type
         * 
         * Author: nlnhat (20/07/2023)
         */
        onClickSortType() {
            try {
                switch (this.sort) {
                    case this.$enums.sortType.blur:
                        this.sort = this.$enums.sortType.asc
                        break;
                    case this.$enums.sortType.asc:
                        this.sort = this.$enums.sortType.desc
                        break;
                    case this.$enums.sortType.desc:
                        this.sort = this.$enums.sortType.blur
                        break;
                    default:
                        break;
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Toggle show or hide filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        toggleDisplayFilter() {
            this.isShowFilter = !this.isShowFilter;
            if (this.isShowFilter) {
                this.$nextTick(() => {
                    this.$refs.filter.focus();
                })
            }
        },
        /**
         * Show filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        showFilter() {
            this.isShowFilter = true;
        },
        /**
         * Hide filter
         * 
         * Author: nlnhat (25/07/2023)
         */
        hideFilter() {
            this.isShowFilter = false;
        },
        /**
         * Hide filter
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} filterData Filter data from filter component
         */
        updateFilterModel(filterData) {
            this.$emit('update:filterModel', filterData);
        }
    },
}
</script>
<style scoped>
.icon-container {
    display: inline-flex;
}
</style>