<template>
    <div
        class="toast-message-container"
        v-if="this.toasts.length"
    >
        <m-toast-message
            v-for="(toast) in toasts"
            :key="toast.id"
            :type="toast.type"
            :title="toast.title"
            :content="toast.content"
            :action="toast.action"
            @emitCloseToast="closeToast(toast.id)"
        ></m-toast-message>
    </div>
</template>
<script>
export default {
    name: 'TheToastContainer',
    created() {
        this.$emitter.on("showToast", (toast) => {
            this.showToast(toast);
        }),
            this.$emitter.on("showToastSuccess", (content) => {
                this.showToastSuccess(content);
            }),
            this.$emitter.on("showToastError", (content) => {
                this.showToastError(content);
            })
    },
    unmounted() {
        this.toasts = []
    },
    data() {
        return {
            toasts: [],
            id: 0,
        }
    },
    methods: {
        /**
         * Push new toast in toasts
         * 
         * Author: nlnhat (03/07/2023)
         */
        pushNewToast(newToast) {
            newToast.id = this.id++;
            this.toasts.push(newToast)
        },
        /**
         * Show a new toast
         * 
         * Author: nlnhat (02/07/2023)
         * @param {*} newToast Toast object
         */
        showToast(newToast) {
            try {
                this.pushNewToast(newToast)
            } catch { }
        },
        /**
         * Show a new success toast
         * 
         * Author: nlnhat (02/07/2023)
         * @param {*} content Content of toast
         */
        showToastSuccess(content) {
            try {
                const newToast = {
                    type: this.$enums.toastType.success,
                    title: this.$resources['vn'].success,
                    content: content,
                }
                this.pushNewToast(newToast)
            } catch { }
        },
        /**
         * Show a new error toast
         * 
         * Author: nlnhat (02/07/2023)
         * @param {*} content Content of toast
         */
        showToastError(content) {
            try {
                const newToast = {
                    type: this.$enums.toastType.error,
                    title: this.$resources['vn'].error,
                    content: content,
                }
                this.pushNewToast(newToast)
            } catch { }
        },
        /**
         * Close a toast
         * 
         * Author: nlnhat (02/07/2023)
         * @param {*} toastId Toast id
         */
        closeToast(toastId) {
            try {
                this.toasts = this.toasts.filter(toast => toast.id !== toastId)
            } catch { }
        }
    }
}
</script>