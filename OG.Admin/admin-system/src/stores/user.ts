import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { UserDto } from '@/types'
import { authApi } from '@/api/auth'
import { storage } from '@/utils/storage'
import router from '@/router'

export const useUserStore = defineStore('user', () => {
  const token = ref<string>(storage.getToken() || '')
  const userInfo = ref<UserDto | null>(storage.getUser())
  const permissions = ref<string[]>([])
  const roles = ref<string[]>([])

  const setToken = (newToken: string) => {
    token.value = newToken
    storage.setToken(newToken)
  }

  const setUserInfo = (user: UserDto) => {
    userInfo.value = user
    storage.setUser(user)
  }

  const setPermissions = (perms: string[]) => {
    permissions.value = perms
  }

  const setRoles = (roleList: string[]) => {
    roles.value = roleList
  }

  const login = async (username: string, password: string, rememberMe: boolean = false) => {
    const res = await authApi.login({ username, password, rememberMe })
    setToken(res.token)
    setUserInfo(res.user)
    
    if (rememberMe) {
      storage.setRemember({ username, password })
    }
    
    return res
  }

  const getCurrentUser = async () => {
    try {
      const res = await authApi.getCurrentUser()
      setUserInfo(res.user)
      setPermissions(res.permissions)
      setRoles(res.roles)
      return res
    } catch {
      return null
    }
  }

  const logout = async () => {
    try {
      await authApi.logout()
    } catch {
      // ignore error
    }
    token.value = ''
    userInfo.value = null
    permissions.value = []
    roles.value = []
    storage.clearAll()
    router.push('/login')
  }

  const isLoggedIn = () => {
    return !!token.value
  }

  return {
    token,
    userInfo,
    permissions,
    roles,
    setToken,
    setUserInfo,
    setPermissions,
    setRoles,
    login,
    getCurrentUser,
    logout,
    isLoggedIn,
  }
})
