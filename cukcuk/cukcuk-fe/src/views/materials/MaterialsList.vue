<template>
  <m-page
    :title="this.$resources['vn'].titleMaterials"
    @keydown="handleShortKey"
    tabindex="0"
  >
    <template #pageAction>
      <!-- add new -->
      <m-button
        :type="this.$enums.buttonType.primary"
        :click="onClickFeedback"
        :text="this.$resources['vn'].feedback"
        iconLeft="cukcuk-feedback"
      >
      </m-button>
    </template>
    <template #pageBody>
      <!-- table -->
      <m-table
        :totalRecord="totalRecord"
        :isLoading="isLoading"
        ref="table"
      >
        <template #toolbarLeft>
          <!-- Thêm -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :click="showEmptyMaterialForm"
            :text="this.$resources['vn'].create"
            iconLeft="cukcuk-add"
            title="Ctrl + 1"
          >
          </m-button>
          <!-- Nạp -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :click="onReloadData"
            :text="this.$resources['vn'].reload"
            :hasLoading="true"
            iconLeft="cukcuk-reload"
            title="Ctrl + Y"
          >
          </m-button>
          <m-separator></m-separator>
          <!-- Nhân bản -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :click="duplicateMaterial"
            :text="this.$resources['vn'].duplicate"
            :isDisabled="focusedId == null || focusedIds.length > 1"
            iconLeft="cukcuk-duplicate"
            title="Ctrl + V"
          >
          </m-button>
          <!-- Sửa -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :click="showMaterialForm"
            :text="this.$resources['vn'].fix"
            :isDisabled="focusedId == null || focusedIds.length > 1"
            iconLeft="cukcuk-edit"
            title="Ctrl + U"
          >
          </m-button>
          <!-- Xoá -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :click="onClickDelete"
            :text="this.$resources['vn'].delete"
            :isDisabled="focusedId == null"
            iconLeft="cukcuk-delete"
            title="Ctrl + D"
          >
          </m-button>
          <m-separator></m-separator>
          <!-- Thống kê -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :text="this.$resources['vn'].stat"
            :click="showMaterialStat"
            title="Ctrl + S"
            iconLeft="cukcuk-double-arrow"
          ></m-button>
          <m-separator></m-separator>
          <!-- Nhập khẩu -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :click="showMaterialImport"
            :text="this.$resources['vn'].import"
            iconLeft="cukcuk-import"
            title="Ctrl + I"
          >
          </m-button>
          <!-- Xuất khẩu -->
          <m-button
            :type="this.$enums.buttonType.linkIcon"
            :click="exportToExcel"
            :text="this.$resources['vn'].export"
            :isDisabled="totalRecord <= 0"
            :hasLoading="true"
            iconLeft="cukcuk-excel"
            title="Ctrl + E"
          >
          </m-button>
        </template>
        <template #toolbarRight>
          <!-- search in list -->
          <m-search-box
            v-model="keySearch"
            :placeholder="this.$resources['vn'].searchMaterial"
            ref="searchBox"
            @emitFocusFirst="focusRow(0)"
          ></m-search-box>
        </template>
        <template #thead>
          <!-- table head -->
          <m-th
            v-for="(head, index) in table.heads"
            :key="index"
            :textAlign="head.textAlign"
            :widthCell="head.widthCell"
            :name="head.name"
            :title="head.title"
            :fullTitle="head?.fullTitle"
            :sortType="head.sortType"
            :filterConfig="head.filterConfig"
            v-model:sortModel="sortModels[index]"
            v-model:filterModel="filterModels[index]"
          >
            {{ head.title }}
          </m-th>
        </template>
        <template #tbody>
          <!-- table body -->
          <m-tr
            v-for="(material, index) in materials"
            :key="material.MaterialId"
            :index="index"
            :id="material.MaterialId"
            :isSelected="isSelected(material.MaterialId)"
            ref="tr"
            v-model:focusedId="focusedId"
            v-model:focusedIds="focusedIds"
            @emitUpdate="showMaterialForm(material)"
            @emitDelete="handleDeleteOnRow(material.MaterialId)"
            @emitDuplicate="duplicateMaterial(material)"
            @emitCreate="showEmptyMaterialForm"
            @emitReload="onReloadData"
            @emitExport="exportToExcel"
            @emitFocusNext="focusNextRow"
            @emitFocusPrevious="focusPreviousRow"
            @emitFocusById="focusById"
            @emitSelectMany="selectMany"
          >
            <template #content>
              <!-- Mã nguyên vật liệu -->
              <m-td
                textAlign="left"
                :content="material.MaterialCode"
              >
              </m-td>
              <!-- Tên nguyên vật liệu -->
              <m-td
                textAlign="left"
                :content="material.MaterialName"
              ></m-td>
              <!-- Số lượng tồn tối thiểu -->
              <m-td
                textAlign="right"
                :content="formatDecimal(material.MinimumInventory)"
              ></m-td>
              <!-- Đơn vị tính -->
              <m-td
                textAlign="left"
                :content="material.UnitName"
              ></m-td>
              <!-- Mã kho ngầm định -->
              <m-td
                textAlign="left"
                :content="material.WarehouseCode"
              ></m-td>
              <!-- Tên kho -->
              <m-td
                textAlign="left"
                :content="material.WarehouseName"
              ></m-td>
              <!-- Ghi chú -->
              <m-td
                textAlign="left"
                :content="material.Note"
              ></m-td>
              <!-- Ngừng theo dõi-->
              <m-td textAlign="center">
                <m-checkbox
                  :checked="material.IsUnfollow"
                  :isReadOnly="true"
                ></m-checkbox>
              </m-td>
            </template>
          </m-tr>
        </template>
        <template #tfooter>
          <!-- pagination -->
          <m-pagination
            v-model:totalRecord="totalRecord"
            v-model:allRecord="allRecord"
            v-model:pageModel="page"
            :canAccessRandom="true"
            @emitUpdatePage="updatePage"
            @emitReload="onReloadData()"
          >
          </m-pagination>
        </template>
      </m-table>
    </template>
  </m-page>
  <!-- Form nguyên vật liệu -->
  <MaterialForm
    v-if="isShowedMaterialForm"
    :hideForm="hideMaterialForm"
    :materialId="focusedId"
    :formMode="formMode"
    :key="formKey"
    @emitReloadData="filterMaterials"
    @emitReRenderForm="reRenderForm()"
    @emitUpdateFocusedId="updateFocusedId"
  >
  </MaterialForm>
  <!-- Thống kê nguyên vật liệu -->
  <MaterialStat
    v-if="isShowMaterialStat"
    :closeThis="closeMaterialStat"
    :focusedId="focusedId"
    :title="`${this.$resources['vn'].stat} ${this.$resources['vn'].material}`"
    :filterCount="filterCountComputed"
  >
  </MaterialStat>
  <!-- Nhập khẩu nguyên vật liệu -->
  <MaterialImport
    v-if="isShowMaterialImport"
    :closeThis="closeMaterialImport"
    :title="`${this.$resources['vn'].import} ${this.$resources['vn'].material}`"
    @emitReloadData="filterMaterials"
    @emitUpdateFocusedId="updateFocusedId"
    @emitUpdateFocusedIds="updateFocusedIds"
  >
  </MaterialImport>
  <!-- Dialog delete confirm -->
  <m-dialog
    :type="this.deleteConfirmDialog.type"
    :title="this.deleteConfirmDialog.title"
    :content="this.deleteConfirmDialog.content"
    @emitCloseDialog="this.deleteConfirmDialog.onClickCancel()"
    v-if="this.deleteConfirmDialog.isShowed"
  >
    <template #footer>
      <div class="button-container">
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].yes"
          ref="buttonFocus"
          :click="this.deleteConfirmDialog.onClickDelete"
        >
        </m-button>
        <m-button
          :type="this.$enums.buttonType.primary"
          :text="this.$resources['vn'].no"
          :click="this.deleteConfirmDialog.onClickCancel"
        >
        </m-button>
      </div>
    </template>
  </m-dialog>
</template>
<script>
import MaterialForm from './material-form/MaterialForm.vue';
import MaterialStat from './material-stat/MaterialStat.vue';
import MaterialImport from './material-import/MaterialImport.vue';
import {
  formatDate,
  formatStringByDot,
  formatStringBySpace,
  formatDecimal
} from "@/js/utils/format.js";
import { sortByName } from '@/js/utils/array.js'
import { download } from '@/js/utils/file.js'
import { openUrl } from "@/js/utils/window.js";
import { materialService } from '@/services/services.js';
import { useUnitStore, useWarehouseStore } from '@/stores/stores.js';
import { mapStores, mapState } from 'pinia';

export default {
  name: "MaterialsList",
  components: {
    MaterialForm,
    MaterialStat,
    MaterialImport,
  },
  data() {
    return {
      /**
       * Show form or not 
       */
      isShowedMaterialForm: false,
      /**
       * Mode of form create or update
       */
      formMode: null,
      /**
       * List of materials to show on table
       */
      materials: [],
      /**
       * List of selected materials
       */
      materialsSelect: [],
      /**
       * Material showed on form to update
       */
      materialUpdate: {},
      /**
       * Id of material to focus
       */
      focusedId: null,
      /**
       * Ids of material to focus
       */
      focusedIds: [],
      /**
       * Delete confirm dialog object
       */
      deleteConfirmDialog: {
        type: this.$enums.dialogType.question,
        title: `${this.$resources['vn'].delete} ${this.$resources['vn'].material}`,
        content: '',
        isShowed: false,
        onClickCancel: () => {
          this.deleteConfirmDialog.isShowed = false;
        }
      },
      /**
       * 
       */
      page: {
        pageNumber: 1,
        pageSize: 25,
      },
      /**
       * Key search
       */
      keySearch: '',
      /**
       * Total record on table
       */
      totalRecord: 0,
      /**
       * All record in database
       */
      allRecord: 0,
      /**
       * Loading flag
       */
      isLoading: true,
      /**
       * Key of material form
       */
      formKey: 0,
      /**
       * Danh sách các sort model mới từ các cột
       */
      sortModels: [],
      /**
       * Danh sách cũ các sort model từ các cột 
       */
      oldSortModels: [],
      /**
       * Hàng đợi sort (Sắp xếp sort model theo thứ tự chọn)
       */
      queueSortModels: [],
      /**
       * Table data
       */
      table: {
        heads: [
          {
            title: this.$resources['vn'].materialCode,
            textAlign: 'left',
            widthCell: 160,
            name: "MaterialCode",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.text,
            }
          },
          {
            title: this.$resources['vn'].materialName,
            textAlign: 'left',
            widthCell: 240,
            name: "MaterialName",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.text,
            }
          },
          {
            title: this.$resources['vn'].minimumInventory,
            fullTitle: this.$resources['vn'].minimumInventoryFullTitle,
            textAlign: 'left',
            widthCell: 160,
            name: "MinimumInventory",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.number,
            }
          },
          {
            title: this.$resources['vn'].unit,
            textAlign: 'left',
            widthCell: 160,
            name: "UnitName",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.selectName,
            }
          },
          {
            title: this.$resources['vn'].warehouseCode,
            textAlign: 'left',
            widthCell: 160,
            name: "WarehouseCode",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.selectName,
            }
          },
          {
            title: this.$resources['vn'].warehouseName,
            textAlign: 'left',
            widthCell: 260,
            name: "WarehouseName",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.text,
            }
          },
          {
            title: this.$resources['vn'].note,
            textAlign: 'left',
            widthCell: 240,
            name: "Note",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.text,
            }
          },
          {
            title: this.$resources['vn'].isUnfollow,
            textAlign: 'left',
            widthCell: 140,
            name: "IsUnfollow",
            sortType: this.$enums.sortType.blur,
            filterConfig: {
              filterType: this.$enums.filterType.selectId,
              selects: this.$common.selects.boolean,
            }
          }
        ]
      },
      /**
       * Các filter model từ các cột
       */
      filterModels: [],
      /**
       * Các filter model không rỗng từ các cột
       */
      filterModelsClean: [],
      /**
       * Các filter queries
       */
      filterQueries: [],
      /**
       * Flag
       */
      isFilterSuccess: true,
      /**
       * Date format
       */
      dateFormat: 'dd/MM/yyyy',
      /**
       * Show or hide material stat
       */
      isShowMaterialStat: false,
      /**
       * Show or hide material import
       */
      isShowMaterialImport: false,
    };
  },
  async created() {
    document.title = `${this.$resources['vn'].titleMaterials} - ${this.$resources['vn'].appName}`;
    await Promise.all([
      this.filterMaterialsOnCreated(),
      this.getWarehouses(),
      this.getUnits()
    ])
    this.isLoading = false
  },
  async mounted() {
    this.focusOnSearchBox();
  },
  watch: {
    /**
     * Reload data if search changes
     * 
     * Author: nlnhat (05/08/2023)
     */
    keySearch() {
      this.reloadMaterials();
    },
    /**
     * Theo dõi thay đổi của mỗi sort item
     * 
     * Author: nlnhat (05/08/2023)
     */
    sortModels: {
      async handler() {
        // Tìm item bị thay đổi
        const newItem = this.sortModels.find(item => item != null && !this.oldSortModels.includes(item)) ?? null;
        const oldItem = this.oldSortModels.find(item => item != null && !this.sortModels.includes(item)) ?? null;

        // Cập nhật item trong hàng đợi
        if (oldItem) {
          const oldIndex = this.queueSortModels.indexOf(oldItem);
          if (oldIndex != -1) this.queueSortModels[oldIndex] = newItem
          else this.queueSortModels.push(newItem);
        }
        else this.queueSortModels.push(newItem);

        this.queueSortModels = this.queueSortModels.filter(item => item != oldItem);

        // Gán vào danh sách cũ
        this.oldSortModels = [...this.sortModels];

        // Load lại dữ liệu
        await this.reloadMaterials()
      },
      deep: true
    },
    /**
     * Clean filter models when filters change
     * 
     * Author: nlnhat (26/08/2023)
     */
    filterModels: {
      handler() {
        this.filterModelsClean = this.filterModels.filter(item => item && item != null);
      },
      deep: true
    },
    /**
     * Reload data when filters change
     * 
     * Author: nlnhat (26/08/2023)
     */
    filterModelsClean: {
      async handler() {
        await this.reloadMaterials();
      },
      deep: true
    },
    /**
     * Watch number of materials
     */
    materials: {
      handler() {
        this.focusedId = this.materials.length > 0 ? this.focusedId : null;
      },
      deep: false
    },
    "unitStore.unitSelects": function () {
      const head = this.table.heads.find(head => head.name == "UnitName")
      head.filterConfig.selects = this.unitStore.unitSelects;
    },
    "warehouseStore.warehouseSelects": function () {
      const head = this.table.heads.find(head => head.name == "WarehouseCode")
      head.filterConfig.selects = this.warehouseStore.warehouseSelects;
    },
  },
  computed: {
    /**
     * Check all records in page are selected or not
     * 
     * Author: nlnhat (05/08/2023)
     * @return True if are selected, otherwise false
     */
    isCheckedAllComputed() {
      if (!this.materials || this.materials.length === 0) {
        return false;
      }
      return this.materials.every(material => this.materialsSelect.includes(material.MaterialId));
    },
    /**
     * Check some records (has check but not all) or not
     * 
     * Author: nlnhat (05/08/2023)
     * @return True if are selected, otherwise false
     */
    isCheckSomeComputed() {
      const isIncludes = this.materialsSelect.every(id =>
        this.materials.some(material => material.MaterialId == id));

      return (this.materials
        && this.materialsSelect.length > 0
        && this.materialsSelect.length < this.materials.length
        && isIncludes)
    },
    /**
     * Filter
     * 
     * Author: nlnhat (19/08/2023)
     */
    filterComputed() {
      const filter = {
        keySearch: this.keySearch,
        pagingModel: this.page,
        sortModels: this.sortModelsComputed,
        filterModels: this.filterModelsComputed,
      };
      return filter;
    },
    /**
     * Data sort models truyền đến server
     * 
     * Author: nlnhat (19/08/2023)
     */
    sortModelsComputed() {
      const sortModelsDto = this.queueSortModels.filter(item => item && item != null);
      return sortModelsDto;
    },
    /**
     * Data filter models truyền đến server
     * 
     * Author: nlnhat (25/08/2023)
     * @return Câu truy vấn sort
     */
    filterModelsComputed() {
      const filterModelsDto = this.filterModelsClean.map(item => ({
        columnName: item.column,
        logicType: item.logicType,
        logicName: item.logicName,
        compareType: item.compareType,
        compareName: item.compareName,
        filterValue: item.value.key,
      }));
      return filterModelsDto;
    },
    /**
     * Filter count object
     */
    filterCountComputed() {
      return {
        totalRecord: this.totalRecord,
        allRecord: this.allRecord,
      }
    },
    /**
     * Scroll position
     * 
     * Author: nlnhat (06/07/2023)
     */
    scrollTopComputed() {
      if (this.materials.length > 0) {
        const index = this.materials.indexOf(this.materials.find(material => material.MaterialId == this.focusedId));
        return index * 28;
      }
      return 0;
    },
    /**
     * Map unit store
     */
    ...mapStores(useUnitStore),
    ...mapState(useUnitStore, {
      unitsComputed: 'unitSelects'
    }),
    /**
     * Map warehouse store
     */
    ...mapStores(useWarehouseStore),
    ...mapState(useWarehouseStore, {
      warehousesComputed: 'warehouseSelects'
    }),
  },
  methods: {
    /**
     * Filter not unfollow materials on created
     * 
     * Author: nlnhat (05/08/2023)
     */
    async filterMaterialsOnCreated() {
      this.filterModels[7] = {
        column: "IsUnfollow",
        compareType: this.$enums.compareType.equal,
        logicType: this.$enums.logicType.and,
        value: { key: '0', name: this.$resources['vn'].no }
      };
    },
    /**
     * Handle checkbox check all records on table in a page
     * 
     * Author: nlnhat (05/08/2023)
     */
    onChangeCheckAll() {
      // Uncheck all records in a page
      if (this.isCheckedAllComputed) {
        this.materialsSelect = this.materialsSelect.filter(id => {
          return !this.materials.some(material => material.MaterialId === id);
        });
      }
      // Check all records in a page
      else {
        this.materials.forEach(material => {
          if (!this.materialsSelect.includes(material.MaterialId)) {
            this.materialsSelect.push(material.MaterialId)
          }
        })
      }
    },
    /**
     * Handle on click un select allk
     * 
     * Author: nlnhat (05/08/2023)
     */
    onClickUnselectAll() {
      this.materialsSelect = []
    },
    /**
     * Check id in selected list or not
     * 
     * Author: nlnhat (05/08/2023)
     * @return True if selected, otherwise false
     */
    isSelected(id) {
      return this.materialsSelect.includes(id)
    },
    /**
     * Make loading effect
     * 
     * Author: nlnhat (05/08/2023)
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
     * Get warehouses
     * 
     * Author: nlnhat (02/08/2023)
     */
    async getWarehouses() {
      try {
        await this.warehouseStore.getWarehouses();
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Get units
     * 
     * Author: nlnhat (02/08/2023)
     */
    async getUnits() {
      try {
        await this.unitStore.getUnits();
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Get all materials
     * 
     * Author: nlnhat (26/06/2023)
     */
    async getMaterials() {
      await this.makeLoadingEffect(async () => {
        const response = await materialService.getAll();
        if (response?.status == this.$enums.status.ok)
          this.materials = response.data;
      })
    },
    /**
     * Get filtered materials
     * 
     * Author: nlnhat (26/06/2023)
     */
    async filterMaterials() {
      try {
        const response = await materialService.filter(this.filterComputed);
        if (response?.status == this.$enums.status.ok) {
          this.materials = response.data.Data;
          this.totalRecord = response.data.TotalRecord;
          this.allRecord = response.data.AllRecord;
          this.page.pageNumber = response.data.PageNumber;
          this.isFilterSuccess = true;
        } else if (response?.status == this.$enums.status.noContent) {
          this.materials = []
          this.totalRecord = 0
          this.allRecord = response.data.AllRecord;
          this.page.pageNumber = response.data.PageNumber;
          this.isFilterSuccess = true;
        } else this.isFilterSuccess = false;
      } catch (error) {
        console.error(error);
        this.isFilterSuccess = false;
      }
    },
    /**
     * Reload all 
     * 
     * Author: nlnhat (26/06/2023)
     */
    async reloadAll() {
      await this.makeLoadingEffect(async () => {
        await Promise.all([
          this.filterMaterials(),
          this.getWarehouses(),
          this.getUnits()
        ]);
      })
    },
    /**
     * Reload material
     * 
     * Author: nlnhat (26/06/2023)
     */
    async reloadMaterials() {
      await this.makeLoadingEffect(this.filterMaterials);
    },
    /**
     * On click reload data button
     * 
     * Author: nlnhat (29/06/2023)
     */
    async onReloadData() {
      await this.reloadAll();
      if (this.isFilterSuccess) {
        this.showToastReloadSuccess();
        return true;
      }
    },
    /**
     * Delete one material
     * 
     * Author: nlnhat (26/06/2023)
     * @param {object} material Material object to delete
     */
    async deleteMaterial(material) {
      try {
        const materialId = material.MaterialId;
        const materialCode = material.MaterialCode;
        const materialName = material.MaterialName;

        const oldIndex = this.materials.indexOf(material);

        const response = await materialService.delete(materialId);
        const refDelete = this.$refs.tr.find(tr => tr.id == materialId);

        if (response?.status == this.$enums.status.ok) {
          if (refDelete) {
            refDelete.vanish();
          }
          setTimeout(async () => {
            // Làm mới danh sách
            this.materialsSelect = this.materialsSelect.filter(id => id != materialId);
            this.materials = this.materials.filter(id => id != materialId);
            await this.filterMaterials();

            // Focus vào dòng mới
            const length = this.materials.length;
            if (length > 0) {
              const indexFocus = oldIndex < length ? oldIndex : length - 1;
              this.focusedId = this.materials[indexFocus].MaterialId;
              this.focusFocusedId();
            }
          }, 300);

          // Hiện thông báo
          const message
            = `${this.$resources['vn'].deleted} ${this.$resources['vn'].material} <${materialCode} - ${materialName}>`;
          this.showToastDeleteSuccess(message);
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Delete selected materials
     * 
     * Author: nlnhat (26/06/2023)
     */
    async deleteMaterials() {
      await this.makeLoadingEffect(async () => {
        try {
          const response = await materialService.deleteMany(this.focusedIds);

          if (response?.status == this.$enums.status.ok) {
            const deletedCount = response.data;

            // Nếu xoá được > 0 thì thông báo thành công
            if (deletedCount > 0) {
              this.showToastDeleteSuccess(`${this.$resources['vn'].deleted} ${deletedCount} ${this.$resources['vn'].material}`);

              // Nếu không xoá được hết thì báo lỗi
              if (deletedCount < this.focusedIds.length) {
                const errorDeleteCount = this.focusedIds.length - deletedCount;
                this.showToastDeleteError(`${this.$resources['vn'].cannotDelete} ${errorDeleteCount} ${this.$resources['vn'].material}`);
              }

              await this.filterMaterials();

              // Focus vào dòng mới
              const length = this.materials.length;
              if (length > 0) {
                this.focusedId = this.materials[0].MaterialId;
                this.focusedIds = [this.focusedId];
                this.focusFocusedId();
              }
            }
          }
        } catch (error) {
          console.error(error)
        }
      })
    },

    /**
     * On click delete
     * 
     * Author: nlnhat (29/06/2023)
     */
    onClickDelete() {
      if (this.focusedIds.length <= 1) {
        this.handleDeleteMaterial();
      }
      else {
        this.handleDeleteMaterials();
      }
    },
    /**
     * Handle delete one material
     * 
     * Author: nlnhat (29/06/2023)
     */
    handleDeleteMaterial() {
      try {
        const materialId = this.focusedId;
        const material = this.materials.find(material => material.MaterialId == materialId);
        if (material) {
          const materialCode = material.MaterialCode;
          const materialName = material.MaterialName;

          this.deleteConfirmDialog.content =
            `${this.$resources['vn'].deleteConfirm} ${this.$resources['vn'].material} <${materialCode} - ${materialName}> ${this.$resources['vn'].questionNo}`;

          this.deleteConfirmDialog.onClickDelete = () => {
            this.deleteConfirmDialog.isShowed = false;
            this.deleteMaterial(material);
          }
          this.deleteConfirmDialog.onClickCancel = () => {
            this.deleteConfirmDialog.isShowed = false;
          }
          this.deleteConfirmDialog.isShowed = true;
          this.focusOnButton();
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Handle delete many materials
     * 
     * Author: nlnhat (29/06/2023)
     */
    handleDeleteMaterials() {
      try {
        const oldLength = this.focusedIds.length;

        this.deleteConfirmDialog.content = `${this.$resources['vn'].deleteConfirm} ${oldLength} ${this.$resources['vn'].selectedMaterials}`;
        this.deleteConfirmDialog.onClickDelete = () => {
          this.deleteConfirmDialog.isShowed = false;
          this.deleteMaterials();
        }
        this.deleteConfirmDialog.onClickCancel = () => {
          this.deleteConfirmDialog.isShowed = false;
        }
        this.deleteConfirmDialog.isShowed = true;
        this.focusOnButton();
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Show empty material form to add new 
     * 
     * Author: nlnhat (26/06/2023)
     */
    showEmptyMaterialForm() {
      // this.focusedId = null;
      this.formMode = this.$enums.formMode.create;
      this.isShowedMaterialForm = true;
    },
    /**
     * Show material form
     * 
     * Author: nlnhat (26/06/2023)
     * 
     * @param {*} material Selected material
     */
    showMaterialForm() {
      this.formMode = this.$enums.formMode.update;
      this.isShowedMaterialForm = true;
    },
    /**
     * Duplicate material
     * 
     * Author: nlnhat (29/08/2023)
     */
    duplicateMaterial() {
      this.formMode = this.$enums.formMode.duplicate;
      this.isShowedMaterialForm = true;
    },
    /**
     * Hide material form
     * 
     * Author: nlnhat (26/06/2023)
     */
    hideMaterialForm() {
      this.isShowedMaterialForm = false;
      this.focusFocusedId();
    },
    /**
     * Show toast message after reload
     * 
     * Author: nlnhat (29/06/2023)
     */
    showToastReloadSuccess() {
      const content = this.$resources['vn'].reloadedData;
      this.$emitter.emit("showToastSuccess", content);
    },
    /**
     * Show toast message after delete success
     * 
     * Author: nlnhat (29/06/2023)
     * @param {string} content Message added
     */
    showToastDeleteSuccess(content) {
      this.$emitter.emit("showToastSuccess", content);
    },
    /**
     * Show toast message after delete error
     * 
     * Author: nlnhat (29/06/2023)
     * @param {string} content Message added
     */
    showToastDeleteError(content) {
      this.$emitter.emit("showToastError", content);
    },
    /**
     * Re render form
     *
     * Author: nlnhat (29/06/2023)
     */
    reRenderForm() {
      this.formMode = this.$enums.formMode.create;
      this.materialUpdate = {}
      this.formKey += 1;
    },
    /**
     * Handle event click on button Excel
     * 
     * Author: nlnhat (22/08/2023)
     */
    async onClickExcel() {
      await this.exportToExcel();
    },
    /**
     * Handle export to excel
     * 
     * Author: nlnhat (22/08/2023)
     */
    async exportToExcel() {
      try {
        const response = await materialService.export(this.filterComputed);
        if (response?.status == this.$enums.status.ok) {
          const blob = new Blob(
            [response.data],
            { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
          );
          const fileName = `${this.$resources['vn'].materialListFileName}_${formatDate(new Date(), 'ddMMyyyy')}.xlsx`;
          await this.download(blob, fileName);
          return true;
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Handle click feedback
     * 
     * Author: nlnhat (25/08/2023)
     */
    onClickFeedback() {
      this.openUrl(window.externalUrl.feedback);
    },
    /**
     * Handle shortcut keys
     * 
     * Author: nlnhat (25/08/2023)
     * @param {*} event Keydown event
     */
    handleShortKey(event) {
      try {
        const code = this.$enums.keyCode;

        // Handle shortcut keys on delete confirm dialog
        if (this.deleteConfirmDialog.isShowed == true) {
          this.handleShortKeyDeleteDialog(event);
        }
        // Insert: Thêm mới
        else if ((event.ctrlKey && event.keyCode == code.digit1) || (event.keyCode == code.insert)) {
          event.preventDefault();
          event.stopPropagation();
          this.showEmptyMaterialForm();
        }
        // Ctrl + F || Ctrl + F3: Focus ô tìm kiếm
        else if ((event.ctrlKey && event.keyCode == code.f) || event.keyCode == code.f3) {
          event.preventDefault();
          event.stopPropagation();
          this.focusOnSearchBox();
        }
        // Ctrl + Y: Reload
        else if (event.ctrlKey && event.keyCode == code.y) {
          event.preventDefault();
          event.stopPropagation();
          this.onReloadData();
        }
        // Ctrl + D || Delete: Delete
        else if (((event.ctrlKey && event.keyCode == code.d) || event.keyCode == code.delete)
          && this.focusedId != null) {
          event.preventDefault();
          event.stopPropagation();
          this.onClickDeleteMaterial();
        }
        // Ctrl + S || Thống kệ
        else if (event.ctrlKey && event.keyCode == code.s) {
          event.preventDefault();
          event.stopPropagation();
          this.showMaterialStat();
        }
        // Ctrl + I || Nhập khẩu
        else if (event.ctrlKey && event.keyCode == code.i) {
          event.preventDefault();
          event.stopPropagation();
          this.showMaterialImport();
        }
        // Ctrl + E: Export to excel
        else if (event.ctrlKey && event.keyCode == code.e) {
          event.preventDefault();
          event.stopPropagation();
          this.exportToExcel();
        }
        // Ctrl + A: Chọn tất
        // else if (event.ctrlKey && event.keyCode == code.a) {
        //   event.preventDefault();
        //   event.stopPropagation();
        //   this.onChangeCheckAll();
        // }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Handle shortcut keys on delete confirm dialog
     * 
     * Author: nlnhat (25/08/2023)
     * @param {*} event Keydown event
     */
    handleShortKeyDeleteDialog(event) {
      const code = this.$enums.keyCode;
      if (event.keyCode == code.enter) {
        event.stopPropagation();
        event.preventDefault();
        this.deleteConfirmDialog.onClickDelete();
      }
      else if (event.keyCode == code.n || event.keyCode == code.esc) {
        event.stopPropagation();
        event.preventDefault();
        this.deleteConfirmDialog.onClickCancel();
      }
    },
    /**
     * Prevent double click
     */
    preventDoubleClick(event) {
      event.stopPropagation();
    },
    /**
     * Focus on search box
     * 
     * Author: nlnhat (25/08/2023)
     */
    focusOnSearchBox() {
      this.$nextTick(() => {
        this.$refs.searchBox.focus();
      })
    },
    /**
     * Remove filter in a column
     * 
     * Author: nlnhat (25/08/2023)
     * @param {number} index Index of column 
     */
    removeFilterModel(index) {
      this.filterModels[index] = null;
    },
    /**
     * Remove all filters in columns
     * 
     * Author: nlnhat (25/08/2023)
     */
    removeAllFilters() {
      this.filterModels = this.filterModels.fill(null);
    },
    /**
     * Focus on button
     * 
     * Author: nlnhat (25/08/2023)
     */
    focusOnButton() {
      this.$nextTick(() => {
        this.$refs.buttonFocus.focus();
      })
    },
    /**
     * Focus on next row in table
     * 
     * Author: nlnhat (25/08/2023)
     * @param {number} index Index of row 
     */
    focusNextRow(index) {
      let newIndex = index + 1;
      if (newIndex >= this.materials.length)
        newIndex = 0;
      this.focusRow(newIndex);
    },
    /**
     * Focus on previous row in table
     * 
     * Author: nlnhat (25/08/2023)
     * @param {number} index Index of row 
     */
    focusPreviousRow(index) {
      let newIndex = index - 1;
      if (newIndex < 0)
        newIndex = this.materials.length - 1;
      this.focusRow(newIndex);
    },
    /**
     * Focus on a row by index
     * 
     * Author: nlnhat (08/08/2023)
     * @param {number} index Index of row 
     */
    focusRow(index) {
      const refFocus = this.$refs.tr.find(tr => tr.index == index);
      if (refFocus) {
        this.focusedIds = [refFocus.id];
        refFocus.focus();
      }
    },
    /**
     * Focus on a row by id
     * 
     * Author: nlnhat (08/08/2023)
     * @param {string} id Id of row 
     */
    focusById(id) {
      const refFocus = this.$refs.tr.find(tr => tr.id == id);
      if (refFocus)
        refFocus.focus();
    },
    /**
     * Focus on a row has id == focusedId
     * 
     * Author: nlnhat (08/08/2023)
     */
    focusFocusedId() {
      const refFocus = this.$refs.tr.find(tr => tr.id == this.focusedId);
      if (refFocus) {
        refFocus.focus();
      }
    },
    /**
     * Update focused id
     * 
     * Author: nlnhat (08/08/2023) 
     * @param {*} id Focused id
     */
    updateFocusedId(id) {
      this.focusedId = id;
    },
    /**
     * Update focused ids
     * 
     * Author: nlnhat (08/08/2023) 
     * @param {*} ids Focused ids
     */
    updateFocusedIds(ids) {
      this.focusedIds = ids ?? [];
    },
    /**
     * Select many from focused id to id
     * 
     * Author: nlnhat (08/08/2023) 
     * @param {*} id End id selected
     */
    selectMany(startId, endId) {
      const ids = this.materials.map(material => material.MaterialId);
      let startIndex = Math.max(ids.indexOf(startId), 0);
      let endIndex = ids.indexOf(endId);
      let newIds = [];

      if (startIndex < endIndex) {
        newIds = ids.slice(startIndex, endIndex + 1);
      }
      else {
        newIds = ids.slice(endIndex, startIndex + 1);
      }

      newIds = newIds.filter(id => !this.focusedIds.includes(id));
      this.focusedIds.push(...newIds);
    },
    /**
     * Handle delete on a row
     * 
     * Author: nlnhat (04/08/2023)
     * @param {*} materialId Id of material on this row
     */
    handleDeleteOnRow(materialId) {
      this.onClickDeleteMaterial(materialId);
    },
    /**
     * Update page property from pagination component
     * 
     * Author: nlnhat (25/08/2023)
     * @param {*} page Page object from pagination component
     */
    async updatePage(page) {
      this.page = page;
      await this.reloadMaterials();
    },
    /**
     * Show MaterialStat
     *
     * Author: nlnhat (08/09/2023)
     */
    showMaterialStat() {
      this.isShowMaterialStat = true;
    },
    /**
     * Close MaterialStat
     *
     * Author: nlnhat (08/09/2023)
     */
    closeMaterialStat() {
      this.isShowMaterialStat = false;
    },
    /**
     * Show MaterialImport
     *
     * Author: nlnhat (08/09/2023)
     */
    showMaterialImport() {
      this.isShowMaterialImport = true;
    },
    /**
     * Close MaterialImport
     *
     * Author: nlnhat (08/09/2023)
     */
    closeMaterialImport() {
      this.isShowMaterialImport = false;
    },
    /**
     * Imported methods
     */
    formatDate,
    formatStringByDot,
    formatStringBySpace,
    formatDecimal,
    sortByName,
    download,
    openUrl,
  }
}
</script>
<style>
@import "@/css/views/materials/index.css";
</style>