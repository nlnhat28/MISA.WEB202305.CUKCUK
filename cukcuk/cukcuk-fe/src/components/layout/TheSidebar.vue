<template>
    <div
        class="sidebar"
        ref="theSidebar"
        :class="{ 'sidebar--collapse': isCollapse }"
    >
        <div class="sidebar__header">
            <div
                class="toggle"
                @click="toggleSidebar()"
            >
                <m-icon icon="toggle-list"></m-icon>
            </div>
            <div class="logo logo--text">
                MISACUKCUK
            </div>
        </div>
        <div class="sidebar__body">
            <div class="menu menu-column">
                <m-menu-item
                    v-for="(menu, index) in menuItems"
                    :key="index"
                    :target="menu.target ?? null"
                    :icon="menu.icon"
                    :title="menu.title"
                ></m-menu-item>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    name: 'TheSidebar',
    created() {
        this.$emitter.on("togglerSidebar", () => {
            this.toggleSidebar()
        })
    },
    unmounted() {
        this.$emitter.off("togglerSidebar")
    },
    data() {
        return {
            isCollapse: false,
            menuItems: [
                {
                    target: '/home',
                    icon: 'chart-pie',
                    title: 'Tổng quan'
                },
                {
                    target: '/materials',
                    icon: 'buy',
                    title: 'Nguyên vật liệu'
                },
                // {
                //     icon: 'money-pocket',
                //     title: 'Tiền mặt'
                // },
                // {
                //     icon: 'bank',
                //     title: 'Tiền gửi'
                // },
                // {
                //     icon: 'bill',
                //     title: 'Quản lý hoá đơn'
                // },
                // {
                //     icon: 'warehouse',
                //     title: 'Kho'
                // },
                // {
                //     icon: 'tools',
                //     title: 'Công cụ dụng cụ'
                // },
                // {
                //     icon: 'property',
                //     title: 'Tài sản cố định'
                // },
                // {
                //     icon: 'tax',
                //     title: 'Thuế'
                // },
                // {
                //     icon: 'price',
                //     title: 'Giá thành'
                // },
                // {
                //     icon: 'summary',
                //     title: 'Tổng hợp'
                // },
                // {
                //     icon: 'budget',
                //     title: 'Ngân sách'
                // },
                // {
                //     icon: 'report',
                //     title: 'Báo cáo'
                // },
                // {
                //     icon: 'analysis',
                //     title: 'Phân tích tài chính'
                // },
            ]
        }
    },
    methods: {
        /**
         * Toggle full sidebar and collapsed sidebar
         * 
         * Author: nlnhat (26/06/23) 
         */
        toggleSidebar() {
            try {
                this.isCollapse = !this.isCollapse;
            } catch (error) { }
        },
    }
}
</script>