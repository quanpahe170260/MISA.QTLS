/**
 * Hàm format date về múi giờ VN
 * @param {date} isoDate
 * @returns
 * CreatedBy: QuanPA - 20/11/2025
 */
export const formatDateVN = (isoDate) => {
  if (!isoDate) return ''

  const date = new Date(isoDate)

  // +7 giờ (Việt Nam)
  const vnDate = new Date(date.getTime() + 7 * 60 * 60 * 1000)

  const day = String(vnDate.getDate()).padStart(2, '0')
  const month = String(vnDate.getMonth() + 1).padStart(2, '0')
  const year = vnDate.getFullYear()

  return `${day}/${month}/${year}`
}
