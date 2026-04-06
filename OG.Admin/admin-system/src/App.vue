<template>
  <router-view />
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useUserStore } from '@/stores/user'
import { useAppStore } from '@/stores/app'
import { menuApi } from '@/api/menu'

const userStore = useUserStore()
const appStore = useAppStore()

onMounted(async () => {
  // Load menus if logged in
  if (userStore.isLoggedIn()) {
    try {
      const res = await menuApi.getTree()
      appStore.setMenuList(res)
    } catch (error) {
      console.error('Failed to load menus:', error)
    }
  }
})
</script>

<style lang="scss">
// CSS Variables - Theme
:root {
  --primary: #667eea;
  --primary-dark: #5a67d8;
  --primary-light: #a3bffa;
  --gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  --bg-main: #f0f2f5;
  --bg-card: #ffffff;
  --bg-sidebar: #001529;
  --text-primary: #1f2937;
  --text-secondary: #6b7280;
  --text-light: #9ca3af;
  --border-color: #e5e7eb;
  --shadow-sm: 0 1px 2px rgba(0,0,0,0.05);
  --shadow-md: 0 4px 6px -1px rgba(0,0,0,0.1);
  --shadow-lg: 0 10px 15px -3px rgba(0,0,0,0.1);
  --radius-sm: 6px;
  --radius-md: 8px;
  --radius-lg: 12px;
}

// Reset & Base
*,
*::before,
*::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

html, body {
  height: 100%;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
  font-size: 14px;
  color: var(--text-primary);
  background: var(--bg-main);
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

#app {
  height: 100%;
}

// Scrollbar
::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}

::-webkit-scrollbar-thumb {
  background: #d0d5dd;
  border-radius: 3px;
}

::-webkit-scrollbar-thumb:hover {
  background: #a0a8b3;
}

// Element Plus Overrides
.el-button--primary {
  --el-button-bg-color: #667eea;
  --el-button-border-color: #667eea;
  --el-button-hover-bg-color: #5a67d8;
  --el-button-hover-border-color: #5a67d8;
  --el-button-active-bg-color: #4c51b8;
  --el-button-active-border-color: #4c51b8;
}

.el-button--danger {
  --el-button-bg-color: #ef4444;
  --el-button-border-color: #ef4444;
  --el-button-hover-bg-color: #dc2626;
  --el-button-hover-border-color: #dc2626;
}

.el-input__wrapper,
.el-textarea__inner {
  border-radius: var(--radius-md);
  
  &:hover {
    box-shadow: 0 0 0 1px var(--primary-light) inset;
  }
  
  &.is-focus {
    box-shadow: 0 0 0 2px rgba(102, 126, 234, 0.2) inset;
  }
}

.el-table {
  --el-table-border-color: #f0f0f0;
  --el-table-header-bg-color: #f9fafb;
  
  th.el-table__cell {
    font-weight: 600;
    color: var(--text-secondary);
  }
  
  tr:hover > td.el-table__cell {
    background: #f9fafb !important;
  }
}

.el-dialog {
  --el-dialog-border-radius: var(--radius-lg);
  
  .el-dialog__header {
    padding: 20px 24px;
    background: linear-gradient(135deg, #667eea, #764ba2);
    color: #fff;
    margin: 0;
    
    .el-dialog__title {
      color: #fff;
    }
    
    .el-dialog__headerbtn {
      top: 20px;
      
      .el-icon {
        color: rgba(255, 255, 255, 0.8);
        
        &:hover {
          color: #fff;
        }
      }
    }
  }
}

.el-pagination {
  --el-pagination-hover-color: #667eea;
  
  .el-pager li.is-active {
    background: #667eea;
  }
}

.el-tag {
  --el-tag-border-radius: 12px;
}

// Animations
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(-20px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.fade-in {
  animation: fadeIn 0.3s ease;
}

.slide-in {
  animation: slideIn 0.3s ease;
}
</style>
