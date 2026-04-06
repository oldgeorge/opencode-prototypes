import request from '@/utils/request'
import { mockApi, useMock } from './mock'
import type { OrgDto, CreateOrgRequest, UpdateOrgRequest } from '@/types'

export const orgApi = {
  async getAll(): Promise<OrgDto[]> {
    if (useMock) {
      return mockApi.getOrgs()
    }
    return request.get<any, OrgDto[]>('/orgs')
  },

  async getTree(): Promise<OrgDto[]> {
    if (useMock) {
      return mockApi.getOrgTree()
    }
    return request.get<any, OrgDto[]>('/orgs/tree')
  },

  async getById(id: number): Promise<OrgDto | undefined> {
    if (useMock) {
      return mockApi.getOrgById(id)
    }
    return request.get<any, OrgDto>(`/orgs/${id}`)
  },

  async create(data: CreateOrgRequest): Promise<number> {
    if (useMock) {
      return mockApi.createOrg(data as Partial<OrgDto>)
    }
    return request.post<any, number>('/orgs', data)
  },

  async update(id: number, data: UpdateOrgRequest): Promise<void> {
    if (useMock) {
      return mockApi.updateOrg(id, data as Partial<OrgDto>)
    }
    return request.put(`/orgs/${id}`, data)
  },

  async delete(id: number): Promise<void> {
    if (useMock) {
      return mockApi.deleteOrg(id)
    }
    return request.delete(`/orgs/${id}`)
  },
}
