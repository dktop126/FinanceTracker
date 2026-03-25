<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'

const accounts = ref([])
const isLoading = ref(true)

// Считаем общую сумму всех аккаунтов
const totalBalance = computed(() => {
  return accounts.value.reduce((sum, account) => sum + account.balance, 0)
})

async function fetchAccounts() {
  try {
    const response = await axios.get('https://localhost:44352/api/Finance/accounts')
    accounts.value = response.data
  } catch (error) {
    console.error("Ошибка при загрузке баланса:", error)
  } finally {
    isLoading.value = false
  }
}

onMounted(fetchAccounts)
</script>

<template>
  <header class="header">
    <div class="header-content">
      <div class="logo">
        <span class="logo-icon">💰</span>
        <span class="logo-text">FinanceTracker</span>
      </div>

      <nav class="main-nav">
        <router-link to="/categories" class="nav-item">📁 Категории</router-link>
        <router-link to="/transactions" class="nav-item">💸 Транзакции</router-link>
      </nav>

      <div class="header-right">
        <div class="balance-display" :class="{ 'is-loading': isLoading }">
          <span class="balance-label">Общий баланс</span>
          <span class="balance-value">
            {{ isLoading ? '...' : totalBalance.toLocaleString() + ' ₽' }}
          </span>
        </div>

        <div class="user-profile">
          <div class="user-avatar">D</div>
        </div>
      </div>
    </div>
  </header>

  <main class="app-content">
    <router-view />
  </main>
</template>

<style>
/* Сброс базовых отступов для всей страницы, если еще нет */
body {
  margin: 0;
  padding: 0;
  background-color: #121212;
}

.header {
  background: #1e1e1e;
  border-bottom: 1px solid #333;
  position: sticky;
  top: 0;
  z-index: 100;
  padding: 0 20px;
}

.header-content {
  max-width: 1000px;
  margin: 0 auto;
  height: 70px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 20px;
}

.balance-display {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  padding: 5px 12px;
  background: rgba(187, 134, 252, 0.05);
  border-right: 2px solid #bb86fc;
  border-radius: 4px 0 0 4px;
}

.balance-label {
  font-size: 10px;
  text-transform: uppercase;
  color: #888;
  letter-spacing: 1px;
}

.balance-value {
  font-weight: 700;
  font-size: 1.1rem;
  color: #fff;
  transition: all 0.3s ease;
}

/* Эффект пульсации при загрузке */
.is-loading .balance-value {
  opacity: 0.5;
}

.user-avatar {
  width: 35px;
  height: 35px;
  background: #333;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  color: #bb86fc;
  border: 1px solid #444;
}
/* Логотип */
.logo {
  display: flex;
  align-items: center;
  gap: 10px;
}

.logo-text {
  font-weight: 800;
  font-size: 1.2rem;
  color: #bb86fc;
  letter-spacing: -0.5px;
}

/* Навигация */
.main-nav {
  display: flex;
  gap: 8px;
  background: #252525;
  padding: 5px;
  border-radius: 12px;
}

.nav-item {
  text-decoration: none;
  color: #888;
  padding: 8px 16px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 8px;
}

.nav-item:hover {
  color: #fff;
  background: #333;
}

/* Активная ссылка */
.router-link-active {
  background: #bb86fc !important;
  color: #000 !important;
  box-shadow: 0 4px 12px rgba(187, 134, 252, 0.3);
}

/* Профиль пользователя */
.user-profile {
  display: flex;
  align-items: center;
  gap: 12px;
}

.user-name {
  font-size: 14px;
  color: #e0e0e0;
}

.user-avatar {
  width: 35px;
  height: 35px;
  background: #333;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  color: #bb86fc;
  border: 1px solid #444;
}

/* Контент под навигацией */
.app-content {
  padding-top: 20px;
}
</style>