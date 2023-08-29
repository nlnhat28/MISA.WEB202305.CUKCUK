<template>
    <m-dialog
        v-if="dialog"
        :type="dialog.type"
        :title="dialog.title"
        :content="dialog.content"
        @emitCloseDialog="closeDialog()"
    >
        <template v-slot:footer>
            <m-button
                :text="this.$resources['vn'].gotIt"
                ref="buttonFocus"
                :type="this.$enums.buttonType.primary"
                :click="closeDialog"
            >
            </m-button>
        </template>
    </m-dialog>
</template>
<script>
export default {
    name: 'TheDialogContainer',
    created() {
        this.$emitter.on("showDialog", (dialog) => {
            this.showDialog(dialog);
        }),
            this.$emitter.on("showDialogError", (content, actionOnClose) => {
                this.showDialogError(content, actionOnClose);
            })
    },
    mounted() {
        document.addEventListener('keydown', this.handleShortKey)
        this.focus();
    },
    beforeUnmount() {
        document.removeEventListener('keydown', this.handleShortKey)
    },
    data() {
        return {
            /**
             * Data dialog
             */
            dialog: null,
            /**
             * Action after close dialog
             */
            onClose: null,
        }
    },
    updated() {
        if (this.dialog != null)
            this.focus();
    },
    methods: {
        /**
         * Show a new dialog
         * 
         * Author: nlnhat (02/07/2023)
         * @param {*} newDialog Dialog object
         */
        showDialog(newDialog) {
            this.dialog = { ...newDialog };
        },
        /**
         * Show a new error dialog
         * 
         * Author: nlnhat (02/07/2023)
         * @param {*} content Content of dialog
         * @param {*} actionOnClose Action after close dialog
         */
        showDialogError(content, actionOnClose) {
            this.dialog = {
                type: this.$enums.dialogType.error,
                title: this.$resources['vn'].error,
                content: content,
            }
            this.onClose = actionOnClose;
        },
        /**
         * Close dialog
         * 
         * Author: nlnhat (02/07/2023)
         */
        closeDialog() {
            this.dialog = null;
            if (this.onClose)
                this.onClose();
        },
        /**
         * Handle shortcut keys
         * 
         * Author: nlnhat (25/07/2023)
         * @param {*} event Keydown event
         */
        handleShortKey(event) {
            const code = this.$enums.keyCode;
            // Enter | Esc: Đóng
            if (event.keyCode == code.enter || event.keyCode == code.esc) {
                event.preventDefault();
                event.stopPropagation();
                this.closeDialog();
            }
        },
        /**
         * Focus on button close
         * 
         * Author: nlnhat (25/07/2023)
         */
        focus() {
            if (this.$refs.buttonFocus != null)
                this.$nextTick(() => {
                    this.$refs.buttonFocus.focus();
                })
        }
    }
}
</script>