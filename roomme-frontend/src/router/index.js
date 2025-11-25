// src/router/index.js
import { createRouter, createWebHistory } from 'vue-router'
import AuthLayout from '@/views/layouts/AuthLayout.vue'
import MainLayout from '@/views/layouts/MainLayout.vue'

const routes = [
  //mainpage
  {
    path: '',
    component: MainLayout,
  },
  //auth routes
  {
    path: '/auth',
    component: AuthLayout,
    children:[
      {
        path:'login',
        name: 'login',
        component: ()=> import('@/views/authViews/LoginView.vue')
      },
      {
        path:'register',
        name: 'register',
        component: ()=> import('@/views/authViews/RegisterView.vue')
      }
    ]
  }
  
]

// 3. Crea el router de forma estándar
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes, // Le pasas tu array manual
})

export default router