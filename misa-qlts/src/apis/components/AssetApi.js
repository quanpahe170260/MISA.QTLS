import BaseAPI from '@/apis/base/BaseApi.js'
import api from '../config/ApiConfig'

class AssetApi extends BaseAPI {
  constructor() {
    super()
    this.controler = 'Asset'
  }

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
}
export default new AssetApi()
