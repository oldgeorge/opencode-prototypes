# 后台管理系统 - 开发规格说明书

## 1. 项目概述

**项目名称：** OG.Admin  
**项目类型：** 前后端分离的后台管理系统  
**技术栈：**  
- 前端：Vue 3 + Element Plus + Vite
- 后端：.NET 10 WebAPI + SqlSugar ORM
- 默认数据库：SQLite（支持切换 MySQL/PostgreSQL/SQL Server）

---

## 2. 功能模块

### 2.1 用户登录
- 用户名 + 密码登录
- JWT Token 认证
- Token 过期处理
- 登录页记住密码

### 2.2 用户管理
- 用户列表（分页、搜索）
- 新增用户
- 编辑用户
- 删除用户（软删除）
- 重置密码
- 分配角色

### 2.3 组织架构管理
- 树形组织架构
- 新增组织
- 编辑组织
- 删除组织（需确认无子组织和人员）
- 拖拽排序（可选）

### 2.4 菜单维护
- 树形菜单（左侧菜单来源）
- 菜单类型：目录 / 菜单 / 按钮
- 图标选择
- 路由配置
- 排序

### 2.5 角色权限
- 角色列表
- 新增角色
- 编辑角色
- 删除角色
- 权限分配（菜单勾选）
- 角色绑定用户

---

## 3. 数据库设计

### 3.1 表结构

```
SysUser (用户表)
├── Id (主键)
├── UserName (用户名)
├── Password (密码，加密存储)
├── RealName (真实姓名)
├── Email (邮箱)
├── Phone (手机)
├── Status (状态：0禁用 1启用)
├── OrgId (所属组织)
├── CreateTime (创建时间)
└── IsDeleted (软删除标记)

SysOrg (组织表)
├── Id (主键)
├── ParentId (父级ID)
├── Name (组织名称)
├── Code (组织编码)
├── Sort (排序)
└── Status (状态)

SysMenu (菜单表)
├── Id (主键)
├── ParentId (父级ID)
├── Name (菜单名称)
├── Code (菜单编码)
├── Icon (图标)
├── Path (路由路径)
├── Component (前端组件)
├── Sort (排序)
├── Type (类型：0目录 1菜单 2按钮)
└── Status (状态)

SysRole (角色表)
├── Id (主键)
├── Name (角色名称)
├── Code (角色编码)
├── Description (描述)
├── CreateTime (创建时间)
└── Status (状态)

SysUserRole (用户角色关联表)
├── UserId
└── RoleId

SysRoleMenu (角色菜单关联表)
├── RoleId
└── MenuId
```

### 3.2 多数据库支持
通过配置文件切换数据库类型：
- `SQLite` (默认)
- `MySQL`
- `PostgreSQL`
- `SQL Server`

SqlSugar 实现实体自动建表。

---

## 4. 后端 API 设计

### 4.1 认证模块
| 方法 | 路径 | 说明 |
|------|------|------|
| POST | /api/auth/login | 用户登录 |
| POST | /api/auth/logout | 登出 |
| GET | /api/auth/current | 获取当前用户信息 |

### 4.2 用户管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/users | 分页获取用户列表 |
| GET | /api/users/{id} | 获取用户详情 |
| POST | /api/users | 新增用户 |
| PUT | /api/users/{id} | 更新用户 |
| DELETE | /api/users/{id} | 删除用户 |
| PUT | /api/users/{id}/reset-password | 重置密码 |
| PUT | /api/users/{id}/roles | 分配角色 |

### 4.3 组织管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/orgs | 获取组织树 |
| GET | /api/orgs/{id} | 获取组织详情 |
| POST | /api/orgs | 新增组织 |
| PUT | /api/orgs/{id} | 更新组织 |
| DELETE | /api/orgs/{id} | 删除组织 |

### 4.4 菜单管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/menus | 获取菜单树 |
| GET | /api/menus/{id} | 获取菜单详情 |
| POST | /api/menus | 新增菜单 |
| PUT | /api/menus/{id} | 更新菜单 |
| DELETE | /api/menus/{id} | 删除菜单 |

### 4.5 角色管理
| 方法 | 路径 | 说明 |
|------|------|------|
| GET | /api/roles | 获取角色列表 |
| GET | /api/roles/{id} | 获取角色详情 |
| POST | /api/roles | 新增角色 |
| PUT | /api/roles/{id} | 更新角色 |
| DELETE | /api/roles/{id} | 删除角色 |
| GET | /api/roles/{id}/menus | 获取角色菜单权限 |
| PUT | /api/roles/{id}/menus | 分配菜单权限 |

---

## 5. 前端页面设计

### 5.1 页面列表
- `/login` - 登录页
- `/` - 布局页（包含左侧菜单）
- `/dashboard` - 首页/仪表盘
- `/system/user` - 用户管理
- `/system/org` - 组织管理
- `/system/menu` - 菜单管理
- `/system/role` - 角色管理

### 5.2 布局结构
```
┌──────────────────────────────────────────────────────────┐
│  Logo          [顶部导航栏]              用户信息 │
├─────────────┬────────────────────────────────────────────┤
│             │  [多页签栏]  tab1 tab2 tab3 ✕           │
│  左侧树形    ├────────────────────────────────────────────┤
│  多级菜单    │                                            │
│             │              页面内容区                     │
│             │                                            │
│             │                                            │
└─────────────┴────────────────────────────────────────────┘
```

**顶部栏：**
- 左侧：Logo 区域
- 右侧：用户名、头像、退出登录

**左侧菜单：**
- 树形多级菜单
- 支持展开/折叠
- 根据用户权限动态显示

**多页签功能：**
- 显示已打开的页面 tab
- 关闭当前页
- 关闭其他页
- 关闭全部页
- 右侧快捷操作菜单

### 5.3 公共组件
- 顶部导航栏
- 左侧树形菜单
- 多页签组件（支持关闭操作）
- 面包屑导航
- 分页组件

---

## 6. 项目结构

### 6.1 后端结构
```
AdminSystem.Api/
├── Controllers/
│   ├── AuthController.cs
│   ├── UserController.cs
│   ├── OrgController.cs
│   ├── MenuController.cs
│   └── RoleController.cs
├── Entities/
│   ├── SysUser.cs
│   ├── SysOrg.cs
│   ├── SysMenu.cs
│   ├── SysRole.cs
│   ├── SysUserRole.cs
│   └── SysRoleMenu.cs
├── DTOs/
├── Services/
├── SqlSugar/
└── Program.cs

AdminSystem.Core/
├── IRepository/
├── Repository/
└── Extensions/
```

### 6.2 前端结构
```
admin-system/
├── src/
│   ├── api/
│   ├── components/
│   ├── layouts/
│   ├── router/
│   ├── stores/
│   ├── views/
│   ├── App.vue
│   └── main.ts
├── vite.config.ts
└── package.json
```

---

## 7. 验收标准

### 7.1 功能验收
- [ ] 用户能够正常登录和登出
- [ ] 用户列表能够分页展示、搜索、新增、编辑、删除
- [ ] 组织架构能够树形展示、新增、编辑、删除
- [ ] 菜单能够树形展示、新增、编辑、删除
- [ ] 角色能够新增、编辑、删除、分配权限
- [ ] 左侧菜单根据用户角色动态显示

### 7.2 技术验收
- [ ] 数据库能够通过 SqlSugar 实体自动创建
- [ ] 支持切换 SQLite / MySQL / PostgreSQL / SQL Server
- [ ] 前端页面能够独立运行（Vite Dev Server）
- [ ] 前后端能够正常通信

---

## 8. 开发优先级

**第一阶段：**
1. 搭建项目框架（前后端）
2. SqlSugar 数据库配置 + 实体自动建表
3. 实现用户登录（JWT）
4. 用户管理 CRUD

**第二阶段：**
5. 组织架构管理
6. 菜单管理
7. 角色管理

**第三阶段：**
8. 权限分配（角色绑定菜单）
9. 前端动态菜单渲染
10. 细节优化
