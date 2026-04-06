import request from '@/utils/request'
import { mockApi, useMock } from './mock'
import type { MenuDto, CreateMenuRequest, UpdateMenuRequest } from '@/types'

export const menuApi = {
  async getAll(): Promise<MenuDto[]> {
    if (useMock) {
      return mockApi.getMenus()
    }
    return request.get<any, MenuDto[]>('/menus')
  },

  async getTree(): Promise<MenuDto[]> {
    if (useMock) {
      return mockApi.getMenuTree()
    }
    return request.get<any, MenuDto[]>('/menus/tree')
  },

  async getById(id: number): Promise<MenuDto | undefined> {
    if (useMock) {
      return mockApi.getMenuById(id)
    }
    return request.get<any, MenuDto>(`/menus/${id}`)
  },

  async create(data: CreateMenuRequest): Promise<number> {
    if (useMock) {
      return mockApi.createMenu(data as Partial<MenuDto>)
    }
    return request.post<any, number>('/menus', data)
  },

  async update(id: number, data: UpdateMenuRequest): Promise<void> {
    if (useMock) {
      return mockApi.updateMenu(id, data as Partial<MenuDto>)
    }
    return request.put(`/menus/${id}`, data)
  },

  async delete(id: number): Promise<void> {
    if (useMock) {
      return mockApi.deleteMenu(id)
    }
    return request.delete(`/menus/${id}`)
  },

  async getMenusByRoleId(roleId: number): Promise<MenuDto[]> {
    if (useMock) {
      return mockApi.getMenusByRoleId(roleId)
    }
    return request.get<any, MenuDto[]>(`/menus/role/${roleId}`)
  },
}
