import { ref } from 'vue'

export const showToast = ref(false)
export const toastData = ref({
  type: '',
  title: '',
  message: '',
  icon: '',
})

export function openToast(type, title, message) {
  toastData.value = {
    type,
    title,
    message,
    icon: getIcon(type),
  }

  showToast.value = true

  setTimeout(() => {
    showToast.value = false
  }, 3000)
}

function getIcon(type) {
  switch (type) {
    case 'success':
      return 'fa-solid fa-circle-check fa-lg" style="color: #00d192;'
    case 'error':
      return 'fa-solid fa-circle-exclamation fa-lg" style="color: #ff0000;'
    case 'warning':
      return 'fa-solid fa-circle-exclamation fa-lg" style="color: #FFD43B;'
    case 'info':
      return 'fa-solid fa-circle-info fa-lg" style="color: #FFD43B;'
    default:
      return 'fa-solid fa-circle-info fa-lg" style="color: #FFD43B;'
  }
}
