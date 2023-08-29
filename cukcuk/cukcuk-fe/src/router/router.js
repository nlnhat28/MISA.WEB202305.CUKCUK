import { createRouter, createWebHistory } from 'vue-router'
import HomeIndex from '@/views/home/HomeIndex.vue'
import MaterialsList from '@/views/materials/MaterialsList.vue'

const routes = [
  {
    path: "/",
    component: HomeIndex,
  },
  {
    path: "/home",
    component: HomeIndex,
  },
  {
    path: "/materials",
    component: MaterialsList,
  }
];
const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

export default router
