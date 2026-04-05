import request from '@/utils/request'
import type { OrgDto, CreateOrgRequest, UpdateOrgRequest } from '@/types'

export const orgApi = {
  getAll() {
    return request.get<any, OrgDto[]>('/orgs')
  },

  getTree() {
    return request.get<any, OrgDto[]>('/orgs/tree')
  },

  getById(id: number) {
    return request.get<any, OrgDto>(`/orgs/${id}`)
  },

  create(data: CreateOrgRequest) {
    return request.post<any, OrgDto>('/orgs', data)
  },

  update(id: number, data: UpdateOrgRequest) {
    return request.put<any, OrgDto>(`/orgs/${id}`, data)
  },

  delete(id: number) {
    return request.delete(`/orgs/${id}`)
  },
}
