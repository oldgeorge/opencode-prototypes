<template>
  <el-container class="main-layout">
    <el-aside :width="appStore.collapsed ? '72px' : '240px'" class="aside">
      <div class="logo" :class="{ collapsed: appStore.collapsed }">
        <div class="logo-icon">
          <el-icon :size="28" color="#fff"><Monitor /></el-icon>
        </div>
        <transition name="logo-fade">
          <span class="logo-text" v-if="!appStore.collapsed">OG.Admin</span>
        </transition>
      </div>
      <SideMenu />
    </el-aside>

    <el-container class="content-wrapper">
      <el-header class="header">
        <div class="header-left">
          <div class="collapse-btn" @click="toggleCollapsed">
            <el-icon :size="20">
              <Fold v-if="!appStore.collapsed" />
              <Expand v-else />
            </el-icon>
          </div>
          <div class="breadcrumb-area">
            <el-breadcrumb separator="/">
              <el-breadcrumb-item :to="{ path: '/' }">首页</el-breadcrumb-item>
              <el-breadcrumb-item v-if="currentRouteTitle">{{ currentRouteTitle }}</el-breadcrumb-item>
            </el-breadcrumb>
          </div>
        </div>
        <div class="header-right">
          <div class="header-action" @click="handleFullscreen">
            <el-icon :size="18"><FullScreen /></el-icon>
          </div>
          <div class="header-action">
            <el-badge :value="3" :max="99" class="notification-badge">
              <el-icon :size="18"><Bell /></el-icon>
            </el-badge>
          </div>
          <el-divider direction="vertical" class="header-divider" />
          <el-dropdown @command="handleCommand" trigger="click">
            <div class="user-dropdown">
              <el-avatar :size="32" class="user-avatar">
                <el-icon :size="18"><UserFilled /></el-icon>
              </el-avatar>
              <span class="username">{{ userStore.userInfo?.nickname || userStore.userInfo?.username || 'Admin' }}</span>
              <el-icon :size="12" class="dropdown-arrow"><ArrowDown /></el-icon>
            </div>
            <template #dropdown>
              <el-dropdown-menu class="user-dropdown-menu">
                <el-dropdown-item command="profile">
                  <el-icon><User /></el-icon>
                  <span>个人中心</span>
                </el-dropdown-item>
                <el-dropdown-item command="settings">
                  <el-icon><Setting /></el-icon>
                  <span>系统设置</span>
                </el-dropdown-item>
                <el-dropdown-item divided command="logout">
                  <el-icon><SwitchButton /></el-icon>
                  <span>退出登录</span>
                </el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </el-header>

      <div class="tab-bar">
        <Tabs />
      </div>

      <el-main class="main">
        <router-view v-slot="{ Component, route }">
          <transition name="page-fade" mode="out-in">
            <keep-alive>
              <component :is="Component" :key="route.path" />
            </keep-alive>
          </transition>
        </router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import {
  Fold, Expand, ArrowDown, Monitor, UserFilled, User,
  Setting, SwitchButton, FullScreen, Bell
} from '@element-plus/icons-vue'
import SideMenu from '@/components/SideMenu.vue'
import Tabs from '@/components/Tabs.vue'
import { useAppStore } from '@/stores/app'
import { useUserStore } from '@/stores/user'
import { menuApi } from '@/api/menu'

const route = useRoute()
const router = useRouter()
const appStore = useAppStore()
const userStore = useUserStore()

const currentRouteTitle = computed(() => route.meta.title as string | undefined)

const toggleCollapsed = () => {
  appStore.toggleCollapsed()
}

const handleFullscreen = () => {
  if (!document.fullscreenElement) {
    document.documentElement.requestFullscreen()
  } else {
    document.exitFullscreen()
  }
}

const handleCommand = async (command: string) => {
  switch (command) {
    case 'logout':
      await userStore.logout()
      ElMessage.success('已退出登录')
      break
    case 'profile':
      ElMessage.info('个人中心')
      break
    case 'settings':
      ElMessage.info('系统设置')
      break
  }
}

const loadMenus = async () => {
  try {
    const res = await menuApi.getTree()
    appStore.setMenuList(res)
  } catch (error) {
    console.error(error)
  }
}

onMounted(() => {
  loadMenus()
})
</script>

<style scoped>
.main-layout {
  height: 100vh;
}

.aside {
  background: #001529;
  transition: width 0.3s cubic-bezier(0.2, 0, 0, 1);
  overflow: hidden;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.15);
}

.logo {
  height: 64px;
  display: flex;
  align-items: center;
  justify-content: flex-start;
  padding: 0 16px;
  gap: 12px;
  background: rgba(0, 0, 0, 0.2);
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  transition: all 0.3s cubic-bezier(0.2, 0, 0, 1);
}

.logo.collapsed {
  justify-content: center;
  padding: 0;
}

.logo-icon {
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

.logo-text {
  font-size: 18px;
  font-weight: 700;
  color: #fff;
  white-space: nowrap;
  letter-spacing: 1px;
}

.logo-fade-enter-active,
.logo-fade-leave-active {
  transition: all 0.3s ease;
}

.logo-fade-enter-from,
.logo-fade-leave-to {
  opacity: 0;
  transform: translateX(-10px);
}

.content-wrapper {
  display: flex;
  flex-direction: column;
  background: #f0f2f5;
}

.header {
  background: #fff;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.04);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 20px;
  height: 56px;
  z-index: 10;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 16px;
}

.collapse-btn {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  cursor: pointer;
  color: #5a6474;
  transition: all 0.2s ease;
}

.collapse-btn:hover {
  background: #f5f6fa;
  color: #667eea;
}

.breadcrumb-area {
  color: #5a6474;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 8px;
}

.header-action {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  cursor: pointer;
  color: #5a6474;
  transition: all 0.2s ease;
}

.header-action:hover {
  background: #f5f6fa;
  color: #667eea;
}

.notification-badge {
  display: flex;
  align-items: center;
  justify-content: center;
}

.header-divider {
  margin: 0 8px;
  border-color: #e8e8e8;
}

.user-dropdown {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 6px 12px;
  border-radius: 10px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.user-dropdown:hover {
  background: #f5f6fa;
}

.user-avatar {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.username {
  font-size: 14px;
  font-weight: 500;
  color: #1f2937;
}

.dropdown-arrow {
  color: #8b8fa3;
  transition: transform 0.2s ease;
}

.user-dropdown:hover .dropdown-arrow {
  color: #667eea;
}

.tab-bar {
  background: #fff;
  border-bottom: 1px solid #e8e8e8;
  padding: 0 16px;
  height: 40px;
  display: flex;
  align-items: center;
}

.main {
  background: #f0f2f5;
  padding: 20px;
  overflow-y: auto;
}

.page-fade-enter-active,
.page-fade-leave-active {
  transition: all 0.3s ease;
}

.page-fade-enter-from {
  opacity: 0;
  transform: translateY(10px);
}

.page-fade-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

:deep(.user-dropdown-menu) {
  min-width: 140px;
  padding: 6px;
}

:deep(.user-dropdown-menu .el-dropdown-menu__item) {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 12px;
  border-radius: 6px;
}

:deep(.el-breadcrumb__item:last-child .el-breadcrumb__inner) {
  color: #667eea;
  font-weight: 500;
}
</style>
