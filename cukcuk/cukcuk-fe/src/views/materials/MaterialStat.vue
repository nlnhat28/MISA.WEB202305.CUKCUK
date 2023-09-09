<template>
    <m-popup
        :title="title"
        id="converison-unit-detail"
        class="popup--md"
        @emitClose="onClickCloseForm()"
        @keydown="handleShortKey"
        tabindex="0"
    >
        <template #mask>
            <m-loading v-if="isLoading"></m-loading>
        </template>
        <template #body>
            <div class="chart-container">
                <div class="chart-container--row">
                    <!-- Số lượng theo kho -->
                    <m-chart-section
                        v-if="dataCountByWarehouseComputed && !isLoading"
                        :title="this.$resources['vn'].countByWarehouse"
                        id="chart--count-by-warehouse"
                    >
                        <Bar
                            :data="dataCountByWarehouseComputed"
                            :options="optionsCountByWarehouse"
                        />
                    </m-chart-section>
                    <!-- Tỷ lệ theo dõi -->
                    <m-chart-section
                        v-if="dataCountByFollowComputed && !isLoading"
                        :title="this.$resources['vn'].followRate"
                        id="chart--count-by-follow"
                    >
                        <Doughnut
                            :data="dataCountByFollowComputed"
                            :options="optionsCountByFollow"
                        />
                    </m-chart-section>
                </div>
                <div class="chart-container--row">
                    <!-- Số lượng theo năm -->
                    <m-chart-section
                        v-if="dataCountByYearComputed && !isLoading"
                        :title="this.$resources['vn'].countByYear"
                        id="chart--count-by-year"
                    >
                        <Line
                            :data="dataCountByYearComputed"
                            :options="optionsCountByYear"
                        />
                    </m-chart-section>
                    <!-- Tỷ lệ lọc -->
                    <m-chart-section
                        v-if="dataFilterRateComputed && !isLoading"
                        :title="this.$resources['vn'].filterRate"
                        id="chart--filter-rate"
                    >
                        <Pie
                            :data="dataFilterRateComputed"
                            :options="optionsFilterRate"
                        />
                    </m-chart-section>
                </div>
            </div>
        </template>
        <!-- footer's form -->
        <template #footer>
            <div class="button-container">
                <m-button
                    :type="this.$enums.buttonType.primary"
                    :text="this.$resources['vn'].close"
                    :click="onClickCloseForm"
                    iconLeft="cukcuk-cancel"
                    title="Esc"
                    ref="buttonFocus"
                ></m-button>
            </div>
        </template>
    </m-popup>
</template>

<script>
import { openUrl } from "@/js/utils/window.js";
import { percentageFormatter, gradientBackground } from "@/js/utils/chart.js";
import { materialService } from '@/services/services.js';
import { generateColorsByNumbers } from '@/js/utils/color.js';
import {
    Chart,
    Title,
    Tooltip,
    Legend,
    BarElement,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Filler,
    ArcElement
} from 'chart.js'
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Bar, Line, Doughnut, Pie } from 'vue-chartjs'

Chart.register(CategoryScale, LinearScale, LineElement, BarElement, PointElement, ArcElement,
    ChartDataLabels, Filler, Title, Tooltip, Legend)

Chart.defaults.set('plugins.datalabels', {
    labels: {
        title: null
    }
});
Chart.defaults.font.family = 'tahoma';
Chart.defaults.color = '#000';


export default {
    name: "MaterialStat",
    components: {
        Bar,
        Line,
        Doughnut,
        Pie,
    },
    props: {
        /**
         * Function to hide this form
         */
        closeThis: {
            type: Function,
        },
        /**
         * Title of window
         */
        title: {
            type: String,
        },
        /**
         * Filter count
         */
        filterCount: {
            type: Object
        }
    },
    data() {
        return {
            /**
             * Loading effect
             */
            isLoading: false,
            /**
             * Data count by warehouse chart
             */
            dataCountByWarehouse: null,
            /**
             * Data count by folow chart
             */
            dataCountByFollow: null,
            /**
             * Data count by year chart
             */
            dataCountByYear: null,
            /**
             * Data filter count
             */
            dataFilterCount: null,
        }
    },
    async created() {
        await this.makeLoadingEffect(async () => {
            await this.getData();
        });
    },
    mounted() {
        this.focus();
    },
    emits: [

    ],
    watch: {

    },
    computed: {
        /**
         * Data count by warehouse
         * 
         * Author: nlnhat (08/09/2023)
         */
        dataCountByWarehouseComputed() {
            if (this.dataCountByWarehouse) {
                const warehouses = this.dataCountByWarehouse.map(item => item.WarehouseName);
                const counts = this.dataCountByWarehouse.map(item => item.Count);
                const data = {
                    labels: [
                        ...warehouses,
                    ],
                    datasets: [
                        {
                            label: this.$resources['vn'].amount,
                            backgroundColor: this.generateColorsByNumbers(this.$enums.color.primary.hue, 90, 20, counts),
                            data: [
                                ...counts
                            ]
                        },
                    ]
                };
                return data;
            };
        },
        /**
         * Data count by follow
         * 
         * Author: nlnhat (08/09/2023)
         */
        dataCountByFollowComputed() {
            if (this.dataCountByFollow) {
                const data = {
                    labels: [
                        this.$resources['vn'].following,
                        this.$resources['vn'].unfollow,
                    ],
                    datasets: [
                        {
                            label: this.$resources['vn'].amount,
                            backgroundColor: [
                                this.$enums.color.primary.hex,
                                this.$enums.color.secondary.hex,
                                '#ccc',
                            ],
                            hoverOffset: 4,
                            data: [
                                this.dataCountByFollow.FollowCount,
                                this.dataCountByFollow.UnfollowCount,
                            ]
                        },
                    ]
                };
                return data;
            };
        },
        /**
         * Data count by year
         * 
         * Author: nlnhat (08/09/2023)
         */
        dataCountByYearComputed() {
            if (this.dataCountByYear) {

                const years = this.dataCountByYear.map(item => item.Year);
                const createCounts = this.dataCountByYear.map(item => item.CreateCount);
                const deleteCounts = this.dataCountByYear.map(item => item.DeleteCount);
                const data = {
                    labels: [
                        ...years,
                    ],
                    datasets: [
                        {
                            label: this.$resources['vn'].createNew,
                            borderColor: this.$enums.color.primary.hex,
                            pointBackgroundColor: this.$enums.color.primary.hex,
                            lineTension: 0.4,
                            borderWidth: 1,
                            data: [
                                ...createCounts
                            ],
                            fill: true,
                            backgroundColor: (context) => this.gradientBackground(context, [
                                {
                                    offset: 1,
                                    color: "rgba(1, 153, 251, 0.9)",
                                },
                                {
                                    offset: 0.5,
                                    color: "rgba(1, 153, 251, 0.4)",
                                },
                                {
                                    offset: 0,
                                    color: "rgba(1, 153, 251, 0)",
                                },
                            ]),
                        },
                        {
                            label: this.$resources['vn'].deleted,
                            borderColor: this.$enums.color.danger.hex,
                            pointBackgroundColor: this.$enums.color.danger.hex,
                            lineTension: 0.4,
                            borderWidth: 1,
                            data: [
                                ...deleteCounts
                            ],
                            fill: true,
                            backgroundColor: (context) => this.gradientBackground(context, [
                                {
                                    offset: 1,
                                    color: "rgba(255, 0, 0, 0.9)",
                                },
                                {
                                    offset: 0.5,
                                    color: "rgba(255, 0, 0, 0.4)",
                                },
                                {
                                    offset: 0,
                                    color: "rgba(255, 0, 0, 0)",
                                },
                            ]),
                        },
                    ]
                };
                return data;
            };
        },
        /**
         * Data count by filter
         * 
         * Author: nlnhat (08/09/2023)
         */
        dataFilterRateComputed() {
            if (this.dataFilterCount) {
                const data = {
                    labels: [
                        this.$resources['vn'].filtered,
                        this.$resources['vn'].remain,
                    ],
                    datasets: [
                        {
                            label: this.$resources['vn'].amount,
                            backgroundColor: [
                                this.$enums.color.primary.hex,
                                this.$enums.color.secondary.hex,
                            ],
                            hoverOffset: 4,
                            data: [
                                this.filterCount.totalRecord,
                                this.filterCount.allRecord - this.filterCount.totalRecord
                            ]
                        },
                    ]
                };
                return data;
            };
        },
        /**
         * Option count by warehouse chart
         */
        optionsCountByWarehouse() {
            return {
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: 'y',
                plugins: {
                    legend: {
                        labels: {
                            generateLabels(chart) {
                                const data = chart.data;
                                if (data.datasets) {
                                    let numbers = data.datasets[0].data;
                                    let colors = data.datasets[0].backgroundColor;

                                    numbers = Array.from(new Set(numbers));
                                    colors = Array.from(new Set(colors));

                                    let newData = numbers.map((number, index) => ({
                                        number,
                                        color: colors[index]
                                    }));
                                    newData = newData.filter(item => item.number > 0)

                                    let labels = [];
                                    labels = newData.map(item => ({
                                        text: item.number,
                                        fillStyle: item.color,
                                        lineWidth: 0
                                    }));

                                    return labels
                                }
                                return [];
                            },
                        },
                    }
                }
            }
        },
        /**
         * Option count by follow chart
         */
        optionsCountByFollow() {
            return {
                responsive: true,
                cutout: '60%',
                plugins: {
                    datalabels: {
                        formatter: (value, ctx) => this.percentageFormatter(value, ctx),
                        labels: {
                            title: {
                                font: {
                                    size: 14,
                                },
                                color: [
                                    '#fff',
                                    '#444'
                                ],
                            },
                        }
                    },
                }
            }
        },
        /**
         * Option count by year chart
         */
        optionsCountByYear() {
            return {
                responsive: true,
                maintainAspectRatio: false,
            }
        },
        /**
         * Option filter rate chart
         */
        optionsFilterRate() {
            return {
                responsive: true,
                plugins: {
                    datalabels: {
                        formatter: (value, ctx) => this.percentageFormatter(value, ctx),
                        labels: {
                            title: {
                                font: {
                                    size: 14,
                                },
                                color: [
                                    '#fff',
                                    '#444'
                                ],
                            },
                        },
                    },
                }
            }
        },
    },
    methods: {
        /**
         * Make loading effect
         *
         * Author: nlnhat (05/07/2023)
         * @param {function} func Function executes in loading process
         */
        async makeLoadingEffect(func) {
            try {
                this.isLoading = true;
                await func();
            } catch (error) {
                console.error(error);
            } finally {
                this.isLoading = false;
            }
        },
        /**
         * Lấy toàn bộ dữ liệu thống kê
         * 
         * Author: nlnhat (09/09/2023)
         */
        async getData() {
            await Promise.all([
                this.getCountByYear(),
                this.getCountByWarehouse(),
                this.getCountByFollow(),
            ]);
            this.dataFilterCount = this.filterCount;
        },
        /**
         * Lấy số lượng thêm mới theo năm
         * 
         * Author: nlnhat (08/09/2023)
         */
        async getCountByYear() {
            const response = await materialService.countByYear();
            if (response?.status == this.$enums.status.ok) {
                this.dataCountByYear = response.data;
            }
        },
        /**
         * Lấy số lượng thêm mới theo kho
         * 
         * Author: nlnhat (08/09/2023)
         */
        async getCountByWarehouse() {
            const response = await materialService.countByWarehouse();
            if (response?.status == this.$enums.status.ok) {
                this.dataCountByWarehouse = response.data;
            }
        },
        /**
         * Lấy số lượng theo trạng thái follow
         * 
         * Author: nlnhat (08/09/2023)
         */
        async getCountByFollow() {
            const response = await materialService.countByFollow();
            if (response?.status == this.$enums.status.ok) {
                this.dataCountByFollow = response.data;
            }
        },
        /**
         * Show toast message after created, updated, duplicated success
         *
         * Author: nlnhat (02/07/2023)
         */
        showToastSuccess() {
            this.$emitter.emit("showToastSuccess", this.messageOnToast);
        },
        /**
         * Handle click close this form
         *
         * Author: nlnhat (26/06/2023)
         */
        onClickCloseForm() {
            this.closeThis();
        },
        /**
         * Handle shortcut keys
         *
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            const code = this.$enums.keyCode;

            // Esc: Đóng form
            if (event.keyCode == code.esc) {
                event.preventDefault();
                event.stopPropagation();
                this.onClickCloseForm();
            }
        },
        /**
         * Focus button
         * 
         * Author: nlnhat (08/09/2023)
         */
        focus() {
            if (this.$refs.buttonFocus) {
                this.$refs.buttonFocus.focus();
            }
        },
        /**
         * Utils
         */
        openUrl,
        generateColorsByNumbers,
        percentageFormatter,
        gradientBackground,
    },
};
</script>
<style>
#converison-unit-detail .popup__body {
    height: 800px;
    padding: 20px 36px;
    overflow: auto;
}

#converison-unit-detail .popup {
    width: 1400px;
}

#chart--count-by-warehouse {
    width: 800px;
}

#chart--count-by-follow {
    width: 320px;
}

#chart--count-by-year {
    width: 800px;
}

#chart--filter-rate {
    width: 320px;
}
</style>
