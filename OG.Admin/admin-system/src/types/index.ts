export interface LoginRequest {
  username: string
  password: string
  rememberMe?: boolean
}

export interface LoginResponse {
  code: number
  message: string
  data?: {
    token: string
    user: UserDto
  }
}

export interface CurrentUserResponse {
  code: number
  message: string
  data?: {
    user: UserDto
  }
}

export interface UserDto {
  id: number
  username: string
  realName?: string
  nickname?: string
  email?: string
  phone?: string
  avatar?: string
  status: number
  orgId?: number
  orgName?: string
  roleId?: number
  roleName?: string
  createTime?: string
  updateTime?: string
  remark?: string
  roles?: RoleSimpleDto[]
}

export interface CreateUserRequest {
  username: string
  password?: string
  realName?: string
  nickname?: string
  email?: string
  phone?: string
  avatar?: string
  status?: number
  orgId?: number
  remark?: string
  roleId?: number
}

export interface UpdateUserRequest {
  realName?: string
  nickname?: string
  email?: string
  phone?: string
  avatar?: string
  status?: number
  orgId?: number
  remark?: string
  roleId?: number
}

export interface UserQueryRequest {
  keyword?: string
  orgId?: number
  status?: number
  pageNum?: number
  pageSize?: number
}

export interface OrgDto {
  id: number
  orgName: string
  orgCode?: string
  parentId?: number
  sort: number
  status: number
  createTime?: string
  updateTime?: string
  remark?: string
  children?: OrgDto[]
}

export interface CreateOrgRequest {
  orgName: string
  orgCode?: string
  parentId?: number
  sort?: number
  status?: number
  remark?: string
}

export interface UpdateOrgRequest {
  orgName: string
  orgCode?: string
  parentId?: number
  sort?: number
  status?: number
  remark?: string
}

export interface MenuDto {
  id: number
  menuName: string
  menuCode?: string
  menuType: number
  path?: string
  component?: string
  icon?: string
  parentId?: number
  sort: number
  status: number
  isVisible?: boolean
  isCache?: boolean
  isAffix?: boolean
  isKeepAlive?: boolean
  createTime?: string
  updateTime?: string
  remark?: string
  children?: MenuDto[]
}

export interface CreateMenuRequest {
  menuName: string
  menuCode?: string
  menuType?: number
  path?: string
  component?: string
  icon?: string
  parentId?: number
  sort?: number
  status?: number
  isVisible?: boolean
  isCache?: boolean
  isAffix?: boolean
  isKeepAlive?: boolean
  remark?: string
}

export interface UpdateMenuRequest {
  menuName: string
  menuCode?: string
  menuType?: number
  path?: string
  component?: string
  icon?: string
  parentId?: number
  sort?: number
  status?: number
  isVisible?: boolean
  isCache?: boolean
  isAffix?: boolean
  isKeepAlive?: boolean
  remark?: string
}

export interface RoleDto {
  id: number
  roleName: string
  roleCode?: string
  sort: number
  status: number
  createTime?: string
  updateTime?: string
  remark?: string
  menus?: MenuSimpleDto[]
  menuIds?: number[]
}

export interface RoleSimpleDto {
  id: number
  roleName: string
  roleCode?: string
}

export interface MenuSimpleDto {
  id: number
  menuName: string
}

export interface CreateRoleRequest {
  roleName: string
  roleCode?: string
  sort?: number
  status?: number
  remark?: string
  menuIds?: number[]
}

export interface UpdateRoleRequest {
  roleName: string
  roleCode?: string
  sort?: number
  status?: number
  remark?: string
  menuIds?: number[]
}

export interface AssignPermissionsRequest {
  menuIds: number[]
}

export interface ChangePasswordRequest {
  oldPassword: string
  newPassword: string
}

// 分页结果
export interface PageResult<T> {
  items: T[]
  total: number
  pageNum: number
  pageSize: number
}

// API 统一响应格式
export interface ApiResponse<T = any> {
  code: number
  message: string
  data?: T
}

export interface RouteTab {
  path: string
  name: string
  title: string
  closable?: boolean
}
