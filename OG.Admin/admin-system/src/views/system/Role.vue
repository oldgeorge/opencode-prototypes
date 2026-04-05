<template>
  <div class="role-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>角色管理</span>
          <el-button type="primary" @click="handleAdd">
            <el-icon><Plus /></el-icon>新增角色
          </el-button>
        </div>
      </template>

      <el-table :data="tableData" v-loading="loading" stripe>
        <el-table-column prop="id" label="ID" width="80" />
        <el-table-column prop="roleName" label="角色名称" min-width="150" />
        <el-table-column prop="roleCode" label="角色编码" min-width="150" />
        <el-table-column prop="sort" label="排序" width="100" align="center" />
        <el-table-column prop="status" label="状态" width="100" align="center">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'danger'">
              {{ row.status === 1 ? '启用' : '禁用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="创建时间" min-width="180" />
        <el-table-column label="操作" width="280" fixed="right">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleEdit(row)">编辑</el-button>
            <el-button link type="warning" @click="handleAssignPermission(row)">分配权限</el-button>
            <el-button link type="danger" @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="pagination">
        <el-pagination
          v-model:current-page="queryForm.pageNum"
          v-model:page-size="queryForm.pageSize"
          :total="total"
          :page-sizes="[10, 20, 50]"
          layout="total, sizes, prev, pager, next"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>
    </el-card>

    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑角色' : '新增角色'"
      width="500px"
    >
      <el-form ref="formRef" :model="form" :rules="rules" label-width="80px">
        <el-form-item label="角色名称" prop="roleName">
          <el-input v-model="form.roleName" placeholder="请输入角色名称" />
        </el-form-item>
        <el-form-item label="角色编码" prop="roleCode">
          <el-input v-model="form.roleCode" placeholder="请输入角色编码" />
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number v-model="form.sort" :min="0" />
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-radio-group v-model="form.status">
            <el-radio :label="1">启用</el-radio>
            <el-radio :label="0">禁用</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="备注" prop="remark">
          <el-input v-model="form.remark" type="textarea" :rows="3" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSubmit" :loading="submitLoading">确定</el-button>
      </template>
    </el-dialog>

    <el-dialog
      v-model="permissionDialogVisible"
      title="分配权限"
      width="500px"
    >
      <div class="permission-tree">
        <el-tree
          ref="permissionTreeRef"
          :data="menuTree"
          node-key="id"
          :props="{ label: 'menuName', children: 'children' }"
          show-checkbox
          default-expand-all
          :default-checked-keys="checkedKeys"
        />
      </div>
      <template #footer>
        <el-button @click="permissionDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSavePermission" :loading="permissionLoading">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { FormInstance, FormRules } from 'element-plus'
import type { ElTree } from 'element-plus'
import { roleApi } from '@/api/role'
import { menuApi } from '@/api/menu'
import type { RoleDto, CreateRoleRequest, MenuDto } from '@/types'

const loading = ref(false)
const tableData = ref<RoleDto[]>([])
const total = ref(0)
const menuTree = ref<MenuDto[]>([])

const queryForm = reactive({
  pageNum: 1,
  pageSize: 10,
})

const dialogVisible = ref(false)
const isEdit = ref(false)
const submitLoading = ref(false)
const formRef = ref<FormInstance>()
const currentId = ref<number>()

const form = reactive<CreateRoleRequest>({
  roleName: '',
  roleCode: '',
  sort: 0,
  status: 1,
  remark: '',
})

const rules: FormRules = {
  roleName: [
    { required: true, message: '请输入角色名称', trigger: 'blur' },
  ],
  roleCode: [
    { required: true, message: '请输入角色编码', trigger: 'blur' },
  ],
}

const permissionDialogVisible = ref(false)
const permissionLoading = ref(false)
const permissionTreeRef = ref<InstanceType<typeof ElTree>>()
const checkedKeys = ref<number[]>([])

const loadData = async () => {
  loading.value = true
  try {
    const res = await roleApi.getPageList(queryForm)
    tableData.value = res.items
    total.value = res.total
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const loadMenuTree = async () => {
  const res = await menuApi.getTree()
  menuTree.value = res
}

const handleAdd = () => {
  isEdit.value = false
  Object.assign(form, {
    roleName: '',
    roleCode: '',
    sort: 0,
    status: 1,
    remark: '',
  })
  dialogVisible.value = true
}

const handleEdit = async (row: RoleDto) => {
  isEdit.value = true
  currentId.value = row.id
  const res = await roleApi.getById(row.id)
  Object.assign(form, {
    roleName: res.roleName,
    roleCode: res.roleCode,
    sort: res.sort,
    status: res.status,
    remark: res.remark,
  })
  dialogVisible.value = true
}

const handleSubmit = async () => {
  if (!formRef.value) return
  await formRef.value.validate(async (valid) => {
    if (!valid) return
    submitLoading.value = true
    try {
      if (isEdit.value && currentId.value) {
        await roleApi.update(currentId.value, form)
        ElMessage.success('更新成功')
      } else {
        await roleApi.create(form)
        ElMessage.success('创建成功')
      }
      dialogVisible.value = false
      loadData()
    } catch (error: any) {
      ElMessage.error(error.message || '操作失败')
    } finally {
      submitLoading.value = false
    }
  })
}

const handleDelete = async (row: RoleDto) => {
  await ElMessageBox.confirm('确定要删除该角色吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  })
  await roleApi.delete(row.id)
  ElMessage.success('删除成功')
  loadData()
}

const handleAssignPermission = async (row: RoleDto) => {
  currentId.value = row.id
  const res = await roleApi.getRoleMenus(row.id)
  checkedKeys.value = res.map(m => m.id)
  permissionDialogVisible.value = true
}

const handleSavePermission = async () => {
  if (!permissionTreeRef.value || !currentId.value) return
  const checkedNodes = permissionTreeRef.value.getCheckedNodes()
  const menuIds = checkedNodes.map(n => n.id)
  const halfCheckedKeys = permissionTreeRef.value.getHalfCheckedKeys()
  menuIds.push(...(halfCheckedKeys as number[]))
  
  permissionLoading.value = true
  try {
    await roleApi.assignPermissions(currentId.value, menuIds)
    ElMessage.success('权限分配成功')
    permissionDialogVisible.value = false
  } catch (error: any) {
    ElMessage.error(error.message || '操作失败')
  } finally {
    permissionLoading.value = false
  }
}

const handleSizeChange = () => {
  queryForm.pageNum = 1
  loadData()
}

const handleCurrentChange = () => {
  loadData()
}

onMounted(() => {
  loadData()
  loadMenuTree()
})
</script>

<style scoped>
.role-container {
  padding: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.pagination {
  margin-top: 20px;
  display: flex;
  justify-content: flex-end;
}

.permission-tree {
  max-height: 400px;
  overflow-y: auto;
}
</style>
