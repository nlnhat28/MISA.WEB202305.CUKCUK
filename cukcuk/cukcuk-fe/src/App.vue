<template>
  <div class="d-flex" id="main">
    <TheSidebar></TheSidebar>
    <div class="d-flex--column">
      <TheHeader></TheHeader>
      <TheMain></TheMain>
    </div>
  </div>
  <TheDialogContainer id="popup-container--main"></TheDialogContainer>
  <TheToastContainer></TheToastContainer>
</template>

<script>
import TheHeader from '@/components/layout/TheHeader.vue';
import TheSidebar from '@/components/layout/TheSidebar.vue';
import TheMain from '@/components/layout/TheMain.vue';
import TheDialogContainer from '@/components/layout/TheDialogContainer.vue';
import TheToastContainer from '@/components/layout/TheToastContainer.vue';
export default {
  name: 'App',
  components: {
    TheHeader,
    TheSidebar,
    TheMain,
    TheDialogContainer,
    TheToastContainer
  },
};
</script>

<style>
@import '@/css/main.css';
@import '@/css/layout/layout.css';
</style>
