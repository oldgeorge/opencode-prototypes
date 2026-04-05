export const validate = {
  isPhone(phone: string): boolean {
    return /^1[3-9]\d{9}$/.test(phone)
  },

  isEmail(email: string): boolean {
    return /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/.test(email)
  },

  isUsername(username: string): boolean {
    return /^[a-zA-Z0-9_]{4,20}$/.test(username)
  },

  isPassword(password: string): boolean {
    return password.length >= 6
  },
}
