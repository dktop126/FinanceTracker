<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const categoryName = ref('')
const selectedType = ref(2)
const message = ref('')
const categories = ref([])
const isLoading = ref(true)
const transactionLabels = {
  1: 'Доход',
  2: 'Расход'
};

async function createCategory() {
  try {
    const response = await axios.post('https://localhost:44352/api/finance/categories', {
      name: categoryName.value,
      icon: '📁',
      type: selectedType.value
    })
    message.value = `Успех! ID категории: ${response.data.categoryId}`
    await fetchCategories()
  } catch (error) {
    message.value = 'Ошибка при создании'
    console.error(error)
  }
}

async function fetchCategories() {
  try {
    const response = await axios.get('https://localhost:44352/api/finance/categories')
    categories.value = response.data
  } catch (error) {
    console.error("Ошибка при загрузке категорий:", error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  fetchCategories()
})

async function deleteCategory(id) {
  if (!confirm('Вы уверены, что хотите удалить эту категорию?')) return;

  try {
    await axios.delete(`https://localhost:44352/api/finance/categories/${id}`);

    categories.value = categories.value.filter(c => c.id !== id);

    alert('Категория удалена');
    await fetchCategories()
    
  } catch (error) {
    console.error("Ошибка при удалении:", error);
    alert('Не удалось удалить категорию. Возможно, она используется в транзакциях.');
  }
}
</script>

<template>
  <div>
    <h1>Добавление категории</h1>
    <input v-model="categoryName" placeholder="Название категории" />
    &nbsp;
    <button @click="createCategory">Добавить категорию</button>
    <p>{{ message }}</p>
  </div>
  <div class="transaction-type-selector">
    <input type="radio" id="income" :value="1" v-model="selectedType" />
    <label for="income">Доход</label>
    <input type="radio" id="expense" :value="2" v-model="selectedType" />
    <label for="expense">Расход</label>
  </div>
    <h1>Список категорий</h1>
  <div class="container">

    <p v-if="isLoading">Загрузка данных...</p>

    <ul v-else-if="categories.length > 0">
      <li v-for="category in categories" :key="category.id" class="category-item">
        <div class="category-info">
          <span class="icon">{{ category.icon || '📁' }}</span>
          <span class="name">{{ category.name }}</span>
          <span class="type">{{ transactionLabels[category.type] }}</span>
        </div>

        <button @click="deleteCategory(category.id)" class="delete-btn" style="margin-left: auto;">
          Удалить
        </button>
      </li>
    </ul>

    <p v-else>Категорий пока нет. Создайте первую!</p>
  </div>
</template>

<style scoped>
.category-info {
  display: flex;
  gap:10px;
  flex-grow: 1;
}
.category-item {
  list-style: none;
  padding: 10px 10px 10px 10px;
  border-bottom: 1px solid #505050;
  display: flex;
  gap: 20px;
}
.type { margin-left: auto; }
.icon { font-size: 1.2rem; }
</style>