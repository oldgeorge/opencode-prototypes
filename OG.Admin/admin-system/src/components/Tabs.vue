<template>
  <div class="tabs-container">
    <div class="tabs-scroll">
      <div
        v-for="tab in tabs"
        :key="tab.path"
        :class="['tab-item', { active: activeTab === tab.path }]"
        @click="switchTab(tab)"
      >
        <span class="tab-icon">{{ getTabIcon(tab.path) }}</span>
        <span class="tab-title">{{ tab.title }}</span>
        <span
          v-if="tab.closable !== false"
          class="tab-close"
          @click.stop="closeTab(tab)"
        >
          <el-icon><Close /></el-icon>
        </span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { Close } from '@element-plus/icons-vue'
import { useAppStore } from '@/stores/app'

const router = useRouter()
const appStore = useAppStore()

const tabs = computed(() => appStore.tabs)
const activeTab = computed(() => appStore.activeTab)

const getTabIcon = (path: string) => {
  if (path.includes('dashboard')) return '📊'
  if (path.includes('user')) return '👥'
  if (path.includes('role')) return '🔐'
  if (path.includes('menu')) return '📋'
  if (path.includes('org')) return '🏢'
  return '📄'
}

const switchTab = (tab: any) => {
  appStore.setActiveTab(tab.path)
  router.push(tab.path)
}

const closeTab = (tab: any) => {
  appStore.removeTab(tab.path)
  const lastTab = tabs.value[tabs.value.length - 1]
  if (lastTab) {
    router.push(lastTab.path)
  } else {
    router.push('/dashboard')
  }
}
</script>

<style scoped lang="scss">
.tabs-container {
  height: 100%;
  overflow: hidden;
}

.tabs-scroll {
  display: flex;
  align-items: center;
  height: 100%;
  gap: 4px;
  overflow-x: auto;
  padding: 4px 0;

  &::-webkit-scrollbar {
    height: 4px;
  }

  &::-webkit-scrollbar-thumb {
    background: #e0e0e0;
    border-radius: 2px;
  }
}

.tab-item {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 6px 12px;
  background: transparent;
  border: 1px solid transparent;
  border-radius: 6px;
  cursor: pointer;
  white-space: nowrap;
  transition: all 0.2s ease;
  font-size: 13px;
  color: #6b7280;

  &:hover {
    background: #f5f6fa;
    color: #374151;
  }

  &.active {
    background: linear-gradient(135deg, #667eea, #764ba2);
    color: #fff;
    border-color: transparent;
    box-shadow: 0 2px 8px rgba(102, 126, 234, 0.3);

    .tab-close {
      opacity: 1;
      color: rgba(255, 255, 255, 0.8);

      &:hover {
        color: #fff;
        background: rgba(255, 255, 255, 0.2);
      }
    }
  }
}

.tab-icon {
  font-size: 14px;
}

.tab-title {
  font-weight: 500;
}

.tab-close {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 16px;
  height: 16px;
  border-radius: 4px;
  opacity: 0;
  transition: all 0.2s ease;
  margin-left: 2px;

  .el-icon {
    font-size: 12px;
  }
}
</style>
