<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'

const categoryName = ref('')
const selectedType = ref(2)
const selectedIcon = ref('📁')
const message = ref('')
const categories = ref([])
const isLoading = ref(true)
const transactionLabels = {
  1: 'Доход',
  2: 'Расход'
};
const availableIcons = [
  '📁', '🛒', '🍕', '🚗', '🏠', '🎮', '💊', '👕',
  '🎓', '🔋', '🎁', '✈️', '💰', '🏦', '📈', '☕'
]

const groupedCategories = computed(() => {
  return {
    'Доходы': categories.value.filter(c => c.type === 1),
    'Расходы': categories.value.filter(c => c.type === 2)
  }
});

async function createCategory() {
  if (!categoryName.value.trim()) return;
  try {
    const response = await axios.post('https://localhost:44352/api/finance/categories', {
      name: categoryName.value,
      icon: selectedIcon.value, // Используем выбранную иконку
      type: selectedType.value
    })

    // Сброс полей после успеха
    message.value = `Категория создана!`
    categoryName.value = ''
    selectedIcon.value = '📁'
    await fetchCategories()
    setTimeout(() => message.value = '', 3000)
  } catch (error) {
    message.value = 'Ошибка при создании'
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
  <div class="main-container">

    <div class="card add-card">
      <h2>Добавление категории</h2>

      <div class="icon-selector-label">Выберите иконку:</div>
      <div class="icon-grid">
        <div
            v-for="icon in availableIcons"
            :key="icon"
            class="icon-option"
            :class="{ selected: selectedIcon === icon }"
            @click="selectedIcon = icon"
        >
          {{ icon }}
        </div>
      </div>

      <div class="type-selector">
        <label :class="{ active: selectedType === 1 }" class="type-label income">
          <input type="radio" :value="1" v-model="selectedType" /> Доход
        </label>
        <label :class="{ active: selectedType === 2 }" class="type-label expense">
          <input type="radio" :value="2" v-model="selectedType" /> Расход
        </label>
      </div>

      <div class="form-inline">
        <div class="input-wrapper">
          <span class="input-icon-preview">{{ selectedIcon }}</span>
          <input v-model="categoryName" placeholder="Название категории" class="main-input" />
        </div>
        <button class="submit-btn" @click="createCategory">Добавить</button>
      </div>
    </div>
    

    <div class="content-section">
      <h1>Список категорий</h1>

      <p v-if="isLoading" class="info-text">Загрузка...</p>

      <div v-else-if="categories.length > 0">
        <div v-for="(list, typeName) in groupedCategories" :key="typeName">
          <div v-if="list.length > 0">
            <h3 class="group-divider"><span>{{ typeName }}</span></h3>

            <div v-for="category in list" :key="category.id" class="item-card">
              <div class="item-main">
                <span class="item-icon">{{ category.icon || '📁' }}</span>
                <span class="item-name">{{ category.name }}</span>
              </div>

              <div class="item-badge" :class="category.type === 1 ? 'badge-income' : 'badge-expense'">
                {{ transactionLabels[category.type] }}
              </div>

              <button @click="deleteCategory(category.id)" class="delete-icon-btn">
                🗑
              </button>
            </div>
          </div>
        </div>
      </div>

      <p v-else class="info-text">Категорий пока нет.</p>
    </div>
  </div>
</template>

<style scoped>
.main-container {
  max-width: 600px; /* Для категорий можно чуть уже */
  margin: 0 auto;
  padding: 20px;
  font-family: 'Inter', sans-serif;
  color: #e0e0e0;
  background-color: #121212;
  min-height: 100vh;
}

/* Подпись к иконкам */
.icon-selector-label {
  text-align: left;
  font-size: 12px;
  color: #888;
  margin-bottom: 10px;
  text-transform: uppercase;
}

/* Сетка иконок */
.icon-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 20px;
  background: #252525;
  padding: 10px;
  border-radius: 12px;
}

.icon-option {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
  cursor: pointer;
  border-radius: 8px;
  transition: all 0.2s;
  border: 2px solid transparent;
}

.icon-option:hover {
  background: #333;
  transform: scale(1.1);
}

.icon-option.selected {
  background: rgba(187, 134, 252, 0.2);
  border-color: #bb86fc;
}

/* Обертка для инпута с иконкой */
.input-wrapper {
  position: relative;
  flex: 1;
  display: flex;
  align-items: center;
}

.input-icon-preview {
  position: absolute;
  left: 12px;
  font-size: 18px;
}

.main-input {
  width: 100%;
  padding-left: 45px !important; /* Отступ под иконку */
}

.card {
  background: #1e1e1e;
  border-radius: 16px;
  padding: 24px;
  margin-bottom: 30px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.3);
}

.form-inline {
  display: flex;
  gap: 10px;
}

.main-input {
  flex: 1;
  background: #2d2d2d;
  border: 1px solid #444;
  color: white;
  padding: 12px;
  border-radius: 8px;
  outline: none;
}

.main-input:focus { border-color: #bb86fc; }

.submit-btn {
  padding: 0 25px;
  background: #bb86fc;
  color: #000;
  border: none;
  border-radius: 8px;
  font-weight: bold;
  cursor: pointer;
  transition: 0.3s;
}

/* Переключатель типа */
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
  font-size: 14px;
}

.type-label.active.income { background: #1b5e20; border-color: #4caf50; }
.type-label.active.expense { background: #b71c1c; border-color: #f44336; }
.type-label input { display: none; }

/* Стили списка */
.group-divider {
  display: flex;
  align-items: center;
  color: #888;
  font-size: 14px;
  margin: 25px 0 15px;
}

.group-divider::after {
  content: '';
  flex: 1;
  margin-left: 15px;
  border-bottom: 1px solid #333;
}

.item-card {
  display: flex;
  align-items: center;
  background: #1e1e1e;
  padding: 12px 16px;
  border-radius: 12px;
  margin-bottom: 8px;
  transition: 0.2s;
}

.item-card:hover { background: #252525; }

.item-main {
  display: flex;
  align-items: center;
  gap: 15px;
  flex: 1;
}

.item-icon { font-size: 20px; }
.item-name { font-weight: 500; }

.item-badge {
  font-size: 11px;
  padding: 2px 10px;
  border-radius: 10px;
  margin-right: 15px;
  text-transform: uppercase;
}

.badge-income { background: rgba(76, 175, 80, 0.1); color: #4caf50; }
.badge-expense { background: rgba(244, 67, 54, 0.1); color: #f44336; }

.delete-icon-btn {
  background: transparent;
  border: none;
  color: #555;
  cursor: pointer;
  font-size: 18px;
  transition: 0.2s;
}

.delete-icon-btn:hover { color: #f44336; }

.nav-header { text-align: center; margin-bottom: 20px; }
.nav-link { color: #888; text-decoration: none; font-size: 14px; }
.nav-link.active { color: #bb86fc; font-weight: bold; }
.nav-divider { color: #444; margin: 0 10px; }
.status-message { color: #4caf50; font-size: 13px; margin-top: 10px; }
</style>