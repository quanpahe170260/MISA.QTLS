import { createRouter, createWebHistory } from 'vue-router'
import Asset from '@/views/asset/Asset.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: Asset,
    },
  ],
})

export default router
