import BaseService from "./base.js";
/**
 * Service nguyên vật liệu
 *
 * Author: nlnhat (17/08/2023)
 */
class MaterialService extends BaseService {
  constructor() {
    super("Materials");
  }
  /**
   * Get new material code
   *
   * Author: nlnhat (17/08/2023)
   * @param {*} name Tên nguyên vật liệu
   * @return Code mới
   */
  async getNewCode(name) {
    const response = await this.baseRequest.get(this.baseUrl + "/NewCode", {
      params: {
        materialName: name,
      },
    });
    return response;
  }
  /**
   * Get filtered materials
   *
   * Author: nlnhat (17/08/2023)
   * @param {Object} dataFilter Data chứa các thuộc tính lọc
   * @return Danh sách nguyên vật liệu được lọc
   */
  async filter(dataFilter) {
    const response = await this.baseRequest.post(
      this.baseUrl + "/Filter",
      dataFilter
    );
    return response;
  }
  /**
   * Export to excel
   *
   * Author: nlnhat (17/08/2023)
   * @param {Object} dataFilter Data filter
   * @return Dữ liệu excel
   */
  async export(dataFilter) {
    const response = await this.baseRequest.post(
      this.baseUrl + "/Excel/Export",
      dataFilter,
      {
        responseType: "blob",
      }
    );
    return response;
  }
  /**
   * Get import template file
   *
   * Author: nlnhat (11/09/2023)
   * @return File nhập khẩu mẫu
   */
  async getImportTemplate() {
    const response = await this.baseRequest.get(
      this.baseUrl + "/Excel/ImportTemplate",
      {
        responseType: "blob",
      }
    );
    return response;
  }
  /**
   * Map data from import file
   *
   * Author: nlnhat (11/09/2023)
   * @param {Object} formData Form data
   * @return Danh sách nguyên vật liệu được map
   */
  async mapImport(formData) {
    const response = await this.baseRequest.post(
      this.baseUrl + "/Excel/MappingImport",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );
    return response;
  }
  /**
   * Map data from import file
   *
   * Author: nlnhat (11/09/2023)
   * @param {Array} materials Danh sách nguyên vật liệu
   * @return Số lượng thành công
   */
  async import(materials) {
    const response = await this.baseRequest.post(
      this.baseUrl + "/Excel/Import",
      materials
    );
    return response;
  }
  /**
   * Get count by year
   *
   * Author: nlnhat (17/08/2023)
   * @return Số lượng thêm/xoá theo năm
   */
  async countByYear() {
    const response = await this.baseRequest.get(this.baseUrl + "/CountByYear");
    return response;
  }
  /**
   * Get count by warehouse
   *
   * Author: nlnhat (17/08/2023)
   * @return Số lượng theo kho
   */
  async countByWarehouse() {
    const response = await this.baseRequest.get(
      this.baseUrl + "/CountByWarehouse"
    );
    return response;
  }
  /**
   * Get count by follow state
   *
   * Author: nlnhat (17/08/2023)
   * @return Số lượng theo trạng thái theo dõi
   */
  async countByFollow() {
    const response = await this.baseRequest.get(
      this.baseUrl + "/CountByFollow"
    );
    return response;
  }
}

const materialService = new MaterialService();

export default materialService;
