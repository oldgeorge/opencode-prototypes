import request from '@/utils/request'
import type { UserDto, CreateUserRequest, UpdateUserRequest, UserQueryRequest, PagedResult } from '@/types'

export const userApi = {
  getPageList(params: UserQueryRequest) {
    return request.get<any, PagedResult<UserDto>>('/users', { params })
  },

  getById(id: number) {
    return request.get<any, UserDto>(`/users/${id}`)
  },

  create(data: CreateUserRequest) {
    return request.post<any, UserDto>('/users', data)
  },

  update(id: number, data: UpdateUserRequest) {
    return request.put<any, UserDto>(`/users/${id}`, data)
  },

  delete(id: number) {
    return request.delete(`/users/${id}`)
  },

  updateStatus(id: number, status: number) {
    return request.put(`/users/${id}/status/${status}`)
  },

  changePassword(id: number, oldPassword: string, newPassword: string) {
    return request.put(`/users/${id}/password`, { oldPassword, newPassword })
  },
}
