<template>
    <div class="popup-container">
        <div
            class="dialog"
            ref="refDialog"
        >
            <div
                class="dialog__header"
                v-tooltip="tooltip"
                @mousedown="startMove"
                @dblclick="resetPosition"
            >
                <div class="dialog__title">
                    {{ this.$resources['vn'].manageRestaurant }}
                </div>
                <div
                    class="icon-container icon-close"
                    @click="onClose()"
                    v-tooltip="'Đóng (Esc)'"
                >
                    <m-icon-close></m-icon-close>
                </div>
            </div>
            <div class="dialog__content">
                <div class="icon-container">
                    <m-icon :icon="icon"></m-icon>
                </div>
                {{ content }}
            </div>
            <div class="dialog__footer">
                <slot name="footer"></slot>
            </div>
        </div>
    </div>
</template>
<script>
import enums from '@/constants/enums.js'
import resources from '@/constants/resources.js'
const dialogType = enums.dialogType;

export default {
    name: 'MISADialog',
    props: {
        /**
         * Type of dialog
         */
        type: {
            type: Number,
            default: resources['vn'].info,
            validator: (value) => {
                return [
                    dialogType.question,
                    dialogType.danger,
                    dialogType.error,
                    dialogType.warning,
                    dialogType.info,
                ].includes(value);
            },
        },
        /**
          * Title of dialog
          */
        title: {
            type: String,
            default: resources['vn'].info,
        },
        /**
         * Content of dialog
         */
        content: {
            type: String,
            default: null
        },
    },
    data() {
        return {
            /**
             * Icon class
             */
            icon: '',
            /**
             * Đang di chuyển hay không
             */
            isMoving: false,
            /**
             * Offset toạ độ mới so với toạ độ cũ
             */
            offset: [0, 0],
            /**
             * Popup content
             */
            refDialog: null,
            /**
             * Tooltip content
             */
            tooltip: null,
            /**
             * Limit of position to keep dialog on screen
             */
            limitPosition: {},
        }
    },
    created() {
        this.setClass();
    },
    mounted() {
        try {
            this.refDialog = this.$refs.refDialog;
        } catch (error) {
            console.error(error);
        }
    },
    methods: {
        /**
         * Close dialog
         * 
         * Author: nlnhat (04/07/2023)
         */
        onClose() {
            this.$emit('emitCloseDialog')
        },
        /**
         * Set class by type of dialog
         * 
         * Author: nlnhat (04/07/2023)
         */
        setClass() {
            switch (this.type) {
                case dialogType.question:
                    this.icon = 'question';
                    break;
                case dialogType.danger:
                    this.icon = 'danger';
                    break;
                case dialogType.error:
                    this.icon = 'danger';
                    break;
                case dialogType.warning:
                    this.icon = 'warning--lg';
                    break;
                default:
                    this.icon = 'info--lg';
                    break;
            }
        },

        /**
         * Bắt đầu di chuyển dialog
         * 
         * Author: nlnhat (01/07/2023)
         * @param {*} event Sự kiện ấn chuột
         */
        startMove(event) {
            try {
                this.limitPosition = {
                    minX: this.refDialog.offsetWidth / 2,
                    minY: this.refDialog.offsetHeight / 2,
                    maxX: window.innerWidth - this.refDialog.offsetWidth / 2,
                    maxY: window.innerHeight - this.refDialog.offsetHeight / 2,
                }
                this.offset = [
                    this.refDialog.offsetLeft - event.clientX,
                    this.refDialog.offsetTop - event.clientY
                ];
                document.addEventListener('mousemove', this.onMove);
                document.addEventListener('mouseup', this.endMove);
                this.isMoving = true;
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Di chuyển dialog
         * 
         * Author: nlnhat (01/07/2023)
         * @param {*} event Sự kiện di chuyển chuột
         */
        onMove(event) {
            try {
                if (this.isMoving) {
                    event.preventDefault();

                    // Giữ dialog trong màn hình
                    const newX = Math.max(this.limitPosition.minX,
                        Math.min((event.clientX + this.offset[0]), this.limitPosition.maxX));

                    const newY = Math.max(this.limitPosition.minY,
                        Math.min((event.clientY + this.offset[1]), this.limitPosition.maxY));

                    // Thiết lập toạ độ mới
                    this.refDialog.style.left = newX + 'px';
                    this.refDialog.style.top = newY + 'px';
                    this.tooltip = null;
                }
            } catch (error) {
                console.error(error);
                this.isMoving = false;
            }
        },
        /**
         * Thả dialog
         * 
         * Author: nlnhat (01/07/2023)
         */
        endMove() {
            try {
                if (this.refDialog.style.left != '50%' && this.refDialog.style.top != '50%') {
                    this.tooltip = `${this.$resources['vn'].doubleClick} ${this.$resources['vn'].toResetPosition}`;
                };
                document.removeEventListener('mousemove', this.onMove);
                document.removeEventListener('mouseup', this.endMove);
            } catch (error) {
                console.error(error);
            }
            finally {
                this.isMoving = false;
            }
        },
        /**
         * Đưa dialog về vị trí ban đầu (giữa màn hình)
         * 
         * Author: nlnhat (01/07/2023)
         */
        resetPosition() {
            this.refDialog.style.left = '50%';
            this.refDialog.style.top = '50%';
            this.tooltip = null;
        },
    }
}
</script>
<style scoped>
.dialog__header .icon-container.icon-close {
    padding: 6px 0px 6px 8px;
    box-sizing: content-box;
    cursor: pointer;
}
</style>