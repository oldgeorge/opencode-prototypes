import request from '@/utils/request'
import { mockApi, useMock } from './mock'
import type { LoginRequest, LoginResponse, CurrentUserResponse } from '@/types'

export const authApi = {
  async login(data: LoginRequest): Promise<LoginResponse> {
    if (useMock) {
      return mockApi.login(data)
    }
    return request.post<any, LoginResponse>('/auth/login', data)
  },

  async logout() {
    if (useMock) return { code: 200, message: 'success' }
    return request.post('/auth/logout')
  },

  async getCurrentUser(): Promise<CurrentUserResponse> {
    if (useMock) {
      return mockApi.getCurrentUser()
    }
    return request.get<any, CurrentUserResponse>('/auth/current')
  },
}
