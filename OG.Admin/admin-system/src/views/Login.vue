<template>
  <div class="login-container">
    <div class="animated-bg">
      <div class="circle circle-1"></div>
      <div class="circle circle-2"></div>
      <div class="circle circle-3"></div>
      <div class="circle circle-4"></div>
      <div class="circle circle-5"></div>
    </div>

    <div class="login-wrapper">
      <div class="login-card">
        <div class="login-left">
          <div class="brand-section">
            <div class="brand-logo">
              <el-icon :size="40" color="#fff"><Monitor /></el-icon>
            </div>
            <h1 class="brand-title">OG.Admin</h1>
            <p class="brand-subtitle">Enterprise Management System</p>
            <div class="brand-features">
              <div class="feature-item">
                <el-icon color="#a3bffa"><Check /></el-icon>
                <span>现代化的后台管理系统</span>
              </div>
              <div class="feature-item">
                <el-icon color="#a3bffa"><Check /></el-icon>
                <span>安全可靠的权限控制</span>
              </div>
              <div class="feature-item">
                <el-icon color="#a3bffa"><Check /></el-icon>
                <span>灵活高效的组织架构</span>
              </div>
            </div>
          </div>
        </div>

        <div class="login-right">
          <div class="login-form-container">
            <div class="form-header">
              <h2 class="form-title">欢迎登录</h2>
              <p class="form-subtitle">请输入您的账号信息</p>
            </div>

            <el-form
              ref="formRef"
              :model="form"
              :rules="rules"
              class="login-form"
              @submit.prevent="handleLogin"
            >
              <el-form-item prop="username">
                <el-input
                  v-model="form.username"
                  placeholder="请输入用户名"
                  size="large"
                  :prefix-icon="User"
                  clearable
                  class="custom-input"
                />
              </el-form-item>

              <el-form-item prop="password">
                <el-input
                  v-model="form.password"
                  type="password"
                  placeholder="请输入密码"
                  size="large"
                  :prefix-icon="Lock"
                  show-password
                  clearable
                  class="custom-input"
                  @keyup.enter="handleLogin"
                />
              </el-form-item>

              <el-form-item>
                <div class="form-options">
                  <el-checkbox v-model="form.rememberMe">记住密码</el-checkbox>
                </div>
              </el-form-item>

              <el-form-item>
                <el-button
                  type="primary"
                  size="large"
                  :loading="loading"
                  class="login-btn"
                  @click="handleLogin"
                >
                  {{ loading ? '登录中...' : '登 录' }}
                </el-button>
              </el-form-item>
            </el-form>

            <div class="login-footer">
              <span class="tips">默认账号: admin / 123456</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { User, Lock, Monitor, Check } from '@element-plus/icons-vue'
import type { FormInstance, FormRules } from 'element-plus'
import { useUserStore } from '@/stores/user'

const router = useRouter()
const userStore = useUserStore()

const formRef = ref<FormInstance>()
const loading = ref(false)

const form = reactive({
  username: '',
  password: '',
  rememberMe: false,
})

const rules: FormRules = {
  username: [
    { required: true, message: '请输入用户名', trigger: 'blur' },
  ],
  password: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    { min: 6, message: '密码长度不能少于6位', trigger: 'blur' },
  ],
}

const handleLogin = async () => {
  if (!formRef.value) return

  await formRef.value.validate(async (valid) => {
    if (!valid) return

    loading.value = true
    try {
      await userStore.login(form.username, form.password, form.rememberMe)
      ElMessage.success('登录成功')
      router.push('/')
    } catch (error: any) {
      ElMessage.error(error.message || '登录失败')
    } finally {
      loading.value = false
    }
  })
}
</script>

<style scoped>
.login-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 50%, #0f3460 100%);
  position: relative;
  overflow: hidden;
}

.animated-bg {
  position: absolute;
  inset: 0;
  overflow: hidden;
}

.circle {
  position: absolute;
  border-radius: 50%;
  background: rgba(102, 126, 234, 0.1);
  animation: float 20s infinite ease-in-out;
}

.circle-1 {
  width: 400px;
  height: 400px;
  top: -100px;
  left: -100px;
  animation-delay: 0s;
  background: radial-gradient(circle, rgba(102, 126, 234, 0.2), transparent);
}

.circle-2 {
  width: 300px;
  height: 300px;
  bottom: -50px;
  right: -50px;
  animation-delay: -5s;
  background: radial-gradient(circle, rgba(118, 75, 162, 0.2), transparent);
}

.circle-3 {
  width: 200px;
  height: 200px;
  top: 50%;
  left: 60%;
  animation-delay: -10s;
  background: radial-gradient(circle, rgba(163, 191, 255, 0.15), transparent);
}

.circle-4 {
  width: 150px;
  height: 150px;
  top: 20%;
  right: 20%;
  animation-delay: -15s;
  background: radial-gradient(circle, rgba(102, 126, 234, 0.15), transparent);
}

.circle-5 {
  width: 250px;
  height: 250px;
  bottom: 20%;
  left: 10%;
  animation-delay: -7s;
  background: radial-gradient(circle, rgba(118, 75, 162, 0.12), transparent);
}

@keyframes float {
  0%, 100% { transform: translate(0, 0) scale(1); }
  25% { transform: translate(30px, -30px) scale(1.1); }
  50% { transform: translate(-20px, 20px) scale(0.95); }
  75% { transform: translate(20px, 10px) scale(1.05); }
}

.login-wrapper {
  position: relative;
  z-index: 1;
  width: 100%;
  max-width: 900px;
  padding: 20px;
  animation: slideUp 0.8s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(40px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.login-card {
  display: flex;
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border-radius: 20px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  overflow: hidden;
  min-height: 520px;
}

.login-left {
  flex: 1;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 50px 40px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  position: relative;
  overflow: hidden;
}

.login-left::before {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: radial-gradient(circle, rgba(255, 255, 255, 0.1) 0%, transparent 70%);
  animation: rotate 30s linear infinite;
}

@keyframes rotate {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

.brand-section {
  position: relative;
  z-index: 1;
}

.brand-logo {
  width: 70px;
  height: 70px;
  background: rgba(255, 255, 255, 0.15);
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 24px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.brand-title {
  font-size: 32px;
  font-weight: 700;
  color: #fff;
  margin: 0 0 8px;
  letter-spacing: 1px;
}

.brand-subtitle {
  font-size: 14px;
  color: rgba(255, 255, 255, 0.7);
  margin: 0 0 40px;
  letter-spacing: 2px;
}

.brand-features {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.feature-item {
  display: flex;
  align-items: center;
  gap: 12px;
  color: rgba(255, 255, 255, 0.9);
  font-size: 15px;
}

.login-right {
  flex: 1;
  padding: 50px 40px;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.form-header {
  margin-bottom: 32px;
}

.form-title {
  font-size: 28px;
  font-weight: 700;
  color: #1a1a2e;
  margin: 0 0 8px;
}

.form-subtitle {
  font-size: 14px;
  color: #8b8fa3;
  margin: 0;
}

.login-form {
  margin-top: 0;
}

:deep(.custom-input .el-input__wrapper) {
  padding: 12px 16px;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
  transition: all 0.3s ease;
}

:deep(.custom-input .el-input__wrapper:hover) {
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.15);
}

:deep(.custom-input .el-input__wrapper.is-focus) {
  box-shadow: 0 4px 16px rgba(102, 126, 234, 0.25);
}

.form-options {
  width: 100%;
  display: flex;
  justify-content: flex-start;
}

.login-btn {
  width: 100%;
  height: 48px;
  border-radius: 12px;
  font-size: 16px;
  font-weight: 600;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  transition: all 0.3s ease;
  letter-spacing: 4px;
}

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.4);
}

.login-btn:active {
  transform: translateY(0);
}

.login-footer {
  margin-top: 24px;
  text-align: center;
}

.tips {
  font-size: 13px;
  color: #8b8fa3;
}

@media (max-width: 768px) {
  .login-card {
    flex-direction: column;
  }

  .login-left {
    padding: 30px 24px;
    min-height: auto;
  }

  .brand-features {
    display: none;
  }

  .login-right {
    padding: 30px 24px;
  }

  .brand-title {
    font-size: 24px;
  }

  .form-title {
    font-size: 22px;
  }
}
</style>
