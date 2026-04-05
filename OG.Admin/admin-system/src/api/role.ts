import request from '@/utils/request'
import type { RoleDto, CreateRoleRequest, UpdateRoleRequest, MenuDto, PagedResult } from '@/types'

export const roleApi = {
  getPageList(params?: { keyword?: string; pageNum?: number; pageSize?: number }) {
    return request.get<any, PagedResult<RoleDto>>('/roles', { params })
  },

  getAll() {
    return request.get<any, RoleDto[]>('/roles/all')
  },

  getById(id: number) {
    return request.get<any, RoleDto>(`/roles/${id}`)
  },

  create(data: CreateRoleRequest) {
    return request.post<any, RoleDto>('/roles', data)
  },

  update(id: number, data: UpdateRoleRequest) {
    return request.put<any, RoleDto>(`/roles/${id}`, data)
  },

  delete(id: number) {
    return request.delete(`/roles/${id}`)
  },

  getRoleMenus(roleId: number) {
    return request.get<any, MenuDto[]>(`/roles/${roleId}/menus`)
  },

  assignPermissions(roleId: number, menuIds: number[]) {
    return request.put(`/roles/${roleId}/menus`, { menuIds })
  },
}
