<template>
    <div class="chart-section">
        <div class="chart-section__title">
            {{ title }}
        </div>
        <div class="chart-section__chart">
            <!-- Chart slot -->
            <slot></slot>
        </div>
    </div>
</template>
<script>
export default {
    name: 'MISAChartSection',
    props: {
        /**
         * Tiêu đề
         */
        title: {
            type: String,
        },
    }
}
</script>