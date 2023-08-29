<template>
  <div class="action-dropdown">
    <div
      class="btn--combo p-relative"
      v-click-outside="hideMenu"
    >
      <m-button
        :type="this.$enums.buttonType.link"
        class="action-now"
        :text="this.$resources['vn'].fix"
        :click="onUpdate"
      >
      </m-button>
      <span class="divider divider--light"></span>
      <m-button
        :type="this.$enums.buttonType.link"
        class="dropdown"
        iconCenter="dropdown--blue"
        :click="toggleDisplayMenu"
      >
      </m-button>
      <ul
        class="select-list context-menu"
        v-if="isShowMenu"
      >
        <li
          class="select-item"
          @click="onDuplicate()"
        >
          Nhân bản
        </li>
        <li
          class="select-item"
          @click="onDelete()"
        >
          Xoá
        </li>
      </ul>
    </div>
  </div>
</template>
<script>
export default {
  name: "MISAActionDropdown",
  data() {
    return {
      /**
       * Show or hide menu context
       */
      isShowMenu: false,
    }
  },
  methods: {
    /**
     * Click update
     * 
     * Author: nlnhat (27/07/2023)
     */
    onUpdate() {
      this.$emit('emitUpdate');
      this.hideMenu();
    },
    /**
     * Click delete
     
     * Author: nlnhat (27/07/2023)
     */
    onDelete() {
      this.$emit('emitDelete')
      this.hideMenu();
    },
    /**
     * Click dupalicate
     
     * Author: nlnhat (27/07/2023)
     */
    onDuplicate() {
      this.$emit('emitDuplicate')
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
<style scoped>@import "./action-dropdown.css";</style>