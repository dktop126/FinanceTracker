<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'

const transactions = ref([])
const accounts = ref([])
const amount = ref(0)
const transactionAmount = ref()
const selectedType = ref(2)
const categories = ref([])
const selectedCategoryId = ref(null)
const selectedAccountId = ref(null)
const message = ref('')
const isLoading = ref(true)
const transactionLabels = {
  1: 'Пополнение',
  2: 'Трата'
};

async function createTransaction() {
  if (!selectedCategoryId.value || transactionAmount.value <= 0) {
    alert("Выберите категорию и введите сумму");
    return;
  }
  try {
    const response = await axios.post('https://localhost:44352/api/finance/transactions', {
      amount: transactionAmount.value,
      accountId: selectedAccountId.value,
      categoryId: selectedCategoryId.value,
      transactionType: selectedType.value
    })
    message.value = `Успех!`
    await fetchTransactions()
    amount.value = 0
  } catch (error) {
    message.value = 'Ошибка при создании'
    console.error("Ошибка при создании транзакции:", error.response?.data || error.message);
  }
}

async function fetchTransactions() {
  try {
    const response = await axios.get('https://localhost:44352/api/finance/transactions')
    transactions.value = response.data 
  } catch (error) {
    console.error("Ошибка при загрузке транзакций:", error)
  } finally {
    isLoading.value = false
  }
}

async function deleteTransaction(id) {
  if (!confirm('Вы уверены, что хотите удалить эту транзакцию?')) return;

  try {
    await axios.delete(`https://localhost:44352/api/Finance/transactions/${id}`);

    transactions.value = transactions.value.filter(c => c.id !== id);

    alert('Транзакция удалена');
    await fetchTransactions()

  } catch (error) {
    console.error("Ошибка при удалении:", error);
    alert('Не удалось удалить транзакцию.');
  }
}

async function fetchCategories() {
  try {
    const response = await axios.get('https://localhost:44352/api/Finance/categories')
    categories.value = response.data
  } catch (error) {
    console.error("Ошибка при загрузке категорий:", error)
  } finally {
    isLoading.value = false
  }
};

async function fetchAccounts() {
  try {
    const response = await axios.get('https://localhost:44352/api/Finance/accounts')
    accounts.value = response.data
  } catch (error) {
    console.error("Ошибка при загрузке Accounts:", error)
  } finally {
    isLoading.value = false
  }
};

const filteredCategories = computed(() => {
  return categories.value.filter(c => c.type === selectedType.value)
})

const formatDate = (dateString) => {
  if (!dateString) return '';
  const date = new Date(dateString);
  
  return new Intl.DateTimeFormat('ru-RU', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  }).format(date);
};

onMounted(() => { fetchTransactions() })
onMounted(() => { fetchCategories() })
onMounted(() => { fetchAccounts() })

</script>

<template>

  <div class="transaction-type-selector">
    <h1>Добавление транзакции</h1>
    <input type="radio" id="income" :value="1" v-model="selectedType" />
    <label for="income">Доход</label>

    <input type="radio" id="expense" :value="2" v-model="selectedType" />
    <label for="expense">Расход</label>
  </div>
  <div>
    <label>Сумма:</label>
    <input type="number" v-model="transactionAmount" placeholder="0.00" />
    <button @click="createTransaction">Добавить транзакцию</button>
    <p>{{ message }}</p>
  </div>
  
  <div class="category-selector">
    <label>Категория транзакции:</label>

    <select v-model="selectedCategoryId" :disabled="isLoading">
      <option :value="null" disabled>
        {{ isLoading ? 'Загрузка...' : 'Выберите категорию' }}
      </option>

      <option v-for="category in filteredCategories" :key="category.id" :value="category.id">
        {{ category.name }}
      </option>
    </select>

    <!-- Кнопка обновления, если список не загрузился -->
    <button v-if="!isLoading && categories?.length === 0" @click="fetchCategories">
      Обновить
    </button>
  </div>

  <div class="account-selector">
    <label>Аккаунт:</label>

    <select v-model="selectedAccountId" :disabled="isLoading">
      <option :value="null" disabled>
        {{ isLoading ? 'Загрузка...' : 'Выберите аккаунт' }}
      </option>

      <option v-for="account in accounts" :key="account.id" :value="account.id">
        {{ account.name }} (Баланс: {{ account.balance }})
      </option>
    </select>
  </div>
  
  <div class="container">
    <h1>Список транзакций</h1>

    <p v-if="isLoading">Загрузка данных...</p>

    <ul v-else-if="transactions.length > 0">
      <li v-for="transaction in transactions" :key="transaction.transactionId" class="transaction-item">
        <div class="transaction-info">
          <span class="category">{{ transaction.categoryName }}</span>
          <span class="amount">Сумма: {{ transaction.amount }}</span>
          <span class="transactionType">{{ transactionLabels[transaction.transactionType] }}</span>
          <span class="date">{{ formatDate(transaction.date) }}</span>
          <span class="account">{{ transaction.accountName }}</span>
        </div>

        <button @click="deleteTransaction(transaction.transactionId)" class="delete-btn" style="margin-left: auto;">
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

.date { margin-left: auto; }

.category {
  flex: 0 0 100px; /* 150px — фиксированная ширина. Не растет и не сжимается */
  white-space: nowrap; /* Текст в одну строку */
  text-align: left;
  overflow: hidden;    /* Скрываем то, что не влезло */
  text-overflow: ellipsis; /* Добавляем "..." если название категории слишком длинное */
}
.amount {
  flex: 0 0 150px; /* 150px — фиксированная ширина. Не растет и не сжимается */
  text-align: left;
  white-space: nowrap; /* Текст в одну строку */
  overflow: hidden;    /* Скрываем то, что не влезло */
  text-overflow: ellipsis; /* Добавляем "..." если название категории слишком длинное */
}
</style>