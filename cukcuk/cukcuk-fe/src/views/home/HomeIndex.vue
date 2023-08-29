<template>
    <m-page :title="this.$resources['vn'].home">
        <template #pageBody>
            <!-- <div class="home-page">
                <img
                    class="logo--misa"
                    src="@/assets/img/MISA-logo.png"
                    alt="MISA JSC"
                    @click="openURL('https://www.misa.vn/')"
                />
                <div class="home-page__title">
                    BÀI TẬP ĐÁNH GIÁ GIAI ĐOẠN I
                </div>
                <div class="home-page__content">
                    Nguyễn Long Nhật - Customize
                </div>
            </div> -->
        </template>
    </m-page>
</template>
<script>
export default {
    name: 'HomeIndex',
    created() {
        document.title = `${this.$resources['vn'].titleHome} - ${this.$resources['vn'].appName}`;
    },
    methods: {
        /**
         * Open url in new tab
         * 
         * Author: nlnhat (10/08/2023)
         * @param {*} url Url 
         */
        openURL(url) {
            window.open(url, "_blank")
        }
    }
};
</script>
<style scoped>
.home-page {
    display: flex;
    flex-direction: column;
    align-items: center;
    row-gap: 12px;
    width: 100%;
    height: 100%;
    padding: 32px 0px;
}

.home-page__title {
    font-size: 32px;
    font-weight: bold;
}

.home-page__content {
    font-size: 22px;
    font-weight: 500;
}

.logo--misa {
    width: 200px;
    margin-bottom: 24px;
    cursor: pointer;
}</style>
