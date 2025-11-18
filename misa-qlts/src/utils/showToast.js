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
      return 'icon-success icon-mask'
    case 'error':
      return 'icon-exclamation'
    case 'warning':
      return 'icon-warning icon-default'
    case 'info':
      return 'icon-info'
    default:
      return 'icon-info'
  }
}
