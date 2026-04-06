<template>
  <el-container class="main-layout">
    <el-aside :width="collapsed ? '64px' : '220px'" class="aside">
      <div class="logo" :class="{ collapsed }">
        <span class="logo-text" v-if="!collapsed">OG.Admin</span>
        <span class="logo-text collapsed" v-else>OG</span>
      </div>
      <SideMenu />
    </el-aside>
    
    <el-container>
      <el-header class="header">
        <div class="header-left">
          <el-icon class="collapse-btn" @click="toggleCollapsed">
            <Fold v-if="!collapsed" />
            <Expand v-else />
          </el-icon>
          <Tabs />
        </div>
        <div class="header-right">
          <el-dropdown @command="handleCommand">
            <span class="user-info">
              <el-avatar :size="32" src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
              <span class="username">{{ userStore.userInfo?.nickname || userStore.userInfo?.username }}</span>
              <el-icon><ArrowDown /></el-icon>
            </span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item command="profile">个人中心</el-dropdown-item>
                <el-dropdown-item command="settings">系统设置</el-dropdown-item>
                <el-dropdown-item divided command="logout">退出登录</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </el-header>
      
      <el-main class="main">
        <router-view v-slot="{ Component }">
          <keep-alive>
            <component :is="Component" />
          </keep-alive>
        </router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Fold, Expand, ArrowDown } from '@element-plus/icons-vue'
import SideMenu from '@/components/SideMenu.vue'
import Tabs from '@/components/Tabs.vue'
import { useAppStore } from '@/stores/app'
import { useUserStore } from '@/stores/user'
import { menuApi } from '@/api/menu'

const router = useRouter()
const appStore = useAppStore()
const userStore = useUserStore()

const collapsed = appStore.collapsed

const toggleCollapsed = () => {
  appStore.toggleCollapsed()
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
  background: #304156;
  transition: width 0.3s;
  overflow-x: hidden;
}

.logo {
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  background: #263445;
  color: #fff;
  font-size: 18px;
  font-weight: bold;
  transition: all 0.3s;
}

.logo.collapsed {
  gap: 0;
}

.logo-text {
  font-size: 18px;
  font-weight: bold;
  color: #fff;
}

.header {
  background: #fff;
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 16px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 16px;
}

.collapse-btn {
  font-size: 20px;
  cursor: pointer;
  color: #666;
}

.collapse-btn:hover {
  color: #409EFF;
}

.header-right {
  display: flex;
  align-items: center;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
}

.username {
  font-size: 14px;
  color: #333;
}

.main {
  background: #f0f2f5;
  padding: 16px;
  overflow-y: auto;
}
</style>
