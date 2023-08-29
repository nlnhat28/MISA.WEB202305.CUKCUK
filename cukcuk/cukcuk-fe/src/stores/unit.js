import { defineStore } from "pinia";
import { sortByName } from "@/js/utils/array.js";
import { unitService } from "@/services/services.js";
import enums from "@/constants/enums.js"

const useUnitStore = defineStore("unit", {
  state: () => ({
    units: [],
    unitSelects: [],
  }),
  actions: {
    async getUnits() {
      let selects = [];
      const response = await unitService.getAll();
      if (response?.status == enums.status.ok) {
        this.units = response.data;
        this.units.forEach((unit) => {
          selects.push({
            id: unit.UnitId,
            name: unit.UnitName,
          });
        });
        this.unitSelects = sortByName(selects);
      }
    },
  },
});

export default useUnitStore; 
