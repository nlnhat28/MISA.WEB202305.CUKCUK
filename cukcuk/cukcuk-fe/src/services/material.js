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
   * @return Response
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
   * @param {Object} data Data chứa các thuộc tính lọc
   * @return Response
   */
  async filter(data) {
    const response = await this.baseRequest.post(
      this.baseUrl + "/Filter",
      data
    );
    return response;
  }
  /**
   * Export to excel
   *
   * Author: nlnhat (17/08/2023)
   * @param {Object} data Data filter
   * @return Response
   */
  async export(data) {
    const response = await this.baseRequest.post(
      this.baseUrl + "/Excel",
      data,
      {
        responseType: "blob",
      }
    );
    return response;
  }
}

const materialService = new MaterialService();

export default materialService;
