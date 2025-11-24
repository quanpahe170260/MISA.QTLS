import api from '@/apis/config/ApiConfig.js'

export default class BaseApi {
  constructor() {
    this.controler = null
  }
  /**
   * Phương thức lấy tất cả dữ liệu
   */
  getAll() {
    return api.get(`${this.controler}`)
  }
  /**
   * Hàm lấy dữ liệu theo id
   * @param {*} id
   * @returns
   */
  getById(id) {
    return api.get(`${this.controler}/${id}`)
  }
  /**
   * Hàm lấy dữ liệu phân trang
   * @param {*} payload
   */
  paging(payload) {
    return api.post(`${this.controler}/paging`, payload)
  }
  /**
   * Hàm thêm mới dữ liệu
   * @param {*} body
   * @returns
   */
  create(body) {
    return api.post(`${this.controler}`, body)
  }
  /**
   * Hàm cập nhật dữ liệu
   * @param {*} id
   * @param {*} body
   */
  update(id, body) {
    return api.put(`${this.controler}/${id}`, body)
  }
  /**
   * Hàm xóa bản ghi
   * @param {*} id
   */
  delete(id) {
    return api.delete(`${this.controler}/${id}`)
  }

  deleteMultiple(lsId) {
    return api.delete(`${this.controler}`, {
      data: lsId,
    })
  }
}
