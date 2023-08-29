import resources from "@/constants/resources.js";

export const fields = {
  materialName: {
    label: resources["vn"].name,
    maxLength: 255,
  },
  materialCode: {
    label: resources["vn"].code,
    maxLength: 20,
  },
  unit: {
    label: resources["vn"].unit,
    maxLength: 255,
  },
  conversionUnit: {
    label: resources["vn"].conversionUnit,
    maxLength: 255,
  },
  warehouse: {
    label: resources["vn"].warehouse,
    maxLength: 255,
  },
  expiryTime: {
    label: resources["vn"].expiryTime,
    maxLength: 20,
  },
  timeUnit: {
    label: resources["vn"].timeUnit,
    maxLength: 255,
  },
  minimumInventory: {
    label: resources["vn"].minimumInventory,
    title: resources["vn"].minimumInventoryFullTitle,
    maxLength: 21,
  },
  note: {
    label: resources["vn"].note,
    maxLength: 255,
  },
  operator: {
    label: resources["vn"].operator,
    maxLength: 255,
  },
  unitName: {
    label: resources["vn"].unitName,
    maxLength: 255,
  },
  description: {
    label: resources["vn"].description,
    maxLength: 255,
  },
  warehouseCode: {
    label: resources["vn"].warehouseCode,
    maxLength: 20,
  },
  warehouseName: {
    label: resources["vn"].warehouseName,
    maxLength: 255,
  },
  address: {
    label: resources["vn"].address,
    maxLength: 255,
  },
};
