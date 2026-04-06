<template>
  <div class="user-page">
    <!-- 页面标题 -->
    <div class="page-header">
      <div class="header-left">
        <h2 class="page-title">用户管理</h2>
        <span class="page-desc">管理系统用户，支持增删查改</span>
      </div>
      <el-button type="primary" @click="openAddDialog">
        <el-icon><Plus /></el-icon>
        新增用户
      </el-button>
    </div>

    <!-- 搜索栏 -->
    <div class="search-bar">
      <el-input
        v-model="searchKeyword"
        placeholder="搜索用户名、姓名或邮箱..."
        clearable
        class="search-input"
        @clear="handleSearch"
        @keyup.enter="handleSearch"
      >
        <template #prefix>
          <el-icon><Search /></el-icon>
        </template>
      </el-input>
      <el-button type="primary" @click="handleSearch">搜索</el-button>
    </div>

    <!-- 数据表格 -->
    <div class="table-card">
      <el-table
        v-loading="loading"
        :data="userList"
        stripe
        class="user-table"
      >
        <el-table-column type="index" label="序号" width="70" align="center" />
        <el-table-column prop="username" label="用户名" min-width="120">
          <template #default="{ row }">
            <div class="user-cell">
              <el-avatar :size="32" class="user-avatar">
                {{ row.username?.charAt(0)?.toUpperCase() }}
              </el-avatar>
              <div class="user-info">
                <span class="username">{{ row.username }}</span>
                <span class="user-id">ID: {{ row.id }}</span>
              </div>
            </div>
          </template>
        </el-table-column>
        <el-table-column prop="realName" label="姓名" min-width="100" />
        <el-table-column prop="email" label="邮箱" min-width="160" />
        <el-table-column prop="phone" label="手机" min-width="120" />
        <el-table-column prop="orgName" label="所属组织" min-width="100" />
        <el-table-column prop="roleName" label="角色" min-width="100">
          <template #default="{ row }">
            <el-tag :type="row.roleId === 1 ? 'danger' : 'primary'" size="small">
              {{ row.roleName || '-' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="状态" width="90" align="center">
          <template #default="{ row }">
            <el-switch
              v-model="row.status"
              :active-value="1"
              :inactive-value="0"
              @change="handleStatusChange(row)"
            />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180" fixed="right" align="center">
          <template #default="{ row }">
            <div class="action-buttons">
              <el-button type="primary" link @click="openEditDialog(row)">
                <el-icon><Edit /></el-icon>
                编辑
              </el-button>
              <el-button type="danger" link @click="handleDelete(row)">
                <el-icon><Delete /></el-icon>
                删除
              </el-button>
            </div>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <div class="pagination">
        <el-pagination
          v-model:current-page="pagination.pageNum"
          v-model:page-size="pagination.pageSize"
          :total="pagination.total"
          :page-sizes="[10, 20, 50]"
          layout="total, sizes, prev, pager, next"
          background
          @size-change="loadUsers"
          @current-change="loadUsers"
        />
      </div>
    </div>

    <!-- 新增/编辑弹窗 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑用户' : '新增用户'"
      width="560px"
      :close-on-click-modal="false"
      class="user-dialog"
    >
      <el-form
        ref="formRef"
        :model="userForm"
        :rules="formRules"
        label-width="90px"
      >
        <el-form-item label="用户名" prop="username">
          <el-input
            v-model="userForm.username"
            placeholder="请输入用户名"
            :disabled="isEdit"
          />
        </el-form-item>
        <el-form-item label="姓名" prop="realName">
          <el-input v-model="userForm.realName" placeholder="请输入姓名" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="userForm.email" placeholder="请输入邮箱" />
        </el-form-item>
        <el-form-item label="手机" prop="phone">
          <el-input v-model="userForm.phone" placeholder="请输入手机号" />
        </el-form-item>
        <el-form-item label="所属组织" prop="orgId">
          <el-select v-model="userForm.orgId" placeholder="请选择组织" style="width: 100%">
            <el-option label="技术部" :value="1" />
            <el-option label="运营部" :value="2" />
            <el-option label="市场部" :value="3" />
          </el-select>
        </el-form-item>
        <el-form-item label="角色" prop="roleId">
          <el-select v-model="userForm.roleId" placeholder="请选择角色" style="width: 100%">
            <el-option label="超级管理员" :value="1" />
            <el-option label="管理员" :value="2" />
            <el-option label="普通用户" :value="3" />
          </el-select>
        </el-form-item>
        <el-form-item v-if="!isEdit" label="密码" prop="password">
          <el-input
            v-model="userForm.password"
            type="password"
            placeholder="请输入密码"
            show-password
          />
        </el-form-item>
        <el-form-item label="状态">
          <el-radio-group v-model="userForm.status">
            <el-radio :label="1">启用</el-radio>
            <el-radio :label="0">禁用</el-radio>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitLoading" @click="handleSubmit">
          确定
        </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Search, Edit, Delete } from '@element-plus/icons-vue'
import { userApi } from '@/api/user'
import type { UserDto, CreateUserRequest } from '@/types'

const loading = ref(false)
const submitLoading = ref(false)
const dialogVisible = ref(false)
const isEdit = ref(false)
const formRef = ref()

const searchKeyword = ref('')
const userList = ref<UserDto[]>([])
const pagination = reactive({
  pageNum: 1,
  pageSize: 10,
  total: 0
})

const userForm = reactive<CreateUserRequest & { id?: number }>({
  username: '',
  realName: '',
  email: '',
  phone: '',
  orgId: undefined,
  roleId: undefined,
  password: '',
  status: 1
})

const formRules = {
  username: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  password: [{ required: true, message: '请输入密码', trigger: 'blur' }, { min: 6, message: '密码至少6位', trigger: 'blur' }]
}

const loadUsers = async () => {
  loading.value = true
  try {
    const res = await userApi.getPageList({
      keyword: searchKeyword.value,
      pageNum: pagination.pageNum,
      pageSize: pagination.pageSize
    })
    userList.value = res.items
    pagination.total = res.total
  } catch (error) {
    ElMessage.error('加载用户列表失败')
  } finally {
    loading.value = false
  }
}

const handleSearch = () => {
  pagination.pageNum = 1
  loadUsers()
}

const openAddDialog = () => {
  isEdit.value = false
  Object.assign(userForm, {
    id: undefined,
    username: '',
    realName: '',
    email: '',
    phone: '',
    orgId: undefined,
    roleId: undefined,
    password: '',
    status: 1
  })
  dialogVisible.value = true
}

const openEditDialog = (row: UserDto) => {
  isEdit.value = true
  Object.assign(userForm, {
    id: row.id,
    username: row.username,
    realName: row.realName,
    email: row.email,
    phone: row.phone,
    orgId: row.orgId,
    roleId: row.roleId,
    password: '',
    status: row.status
  })
  dialogVisible.value = true
}

const handleSubmit = async () => {
  if (!formRef.value) return
  await formRef.value.validate(async (valid) => {
    if (!valid) return
    submitLoading.value = true
    try {
      if (isEdit.value && userForm.id) {
        await userApi.update(userForm.id, userForm)
        ElMessage.success('更新成功')
      } else {
        await userApi.create(userForm)
        ElMessage.success('创建成功')
      }
      dialogVisible.value = false
      loadUsers()
    } catch (error: any) {
      ElMessage.error(error.message || '操作失败')
    } finally {
      submitLoading.value = false
    }
  })
}

const handleStatusChange = async (row: UserDto) => {
  try {
    await userApi.updateStatus(row.id, row.status)
    ElMessage.success(row.status === 1 ? '已启用' : '已禁用')
  } catch (error) {
    row.status = row.status === 1 ? 0 : 1
    ElMessage.error('状态更新失败')
  }
}

const handleDelete = async (row: UserDto) => {
  try {
    await ElMessageBox.confirm(`确定要删除用户 "${row.username}" 吗？`, '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
    await userApi.delete(row.id)
    ElMessage.success('删除成功')
    loadUsers()
  } catch (error: any) {
    if (error !== 'cancel') {
      ElMessage.error('删除失败')
    }
  }
}

onMounted(() => {
  loadUsers()
})
</script>

<style scoped lang="scss">
.user-page {
  padding: 0;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.header-left {
  display: flex;
  flex-direction: column;
}

.page-title {
  font-size: 20px;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
}

.page-desc {
  font-size: 13px;
  color: #9ca3af;
  margin-top: 4px;
}

.search-bar {
  display: flex;
  gap: 12px;
  margin-bottom: 16px;
}

.search-input {
  width: 320px;
}

.table-card {
  background: #fff;
  border-radius: 12px;
  padding: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
}

.user-table {
  :deep(.el-table__header th) {
    background: #f9fafb;
    color: #374151;
    font-weight: 600;
  }
}

.user-cell {
  display: flex;
  align-items: center;
  gap: 10px;
}

.user-avatar {
  background: linear-gradient(135deg, #667eea, #764ba2);
  color: #fff;
  font-weight: 600;
}

.user-info {
  display: flex;
  flex-direction: column;
}

.username {
  font-weight: 500;
  color: #1f2937;
}

.user-id {
  font-size: 12px;
  color: #9ca3af;
}

.action-buttons {
  display: flex;
  gap: 8px;
  justify-content: center;
}

.pagination {
  display: flex;
  justify-content: flex-end;
  margin-top: 20px;
}

.user-dialog {
  :deep(.el-dialog__header) {
    background: linear-gradient(135deg, #667eea, #764ba2);
    color: #fff;
    margin: 0;
    padding: 20px 24px;
    
    .el-dialog__title {
      color: #fff;
    }
    
    .el-dialog__headerbtn {
      .el-icon {
        color: #fff;
      }
    }
  }
}
</style>
