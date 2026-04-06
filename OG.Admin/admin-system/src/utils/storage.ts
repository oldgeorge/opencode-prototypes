const TOKEN_KEY = 'ogadmin_token'
const USER_KEY = 'ogadmin_user'
const REMEMBER_KEY = 'ogadmin_remember'

export const storage = {
  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY)
  },

  setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token)
  },

  removeToken(): void {
    localStorage.removeItem(TOKEN_KEY)
  },

  getUser(): any {
    const userStr = localStorage.getItem(USER_KEY)
    return userStr ? JSON.parse(userStr) : null
  },

  setUser(user: any): void {
    localStorage.setItem(USER_KEY, JSON.stringify(user))
  },

  removeUser(): void {
    localStorage.removeItem(USER_KEY)
  },

  getRemember(): { username: string; password: string } | null {
    const rememberStr = localStorage.getItem(REMEMBER_KEY)
    return rememberStr ? JSON.parse(rememberStr) : null
  },

  setRemember(credentials: { username: string; password: string }): void {
    localStorage.setItem(REMEMBER_KEY, JSON.stringify(credentials))
  },

  removeRemember(): void {
    localStorage.removeItem(REMEMBER_KEY)
  },

  clearAll(): void {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
    localStorage.removeItem(REMEMBER_KEY)
  },
}
