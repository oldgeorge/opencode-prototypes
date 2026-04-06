<template>
  <el-menu
    :default-active="activeMenu"
    :collapse="appStore.collapsed"
    :collapse-transition="false"
    background-color="transparent"
    text-color="rgba(255, 255, 255, 0.65)"
    active-text-color="#fff"
    class="side-menu"
    :popper-class="'dark-submenu-popper'"
  >
    <template v-for="item in menuList" :key="item.id">
      <el-sub-menu v-if="item.children && item.children.length > 0" :index="String(item.id)">
        <template #title>
          <div class="menu-item-content">
            <el-icon v-if="item.icon" class="menu-icon"><component :is="item.icon" /></el-icon>
            <span class="menu-text">{{ item.menuName }}</span>
          </div>
        </template>
        <el-menu-item
          v-for="child in item.children"
          :key="child.id"
          :index="child.path || String(child.id)"
          @click="handleMenuClick(child)"
        >
          <div class="menu-item-content">
            <el-icon v-if="child.icon" class="menu-icon"><component :is="child.icon" /></el-icon>
            <span class="menu-text">{{ child.menuName }}</span>
          </div>
        </el-menu-item>
      </el-sub-menu>
      <el-menu-item v-else :index="item.path || String(item.id)" @click="handleMenuClick(item)">
        <div class="menu-item-content">
          <el-icon v-if="item.icon" class="menu-icon"><component :is="item.icon" /></el-icon>
          <span class="menu-text">{{ item.menuName }}</span>
        </div>
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
  height: calc(100vh - 64px);
  border-right: none;
  overflow-y: auto;
  overflow-x: hidden;
}

.side-menu:not(.el-menu--collapse) {
  width: 240px;
}

.side-menu::-webkit-scrollbar {
  width: 4px;
}

.side-menu::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.1);
  border-radius: 2px;
}

.side-menu::-webkit-scrollbar-thumb:hover {
  background: rgba(255, 255, 255, 0.2);
}

.menu-item-content {
  display: flex;
  align-items: center;
  gap: 10px;
}

.menu-icon {
  font-size: 16px;
  flex-shrink: 0;
}

.menu-text {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

:deep(.el-menu-item) {
  border-radius: 8px;
  margin: 2px 8px;
  transition: all 0.2s ease;
}

:deep(.el-menu-item:hover) {
  background: rgba(255, 255, 255, 0.08) !important;
}

:deep(.el-menu-item.is-active) {
  background: linear-gradient(135deg, rgba(102, 126, 234, 0.3), rgba(118, 75, 162, 0.3)) !important;
  color: #fff !important;
}

:deep(.el-menu-item.is-active::before) {
  content: '';
  position: absolute;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 3px;
  height: 60%;
  background: linear-gradient(180deg, #667eea, #764ba2);
  border-radius: 0 3px 3px 0;
}

:deep(.el-sub-menu__title) {
  border-radius: 8px;
  margin: 2px 8px;
  transition: all 0.2s ease;
}

:deep(.el-sub-menu__title:hover) {
  background: rgba(255, 255, 255, 0.08) !important;
}

:deep(.el-menu--collapse .el-menu-item),
:deep(.el-menu--collapse .el-sub-menu__title) {
  justify-content: center;
  margin: 2px 4px;
}
</style>
