# OG.Admin 前端项目开发文档

## 项目概述

OG.Admin 是一个基于 Vue 3 + Element Plus + TypeScript 的后台管理系统前端项目。

- **源码目录**: `OG.Admin/admin-system/src/`
- **原型参考**: `prototype/` (HTML 原型，可直接在浏览器打开预览)
- **技术栈**: Vue 3, TypeScript, Element Plus, Pinia, Vue Router, Axios, Vite
- **Mock 模式**: 默认启用，无需后端即可运行
- **默认账号**: `admin / 123456`

---

## 项目结构

```
admin-system/
├── src/
│   ├── api/                    # API 接口封装
│   │   ├── auth.ts            # 登录认证
│   │   ├── user.ts            # 用户管理
│   │   ├── org.ts             # 组织管理
│   │   ├── menu.ts            # 菜单管理
│   │   ├── role.ts            # 角色管理
│   │   └── mock.ts            # Mock 数据（默认启用）
│   ├── components/             # 公共组件
│   │   ├── SideMenu.vue       # 左侧导航菜单
│   │   └── Tabs.vue           # 多页签组件
│   ├── layouts/               # 布局组件
│   │   └── MainLayout.vue     # 主布局（侧边栏+头部+内容）
│   ├── router/                # 路由配置
│   │   └── index.ts
│   ├── stores/                # Pinia 状态管理
│   │   ├── user.ts            # 用户状态（token、用户信息、登录/登出）
│   │   └── app.ts             # 应用状态（侧边栏折叠、菜单列表、Tab页）
│   ├── types/                 # TypeScript 类型定义
│   │   └── index.ts           # 所有接口和类型定义
│   ├── utils/                 # 工具函数
│   │   ├── request.ts         # Axios 封装（拦截器、错误处理）
│   │   ├── storage.ts         # 本地存储封装
│   │   └── validate.ts        # 表单验证
│   └── views/                 # 页面组件
│       ├── Login.vue           # 登录页
│       ├── Dashboard.vue       # 工作台
│       └── system/             # 系统管理模块
│           ├── User.vue        # 用户管理
│           ├── Org.vue         # 组织管理
│           ├── Menu.vue        # 菜单管理
│           └── Role.vue        # 角色管理
├── index.html
├── vite.config.ts             # Vite 配置
├── package.json
├── tsconfig.json
├── .env                       # 环境变量
└── dist/                      # 构建产物
```

---

## 技术规范

### 编码规范

#### 文件命名
- Vue 组件: PascalCase, 如 `UserManagement.vue`
- TypeScript 文件: camelCase, 如 `userService.ts`
- 目录: camelCase 或 kebab-case

#### 组件规范
```vue
<!-- 使用 <script setup lang="ts"> 语法 -->
<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import type { UserDto } from '@/types'

// Props 定义
const props = defineProps<{
  user: UserDto
}>()

// Emits 定义
const emit = defineEmits<{
  (e: 'update', id: number): void
  (e: 'delete', id: number): void
}>()

// 响应式数据
const loading = ref(false)
const users = ref<UserDto[]>([])

// 计算属性
const activeUsers = computed(() => users.value.filter(u => u.status === 1))

// 生命周期
onMounted(() => {
  loadUsers()
})

// 方法
async function loadUsers() {
  loading.value = true
  try {
    const res = await userApi.getPageList({ pageNum: 1, pageSize: 10 })
    users.value = res.items
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="user-manager">
    <!-- 使用 Element Plus 组件 -->
    <el-table v-loading="loading" :data="activeUsers">
      <el-table-column prop="username" label="用户名" />
    </el-table>
  </div>
</template>

<style scoped lang="scss">
.user-manager {
  padding: 20px;
}
</style>
```

#### TypeScript 类型定义
```typescript
// 统一在 src/types/index.ts 中定义

// DTO 数据传输对象
export interface UserDto {
  id: number
  username: string
  realName?: string
  email?: string
  phone?: string
  status: number
  createTime?: string
}

// 请求参数
export interface CreateUserRequest {
  username: string
  password?: string
  realName?: string
  email?: string
  status?: number
}

// 分页结果
export interface PageResult<T> {
  items: T[]
  total: number
  pageNum: number
  pageSize: number
}

// API 响应
export interface ApiResponse<T = any> {
  code: number
  message: string
  data?: T
}
```

---

## 样式规范

### 主题色
```css
:root {
  /* 主色调 - 渐变紫蓝 */
  --primary: #667eea;
  --primary-dark: #5a67d8;
  --primary-light: #a3bffa;
  --gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  
  /* 背景色 */
  --bg-main: #f0f2f5;
  --bg-card: #ffffff;
  --bg-sidebar: #001529;
  
  /* 文字色 */
  --text-primary: #1f2937;
  --text-secondary: #6b7280;
  --text-light: #9ca3af;
  
  /* 边框 */
  --border-color: #e5e7eb;
  
  /* 阴影 */
  --shadow-sm: 0 1px 2px rgba(0,0,0,0.05);
  --shadow-md: 0 4px 6px -1px rgba(0,0,0,0.1);
  --shadow-lg: 0 10px 15px -3px rgba(0,0,0,0.1);
  
  /* 圆角 */
  --radius-sm: 6px;
  --radius-md: 8px;
  --radius-lg: 12px;
}
```

### 样式指南
1. **优先使用 Element Plus 组件**的样式和主题变量
2. **自定义样式添加 `scoped`** 避免污染全局
3. **使用 CSS 变量** 保持主题一致性
4. **移动端适配** 使用媒体查询 `@media (max-width: 768px)`

### 常用样式类
```vue
<style scoped>
/* 卡片容器 */
.card {
  background: var(--bg-card);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-md);
}

/* 页面标题 */
.page-header {
  font-size: 18px;
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 20px;
}

/* 操作按钮组 */
.action-bar {
  display: flex;
  gap: 12px;
  margin-bottom: 16px;
}
</style>
```

---

## API 接口规范

### 请求封装
在 `src/utils/request.ts` 中统一封装 Axios，包含：
- 请求拦截器（添加 Token）
- 响应拦截器（统一错误处理、401 处理）
- 基地址配置

### 接口文件结构
每个模块一个文件，如 `user.ts`:
```typescript
import request from '@/utils/request'
import { mockApi, useMock } from './mock'
import type { UserDto, CreateUserRequest, PageResult } from '@/types'

export const userApi = {
  // 获取分页列表
  async getPageList(params: UserQueryRequest): Promise<PageResult<UserDto>> {
    if (useMock) {
      return mockApi.getUsers(params)
    }
    return request.get('/users', { params })
  },

  // 根据ID获取详情
  async getById(id: number): Promise<UserDto | undefined> {
    if (useMock) {
      return mockApi.getUserById(id)
    }
    return request.get(`/users/${id}`)
  },

  // 新增
  async create(data: CreateUserRequest): Promise<number> {
    if (useMock) {
      return mockApi.createUser(data)
    }
    return request.post('/users', data)
  },

  // 更新
  async update(id: number, data: UpdateUserRequest): Promise<void> {
    if (useMock) {
      return mockApi.updateUser(id, data)
    }
    return request.put(`/users/${id}`, data)
  },

  // 删除
  async delete(id: number): Promise<void> {
    if (useMock) {
      return mockApi.deleteUser(id)
    }
    return request.delete(`/users/${id}`)
  },
}
```

### API 完整列表

#### 认证接口
| 方法 | 路径 | 说明 | 请求体 |
|------|------|------|--------|
| POST | /api/auth/login | 登录 | `{username, password, rememberMe}` |
| POST | /api/auth/logout | 登出 | - |
| GET | /api/auth/current | 获取当前用户 | - |

#### 用户管理
| 方法 | 路径 | 说明 | 参数 |
|------|------|------|------|
| GET | /api/users | 分页列表 | `?keyword=&pageNum=1&pageSize=10` |
| GET | /api/users/{id} | 获取详情 | - |
| POST | /api/users | 新增用户 | `{username, password, realName, ...}` |
| PUT | /api/users/{id} | 更新用户 | `{realName, email, status, ...}` |
| DELETE | /api/users/{id} | 删除用户 | - |
| PUT | /api/users/{id}/status/{status} | 启用/禁用 | status: 0/1 |
| PUT | /api/users/{id}/password | 修改密码 | `{oldPassword, newPassword}` |

#### 组织管理
| 方法 | 路径 | 说明 | 参数 |
|------|------|------|------|
| GET | /api/orgs | 获取列表 | - |
| GET | /api/orgs/tree | 获取树形 | - |
| GET | /api/orgs/{id} | 获取详情 | - |
| POST | /api/orgs | 新增 | `{orgName, orgCode, parentId, ...}` |
| PUT | /api/orgs/{id} | 更新 | `{orgName, orgCode, ...}` |
| DELETE | /api/orgs/{id} | 删除 | - |

#### 菜单管理
| 方法 | 路径 | 说明 | 参数 |
|------|------|------|------|
| GET | /api/menus | 获取列表 | - |
| GET | /api/menus/tree | 获取树形 | - |
| GET | /api/menus/{id} | 获取详情 | - |
| GET | /api/menus/role/{roleId} | 角色菜单 | - |
| POST | /api/menus | 新增 | `{menuName, path, component, ...}` |
| PUT | /api/menus/{id} | 更新 | `{menuName, path, ...}` |
| DELETE | /api/menus/{id} | 删除 | - |

#### 角色管理
| 方法 | 路径 | 说明 | 参数 |
|------|------|------|------|
| GET | /api/roles | 分页列表 | `?keyword=&pageNum=1&pageSize=10` |
| GET | /api/roles/all | 获取全部 | - |
| GET | /api/roles/{id} | 获取详情 | - |
| POST | /api/roles | 新增 | `{roleName, roleCode, ...}` |
| PUT | /api/roles/{id} | 更新 | `{roleName, ...}` |
| DELETE | /api/roles/{id} | 删除 | - |
| PUT | /api/roles/{id}/menus | 分配权限 | `{menuIds: [1,2,3]}` |

---

## Mock 数据规范

### 启用/禁用
```bash
# .env 文件
VITE_USE_MOCK=1          # 启用 Mock
VITE_USE_MOCK=           # 禁用 Mock，使用真实 API

VITE_API_BASE_URL=http://localhost:5000  # 真实 API 地址
```

### Mock 数据结构
在 `src/api/mock.ts` 中定义：
```typescript
// 模拟数据
const mockUsers: UserDto[] = [
  { id: 1, username: 'admin', realName: '系统管理员', status: 1, ... },
  { id: 2, username: 'zhangsan', realName: '张三', status: 1, ... },
]

// 模拟延迟
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms))

// API 实现
export const mockApi = {
  async getUsers(params) {
    await delay(300)  // 模拟网络延迟
    // 过滤、分页逻辑
    return { items: [...], total: 10, pageNum: 1, pageSize: 10 }
  },
}
```

---

## 页面开发流程

### 1. 参考原型
在 `prototype/` 目录下找到对应的 HTML 原型，用浏览器打开预览效果和交互。

### 2. 创建页面组件
在 `src/views/system/` 下创建，如 `User.vue`:

```vue
<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { userApi } from '@/api/user'
import type { UserDto } from '@/types'

// 响应式数据
const loading = ref(false)
const userList = ref<UserDto[]>([])

// 加载数据
async function loadData() {
  loading.value = true
  try {
    const res = await userApi.getPageList({ pageNum: 1, pageSize: 10 })
    userList.value = res.items
  } catch (error) {
    ElMessage.error('加载失败')
  } finally {
    loading.value = false
  }
}

onMounted(loadData)
</script>

<template>
  <div class="user-page">
    <!-- 页面标题 -->
    <h2 class="page-header">用户管理</h2>
    
    <!-- 操作栏 -->
    <div class="action-bar">
      <el-button type="primary" @click="handleAdd">新增用户</el-button>
    </div>
    
    <!-- 表格 -->
    <el-table v-loading="loading" :data="userList">
      <el-table-column prop="username" label="用户名" />
      <el-table-column prop="realName" label="姓名" />
      <el-table-column prop="status" label="状态">
        <template #default="{ row }">
          <el-tag :type="row.status === 1 ? 'success' : 'danger'">
            {{ row.status === 1 ? '启用' : '禁用' }}
          </el-tag>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<style scoped lang="scss">
.user-page {
  padding: 20px;
}
</style>
```

### 3. 配置路由
在 `src/router/index.ts` 中添加：
```typescript
{
  path: 'system/user',
  name: 'UserManagement',
  component: () => import('@/views/system/User.vue'),
  meta: { title: '用户管理', icon: 'User' },
}
```

### 4. 添加菜单（如需要）
在 `src/api/mock.ts` 的 `mockMenus` 中添加：
```typescript
{ 
  id: 7, 
  menuName: '设备管理', 
  menuCode: 'device',
  path: '/system/device', 
  component: '/system/device',
  icon: 'Device', 
  parentId: 2, 
  sort: 5, 
  status: 1 
}
```

---

## 常用命令

```bash
# 安装依赖
cd OG.Admin/admin-system
npm install

# 开发模式（Mock 模式，默认端口 3000）
npm run dev

# 构建生产版本
npm run build

# 预览构建结果
npm run preview
```

---

## 常见问题

### 1. Mock 模式下菜单不显示
检查 `src/api/mock.ts` 中 `mockMenus` 数组是否包含完整菜单数据。

### 2. API 请求失败
- 确认 `.env` 中 `VITE_USE_MOCK` 设置正确
- 或确认后端 `dotnet run` 已启动

### 3. 样式不生效
- 检查是否添加了 `scoped`
- 检查 Element Plus 主题是否正确引入

### 4. TypeScript 类型报错
- 检查 `src/types/index.ts` 是否定义了相关类型
- 使用 `import type` 导入类型

---

## 设计参考

### 原型预览
直接在浏览器打开 `prototype/main.html` 登录后预览完整交互效果。

### UI 风格
- 侧边栏：深色背景 `#001529`
- 主题色：渐变紫蓝 `#667eea → #764ba2`
- 卡片：白色背景 + 圆角 + 阴影
- 参考：Ant Design Pro 风格

---

## 后续开发指南

### 新增模块
1. 在 `src/views/system/` 下创建页面组件
2. 在 `src/api/` 下创建接口文件
3. 在 `src/router/index.ts` 添加路由
4. 在 `src/api/mock.ts` 添加 Mock 数据和菜单配置

### 对接后端
1. 修改 `.env`: `VITE_USE_MOCK=`
2. 启动后端: `cd AdminSystem.Api && dotnet run`
3. 验证接口连通性

### 部署
```bash
npm run build
# 构建产物在 dist/ 目录
# 可部署到任意静态服务器
```
