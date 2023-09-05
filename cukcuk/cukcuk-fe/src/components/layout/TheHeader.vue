<template>
  <div class="header">
    <div class="header__left">
      <div class="select-dropdown">
        <div class="select-dropdown__title">
          <div class="select-dropdown__title--text">Công ty cổ phẩn MISA</div>
          <div class="select-dropdown__title--dropdown">
            <m-icon icon="dropdown--light"></m-icon>
          </div>
        </div>
      </div>
    </div>
    <div class="header__right">
      <!-- <div class="d-flex"> -->
      <div class="header__toolbar">
        <div class="tool-item">
          <div class="icon-container">
            <m-icon icon="bell--light"></m-icon>
          </div>
          <div class="dot--amount">
            10
          </div>
        </div>
      </div>
      <div class="avatar-container">
        <div class="avatar">
          <img
            class="avatar--sm"
            src="@/assets/img/phuong-linh.jpg"
            alt="Ảnh đại diện"
            title="Admin"
          />
          <img
            class="avatar--lg"
            src="@/assets/img/phuong-linh.jpg"
            alt="Ảnh đại diện"
            title="Phương Linh"
          />
        </div>
        <div class="select-dropdown">
          <div class="select-dropdown__title">
            <div class="select-dropdown__title--text">Admin</div>
            <div class="select-dropdown__title--dropdown">
              <m-icon icon="dropdown--light"></m-icon>
            </div>
          </div>
        </div>
      </div>
      <!-- </div> -->
    </div>
  </div>
</template>
<script>
export default {
  name: 'TheHeader',
  methods: {
    onClickToggleSidebar() {
      this.$emitter.emit("togglerSidebar")
    }
  }
};
</script>
<style></style>