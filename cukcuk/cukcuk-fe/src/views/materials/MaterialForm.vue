<template>
  <m-popup
    :title="title"
    class="popup--md"
    @emitClose="onClickCloseForm()"
    @emitStartMove="onStartMove()"
    @keydown="handleShortKey"
    tabindex="0"
  >
    <template #mask>
      <m-loading v-if="isLoading"></m-loading>
    </template>
    <template #body>
      <m-form-body>
        <!-- Row 1: Tên + Mã -->
        <m-form-row>
          <!-- Tên -->
          <m-form-item
            :label="fields.materialName.label"
            :isRequired="true"
            style="width: 49%"
          >
            <m-textfield
              v-model="material.MaterialName"
              :validate="validateMaterialName"
              :isFocused="true"
              :label="fields.materialName.label"
              :maxLength="fields.materialName.maxLength"
              @emitEnter="getCodeOnChangeName()"
              ref="MaterialName"
            >
            </m-textfield>
          </m-form-item>
          <!-- Mã -->
          <m-form-item
            :label="fields.materialCode.label"
            :title="fields.materialCode.title"
            :isRequired="true"
            style="width: 49%"
          >
            <m-textfield
              v-model="material.MaterialCode"
              :validate="validateMaterialCode"
              :label="fields.materialCode.label"
              :maxLength="fields.materialCode.maxLength"
              :action="actionMaterialCode"
              @emitEnter="onClickGetNewMaterialCode()"
              ref="MaterialCode"
            >
            </m-textfield>
          </m-form-item>
        </m-form-row>
        <!-- Row 2: ĐVT + Kho -->
        <m-form-row>
          <!-- Đơn vị tính -->
          <m-form-item
            :label="fields.unit.label"
            :isRequired="true"
            style="width: 49%"
          >
            <m-combobox
              v-model:id="material.UnitId"
              v-model:name="material.UnitName"
              :theSelects="unitsComputed"
              :label="fields.unit.label"
              :maxLength="fields.unit.maxLength"
              :validate="validateUnit"
              :action="actionCreateUnit"
              ref="Unit"
            >
            </m-combobox>
          </m-form-item>
          <!-- Kho -->
          <m-form-item
            :label="fields.warehouse.label"
            style="width: 49%"
          >
            <m-combobox
              v-model:id="material.WarehouseId"
              v-model:name="material.WarehouseName"
              :theSelects="warehousesComputed"
              :label="fields.warehouse.label"
              :maxLength="fields.warehouse.maxLength"
              :validate="validateWarehouse"
              :action="actionCreateWarehouse"
              ref="Warehouse"
            >
            </m-combobox>
          </m-form-item>
        </m-form-row>
        <!-- Row 3: Hạn sử dụng + SL tồn tối thiểu-->
        <m-form-row>
          <!-- Hạn sử dụng -->
          <m-form-item
            :label="fields.expiryTime.label"
            style="width: 49%"
          >
            <div
              class="d-flex"
              style="column-gap: 8px"
            >
              <m-textfield
                v-model="material.ExpiryTime"
                :label="fields.expiryTime.label"
                :maxLength="fields.expiryTime.maxLength"
                :format="formatNumberByDot"
                textAlign="right"
                ref="ExpiryTime"
                style="width: 49%"
              >
              </m-textfield>
              <m-combobox
                v-model:id="material.TimeUnit"
                v-model:name="material.TimeUnitName"
                :theSelects="timeUnitSelects"
                :label="fields.timeUnit.label"
                :maxLength="fields.timeUnit.maxLength"
                style="width: 49%"
                ref="TimeUnit"
              >
              </m-combobox>
            </div>
          </m-form-item>
          <!-- SL tồn tối thiểu -->
          <m-form-item
            :label="fields.minimumInventory.label"
            :title="fields.minimumInventory.title"
            style="width: 49%"
          >
            <m-textfield
              v-model="material.MinimumInventory"
              :label="fields.minimumInventory.label"
              :maxLength="fields.minimumInventory.maxLength"
              :format="formatDecimalInput"
              :canSetSelection="true"
              style="width: 49%"
              textAlign="right"
              ref="MinimumInventory"
            >
            </m-textfield>
          </m-form-item>
        </m-form-row>
        <!-- Row 4: Ghi chú -->
        <m-form-row>
          <m-form-item
            :label="fields.note.label"
            style="width: 100%"
          >
            <m-textfield
              v-model="material.Note"
              :label="fields.note.label"
              :maxLength="fields.note.maxLength"
              :isTextarea="true"
              ref="Note"
            >
            </m-textfield>
          </m-form-item>
        </m-form-row>
        <m-form-row>
          <ConversionUnitTab
            :material="material"
            ref="ConversionUnits"
          >
          </ConversionUnitTab>
        </m-form-row>
        <m-form-row v-if="mode != this.$enums.formMode.create">
          <m-checkbox-item
            v-model:checked="material.IsUnfollow"
            :name="this.$resources['vn'].unfollow"
          ></m-checkbox-item>
        </m-form-row>
      </m-form-body>
    </template>
    <!-- footer's form -->
    <template #footer>
      <div class="button-container">
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].save"
          :click="onClickSave"
          iconLeft="cukcuk-save"
          tooltipContent="Ctrl + S"
        ></m-button>
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].saveAdd"
          :click="onClickSaveAdd"
          iconLeft="cukcuk-save-add"
          tooltipContent="Ctrl + Shift + S"
        ></m-button>
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].cancel"
          :click="onClickCloseForm"
          iconLeft="cukcuk-cancel"
          tooltipContent="Esc"
        ></m-button>
      </div>
      <div class="button-container">
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].help"
          :click="onClickHelp"
          iconLeft="cukcuk-help"
          tooltipContent="F1"
          @keydown="handleShortKeyLastButton"
        ></m-button>
      </div>
    </template>
  </m-popup>
  <!-- dialogs -->
  <m-dialog
    :type="this.noticeDialog.type"
    :title="this.noticeDialog.title"
    :content="this.noticeDialog.content"
    @emitCloseDialog="closeNoticeDialog()"
    v-if="this.noticeDialog.isShowed"
  >
    <template #footer>
      <m-button
        :type="this.$enums.buttonType.primary"
        :text="this.$resources['vn'].gotIt"
        :click="closeNoticeDialog"
        ref="buttonFocus"
      ></m-button>
    </template>
  </m-dialog>
  <m-dialog
    :type="this.confirmDialog.type"
    :title="this.confirmDialog.title"
    :content="this.confirmDialog.content"
    @emitCloseDialog="closeConfirmDialog()"
    v-if="this.confirmDialog.isShowed"
  >
    <template #footer>
      <div class="button-container">
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].save"
          :click="onClickSaveConfirm"
          ref="buttonFocus"
        ></m-button>
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].no"
          :click="hideForm"
        ></m-button>
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].cancel"
          :click="closeConfirmDialog"
        ></m-button>
      </div>
    </template>
  </m-dialog>
  <UnitForm
    v-if="isShowedUnitForm"
    :hideForm="hideUnitForm"
    :formMode="unitFormMode"
    @emitUpdateUnit="updateUnit"
  >
  </UnitForm>
  <WarehouseForm
    v-if="isShowedWarehouseForm"
    :hideForm="hideWarehouseForm"
    :formMode="warehouseFormMode"
    @emitUpdateWarehouse="updateWarehouse"
  >
  </WarehouseForm>
</template>

<script>
import enums from "@/constants/enums.js";
const formMode = enums.formMode;
import { fields } from "@/js/form/form.js";
import { isNullOrEmpty } from '@/js/utils/string.js';
import { sortByName } from "@/js/utils/array.js";
import { reformatDecimal, cleanFormatIntNumber } from "@/js/utils/clean-format.js";
import { copyObject, sameObject } from "@/js/utils/object.js";
import {
  validateMaterialCode,
  validateMaterialName,
  validateWarehouse,
  validateUnit,
} from "@/js/form/validate.js";
import {
  formatDate,
  formatStringByDot,
  formatStringBySpace,
  formatNumberByDot,
  formatDecimalInput,
  formatCode,
} from "@/js/utils/format.js";
import { openUrl } from "@/js/utils/window.js";
import { materialService } from '@/services/services.js';
import { useUnitStore, useWarehouseStore } from "@/stores/stores.js";
import { mapStores, mapState } from 'pinia';

import ConversionUnitTab from './ConversionUnitTab.vue';
import UnitForm from '@/views/units/UnitForm.vue';
import WarehouseForm from '@/views/warehouses/WarehouseForm.vue';

export default {
  name: "MaterialForm",
  components: {
    ConversionUnitTab,
    UnitForm,
    WarehouseForm,
  },
  props: {
    /**
     * (Props) Id of material for update
     */
    materialId: {
      type: String,
      default: null,
    },
    /**
     * Function to hide this form
     */
    hideForm: {
      type: Function,
    },
    /**
     * (Props) Form mode {create | update}
     */
    formMode: {
      type: Number,
      default: formMode.create,
      validator: (value) => {
        return [
          formMode.create,
          formMode.update,
          formMode.duplicate
        ].includes(
          value
        );
      },
    },
  },
  data() {
    return {
      /**
       * Mode of form
       */
      mode: this.formMode,
      /**
       * Fields info
       */
      title: this.formTitle,
      /**
       * Material show on form
       */
      material: {},
      /**
       * Fields info
       */
      fields: fields,
      /**
       * Units list show on combobox
       */
      // unitSelects: [],
      /**
       * Warehouses list show on combobox
       */
      // warehouseSelects: [],
      /**
       * Gender list show on combobox
       */
      timeUnitSelects: this.$common.selects.timeUnit,
      /**
       * Dialog notice info
       */
      noticeDialog: {
        type: this.$enums.dialogType.error,
        title: this.$resources["vn"].error,
        content: "",
        isShowed: false,
      },
      /**
       * Dialog confirm info
       */
      confirmDialog: {
        type: this.$enums.dialogType.question,
        title: this.$resources["vn"].saveChange,
        content: "",
        isShowed: false,
      },
      /**
       * Flag check success response
       */
      isSuccessResponseFlag: true,
      /**
       * Message to show on dialog if invalid form
       */
      messageValidate: null,
      /**
       * Focused input
       */
      refFocus: null,
      /**
       * Form item refs
       */
      refs: [],
      /**
       * Flag loading
       */
      isLoading: false,
      /**
       * Hành động lấy code mới
       */
      actionMaterialCode: {
        icon: "refresh--sm",
        title: this.$resources['vn'].getNewCode,
        method: this.onClickGetNewMaterialCode,
      },
      /**
       * Hành động tạo mới đơn vị tính
       */
      actionCreateUnit: {
        icon: "cukcuk-create",
        title: this.$resources['vn'].createNew,
        method: this.showEmptyUnitForm,
      },
      /**
       * Hành động tạo mới kho
       */
      actionCreateWarehouse: {
        icon: "cukcuk-create",
        title: this.$resources['vn'].createNew,
        method: this.showEmptyWarehouseForm,
      },
      /**
       * Original object
       */
      originalMaterial: {},
      /**
       * Show unit form or not 
       */
      isShowedUnitForm: false,
      /**
       * Mode of unit form 
       */
      unitFormMode: null,
      /**
       * Show warehouse form or not 
       */
      isShowedWarehouseForm: false,
      /**
       * Mode of warehouse form
       */
      warehouseFormMode: null,
    };
  },
  async created() {
    this.title = this.formTitle;

    this.$emitter.on("focusFormItem", (ref) => {
      this.focusErrorItem(ref);
    });
    this.$emitter.on("setMessageFormItem", (ref, errorMessage) => {
      this.setErrorMessage(ref, errorMessage);
    });

    await this.makeLoadingEffect(async () => {
      await this.handleMaterialOnCreate();
    });
  },
  mounted() {
    this.refs = [
      this.$refs.ConversionUnits,
      this.$refs.Note,
      this.$refs.MinimumInventory,
      this.$refs.Warehouse,
      this.$refs.Unit,
      this.$refs.MaterialCode,
      this.$refs.MaterialName,
    ];
  },
  unmounted() {
    this.$emitter.off("focusFormItem")
    this.$emitter.off("setMessageFormItem")
  },
  emits: [
    "emitReloadData",
    "emitReRenderForm",
    "emitUpdateFocusedId",
  ],
  watch: {
    /**
     * Theo dõi chế độ form để đổi title
     */
    mode() {
      this.title = this.formTitle;
    },
  },
  computed: {
    /**
     * Check form is valid or invalid
     *
     * Author: nlnhat (02/07/2023)
     * @return {*} True if valid, false if invalid
     */
    isValidForm() {
      try {
        let isValid = true;
        this.messageValidate = null;
        this.refs.forEach((ref) => {
          const message = ref.checkValidate();
          if (message) {
            this.messageValidate = message;
            isValid = false;
            this.refFocus = ref;
          }
        });
        return isValid;
      } catch (error) {
        console.error(error);
        return false;
      }
    },
    /**
     * Reformat date before call api (remove format)
     *
     * Author: nlnhat (02/07/2023)
     * @return {*} Material with reformatted data to save
     */
    reformatMaterial() {
      let material = this.copyObject(this.material);
      material.MinimumInventory = this.reformatDecimal(material.MinimumInventory);
      material.ConversionUnits.forEach(unit => unit.Rate = this.reformatDecimal(unit.Rate));
      material.ExpiryTime = this.cleanFormatIntNumber(material.ExpiryTime)
      return material;
    },
    /**
     * Change title when change mode
     *
     * Author: nlnhat (02/07/2023)
     * @return {*} New title update or create
     */
    formTitle() {
      switch (this.mode) {
        case this.$enums.formMode.create:
          return this.$resources["vn"].createMaterial;
        case this.$enums.formMode.update:
          return this.$resources["vn"].updateMaterial;
        case this.$enums.formMode.duplicate:
          return this.$resources["vn"].duplicateMaterial;
        default:
          return this.$resources["vn"].createMaterial;
      }
    },
    /**
     * Message of material on toast
     *
     * Author: nlnhat (02/07/2023)
     * @return {*} New title update or create
     */
    messageOnToast() {
      let title = null;
      switch (this.mode) {
        case this.$enums.formMode.create:
          title = this.$resources["vn"].created;
          break;
        case this.$enums.formMode.update:
          title = this.$resources["vn"].updated;
          break;
        case this.$enums.formMode.duplicate:
          title = this.$resources["vn"].duplicated;
          break;
        default:
          break;
      }
      const content = `${title} ${this.$resources["vn"].material} 
      <${this.material.MaterialCode} - ${this.material.MaterialName}>`;
      return content;
    },
    /**
     * Map unit store
     * 
     * Author: nlnhat (28/28/2023)
     */
    ...mapStores(useUnitStore),
    ...mapState(useUnitStore, {
      unitsComputed: 'unitSelects'
    }),
    /**
     * Map warehouse store
     * 
     * Author: nlnhat (28/08/2023)
     */
    ...mapStores(useWarehouseStore),
    ...mapState(useWarehouseStore, {
      warehousesComputed: 'warehouseSelects'
    }),
  },
  methods: {
    /**
     * Make loading effect
     *
     * Author: nlnhat (05/07/2023)
     * @param {function} func Function executes in loading process
     */
    async makeLoadingEffect(func) {
      try {
        this.isLoading = true;
        await func();
      } catch (error) {
        console.error(error);
      } finally {
        this.isLoading = false;
      }
    },
    /**
     * Handle material on created()
     *
     * Author: nlnhat (05/07/2023)
     */
    async handleMaterialOnCreate() {
      // Cập nhật
      if (this.mode == this.$enums.formMode.update) {
        await this.getMaterial(this.materialId)
      }
      // Nhân bản
      else if (this.mode == this.$enums.formMode.duplicate) {
        await this.getMaterial(this.materialId)
        await this.getNewMaterialCode();
      }
      // Thêm mới
      else {
        if (this.material.ConversionUnits == null)
          this.material.ConversionUnits = [];
        this.storeOriginalMaterial();
      }
    },
    /**
     * Lưu nguyên vật liệu gốc
     * 
     * Author: nlnhat (30/08/2023)
     */
    storeOriginalMaterial() {
      this.originalMaterial = this.copyObject(this.material);
    },
    /**
     * Get units
     *
     * Author: nlnhat (02/07/2023)
     */
    async getMaterial(id) {
      try {
        const response = await materialService.get(id);
        if (response?.status == this.$enums.status.ok) {
          this.material = response.data;

          if (this.mode == this.$enums.formMode.duplicate) {
            if (this.material.ConversionUnits?.length > 0) {
              this.material.ConversionUnits.map(unit => unit.EditMode = this.$enums.editMode.create);
            }
          }

          this.storeOriginalMaterial();
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Get new material code
     *
     * Author: nlnhat (02/07/2023)
     */
    async getNewMaterialCode() {
      try {
        const response = await materialService.getNewCode(this.material.MaterialName);
        if (response?.status == this.$enums.status.ok) {
          this.material.MaterialCode = response.data;
          return true;
        }
      } catch (error) {
        console.error(error);
        return false;
      }
    },
    /**
     * Handle click get new material code
     *
     * Author: nlnhat (02/07/2023)
     */
    async onClickGetNewMaterialCode() {
      const message = this.validateMaterialName(
        this.fields.materialName.label, this.material.MaterialName);
      if (this.isNullOrEmpty(this.material.MaterialName)) {
        this.$refs.MaterialCode.setErrorMessage(
          this.$resources['vn'].fillNameBeforeGetCode);
        this.$refs.MaterialCode.focus();
        return false;
      }
      else if (message != null) {
        this.$refs.MaterialCode.setErrorMessage(
          this.$resources['vn'].errorNameCannotGetCode
        );
        this.$refs.MaterialCode.focus();
      }
      else {
        await this.getNewMaterialCode();
      }
    },
    /**
     * Lấy mã mới khi thay tên nguyên vật liệu
     * 
     * Author: nlnhat (12/07/2023)
     */
    async getCodeOnChangeName() {
      if (this.validateMaterialName(null, this.material.MaterialName) == null) {
        this.$refs.MaterialCode.onAction();
        this.$refs.MaterialCode.focus();
      }
    },
    /**
     * Show notice dialog
     *
     * Author: nlnhat (04/07/2023)
     * @param {string} message Content to show on dialog
     */
    showNoticeDialog(message) {
      this.noticeDialog.content = message;
      this.noticeDialog.isShowed = true;
      this.focusOnButton();
    },
    /**
     * Close notice dialog
     *
     * Author: nlnhat (10/07/2023)
     */
    closeNoticeDialog() {
      try {
        this.noticeDialog.isShowed = false;
        if (this.refFocus) this.refFocus.focus();
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Close notice dialog
     *
     * Author: nlnhat (10/07/2023)
     */
    closeConfirmDialog() {
      this.confirmDialog.isShowed = false;
      this.focusFirstInput();
    },
    /**
     * Focus on button
     *
     * Author: nlnhat (25/07/2023)
     */
    focusOnButton() {
      this.$nextTick(() => {
        this.$refs.buttonFocus.focus();
      });
    },
    /**
     * Create new material
     *
     * Author: nlnhat (02/07/2023)
     */
    async createMaterial() {
      try {
        const response = await materialService.post(this.reformatMaterial);
        if (response?.status == this.$enums.status.created) {
          this.material.MaterialId = response.data;
          this.isSuccessResponseFlag = true;
        } else {
          this.isSuccessResponseFlag = false;
        }
      } catch (error) {
        console.error(error);
        this.isSuccessResponseFlag = false;
      }
    },
    /**
     * Update material
     *
     * Author: nlnhat (02/07/2023)
     */
    async updateMaterial() {
      try {
        const response = await materialService.put(
          this.material.MaterialId,
          this.reformatMaterial
        );
        if (response?.status == this.$enums.status.ok) {
          this.isSuccessResponseFlag = true;
        } else {
          this.isSuccessResponseFlag = false;
        }
      } catch (error) {
        console.error(error);
        this.isSuccessResponseFlag = false;
      }
    },
    /**
     * Save when create or update
     *
     * Author: nlnhat (02/07/2023)
     */
    async onSave() {
      await this.makeLoadingEffect(async () => {
        switch (this.mode) {
          case this.$enums.formMode.create:
            await this.createMaterial();
            break;
          case this.$enums.formMode.update:
            await this.updateMaterial();
            break;
          case this.$enums.formMode.duplicate:
            await this.createMaterial();
            break;
          default:
            break;
        };
      });
    },
    /**
     * On click button save (Cất)
     *
     * Author: nlnhat (02/07/2023)
     */
    async onClickSave() {
      try {
        if (this.isValidForm) {
          await this.onSave();
          if (this.isSuccessResponseFlag == true) {
            this.$emit("emitUpdateFocusedId", this.material.MaterialId);
            this.$emit("emitReloadData");
            this.showToastSuccess();
            this.hideForm();
          }
        } else {
          this.showNoticeDialog(this.messageValidate ?? this.$resources["vn"].userMsg500);
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * On click button save and add (Cất và thêm)
     *
     * Author: nlnhat (02/07/2023)
     */
    async onClickSaveAdd() {
      try {
        if (this.isValidForm) {
          await this.onSave();
          if (this.isSuccessResponseFlag == true) {
            this.$emit("emitUpdateFocusedId", this.material.MaterialId);
            this.$emit("emitReloadData");
            this.showToastSuccess();
            this.resetForm();
          }
        } else {
          this.showNoticeDialog(this.messageValidate ?? this.$resources["vn"].userMsg500);
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Reset form 
     * 
     * Author: nlnhat (28/08/2023)
     */
    resetForm() {
      this.$emit("emitReRenderForm");
    },
    /**
     * Show toast message after created, updated, duplicated success
     *
     * Author: nlnhat (02/07/2023)
     */
    showToastSuccess() {
      this.$emitter.emit("showToastSuccess", this.messageOnToast);
    },
    /**
     * Show confirm dialog
     *
     * Author: nlnhat (06/07/2023)
     * @param {string} message Content to show on dialog
     */
    showSaveConfirmDialog(message) {
      this.confirmDialog.content = message;
      this.confirmDialog.isShowed = true;
      this.focusOnButton();
    },
    /**
     * Handle when click save on confirm dialog
     *
     * Author: nlnhat (06/07/2023)
     */
    async onClickSaveConfirm() {
      this.confirmDialog.isShowed = false;
      await this.onClickSave();
    },
    /**
     * Handle click close this form
     *
     * Author: nlnhat (26/08/2023)
     */
    onClickCloseForm() {
      if (!this.sameObject(this.originalMaterial, this.reformatMaterial))
        this.showSaveConfirmDialog(this.$resources["vn"].saveChangeConfirm);
      else
        this.hideForm();
    },
    /**
     * Handle shortcut keys
     *
     * Author: nlnhat (25/07/2023)
     * @param {*} event Keydown event
     */
    handleShortKey(event) {
      const code = this.$enums.keyCode;

      // Handle shortcut keys on confirm dialog
      if (this.confirmDialog.isShowed) {
        this.handleShortKeyConfirmDialog(event);
      }
      // Handle shortcut keys on notice dialog
      else if (this.noticeDialog.isShowed) {
        this.handleShortKeyNoticeDialog(event);
      }

      // Ctrl + S || Ctrl + F8: Cất
      else if (
        ((event.ctrlKey && event.keyCode == code.s) ||
          (event.ctrlKey && event.keyCode == code.f8)) &&
        !event.shiftKey
      ) {
        event.preventDefault();
        event.stopPropagation();
        this.onClickSave();
      }
      // Ctrl + Shift + N: Cất và thêm
      else if (event.ctrlKey && event.shiftKey && event.keyCode == code.s) {
        event.preventDefault();
        event.stopPropagation();
        this.onClickSaveAdd();
      }
      // Esc: Đóng form
      else if (event.keyCode == code.esc) {
        event.preventDefault();
        event.stopPropagation();
        this.onClickCloseForm();
      }
      // F1: Help
      else if (event.keyCode == code.f1) {
        event.preventDefault();
        event.stopPropagation();
        this.onClickHelp();
      }
    },
    /**
     * Handle shortcut keys on confirm dialog
     *
     * Author: nlnhat (25/07/2023)
     * @param {*} event Keydown event on confirm dialog
     */
    handleShortKeyConfirmDialog(event) {
      const code = this.$enums.keyCode;
      switch (event.keyCode) {
        // Enter: Cất
        case code.enter:
          event.stopPropagation();
          event.preventDefault();
          this.onClickSaveConfirm();
          break;
        // N: Không
        case code.n:
          event.stopPropagation();
          event.preventDefault();
          this.hideForm();
          break;
        // Esc: Đóng
        case code.esc:
          event.stopPropagation();
          event.preventDefault();
          this.closeConfirmDialog();
          break;
      }
    },
    /**
     * Handle shortcut keys on notice dialog
     *
     * Author: nlnhat (25/07/2023)
     * @param {*} event Keydown event on notice dialog
     */
    handleShortKeyNoticeDialog(event) {
      const code = this.$enums.keyCode;
      // Enter || Esc: Đã hiểu
      if (event.keyCode == code.enter || event.keyCode == code.enter) {
        event.stopPropagation();
        event.preventDefault();
        this.closeNoticeDialog();
      }
    },
    /**
     * Handle shortcut keys on last button
     *
     * Author: nlnhat (25/07/2023)
     * @param {*} event Keydown event on last button
     */
    handleShortKeyLastButton(event) {
      const code = this.$enums.keyCode;
      // Tab: Focus on first input
      if (event.keyCode == code.tab) {
        event.stopPropagation();
        event.preventDefault();
        this.focusFirstInput();
      }
    },
    /**
     * Focus on first input
     *
     * Author: nlnhat (25/07/2023)
     */
    focusFirstInput() {
      this.$refs.MaterialName.focus();
    },
    /**
     * Focus on error item
     *
     * Author: nlnhat (04/08/2023)
     * @param {*} ref Ref name of error item
     */
    focusErrorItem(ref) {
      if (this.$refs[ref]) {
        this.$refs[ref].focus();
      }
    },
    /**
     * Set message for error item
     *
     * Author: nlnhat (04/08/2023)
     * @param {*} ref Ref name of error item
     * @param {*} errorMessage Error message
     */
    setErrorMessage(ref, errorMessage) {
      if (this.$refs[ref]) {
        this.$refs[ref].errorMessage = errorMessage;
      }
    },
    /**
     * Show empty unit form to add new 
     * 
     * Author: nlnhat (26/08/2023)
     */
    showEmptyUnitForm() {
      this.unitFormMode = this.$enums.formMode.create;
      this.isShowedUnitForm = true;
    },
    /**
     * Hide unit form
     * 
     * Author: nlnhat (26/08/2023)
     */
    hideUnitForm() {
      this.isShowedUnitForm = false;
    },
    /**
     * Show empty warehouse form to add new 
     * 
     * Author: nlnhat (26/08/2023)
     */
    showEmptyWarehouseForm() {
      this.warehouseFormMode = this.$enums.formMode.create;
      this.isShowedWarehouseForm = true;
    },
    /**
     * Hide warehouse form
     * 
     * Author: nlnhat (26/08/2023)
     */
    hideWarehouseForm() {
      this.isShowedWarehouseForm = false;
    },
    /**
     * Update new unit id
     * 
     * Author: nlnhat (26/08/2023)
     */
    updateUnit(id) {
      this.material.UnitId = id;
    },
    /**
     * Update new warehouse id
     * 
     * Author: nlnhat (26/08/2023)
     */
    updateWarehouse(id) {
      this.material.WarehouseId = id;
    },
    /**
     * On click help
     * 
     * Author: nlnhat (26/08/2023)
     */
    onClickHelp() {
      this.openUrl(window.externalUrl.helpAddMaterial);
    },
    /**
     * On start move popup
     * 
     * Author: nlnhat (26/08/2023)
     */
    onStartMove() {
      if (this.$refs.ConversionUnits)
        this.$refs.ConversionUnits.resetComboboxes()
    },
    /**
     * Validate methods
     */
    validateMaterialCode,
    validateMaterialName,
    validateWarehouse,
    validateUnit,
    isNullOrEmpty,
    /**
     * Format methods
     */
    formatCode,
    formatDate,
    formatStringByDot,
    formatStringBySpace,
    formatNumberByDot,
    formatDecimalInput,
    reformatDecimal,
    sortByName,
    cleanFormatIntNumber,
    /**
     * Utils
     */
    copyObject,
    sameObject,
    openUrl,
  },
};
</script>
<style scoped></style>
