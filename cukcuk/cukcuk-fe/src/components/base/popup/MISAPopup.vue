<template>
    <div class="popup-container">
        <div
            class="popup"
            ref="refPopup"
        >
            <slot name="mask"></slot>
            <div
                class="popup__header"
                v-tooltip="tooltip"
                @mousedown="startMove"
                @dblclick="resetPosition"
            >
                <div class="popup__header--left">
                    <div class="popup__title">{{ title }}</div>
                    <slot name="headLeft"></slot>
                </div>
                <div
                    class="icon-container icon-close"
                    @click="onClose()"
                    v-tooltip="'Đóng (Esc)'"
                >
                    <m-icon-close></m-icon-close>
                </div>
            </div>
            <div class="popup__body">
                <slot name="body"></slot>
            </div>
            <div class="popup__footer">
                <slot name="footer"></slot>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    name: 'MISAPopup',
    props: {
        /**
         * Function to hide popup
         */
        closePopup: {
            type: Function,
        },
        /**
         * Title of popup
         */
        title: {
            type: String,
        }
    },
    data() {
        return {
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
            refPopup: null,
            /**
             * Tooltip content
             */
            tooltip: null,
            /**
             * Limit of position to keep dialog on screen
             */
            limitPosition: {},
            /**
             * Center position
             */
            centerPosition: {},

        }
    },
    mounted() {
        try {
            this.refPopup = this.$refs.refPopup;
        } catch (error) {
            console.error(error);
        }
    },
    methods: {
        /**
         * Đóng popup
         * 
         * Author: nlnhat (01/07/2023)
         */
        onClose() {
            this.$emit('emitClose')
        },
        /**
         * Bắt đầu di chuyển popup
         * 
         * Author: nlnhat (01/07/2023)
         * @param {*} event Sự kiện ấn chuột
         */
        startMove(event) {
            try {
                this.limitPosition = {
                    minLeft: 0,
                    minTop: 0,
                    maxLeft: window.innerWidth - this.refPopup.offsetWidth,
                    maxTop: window.innerHeight - this.refPopup.offsetHeight,
                };
                this.centerPosition = {
                    left: (window.innerWidth - this.refPopup.offsetWidth) / 2,
                    top: (window.innerHeight - this.refPopup.offsetHeight) / 2
                };
                this.offset = [
                    this.refPopup.offsetLeft - event.clientX,
                    this.refPopup.offsetTop - event.clientY
                ];
                this.$emit('emitStartMove');
                document.addEventListener('mousemove', this.onMove);
                document.addEventListener('mouseup', this.endMove);
                this.isMoving = true;
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Di chuyển popup
         * 
         * Author: nlnhat (01/07/2023)
         * @param {*} event Sự kiện di chuyển chuột
         */
        onMove(event) {
            try {
                if (this.isMoving) {
                    event.preventDefault();

                    // Giữ popup trong màn hình
                    const newX = Math.max(this.limitPosition.minLeft,
                        Math.min((event.clientX + this.offset[0]), this.limitPosition.maxLeft));

                    const newY = Math.max(this.limitPosition.minTop,
                        Math.min((event.clientY + this.offset[1]), this.limitPosition.maxTop));

                    // Thiết lập toạ độ mới
                    this.refPopup.style.left = newX + 'px';
                    this.refPopup.style.top = newY + 'px';
                    this.tooltip = null;
                };

            } catch (error) {
                console.error(error);
                this.isMoving = false;
            }
        },
        /**
         * Thả popup
         * 
         * Author: nlnhat (01/07/2023)
         */
        endMove() {
            try {
                if (this.refPopup.style.left != `${this.centerPosition.left}px`
                    && this.refPopup.style.top != `${this.centerPosition.top}px`) {
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
         * Đưa popup về vị trí ban đầu (giữa màn hình)
         * 
         * Author: nlnhat (01/07/2023)
         */
        resetPosition() {
            this.refPopup.style.left = this.centerPosition.left + 'px';
            this.refPopup.style.top = this.centerPosition.top + 'px';
            this.tooltip = null;
        },
    }
}
</script>
<style scoped>
.popup__header .icon-container.icon-close {
    padding: 6px 0px 6px 8px;
    box-sizing: content-box;
    cursor: pointer;
}
</style>