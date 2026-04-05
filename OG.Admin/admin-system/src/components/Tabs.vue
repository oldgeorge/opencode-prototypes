<template>
  <div class="tabs-container">
    <div class="tabs-scroll">
      <div
        v-for="tab in tabs"
        :key="tab.path"
        :class="['tab-item', { active: tab.path === activeTab }]"
        @click="handleTabClick(tab)"
      >
        <span class="tab-title">{{ tab.title }}</span>
        <el-icon
          v-if="tab.closable !== false"
          class="tab-close"
          @click.stop="handleTabClose(tab.path)"
        >
          <Close />
        </el-icon>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { Close } from '@element-plus/icons-vue'
import { useAppStore } from '@/stores/app'
import type { RouteTab } from '@/types'

const router = useRouter()
const appStore = useAppStore()

const tabs = computed(() => appStore.tabs)
const activeTab = computed(() => appStore.activeTab)

const handleTabClick = (tab: RouteTab) => {
  appStore.setActiveTab(tab.path)
  router.push(tab.path)
}

const handleTabClose = (path: string) => {
  appStore.removeTab(path)
  if (path === activeTab.value) {
    const currentTab = tabs.value.find(t => t.path === appStore.activeTab)
    if (currentTab) {
      router.push(currentTab.path)
    }
  }
}
</script>

<style scoped>
.tabs-container {
  flex: 1;
  overflow: hidden;
}

.tabs-scroll {
  display: flex;
  align-items: center;
  height: 40px;
  overflow-x: auto;
  gap: 4px;
}

.tabs-scroll::-webkit-scrollbar {
  display: none;
}

.tab-item {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 6px 12px;
  background: #f0f2f5;
  border-radius: 4px;
  cursor: pointer;
  white-space: nowrap;
  font-size: 13px;
  color: #666;
  transition: all 0.3s;
}

.tab-item:hover {
  background: #e8e8e8;
}

.tab-item.active {
  background: #409EFF;
  color: #fff;
}

.tab-close {
  font-size: 12px;
}

.tab-close:hover {
  background: rgba(0, 0, 0, 0.1);
  border-radius: 50%;
}
</style>
