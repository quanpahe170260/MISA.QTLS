<template>
  <div class="d-flex flex-direction-row">
    <the-sidebar />
    <div class="d-flex flex1 flex-direction-column">
      <the-header />
      <RouterView></RouterView>
    </div>
    <div class="toast-wrapper">
      <ms-toast v-if="showToast" :type="toastData.type" :title="toastData.title" :message="toastData.message"
        :icon="toastData.icon" />
    </div>
  </div>
</template>
<script setup>
import TheSidebar from '@/layouts/TheSidebar.vue';
import TheHeader from '@/layouts/TheHeader.vue';
import MsToast from '@/components/ms-toast/MsToast.vue';
import { showToast, toastData } from "@/utils/showToast.js";
import '@/assets/css/base.css';
import '@/assets/css/icon.css';
</script>
<style scoped>
.toast-wrapper {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 9999;

  /* Cho toast fade-in đẹp hơn */
  animation: fadeIn 0.25s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateX(20px);
  }

  to {
    opacity: 1;
    transform: translateX(0);
  }
}
</style>