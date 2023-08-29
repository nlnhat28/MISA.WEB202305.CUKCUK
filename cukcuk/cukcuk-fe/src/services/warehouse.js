import BaseService from "./base.js";
/**
 * Service kho
 * 
 * Author: nlnhat (17/08/2023)
 */
class WarehouseService extends BaseService {
  constructor() {
    super("Warehouses");
  }
}

const warehouseService = new WarehouseService();

export default warehouseService;
