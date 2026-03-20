import { createRouter, createWebHistory } from 'vue-router'
import CategoriesView from '../views/CategoriesView.vue'
import TransactionsView from '../views/TransactionsView.vue'

const routes = [
    { path: '/', redirect: '/categories' },
    { path: '/categories', component: CategoriesView },
    { path: '/transactions', component: TransactionsView }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router