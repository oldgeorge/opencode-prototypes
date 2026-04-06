import request from '@/utils/request'
import { mockApi, useMock } from './mock'
import type { UserDto, CreateUserRequest, UpdateUserRequest, UserQueryRequest, PageResult } from '@/types'

export const userApi = {
  async getPageList(params: UserQueryRequest): Promise<PageResult<UserDto>> {
    if (useMock) {
      return mockApi.getUsers({ keyword: params.keyword, page: params.pageNum, pageSize: params.pageSize })
    }
    return request.get<any, PageResult<UserDto>>('/users', { params })
  },

  async getById(id: number): Promise<UserDto | undefined> {
    if (useMock) {
      return mockApi.getUserById(id)
    }
    return request.get<any, UserDto>(`/users/${id}`)
  },

  async create(data: CreateUserRequest): Promise<number> {
    if (useMock) {
      return mockApi.createUser(data as Partial<UserDto>)
    }
    return request.post<any, number>('/users', data)
  },

  async update(id: number, data: UpdateUserRequest): Promise<void> {
    if (useMock) {
      return mockApi.updateUser(id, data as Partial<UserDto>)
    }
    return request.put(`/users/${id}`, data)
  },

  async delete(id: number): Promise<void> {
    if (useMock) {
      return mockApi.deleteUser(id)
    }
    return request.delete(`/users/${id}`)
  },

  async updateStatus(id: number, status: number): Promise<void> {
    if (useMock) {
      return mockApi.updateUserStatus(id, status)
    }
    return request.put(`/users/${id}/status/${status}`)
  },

  async changePassword(id: number, oldPassword: string, newPassword: string): Promise<void> {
    if (useMock) {
      return Promise.resolve()
    }
    return request.put(`/users/${id}/password`, { oldPassword, newPassword })
  },
}
