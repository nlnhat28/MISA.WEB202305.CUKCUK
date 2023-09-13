import { defineStore } from "pinia";
import { sortByName } from "@/js/utils/array.js";
import { warehouseService } from "@/services/services.js";
import enums from "@/constants/enums.js"

const useWarehouseStore = defineStore("warehouse", {
  state: () => ({
    warehouses: [],
    warehouseSelects: [],
    warehouseNameSelects: [],
  }),
  actions: {
    async getWarehouses() {
      let selects = [];
      let nameSelects = [];
      const response = await warehouseService.getAll();
      if (response?.status == enums.status.ok) {
        this.warehouses = response.data;
        this.warehouses.forEach((warehouse) => {
          selects.push({
            id: warehouse.WarehouseId,
            name: warehouse.WarehouseCode,
          });
          
          nameSelects.push({
            id: warehouse.WarehouseId,
            name: warehouse.WarehouseName,
          });
        });
        this.warehouseSelects = sortByName(selects);
        this.warehouseNameSelects = sortByName(nameSelects);
      }
    },
  },
});

export default useWarehouseStore; 
