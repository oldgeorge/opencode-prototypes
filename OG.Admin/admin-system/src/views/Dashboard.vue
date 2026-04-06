<template>
  <div class="dashboard">
    <!-- 欢迎区域 -->
    <div class="welcome-section">
      <div class="welcome-content">
        <div class="welcome-text">
          <h1 class="welcome-title">早安，{{ userStore.userInfo?.nickname || userStore.userInfo?.username || '管理员' }} 👋</h1>
          <p class="welcome-subtitle">欢迎使用 OG.Admin 企业管理系统，祝您今天工作愉快！</p>
        </div>
        <div class="welcome-time">
          <span class="time">{{ currentTime }}</span>
          <span class="date">{{ currentDate }}</span>
        </div>
      </div>
      <div class="welcome-decoration">
        <div class="decoration-circle circle-1"></div>
        <div class="decoration-circle circle-2"></div>
      </div>
    </div>

    <!-- 统计卡片 -->
    <div class="stats-grid">
      <div class="stat-card stat-users" @click="goToPage('/system/user')">
        <div class="stat-icon">
          <el-icon><User /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.userCount }}</span>
          <span class="stat-label">用户总数</span>
        </div>
        <div class="stat-trend up">
          <el-icon><Top /></el-icon>
          <span>12%</span>
        </div>
      </div>

      <div class="stat-card stat-orgs" @click="goToPage('/system/org')">
        <div class="stat-icon">
          <el-icon><OfficeBuilding /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.orgCount }}</span>
          <span class="stat-label">组织数量</span>
        </div>
        <div class="stat-trend up">
          <el-icon><Top /></el-icon>
          <span>8%</span>
        </div>
      </div>

      <div class="stat-card stat-menus" @click="goToPage('/system/menu')">
        <div class="stat-icon">
          <el-icon><Menu /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.menuCount }}</span>
          <span class="stat-label">菜单数量</span>
        </div>
        <div class="stat-trend neutral">
          <span>0%</span>
        </div>
      </div>

      <div class="stat-card stat-roles" @click="goToPage('/system/role')">
        <div class="stat-icon">
          <el-icon><Key /></el-icon>
        </div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.roleCount }}</span>
          <span class="stat-label">角色数量</span>
        </div>
        <div class="stat-trend up">
          <el-icon><Top /></el-icon>
          <span>5%</span>
        </div>
      </div>
    </div>

    <!-- 快捷操作 & 系统信息 -->
    <div class="content-grid">
      <div class="card quick-actions">
        <div class="card-header">
          <h3 class="card-title">
            <el-icon><Lightning /></el-icon>
            快捷操作
          </h3>
        </div>
        <div class="card-body">
          <div class="action-grid">
            <div class="action-item" @click="goToPage('/system/user')">
              <div class="action-icon bg-blue">
                <el-icon><Plus /></el-icon>
              </div>
              <span class="action-text">新增用户</span>
            </div>
            <div class="action-item" @click="goToPage('/system/role')">
              <div class="action-icon bg-purple">
                <el-icon><Operation /></el-icon>
              </div>
              <span class="action-text">角色分配</span>
            </div>
            <div class="action-item" @click="goToPage('/system/org')">
              <div class="action-icon bg-green">
                <el-icon><OfficeBuilding /></el-icon>
              </div>
              <span class="action-text">组织管理</span>
            </div>
            <div class="action-item" @click="goToPage('/system/menu')">
              <div class="action-icon bg-orange">
                <el-icon><Setting /></el-icon>
              </div>
              <span class="action-text">菜单配置</span>
            </div>
          </div>
        </div>
      </div>

      <div class="card system-info">
        <div class="card-header">
          <h3 class="card-title">
            <el-icon><Monitor /></el-icon>
            系统信息
          </h3>
        </div>
        <div class="card-body">
          <div class="info-list">
            <div class="info-item">
              <span class="info-label">系统版本</span>
              <span class="info-value">v1.0.0</span>
            </div>
            <div class="info-item">
              <span class="info-label">前端框架</span>
              <span class="info-value">Vue 3 + Vite</span>
            </div>
            <div class="info-item">
              <span class="info-label">UI 组件库</span>
              <span class="info-value">Element Plus</span>
            </div>
            <div class="info-item">
              <span class="info-label">后端框架</span>
              <span class="info-value">.NET 10 WebAPI</span>
            </div>
            <div class="info-item">
              <span class="info-label">数据库</span>
              <span class="info-value">SQLite</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { User, Top, OfficeBuilding, Menu, Key, Setting, Monitor, Plus, Operation } from '@element-plus/icons-vue'

const router = useRouter()
const userStore = useUserStore()

const currentTime = ref('')
const currentDate = ref('')
const stats = ref({
  userCount: 12,
  orgCount: 4,
  menuCount: 8,
  roleCount: 3
})

let timer: number

const updateTime = () => {
  const now = new Date()
  currentTime.value = now.toLocaleTimeString('zh-CN', { hour: '2-digit', minute: '2-digit' })
  currentDate.value = now.toLocaleDateString('zh-CN', { week: 'long', year: 'numeric', month: 'long', day: 'numeric' })
}

const goToPage = (path: string) => {
  router.push(path)
}

onMounted(() => {
  updateTime()
  timer = window.setInterval(updateTime, 1000)
})

onUnmounted(() => {
  clearInterval(timer)
})
</script>

<style scoped lang="scss">
.dashboard {
  padding: 0;
}

.welcome-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 16px;
  padding: 32px;
  margin-bottom: 24px;
  position: relative;
  overflow: hidden;
  color: #fff;
}

.welcome-content {
  position: relative;
  z-index: 1;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.welcome-title {
  font-size: 28px;
  font-weight: 700;
  margin: 0 0 8px;
}

.welcome-subtitle {
  font-size: 14px;
  opacity: 0.85;
  margin: 0;
}

.welcome-time {
  text-align: right;
  .time {
    display: block;
    font-size: 36px;
    font-weight: 700;
    line-height: 1.2;
  }
  .date {
    font-size: 13px;
    opacity: 0.75;
  }
}

.welcome-decoration {
  position: absolute;
  inset: 0;
  pointer-events: none;
}

.decoration-circle {
  position: absolute;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
}

.circle-1 {
  width: 200px;
  height: 200px;
  top: -80px;
  right: 100px;
}

.circle-2 {
  width: 150px;
  height: 150px;
  bottom: -60px;
  right: -30px;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 24px;
}

.stat-card {
  background: #fff;
  border-radius: 12px;
  padding: 24px;
  display: flex;
  align-items: center;
  position: relative;
  overflow: hidden;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
  }
}

.stat-icon {
  width: 56px;
  height: 56px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  margin-right: 16px;

  .el-icon {
    font-size: 28px;
  }
}

.stat-users .stat-icon {
  background: linear-gradient(135deg, #667eea22, #667eea44);
  color: #667eea;
}

.stat-orgs .stat-icon {
  background: linear-gradient(135deg, #10b98122, #10b98144);
  color: #10b981;
}

.stat-menus .stat-icon {
  background: linear-gradient(135deg, #f59e0b22, #f59e0b44);
  color: #f59e0b;
}

.stat-roles .stat-icon {
  background: linear-gradient(135deg, #764ba222, #764ba244);
  color: #764ba2;
}

.stat-info {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.stat-value {
  font-size: 28px;
  font-weight: 700;
  color: #1f2937;
  line-height: 1.2;
}

.stat-label {
  font-size: 13px;
  color: #6b7280;
  margin-top: 4px;
}

.stat-trend {
  position: absolute;
  top: 16px;
  right: 16px;
  display: flex;
  align-items: center;
  gap: 2px;
  font-size: 12px;
  padding: 4px 8px;
  border-radius: 12px;

  &.up {
    background: #dcfce7;
    color: #16a34a;
  }

  &.down {
    background: #fee2e2;
    color: #dc2626;
  }

  &.neutral {
    background: #f3f4f6;
    color: #6b7280;
  }
}

.content-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.card {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
  overflow: hidden;
}

.card-header {
  padding: 20px 24px;
  border-bottom: 1px solid #f0f0f0;
}

.card-title {
  font-size: 16px;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 8px;

  .el-icon {
    color: #667eea;
  }
}

.card-body {
  padding: 24px;
}

.action-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
}

.action-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
  border-radius: 12px;
  background: #f9fafb;
  cursor: pointer;
  transition: all 0.3s ease;

  &:hover {
    background: #f0f2f5;
    transform: translateY(-2px);
  }
}

.action-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
  margin-bottom: 12px;

  .el-icon {
    font-size: 24px;
  }

  &.bg-blue {
    background: linear-gradient(135deg, #667eea22, #667eea44);
    color: #667eea;
  }

  &.bg-purple {
    background: linear-gradient(135deg, #764ba222, #764ba244);
    color: #764ba2;
  }

  &.bg-green {
    background: linear-gradient(135deg, #10b98122, #10b98144);
    color: #10b981;
  }

  &.bg-orange {
    background: linear-gradient(135deg, #f59e0b22, #f59e0b44);
    color: #f59e0b;
  }
}

.action-text {
  font-size: 13px;
  color: #374151;
  font-weight: 500;
}

.info-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.info-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-bottom: 16px;
  border-bottom: 1px solid #f0f0f0;

  &:last-child {
    border-bottom: none;
    padding-bottom: 0;
  }
}

.info-label {
  font-size: 14px;
  color: #6b7280;
}

.info-value {
  font-size: 14px;
  font-weight: 500;
  color: #1f2937;
}

@media (max-width: 1200px) {
  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }

  .content-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .welcome-content {
    flex-direction: column;
    text-align: center;
  }

  .welcome-time {
    text-align: center;
    margin-top: 16px;
  }

  .stats-grid {
    grid-template-columns: 1fr;
  }

  .action-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}
</style>
