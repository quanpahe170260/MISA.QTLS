import BaseAPI from '@/apis/base/BaseApi.js'
import api from '../config/ApiConfig'

class AssetApi extends BaseAPI {
  constructor() {
    super()
    this.controler = 'Asset'
  }

  /**
   * Hàm lấy danh sách tài sản có phân trang, tìm kiếm ,loc
   * @param {*} search
   * @param {*} departmentID
   * @param {*} assetTypeID
   * @param {*} page
   * @param {*} pageSize
   * @returns
   * CreatedBy: QuanPA - 17/11/2025
   */
  getAllAssetPaging(search, departmentID, assetTypeID, page, pageSize) {
    return api.get(`${this.controler}/paging`, {
      params: {
        search,
        departmentID,
        assetTypeID,
        page,
        pageSize,
      },
    })
  }

  /**
   * Hàm lấy mã tài sản
   * @returns
   * CreatedBy: QuanPA - 17/11/2025
   */
  generateAssetCode() {
    return api.get(`${this.controler}/generate`)
  }

  /**
   * Hàm lấy tổng tài sản
   * CreatedBy: QuanPA - 17/11/2025
   */
  getTotalAsset() {
    return api.get(`${this.controler}/total`)
  }
}
export default new AssetApi()
