<template>
    <div :class="{ 'button-wrapper': true, 'disabled': isDisabled }">
        <button
            :class="[
                'btn',
                { 'btn--icon-left': iconLeft },
                { 'btn--icon': iconCenter },
                { 'btn--icon-right': iconRight },
                tooltip,
                getTypeComputed]"
            v-tooltip="tooltipContent"
            ref="button"
            @click="onClick()"
            @keydown="onKeyDown"
            :tabindex="isDisabled ? '-1' : ''"
        >
            <div
                class="icon-container"
                v-if="iconLeft || iconCenter"
            >
                <m-icon
                    :icon="iconLeft"
                    v-if="iconLeft && !isLoading && !isSuccess"
                ></m-icon>
                <m-icon
                    :icon="iconCenter"
                    v-if="iconCenter && !isLoading && !isSuccess"
                ></m-icon>
                <m-icon
                    icon="check--blue"
                    v-if="isSuccess"
                ></m-icon>
                <m-spinner v-if="isLoading && !isSuccess"></m-spinner>
            </div>
            {{ text }}
            <div
                class="icon-container"
                v-if="iconRight"
            >
                <m-icon
                    :icon="iconRight"
                    v-if="iconRight"
                ></m-icon>
            </div>
        </button>
    </div>
</template>
<script>
import enums from '@/constants/enums.js';
const buttonType = enums.buttonType;

export default {
    name: 'MISAButton',
    props: {
        /**
         * Type of button
         */
        type: {
            type: Number,
            default: buttonType.primary,
            validator: (value) => {
                return [
                    buttonType.primary,
                    buttonType.secondary,
                    buttonType.danger,
                    buttonType.link,
                    buttonType.linkDanger,
                    buttonType.linkIcon,
                ].includes(value);
            },
        },
        /**
         * Text in button
         */
        text: {
            type: [String, Number],
            default: '',
        },
        /**
         * Content of tooltip
         */
        tooltipContent: {
            type: String,
            default: null
        },
        /**
         * Position of tooltip
         */
        tooltipPos: {
            type: String,
            validator: (value) => {
                return [
                    'top-left',
                    'top-center',
                    'top-right',
                ].includes(value)
            }
        },
        /**
         * Name of icon left
         */
        iconLeft: {
            type: String
        },
        /**
         * Name of icon main
         */
        iconCenter: {
            type: String
        },
        /**
         * Name of icon right
         */
        iconRight: {
            type: String
        },
        /**
         * Action when click
         */
        click: {
            type: Function
        },
        /**
         * Has loading effect or not
         */
        hasLoading: {
            type: Boolean,
            default: false,
        },
        /**
         * Disabled or not
         */
        isDisabled: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            /**
             * Tooltip
             */
            tooltip: null,
            /**
             * Trạng thái thực hiện hành động
             */
            isLoading: {
                type: Boolean,
                default: false
            },
            /**
             * Show icon after success action
             */
            isSuccess: false,
        }
    },
    expose: ['focus'],
    created() {
        if (this.tooltipPos != null) this.tooltip = `tooltip--${this.tooltipPos}`;
        this.isLoading = false;
        this.isSuccess = false;
    },
    watch: {
        isDisabled() {
            if (this.isDisabled)
                this.blur();
        },
    },
    computed: {
        getTypeComputed() {
            switch (this.type) {
                case buttonType.primary:
                    return 'btn--primary';
                case buttonType.secondary:
                    return 'btn--secondary';
                case buttonType.danger:
                    return 'btn--danger';
                case buttonType.link:
                    return 'btn--link';
                case buttonType.linkIcon:
                    return 'btn--link-icon';
                case buttonType.linkDanger:
                    return 'btn--link btn--link--danger';
                default:
                    return 'btn--primary';
            }
        }
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
                if (await func()) {
                    this.isSuccess = true;
                    await new Promise((resolve) => setTimeout(resolve, 2000));
                    this.isSuccess = false;
                }
            } catch (error) {
                console.error(error);
            } finally {
                this.isLoading = false;
            }
        },
        /**
         * On click
         * 
         * Author: nlnhat (05/07/2023)
         */
        onClick() {
            if (this.hasLoading)
                this.makeLoadingEffect(this.click);
            else this.click();
        },
        /**
         * Focus on button
         * 
         * Author: nlnhat (05/07/2023)
         */
        focus() {
            this.$refs.button.focus();
        },
        /**
         * Blur button
         * 
         * Author: nlnhat (05/07/2023)
         */
        blur() {
            this.$refs.button.blur();
        },
        /**
         * Execute click when press enter
         * 
         * Author: nlnhat (29/07/2023)
         */
        onKeyDown(event) {
            if (event.keyCode == this.$enums.keyCode.enter) {
                event.preventDefault();
                event.stopPropagation();
                this.onClick();
            }
        }
    }
}
</script>
<style scoped>
.loading-spinner {
    position: absolute;
    scale: 0.24;
}

.icon--check--green {
    scale: 1.2;
}
</style>