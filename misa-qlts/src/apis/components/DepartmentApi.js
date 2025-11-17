import BaseAPI from '@/apis/base/BaseApi.js'
import api from '../config/ApiConfig'

class DepartmentApi extends BaseAPI {
  constructor() {
    super()
    this.controler = 'Department'
  }
}
export default new DepartmentApi()
