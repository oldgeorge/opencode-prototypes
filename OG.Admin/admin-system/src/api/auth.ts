import request from '@/utils/request'
import type { LoginRequest, LoginResponse, CurrentUserResponse } from '@/types'

export const authApi = {
  login(data: LoginRequest) {
    return request.post<any, LoginResponse>('/auth/login', data)
  },

  logout() {
    return request.post('/auth/logout')
  },

  getCurrentUser() {
    return request.get<any, CurrentUserResponse>('/auth/current')
  },
}
