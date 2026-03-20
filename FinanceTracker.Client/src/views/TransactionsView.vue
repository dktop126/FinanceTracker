<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const transactions = ref([])
const isLoading = ref(true)
const transactionLabels = {
  1: 'Пополнение',
  2: 'Трата'
};

async function fetchTransactions() {
  try {
    const response = await axios.get('https://localhost:44352/api/finance/transactions')
    transactions.value = response.data // Записываем массив из API в нашу переменную
  } catch (error) {
    console.error("Ошибка при загрузке транзакций:", error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchTransactions()
})
</script>

<template>
  <div class="container">
    <h1>Список транзакций</h1>

    <p v-if="isLoading">Загрузка данных...</p>

    <ul v-else-if="transactions.length > 0">
      <li v-for="transaction in transactions" :key="transaction.id" class="transaction-item">
        <div class="transaction-info">
          <span class="category">{{ transaction.category }}</span>
          <span class="amount">Сумма: {{ transaction.amount }}</span>
          <span class="transactionType">{{ transactionLabels[transaction.transactionType] }}</span>
          <span class="date">{{ transaction.date }}</span>
        </div>

        <button @click="deleteCategory(category.id)" class="delete-btn" style="margin-left: auto;">
          Удалить
        </button>
      </li>
    </ul>

    <p v-else>Транзакций пока нет. Создайте первую!</p>
  </div>
</template>

<style scoped>
.transaction-info {
  display: flex;
  gap: 15px;
  flex-grow: 1;
}
.transaction-item {
  list-style: none;
  padding: 10px 10px 10px 10px;
  border-bottom: 1px solid #505050;
  display: flex;
  gap: 20px;
}

.date {
  margin-left: auto;
}
</style>