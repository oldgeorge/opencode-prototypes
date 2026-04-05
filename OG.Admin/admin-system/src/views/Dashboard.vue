<template>
  <div class="dashboard">
    <el-row :gutter="20" class="stat-cards">
      <el-col :xs="24" :sm="12" :md="6" v-for="stat in stats" :key="stat.title">
        <el-card shadow="hover" class="stat-card">
          <div class="stat-content">
            <div class="stat-info">
              <p class="stat-title">{{ stat.title }}</p>
              <p class="stat-value">{{ stat.value }}</p>
            </div>
            <el-icon class="stat-icon" :style="{ color: stat.color }">
              <component :is="stat.icon" />
            </el-icon>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="main-content">
      <el-col :xs="24" :lg="16">
        <el-card class="chart-card">
          <template #header>
            <span>数据统计</span>
          </template>
          <div class="chart-placeholder">
            <el-icon :size="48" color="#409EFF"><TrendCharts /></el-icon>
            <p>数据图表区域</p>
          </div>
        </el-card>
      </el-col>
      
      <el-col :xs="24" :lg="8">
        <el-card class="notice-card">
          <template #header>
            <span>系统通知</span>
          </template>
          <div class="notice-list">
            <div v-for="i in 5" :key="i" class="notice-item">
              <span class="notice-title">系统公告 {{ i }}</span>
              <span class="notice-time">2024-01-{{ String(i).padStart(2, '0') }}</span>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-card class="quick-card">
      <template #header>
        <span>快捷操作</span>
      </template>
      <div class="quick-actions">
        <el-button type="primary" plain @click="$router.push('/system/user')">
          <el-icon><User /></el-icon> 用户管理
        </el-button>
        <el-button type="success" plain @click="$router.push('/system/role')">
          <el-icon><UserFilled /></el-icon> 角色管理
        </el-button>
        <el-button type="warning" plain @click="$router.push('/system/menu')">
          <el-icon><Menu /></el-icon> 菜单管理
        </el-button>
        <el-button type="info" plain @click="$router.push('/system/org')">
          <el-icon><OfficeBuilding /></el-icon> 组织管理
        </el-button>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useUserStore } from '@/stores/user'

const userStore = useUserStore()

const stats = ref([
  { title: '用户总数', value: '1,234', icon: 'User', color: '#409EFF' },
  { title: '角色总数', value: '12', icon: 'UserFilled', color: '#67C23A' },
  { title: '菜单总数', value: '45', icon: 'Menu', color: '#E6A23C' },
  { title: '访问量', value: '8,888', icon: 'View', color: '#F56C6C' },
])
</script>

<style scoped>
.dashboard {
  padding: 20px;
}

.stat-cards {
  margin-bottom: 20px;
}

.stat-card {
  margin-bottom: 20px;
}

.stat-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.stat-title {
  margin: 0;
  font-size: 14px;
  color: #999;
}

.stat-value {
  margin: 8px 0 0;
  font-size: 24px;
  font-weight: bold;
  color: #333;
}

.stat-icon {
  font-size: 48px;
  opacity: 0.8;
}

.main-content {
  margin-bottom: 20px;
}

.chart-card,
.notice-card,
.quick-card {
  margin-bottom: 20px;
}

.chart-placeholder {
  height: 300px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #999;
}

.notice-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.notice-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.notice-title {
  color: #333;
}

.notice-time {
  color: #999;
  font-size: 12px;
}

.quick-actions {
  display: flex;
  flex-wrap: wrap;
  gap: 12px;
}
</style>
