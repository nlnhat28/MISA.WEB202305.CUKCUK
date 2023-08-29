<template>
    <div
        class="action-many p-relative"
        v-click-outside="hideMenu"
    >
        <m-button
            :type="this.$enums.buttonType.secondary"
            :text="this.$resources['vn'].actionMany"
            iconRight="dropdown"
            :click="toggleDisplayMenu"
        >
        </m-button>
        <ul
            class="select-list context-menu"
            v-if="isShowMenu"
        >
            <li
                class="select-item"
                @click="onDelete()"
            >
                Xoá
            </li>
        </ul>
    </div>
</template>
<script>
export default {
    name: "MISAActionMany",
    data() {
        return {
            /**
             * Show or hide menu context
             */
            isShowMenu: false
        }
    },
    methods: {
        /**
         * Click delete
         
         * Author: nlnhat (27/07/2023)
         */
        onDelete() {
            this.$emit('emitDelete')
            this.hideMenu();
        },
        /**
         * Show menu context
         * 
         * Author: nlnhat (01/07/2023)
         */
        showMenu() {
            this.isShowMenu = true;
        },
        /**
         * Hide menu context
         * 
         * Author: nlnhat (01/07/2023)
         */
        hideMenu() {
            this.isShowMenu = false;
        },
        /**
         * Toggle show or hide menu context
         * 
         * Author: nlnhat (01/07/2023)
         */
        toggleDisplayMenu() {
            this.isShowMenu = !this.isShowMenu;
        }
    }
};
</script>
<style scoped>@import "./action-many.css";</style>