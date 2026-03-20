<template>
  <nav class="main-nav">
    <router-link to="/categories">Категории</router-link> |
    <router-link to="/transactions">Транзакции</router-link>
  </nav>



  <router-view />
</template>

<style>
.main-nav { padding: 20px; background: #333543; }
.router-link-active { font-weight: bold; color: #3d5dc8; } /* Стиль для активной ссылки */
</style>