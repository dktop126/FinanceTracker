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

const groupedTransactions = computed(() => {
  const groups = {};
  // Сортируем по дате (от новых к старым), если бэкенд не отсортировал
  const sorted = [...transactions.value].sort((a, b) => new Date(b.date) - new Date(a.date));

  sorted.forEach(t => {
    const date = formatDate(t.date);
    if (!groups[date]) groups[date] = [];
    groups[date].push(t);
  });
  return groups;
});

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
  <div class="main-container">
    <div class="card add-transaction-card">
      <h2>Добавление транзакции</h2>

      <div class="type-selector">
        <label :class="{ active: selectedType === 1 }" class="type-label income">
          <input type="radio" :value="1" v-model="selectedType" /> Пополнение
        </label>
        <label :class="{ active: selectedType === 2 }" class="type-label expense">
          <input type="radio" :value="2" v-model="selectedType" /> Расход
        </label>
      </div>

      <div class="form-grid">
        <div class="form-group">
          <label>Сумма</label>
          <input type="number" v-model="transactionAmount" placeholder="0.00" />
        </div>

        <div class="form-group">
          <label>Категория</label>
          <select v-model="selectedCategoryId" :disabled="isLoading">
            <option :value="null" disabled>Выберите категорию</option>
            <option v-for="category in filteredCategories" :key="category.id" :value="category.id">
              {{ category.name }}
            </option>
          </select>
        </div>

        <div class="form-group">
          <label>Счёт</label>
          <select v-model="selectedAccountId" :disabled="isLoading">
            <option :value="null" disabled>Выберите счёт</option>
            <option v-for="account in accounts" :key="account.id" :value="account.id">
              {{ account.name }}
            </option>
          </select>
        </div>
      </div>

      <button class="submit-btn" @click="createTransaction" :disabled="isLoading">
        Добавить транзакцию
      </button>
      <p v-if="message" class="status-message">{{ message }}</p>
    </div>

    <div class="transactions-container">
      <div class="header-row">
        <h1>История операций</h1>
      </div>

      <p v-if="isLoading" class="info-text">Загрузка данных...</p>

      <div v-else-if="transactions.length > 0">
        <div v-for="(items, date) in groupedTransactions" :key="date" class="date-group">
          <h3 class="date-divider"><span>{{ date }}</span></h3>

          <div v-for="t in items" :key="t.transactionId" class="transaction-card">
            <div class="t-main">
              <div class="t-icon" :class="t.transactionType === 1 ? 'icon-up' : 'icon-down'">
                {{ t.transactionType === 1 ? '↓' : '↑' }}
              </div>
              <div class="t-details">
                <span class="t-category">{{ t.categoryName }}</span>
                <span class="t-account">{{ t.accountName }}</span>
              </div>
            </div>

            <div class="t-amount" :class="t.transactionType === 1 ? 'text-income' : 'text-expense'">
              {{ t.transactionType === 1 ? '+' : '-' }} {{ t.amount.toLocaleString() }} ₽
            </div>

            <button @click="deleteTransaction(t.transactionId)" class="delete-icon-btn" title="Удалить">
              🗑
            </button>
          </div>
        </div>
      </div>

      <p v-else class="info-text">Транзакций пока нет.</p>
    </div>
  </div>
</template>

<style scoped>
/* Общий контейнер */
.main-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Inter', sans-serif;
  color: #e0e0e0;
  background-color: #121212;
  min-height: 100vh;
}

/* Карточки */
.card {
  background: #1e1e1e;
  border-radius: 16px;
  padding: 24px;
  margin-bottom: 30px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.3);
}

/* Форма добавления */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 15px;
  margin-bottom: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
  text-align: left;
}

.form-group label {
  font-size: 12px;
  color: #888;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

input, select {
  background: #2d2d2d;
  border: 1px solid #444;
  color: white;
  padding: 10px;
  border-radius: 8px;
  outline: none;
}

input:focus, select:focus {
  border-color: #bb86fc;
}

/* Селектор типа (Доход/Расход) */
.type-selector {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
  justify-content: center;
}

.type-label {
  padding: 8px 20px;
  border-radius: 20px;
  cursor: pointer;
  border: 1px solid #444;
  transition: 0.3s;
}

.type-label.active.income { background: #1b5e20; border-color: #4caf50; }
.type-label.active.expense { background: #b71c1c; border-color: #f44336; }
.type-label input { display: none; }

/* Кнопка */
.submit-btn {
  width: 100%;
  padding: 12px;
  background: #bb86fc;
  color: #000;
  border: none;
  border-radius: 8px;
  font-weight: bold;
  cursor: pointer;
  transition: 0.3s;
}

.submit-btn:hover { background: #9965f4; }

/* Список транзакций */
.date-divider {
  display: flex;
  align-items: center;
  text-align: center;
  color: #888;
  font-size: 14px;
  margin: 25px 0 15px;
}

.date-divider::before, .date-divider::after {
  content: '';
  flex: 1;
  border-bottom: 1px solid #333;
}

.date-divider span { padding: 0 15px; }

.transaction-card {
  display: flex;
  align-items: center;
  background: #1e1e1e;
  padding: 12px 16px;
  border-radius: 12px;
  margin-bottom: 8px;
  transition: transform 0.2s;
}

.transaction-card:hover {
  transform: translateX(5px);
  background: #252525;
}

.t-main {
  display: flex;
  align-items: center;
  gap: 15px;
  flex: 1;
}

.t-icon {
  width: 35px;
  height: 35px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
}

.icon-up { background: rgba(76, 175, 80, 0.1); color: #4caf50; }
.icon-down { background: rgba(244, 67, 54, 0.1); color: #f44336; }

.t-details {
  display: flex;
  flex-direction: column;
  text-align: left;
}

.t-category { font-weight: 600; font-size: 15px; }
.t-account { font-size: 12px; color: #888; }

.t-amount {
  font-weight: bold;
  font-size: 16px;
  margin-right: 20px;
}

.text-income { color: #4caf50; }
.text-expense { color: #f44336; }

.delete-icon-btn {
  background: transparent;
  border: none;
  color: #555;
  cursor: pointer;
  font-size: 18px;
  transition: 0.2s;
}

.delete-icon-btn:hover { color: #f44336; }

.header-row {
  display: flex;
  justify-content: space-between;
  align-items: stretch;
  margin-bottom: 10px;
}

.nav-links a { color: #bb86fc; text-decoration: none; font-size: 14px; }
</style>