<template>
    <m-popup
        :title="title"
        id="material-stat"
        class="popup--md"
        @emitClose="onClickCloseForm()"
        tabindex="0"
    >
        <template #mask>
            <m-loading v-if="isLoading"></m-loading>
        </template>
        <template #body>
            <div class="chart-container">
                <m-chart-section
                    :title="`${this.$resources['vn'].rate} ${this.$resources['vn'].conversionUnit}`"
                    id="chart--conversion-units-rate"
                >
                    <Bar
                        :data="data"
                        :options="options"
                    />
                </m-chart-section>
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
import {
    Chart as ChartJS,
    Title,
    Tooltip,
    Legend,
    BarElement,
    CategoryScale,
    LinearScale
} from 'chart.js'
import { Bar } from 'vue-chartjs'
ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend)

export default {
    name: "MaterialStat",
    components: {
        Bar
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
         * Data from parent component
         */
        dataModel: {
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
             * Data show on bar char
             */
            data: {
                ...this.dataModel,
            },
            /**
             * Option bar char
             */
            options: {
                responsive: true
            }
        }
    },
    created() {

    },
    mounted() {
        this.focus();
    },
    emits: [

    ],
    watch: {

    },
    computed: {

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
    },
};
</script>
<style>
#material-stat .popup__body {
    height: 360px;
    padding: 20px 36px;
}
#material-stat .popup {
    width: 680px;
}
#chart--conversion-units-rate {
    width: 100%;
}
</style>
