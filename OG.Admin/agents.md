# OG.Admin 前端项目开发文档

## 项目概述

OG.Admin 是一个基于 Vue 3 + Element Plus + TypeScript 的后台管理系统前端项目。

- **源码目录**: `OG.Admin/admin-system/src/`
- **原型参考**: `prototype/` (HTML 原型，可直接在浏览器打开预览)
- **技术栈**: Vue 3, TypeScript, Element Plus, Pinia, Vue Router, Axios, Vite
- **Mock 模式**: 默认启用，无需后端即可运行

---

## 项目结构

```
admin-system/
├── src/
│   ├── api/              # API 接口封装
│   │   ├── auth.ts       # 登录相关
│   │   ├── user.ts       # 用户管理
│   │   ├── org.ts        # 组织管理
│   │   ├── menu.ts       # 菜单管理
│   │   ├── role.ts       # 角色管理
│   │   └── mock.ts       # Mock 数据（默认启用）
│   ├── components/        # 公共组件
│   │   ├── SideMenu.vue  # 左侧菜单
│   │   └── Tabs.vue      # 多页签
│   ├── layouts/          # 布局组件
│   │   └── MainLayout.vue
│   ├── router/           # 路由配置
│   ├── stores/           # Pinia 状态管理
│   │   ├── user.ts       # 用户状态
│   │   └── app.ts        # 应用状态
│   ├── types/            # TypeScript 类型定义
│   ├── utils/            # 工具函数
│   │   ├── request.ts    # Axios 封装
│   │   ├── storage.ts    # 本地存储
│   │   └── validate.ts   # 表单验证
│   └── views/            # 页面组件
│       ├── Login.vue     # 登录页
│       ├── Dashboard.vue  # 工作台
│       └── system/        # 系统管理模块
│           ├── User.vue  # 用户管理
│           ├── Org.vue   # 组织管理
│           ├── Menu.vue  # 菜单管理
│           └── Role.vue  # 角色管理
├── index.html
├── vite.config.ts
├── package.json
└── tsconfig.json
```

---

## 常用开发命令

```bash
# 安装依赖
cd OG.Admin/admin-system
npm install

# 开发模式运行（Mock 模式）
npm run dev

# 构建生产版本
npm run build

# 预览构建结果
npm run preview
```

---

## Mock 数据切换

Mock 模式默认启用。如需连接真实后端：

1. 修改 `.env` 文件：
```bash
# 禁用 Mock
VITE_USE_MOCK=

# 启用真实 API 地址
VITE_API_BASE_URL=http://localhost:5000
```

2. 确保后端已启动：
```bash
cd OG.Admin/AdminSystem.Api
dotnet run
# API 地址: http://localhost:5000/swagger
```

---

## API 接口说明

所有 API 统一前缀 `/api`，在 `src/utils/request.ts` 中配置。

### 认证接口
| 方法 | 路径 | 说明 |
|------|------|------|
| POST | /api/auth/login | 登录 |
| POST | /api/auth/logout | 登出 |
| GET | /api/auth/current | 获取当前用户 |

### 用户管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/users | 分页列表 |
| GET | /api/users/{id} | 获取详情 |
| POST | /api/users | 新增用户 |
| PUT | /api/users/{id} | 更新用户 |
| DELETE | /api/users/{id} | 删除用户 |
| PUT | /api/users/{id}/status/{status} | 启用/禁用 |

### 组织管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/orgs | 获取列表 |
| GET | /api/orgs/tree | 获取树形 |
| GET | /api/orgs/{id} | 获取详情 |
| POST | /api/orgs | 新增 |
| PUT | /api/orgs/{id} | 更新 |
| DELETE | /api/orgs/{id} | 删除 |

### 菜单管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/menus | 获取列表 |
| GET | /api/menus/tree | 获取树形 |
| GET | /api/menus/role/{roleId} | 获取角色菜单 |
| POST | /api/menus | 新增 |
| PUT | /api/menus/{id} | 更新 |
| DELETE | /api/menus/{id} | 删除 |

### 角色管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/roles | 分页列表 |
| GET | /api/roles/all | 获取全部 |
| GET | /api/roles/{id} | 获取详情 |
| POST | /api/roles | 新增 |
| PUT | /api/roles/{id} | 更新 |
| DELETE | /api/roles/{id} | 删除 |
| PUT | /api/roles/{id}/menus | 分配权限 |

---

## 开发规范

### 组件规范
- 使用 `<script setup lang="ts">` 语法
- 组件文件用 PascalCase 命名
- Props 使用 TypeScript 类型定义

### 样式规范
- 优先使用 Element Plus 组件
- 自定义样式添加 `scoped`
- 使用 CSS 变量统一主题色

### API 规范
- 每个模块对应一个 `api/xxx.ts` 文件
- 使用 async/await
- 统一错误处理

### Mock 数据规范
- 在 `src/api/mock.ts` 中添加模拟数据
- 保持数据结构与真实 API 一致
- 模拟延迟 200-500ms

---

## 页面开发流程

### 1. 参考原型
在 `prototype/` 目录下找到对应的 HTML 原型文件，用浏览器打开预览效果。

### 2. 创建页面
在 `src/views/` 或 `src/views/system/` 下创建 Vue 组件。

### 3. 配置路由
在 `src/router/index.ts` 中添加路由配置：

```typescript
{
  path: 'system/xxx',
  name: 'XxxManagement',
  component: () => import('@/views/system/Xxx.vue'),
  meta: { title: 'XXX管理', icon: 'xxx' },
}
```

### 4. 实现功能
- 调用 API 或 Mock 数据
- 使用 Element Plus 组件
- 遵循项目样式规范

### 5. 添加菜单（如需要）
在 `mock.ts` 的 `mockMenus` 数组中添加菜单配置。

---

## 登录账号

- **Mock 模式**: `admin / 123456`
- **后端模式**: `admin / 123456`

---

## 常见问题

### 1. Mock 模式下菜单不显示
检查 `src/api/mock.ts` 中 `mockMenus` 是否包含完整菜单数据。

### 2. API 请求失败
确认 `.env` 中 `VITE_USE_MOCK` 是否正确设置，或后端是否正常启动。

### 3. 样式不生效
检查是否添加了 `scoped`，或 Element Plus 主题是否正确引入。

---

## 后续开发

如需继续完善功能：

1. **后端接口对接**: 关闭 Mock 模式，连接真实 API
2. **新增模块**: 按现有模块结构创建新模块
3. **样式优化**: 参考 Element Plus 官方主题定制
4. **部署**: 使用 `npm run build` 构建，部署到静态服务器
