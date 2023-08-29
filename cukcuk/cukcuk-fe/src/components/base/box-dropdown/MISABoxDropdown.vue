<template>
    <div class="box-dropdown" v-click-outside="hideMenu">
        <div class="box-dropdown__box" @click="toggleMenu">
            *
        </div>
        <m-select-table v-if="isShowMenu" :hasBorder="false">
        </m-select-table>
    </div>
</template>
<script>
export default {
    name: 'MISABoxDropdown',
    props: {
        /**
         * Danh sách lựa chọn
         */
        selects: {
            type: Array,
        }
    },
    data() {
        return {
            /**
             * Hiện menu hay không
             */
            isShowMenu: false
        }
    },
    methods: {
        /**
         * Hiện menu
         * 
         * Author: nlnhat (19/08/2023)
         */
        showMenu() {
            this.isShowMenu = true;
        },
        /**
         * Ẩn menu
         * 
         * Author: nlnhat (19/08/2023)
         */
        hideMenu() {
            this.isShowMenu = false;
        },
        /**
         * Ẩn/Hiện menu
         * 
         * Author: nlnhat (19/08/2023)
         */
        toggleMenu() {
            this.isShowMenu = !this.isShowMenu;
        }
    }
}
</script>