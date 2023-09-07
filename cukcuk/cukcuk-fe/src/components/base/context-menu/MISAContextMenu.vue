<template>
    <div
        class="context-menu"
        ref="contextMenu"
        tabindex="0"
    >
        <div
            v-for="(action, index) in actions"
            :class="{ 'context-menu__item': true, 'context-menu__item--separator': action.isSeparator }"
            :key="index"
            @click="onClickItem(action.method)"
        >
            <m-icon-container
                :icon="action.icon"
                v-if="!action.isSeparator"
            ></m-icon-container>
            <span v-if="!action.isSeparator">{{ action.title }}</span>
        </div>
    </div>
</template>
<script>
export default {
    name: 'MISAContextMenu',
    props: {
        /**
         * Danh sách actions
         */
        actions: {
            type: Array,
        },
        /**
         * Toạ độ xuất hiện
         */
        position: {
            type: Object,
        }
    },
    emits: [
        'emitClickItem',
    ],
    mounted() {
        if (this.position) {
            this.setPosition();
        }
    },
    watch: {
        position: {
            handler() {
                this.setPosition();
            },
            deep: true
        }
    },
    methods: {
        /**
         * Sự kiện click item
         * 
         * Author: nlnhat (07/09/2023)
         */
        onClickItem(method) {
            method();
            this.$emit('emitClickItem');
        },
        /**
         * Đặt vị trí cho context menu
         * 
         * Author: nlnhat (07/09/2023)
         */
        setPosition() {
            try {
                if (this.position.top && this.position.left) {
                    const contextMenu = this.$refs.contextMenu
                    const maxX = window.innerWidth - contextMenu.offsetWidth - 10;
                    const maxY = window.innerHeight - contextMenu.offsetHeight - 10;
                    contextMenu.style.left = Math.min(this.position.left, maxX) + 'px';
                    contextMenu.style.top = Math.min(this.position.top, maxY) + 'px';
                }
            } catch (error) {
                console.error(error);
            }
        }
    }
}
</script>