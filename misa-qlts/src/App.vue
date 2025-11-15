<template>
  <div class="d-flex flex-direction-row">
    <the-sidebar />
    <div class="d-flex flex1 flex-direction-column">
      <the-header />
      <RouterView></RouterView>
    </div>
  </div>
</template>
<script setup>
import TheSidebar from '@/layouts/TheSidebar.vue';
import TheHeader from '@/layouts/TheHeader.vue';
import '@/assets/css/base.css';
import '@/assets/css/icon.css';
</script>
<style scoped></style>