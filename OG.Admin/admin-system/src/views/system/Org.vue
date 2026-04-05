<template>
  <div class="org-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>组织管理</span>
          <el-button type="primary" @click="handleAdd(null)">
            <el-icon><Plus /></el-icon>新增组织
          </el-button>
        </div>
      </template>

      <el-tree
        ref="treeRef"
        :data="treeData"
        node-key="id"
        :props="{ children: 'children', label: 'orgName' }"
        default-expand-all
        highlight-current
      >
        <template #default="{ node, data }">
          <span class="tree-node">
            <span>{{ node.label }}</span>
            <span class="node-actions">
              <el-button link type="primary" size="small" @click="handleAdd(data)">新增</el-button>
              <el-button link type="primary" size="small" @click="handleEdit(data)">编辑</el-button>
              <el-button link type="danger" size="small" @click="handleDelete(data)">删除</el-button>
            </span>
          </span>
        </template>
      </el-tree>
    </el-card>

    <el-dialog
      v-model="dialogVisible"
      :title="isEdit ? '编辑组织' : '新增组织'"
      width="500px"
    >
      <el-form ref="formRef" :model="form" :rules="rules" label-width="80px">
        <el-form-item label="上级组织" v-if="!isEdit">
          <el-tree-select
            v-model="form.parentId"
            :data="treeData"
            :props="{ label: 'orgName', value: 'id', children: 'children' }"
            check-strictly
            placeholder="请选择上级组织"
            clearable
          />
        </el-form-item>
        <el-form-item label="组织名称" prop="orgName">
          <el-input v-model="form.orgName" placeholder="请输入组织名称" />
        </el-form-item>
        <el-form-item label="组织编码" prop="orgCode">
          <el-input v-model="form.orgCode" placeholder="请输入组织编码" />
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
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { FormInstance, FormRules } from 'element-plus'
import type { ElTree } from 'element-plus'
import { orgApi } from '@/api/org'
import type { OrgDto, CreateOrgRequest } from '@/types'

const treeRef = ref<InstanceType<typeof ElTree>>()
const treeData = ref<OrgDto[]>([])
const dialogVisible = ref(false)
const isEdit = ref(false)
const submitLoading = ref(false)
const formRef = ref<FormInstance>()
const currentId = ref<number>()

const form = reactive<CreateOrgRequest>({
  orgName: '',
  orgCode: '',
  parentId: undefined,
  sort: 0,
  status: 1,
  remark: '',
})

const rules: FormRules = {
  orgName: [
    { required: true, message: '请输入组织名称', trigger: 'blur' },
  ],
}

const loadData = async () => {
  const res = await orgApi.getTree()
  treeData.value = res
}

const handleAdd = (data: OrgDto | null) => {
  isEdit.value = false
  Object.assign(form, {
    orgName: '',
    orgCode: '',
    parentId: data?.id,
    sort: 0,
    status: 1,
    remark: '',
  })
  dialogVisible.value = true
}

const handleEdit = async (data: OrgDto) => {
  isEdit.value = true
  currentId.value = data.id
  const res = await orgApi.getById(data.id)
  Object.assign(form, {
    orgName: res.orgName,
    orgCode: res.orgCode,
    parentId: res.parentId,
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
        await orgApi.update(currentId.value, form)
        ElMessage.success('更新成功')
      } else {
        await orgApi.create(form)
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

const handleDelete = async (data: OrgDto) => {
  await ElMessageBox.confirm('确定要删除该组织吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  })
  await orgApi.delete(data.id)
  ElMessage.success('删除成功')
  loadData()
}

onMounted(() => {
  loadData()
})
</script>

<style scoped>
.org-container {
  padding: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.tree-node {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.node-actions {
  margin-left: 20px;
}
</style>
