// Mock API 模块 - 提供模拟数据，支持与真实接口切换
// 设置 VITE_USE_MOCK=1 启用 Mock 模式

import type { UserDto, OrgDto, MenuDto, RoleDto, LoginRequest, LoginResponse } from '@/types'

// 判断是否使用 Mock
const useMock = import.meta.env.VITE_USE_MOCK === '1' || !import.meta.env.VITE_API_BASE_URL

// 延迟模拟
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms))

// Mock 数据
const mockUsers: UserDto[] = [
  { id: 1, username: 'admin', realName: '系统管理员', email: 'admin@example.com', phone: '13800138000', orgId: 1, orgName: '技术部', roleId: 1, roleName: '超级管理员', status: 1, createTime: '2024-01-01' },
  { id: 2, username: 'zhangsan', realName: '张三', email: 'zhangsan@example.com', phone: '13800138001', orgId: 1, orgName: '技术部', roleId: 2, roleName: '管理员', status: 1, createTime: '2024-01-15' },
  { id: 3, username: 'lisi', realName: '李四', email: 'lisi@example.com', phone: '13800138002', orgId: 2, orgName: '运营部', roleId: 3, roleName: '普通用户', status: 1, createTime: '2024-02-01' },
  { id: 4, username: 'wangwu', realName: '王五', email: 'wangwu@example.com', phone: '13800138003', orgId: 3, orgName: '市场部', roleId: 3, roleName: '普通用户', status: 0, createTime: '2024-02-15' },
  { id: 5, username: 'zhaoliu', realName: '赵六', email: 'zhaoliu@example.com', phone: '13800138004', orgId: 1, orgName: '技术部', roleId: 2, roleName: '管理员', status: 1, createTime: '2024-03-01' },
]

const mockOrgs: OrgDto[] = [
  { id: 1, orgName: '总公司', orgCode: 'HQ', parentId: 0, sort: 1, status: 1, children: [] },
  { id: 2, orgName: '技术部', orgCode: 'TECH', parentId: 1, sort: 1, status: 1, children: [] },
  { id: 3, orgName: '运营部', orgCode: 'OPS', parentId: 1, sort: 2, status: 1, children: [] },
  { id: 4, orgName: '市场部', orgCode: 'MKT', parentId: 1, sort: 3, status: 1, children: [] },
]

const mockMenus: MenuDto[] = [
  { id: 1, menuName: '工作台', menuCode: 'dashboard', menuType: 1, path: '/dashboard', component: '/dashboard', icon: 'HomeFilled', parentId: 0, sort: 1, status: 1, children: [] },
  { id: 2, menuName: '系统管理', menuCode: 'system', menuType: 0, path: '/system', icon: 'Setting', parentId: 0, sort: 2, status: 1, children: [] },
  { id: 3, menuName: '用户管理', menuCode: 'user', menuType: 1, path: '/system/user', component: '/system/user', icon: 'User', parentId: 2, sort: 1, status: 1, children: [] },
  { id: 4, menuName: '角色管理', menuCode: 'role', menuType: 1, path: '/system/role', component: '/system/role', icon: 'UserFilled', parentId: 2, sort: 2, status: 1, children: [] },
  { id: 5, menuName: '菜单管理', menuCode: 'menu', menuType: 1, path: '/system/menu', component: '/system/menu', icon: 'Menu', parentId: 2, sort: 3, status: 1, children: [] },
  { id: 6, menuName: '组织管理', menuCode: 'org', menuType: 1, path: '/system/org', component: '/system/org', icon: 'OfficeBuilding', parentId: 2, sort: 4, status: 1, children: [] },
]

const mockRoles: RoleDto[] = [
  { id: 1, roleName: '超级管理员', roleCode: 'super_admin', sort: 1, status: 1, menuIds: [1,2,3,4,5,6], createTime: '2024-01-01' },
  { id: 2, roleName: '管理员', roleCode: 'admin', sort: 2, status: 1, menuIds: [1,2,3], createTime: '2024-01-15' },
  { id: 3, roleName: '普通用户', roleCode: 'user', sort: 3, status: 1, menuIds: [1], createTime: '2024-02-01' },
]

// Mock API 实现
export const mockApi = {
  // 登录
  async login(data: LoginRequest): Promise<LoginResponse> {
    await delay(500)
    if (data.username === 'admin' && data.password === '123456') {
      return {
        code: 200,
        message: '登录成功',
        data: {
          token: 'mock_token_' + Date.now(),
          user: mockUsers[0]
        }
      }
    }
    throw new Error('用户名或密码错误')
  },

  // 获取当前用户
  async getCurrentUser(): Promise<LoginResponse> {
    await delay(300)
    return {
      code: 200,
      message: 'success',
      data: {
        token: 'mock_token',
        user: mockUsers[0]
      }
    }
  },

  // 用户管理
  async getUsers(params: { keyword?: string; page: number; pageSize: number }) {
    await delay(400)
    let filtered = [...mockUsers]
    if (params.keyword) {
      const kw = params.keyword.toLowerCase()
      filtered = filtered.filter(u => 
        u.username.toLowerCase().includes(kw) ||
        u.realName.toLowerCase().includes(kw) ||
        u.email?.toLowerCase().includes(kw)
      )
    }
    const start = (params.page - 1) * params.pageSize
    const end = start + params.pageSize
    return {
      code: 200,
      data: {
        items: filtered.slice(start, end),
        total: filtered.length,
        pageNum: params.page,
        pageSize: params.pageSize
      }
    }
  },

  async getUserById(id: number): Promise<UserDto | undefined> {
    await delay(200)
    return mockUsers.find(u => u.id === id)
  },

  async createUser(data: Partial<UserDto>): Promise<number> {
    await delay(300)
    const newId = Math.max(...mockUsers.map(u => u.id)) + 1
    mockUsers.push({ ...data, id: newId } as UserDto)
    return newId
  },

  async updateUser(id: number, data: Partial<UserDto>): Promise<void> {
    await delay(300)
    const idx = mockUsers.findIndex(u => u.id === id)
    if (idx !== -1) {
      mockUsers[idx] = { ...mockUsers[idx], ...data }
    }
  },

  async deleteUser(id: number): Promise<void> {
    await delay(300)
    const idx = mockUsers.findIndex(u => u.id === id)
    if (idx !== -1) {
      mockUsers.splice(idx, 1)
    }
  },

  async updateUserStatus(id: number, status: number): Promise<void> {
    await delay(200)
    const user = mockUsers.find(u => u.id === id)
    if (user) {
      user.status = status
    }
  },

  // 组织管理
  async getOrgs(): Promise<OrgDto[]> {
    await delay(300)
    return mockOrgs
  },

  async getOrgTree(): Promise<OrgDto[]> {
    await delay(300)
    // 构建树形结构
    const buildTree = (items: OrgDto[], parentId: number = 0): OrgDto[] => {
      return items
        .filter(item => item.parentId === parentId)
        .map(item => ({
          ...item,
          children: buildTree(items, item.id)
        }))
    }
    return buildTree(mockOrgs)
  },

  async getOrgById(id: number): Promise<OrgDto | undefined> {
    await delay(200)
    return mockOrgs.find(o => o.id === id)
  },

  async createOrg(data: Partial<OrgDto>): Promise<number> {
    await delay(300)
    const newId = Math.max(...mockOrgs.map(o => o.id)) + 1
    mockOrgs.push({ ...data, id: newId } as OrgDto)
    return newId
  },

  async updateOrg(id: number, data: Partial<OrgDto>): Promise<void> {
    await delay(300)
    const idx = mockOrgs.findIndex(o => o.id === id)
    if (idx !== -1) {
      mockOrgs[idx] = { ...mockOrgs[idx], ...data }
    }
  },

  async deleteOrg(id: number): Promise<void> {
    await delay(300)
    const idx = mockOrgs.findIndex(o => o.id === id)
    if (idx !== -1) {
      mockOrgs.splice(idx, 1)
    }
  },

  // 菜单管理
  async getMenus(): Promise<MenuDto[]> {
    await delay(300)
    return mockMenus
  },

  async getMenuTree(): Promise<MenuDto[]> {
    await delay(300)
    const buildTree = (items: MenuDto[], parentId: number = 0): MenuDto[] => {
      return items
        .filter(item => item.parentId === parentId)
        .map(item => ({
          ...item,
          children: buildTree(items, item.id)
        }))
    }
    return buildTree(mockMenus)
  },

  async getMenuById(id: number): Promise<MenuDto | undefined> {
    await delay(200)
    return mockMenus.find(m => m.id === id)
  },

  async createMenu(data: Partial<MenuDto>): Promise<number> {
    await delay(300)
    const newId = Math.max(...mockMenus.map(m => m.id)) + 1
    mockMenus.push({ ...data, id: newId } as MenuDto)
    return newId
  },

  async updateMenu(id: number, data: Partial<MenuDto>): Promise<void> {
    await delay(300)
    const idx = mockMenus.findIndex(m => m.id === id)
    if (idx !== -1) {
      mockMenus[idx] = { ...mockMenus[idx], ...data }
    }
  },

  async deleteMenu(id: number): Promise<void> {
    await delay(300)
    const idx = mockMenus.findIndex(m => m.id === id)
    if (idx !== -1) {
      mockMenus.splice(idx, 1)
    }
  },

  async getMenusByRoleId(roleId: number): Promise<MenuDto[]> {
    await delay(200)
    const role = mockRoles.find(r => r.id === roleId)
    if (!role) return []
    return mockMenus.filter(m => role.menuIds.includes(m.id))
  },

  // 角色管理
  async getRoles(params?: { keyword?: string; page?: number; pageSize?: number }): Promise<{ items: RoleDto[]; total: number }> {
    await delay(300)
    let filtered = [...mockRoles]
    if (params?.keyword) {
      const kw = params.keyword.toLowerCase()
      filtered = filtered.filter(r => 
        r.roleName.toLowerCase().includes(kw) ||
        r.roleCode.toLowerCase().includes(kw)
      )
    }
    return {
      items: filtered,
      total: filtered.length
    }
  },

  async getRoleById(id: number): Promise<RoleDto | undefined> {
    await delay(200)
    return mockRoles.find(r => r.id === id)
  },

  async createRole(data: Partial<RoleDto>): Promise<number> {
    await delay(300)
    const newId = Math.max(...mockRoles.map(r => r.id)) + 1
    mockRoles.push({ ...data, id: newId } as RoleDto)
    return newId
  },

  async updateRole(id: number, data: Partial<RoleDto>): Promise<void> {
    await delay(300)
    const idx = mockRoles.findIndex(r => r.id === id)
    if (idx !== -1) {
      mockRoles[idx] = { ...mockRoles[idx], ...data }
    }
  },

  async deleteRole(id: number): Promise<void> {
    await delay(300)
    const idx = mockRoles.findIndex(r => r.id === id)
    if (idx !== -1) {
      mockRoles.splice(idx, 1)
    }
  },

  async assignPermissions(roleId: number, menuIds: number[]): Promise<void> {
    await delay(300)
    const role = mockRoles.find(r => r.id === roleId)
    if (role) {
      role.menuIds = menuIds
    }
  }
}

// 导出是否使用 Mock
export { useMock }
