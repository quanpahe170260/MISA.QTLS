/**
 * Hàm format sô về dang 0.000.000
 * @param {number} value
 * @returns
 * CreatedBy: QuanPA - 20/11/2025
 */
export const formatNumber = (value) => {
  if (value === null || value === undefined || value === '') return ''
  return new Intl.NumberFormat('vi-VN').format(value)
}
