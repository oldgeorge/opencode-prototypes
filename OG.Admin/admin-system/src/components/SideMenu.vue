<template>
  <el-menu
    :default-active="activeMenu"
    :collapse="collapsed"
    :collapse-transition="false"
    background-color="#304156"
    text-color="#bfcbd9"
    active-text-color="#409EFF"
    class="side-menu"
  >
    <template v-for="item in menuList" :key="item.id">
      <el-sub-menu v-if="item.children && item.children.length > 0" :index="String(item.id)">
        <template #title>
          <el-icon v-if="item.icon"><component :is="item.icon" /></el-icon>
          <span>{{ item.menuName }}</span>
        </template>
        <el-menu-item
          v-for="child in item.children"
          :key="child.id"
          :index="child.path || String(child.id)"
          @click="handleMenuClick(child)"
        >
          <el-icon v-if="child.icon"><component :is="child.icon" /></el-icon>
          <span>{{ child.menuName }}</span>
        </el-menu-item>
      </el-sub-menu>
      <el-menu-item v-else :index="item.path || String(item.id)" @click="handleMenuClick(item)">
        <el-icon v-if="item.icon"><component :is="item.icon" /></el-icon>
        <span>{{ item.menuName }}</span>
      </el-menu-item>
    </template>
  </el-menu>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAppStore } from '@/stores/app'
import type { MenuDto } from '@/types'

const route = useRoute()
const router = useRouter()
const appStore = useAppStore()

const collapsed = computed(() => appStore.collapsed)
const menuList = computed(() => appStore.menuList)
const activeMenu = computed(() => route.path)

const handleMenuClick = (menu: MenuDto) => {
  if (menu.path) {
    appStore.addTab({
      path: menu.path,
      name: menu.menuCode || menu.path,
      title: menu.menuName,
      closable: true,
    })
    router.push(menu.path)
  }
}
</script>

<style scoped>
.side-menu {
  height: calc(100vh - 60px);
  border-right: none;
}

.side-menu:not(.el-menu--collapse) {
  width: 220px;
}
</style>
