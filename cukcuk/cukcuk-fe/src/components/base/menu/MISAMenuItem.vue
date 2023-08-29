<template>
    <component
        :is="target ? 'router-link':'div'"
        :to="target"
        class="menu-item menu-item--btn"
    >
        <div class="menu-item__icon">
            <m-icon :icon="icon"></m-icon>
            <span>
                <span
                    class="tooltip-wrapper"
                    v-show="title"
                >
                    <span
                        v-tooltip:right="title"
                        style="position: absolute; left: 12px;"
                    ></span>
                </span>
            </span>
        </div>
        <div class="menu-item__text">{{ title }}</div>
    </component>
</template>
<script>
export default {
    name: 'MISAMenuItem',
    props: {
        /**
         * Target route
         */
        target: {
            type: String,
            default: null,
        },
        /**
         * Icon name
         */
        icon: {
            type: String
        },
        /**
         * Title of menu
         */
        title: {
            type: String
        }
    },
}
</script>
<style>
.sidebar--collapse .menu-item:hover [tooltip]::before,
.sidebar--collapse .menu-item:hover [tooltip]::after {
    display: block;
    opacity: 1;
    transition: opacity 0.6s;
}
</style>