import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { MenuDto, RouteTab } from '@/types'

export const useAppStore = defineStore('app', () => {
  const collapsed = ref(false)
  const menuList = ref<MenuDto[]>([])
  const activeMenuId = ref<string>('')
  const tabs = ref<RouteTab[]>([
    { path: '/dashboard', name: 'dashboard', title: '工作台', closable: false }
  ])
  const activeTab = ref('/dashboard')

  const setCollapsed = (value: boolean) => {
    collapsed.value = value
  }

  const toggleCollapsed = () => {
    collapsed.value = !collapsed.value
  }

  const setMenuList = (list: MenuDto[]) => {
    menuList.value = list
  }

  const setActiveMenuId = (id: string) => {
    activeMenuId.value = id
  }

  const addTab = (tab: RouteTab) => {
    const exists = tabs.value.find(t => t.path === tab.path)
    if (!exists) {
      tabs.value.push(tab)
    }
    activeTab.value = tab.path
  }

  const removeTab = (path: string) => {
    const index = tabs.value.findIndex(t => t.path === path)
    if (index > -1) {
      tabs.value.splice(index, 1)
      if (activeTab.value === path) {
        const newTab = tabs.value[Math.max(0, index - 1)]
        activeTab.value = newTab?.path || '/dashboard'
      }
    }
  }

  const setActiveTab = (path: string) => {
    activeTab.value = path
  }

  return {
    collapsed,
    menuList,
    activeMenuId,
    tabs,
    activeTab,
    setCollapsed,
    toggleCollapsed,
    setMenuList,
    setActiveMenuId,
    addTab,
    removeTab,
    setActiveTab,
  }
})
