import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { UserDto } from '@/types'
import { authApi } from '@/api/auth'
import router from '@/router'

const storage = {
  getToken: () => localStorage.getItem('ogadmin_token') || '',
  setToken: (token: string) => localStorage.setItem('ogadmin_token', token),
  getUser: (): UserDto | null => {
    const user = localStorage.getItem('ogadmin_user')
    return user ? JSON.parse(user) : null
  },
  setUser: (user: UserDto) => localStorage.setItem('ogadmin_user', JSON.stringify(user)),
  getRemember: () => {
    const remember = localStorage.getItem('ogadmin_remember')
    return remember ? JSON.parse(remember) : { username: '', password: '' }
  },
  setRemember: (data: { username: string; password: string }) => localStorage.setItem('ogadmin_remember', JSON.stringify(data)),
  clearAll: () => {
    localStorage.removeItem('ogadmin_token')
    localStorage.removeItem('ogadmin_user')
    localStorage.removeItem('ogadmin_remember')
  }
}

export const useUserStore = defineStore('user', () => {
  const token = ref<string>(storage.getToken())
  const userInfo = ref<UserDto | null>(storage.getUser())

  const setToken = (newToken: string) => {
    token.value = newToken
    storage.setToken(newToken)
  }

  const setUserInfo = (user: UserDto) => {
    userInfo.value = user
    storage.setUser(user)
  }

  const login = async (username: string, password: string, rememberMe: boolean = false) => {
    const res = await authApi.login({ username, password, rememberMe })
    if (res.code === 200 && res.data) {
      setToken(res.data.token)
      setUserInfo(res.data.user)
      
      if (rememberMe) {
        storage.setRemember({ username, password })
      } else {
        localStorage.removeItem('ogadmin_remember')
      }
    }
    return res
  }

  const getCurrentUser = async () => {
    try {
      const res = await authApi.getCurrentUser()
      if (res.code === 200 && res.data) {
        setUserInfo(res.data.user)
      }
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
    storage.clearAll()
    router.push('/login')
  }

  const isLoggedIn = () => {
    return !!token.value
  }

  return {
    token,
    userInfo,
    setToken,
    setUserInfo,
    login,
    getCurrentUser,
    logout,
    isLoggedIn,
  }
})

export { storage }
