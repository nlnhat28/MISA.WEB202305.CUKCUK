<template>
    <div class="toast-message">
        <div class="toast-message__icon">
            <m-icon :icon="icon"></m-icon>
        </div>
        <div
            class="toast-message__content"
            :class="{ 'content-lg': !action }"
        >
            <span :class="`toast-message__title ${color}`">
                {{ title }}
            </span>
            {{ content }}
        </div>
        <div
            class="toast-message__action"
            v-if="action"
        >
            {{ action }}
        </div>
        <div
            class="toast-message__close icon-container"
            @click="onClose()"
        >
            <m-icon-x></m-icon-x>
        </div>
    </div>
</template>
<script>
import enums from '@/constants/enums.js'
import resources from '@/constants/resources.js'
const toastType = enums.toastType;

export default {
    name: 'MISAToastMessage',
    props: {
        /**
         * Type of toast
         */
        type: {
            type: Number,
            default: toastType.info,
            validator: (value) => {
                return [
                    toastType.success,
                    toastType.error,
                    toastType.warning,
                    toastType.info,
                ].includes(value);
            },
        },
        /**
         * Title of toast
         */
        title: {
            type: String,
            default: resources['vn'].info,
        },
        /**
         * Content of toast
         */
        content: {
            type: String,
            default: null
        },
        /**
         * Action of toast
         */
        action: {
            type: Object,
            default: null
        }
    },
    data() {
        return {
            /**
             * Icon class
             */
            icon: '',
            /**
             * Color class
             */
            color: '',
        }
    },
    created() {
        this.setClass();
        /**
         * Set time close auto (5s)
         * 
         * Author: nlnhat (04/07/2023)
         */
        setTimeout(this.onClose, 5000);
    },
    methods: {
        /**
         * Close toast
         * 
         * Author: nlnhat (04/07/2023)
         */
        onClose() {
            this.$emit('emitCloseToast')
        },
        /**
         * Set class by type of toast 
         * 
         * Author: nlnhat (04/07/2023)
         */
        setClass() {
            switch (this.type) {
                case toastType.success:
                    this.color = "color--success";
                    this.icon = "success";
                    break;
                case toastType.error:
                    this.color = "color--error";
                    this.icon = "error";
                    break;
                case toastType.warning:
                    this.color = "color--warning";
                    this.icon = "warning";
                    break;
                default:
                    this.color = "color--info";
                    this.icon = "info";
                    break;
            }
        }
    }
}
</script>

