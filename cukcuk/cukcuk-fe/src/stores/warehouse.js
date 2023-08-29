import { defineStore } from "pinia";
import { sortByName } from "@/js/utils/array.js";
import { warehouseService } from "@/services/services.js";
import enums from "@/constants/enums.js"

const useWarehouseStore = defineStore("warehouse", {
  state: () => ({
    warehouses: [],
    warehouseSelects: [],
  }),
  actions: {
    async getWarehouses() {
      let selects = [];
      const response = await warehouseService.getAll();
      if (response?.status == enums.status.ok) {
        this.warehouses = response.data;
        this.warehouses.forEach((warehouse) => {
          selects.push({
            id: warehouse.WarehouseId,
            name: warehouse.WarehouseName,
          });
        });
        this.warehouseSelects = sortByName(selects);
      }
    },
  },
});

export default useWarehouseStore; 
