document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector('.formdangnhap .fm-form')
    if (!form) return

    const current = JSON.parse(localStorage.getItem('currentUser') || 'null')
    if (current && current.email) {
        const emailInput = form.querySelector('input[name="mail"]')
        if (emailInput) emailInput.value = current.email
    }

    form.addEventListener('submit', (e) => {
        e.preventDefault()

        const email = (form.querySelector('input[name="mail"]').value || '').trim().toLowerCase()
        const password = form.querySelector('input[name="pass"]').value || ''

        if (!email || !password) {
            alert('Vui lòng nhập email và mật khẩu.')
            return
        }

        const users = JSON.parse(localStorage.getItem('users') || '[]')
        const user = users.find(u => u.email === email && u.password === password)

        if (!user) {
            alert('Email hoặc mật khẩu không đúng.')
            return
        }

        localStorage.setItem('currentUser', JSON.stringify({ email: user.email }))
        alert('Đăng nhập thành công.')
        window.location.href = 'TaiKhoan'
    })
})