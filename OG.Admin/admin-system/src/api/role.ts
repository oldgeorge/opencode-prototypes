import request from '@/utils/request'
import { mockApi, useMock } from './mock'
import type { RoleDto, CreateRoleRequest, UpdateRoleRequest, PageResult } from '@/types'

export const roleApi = {
  async getPageList(params?: { keyword?: string; pageNum?: number; pageSize?: number }): Promise<PageResult<RoleDto>> {
    if (useMock) {
      return mockApi.getRoles(params)
    }
    return request.get<any, PageResult<RoleDto>>('/roles', { params })
  },

  async getAll(): Promise<RoleDto[]> {
    if (useMock) {
      const result = await mockApi.getRoles()
      return result.items
    }
    return request.get<any, RoleDto[]>('/roles/all')
  },

  async getById(id: number): Promise<RoleDto | undefined> {
    if (useMock) {
      return mockApi.getRoleById(id)
    }
    return request.get<any, RoleDto>(`/roles/${id}`)
  },

  async create(data: CreateRoleRequest): Promise<number> {
    if (useMock) {
      return mockApi.createRole(data as Partial<RoleDto>)
    }
    return request.post<any, number>('/roles', data)
  },

  async update(id: number, data: UpdateRoleRequest): Promise<void> {
    if (useMock) {
      return mockApi.updateRole(id, data as Partial<RoleDto>)
    }
    return request.put(`/roles/${id}`, data)
  },

  async delete(id: number): Promise<void> {
    if (useMock) {
      return mockApi.deleteRole(id)
    }
    return request.delete(`/roles/${id}`)
  },

  async assignPermissions(roleId: number, menuIds: number[]): Promise<void> {
    if (useMock) {
      return mockApi.assignPermissions(roleId, menuIds)
    }
    return request.put(`/roles/${roleId}/menus`, { menuIds })
  },
}
