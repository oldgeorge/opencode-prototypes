import request from '@/utils/request'
import type { MenuDto, CreateMenuRequest, UpdateMenuRequest } from '@/types'

export const menuApi = {
  getAll() {
    return request.get<any, MenuDto[]>('/menus')
  },

  getTree() {
    return request.get<any, MenuDto[]>('/menus/tree')
  },

  getById(id: number) {
    return request.get<any, MenuDto>(`/menus/${id}`)
  },

  create(data: CreateMenuRequest) {
    return request.post<any, MenuDto>('/menus', data)
  },

  update(id: number, data: UpdateMenuRequest) {
    return request.put<any, MenuDto>(`/menus/${id}`, data)
  },

  delete(id: number) {
    return request.delete(`/menus/${id}`)
  },

  getMenusByRoleId(roleId: number) {
    return request.get<any, MenuDto[]>(`/menus/role/${roleId}`)
  },
}
