import BaseAPI from '@/apis/base/BaseApi.js'
import api from '../config/ApiConfig'

class AssetTypeApi extends BaseAPI {
  constructor() {
    super()
    this.controler = 'AssetType'
  }
}
export default new AssetTypeApi()
