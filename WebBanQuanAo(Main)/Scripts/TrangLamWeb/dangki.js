document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector('.formdangki .fm-form')
    if (!form) return

    form.addEventListener('submit', (e) => {
        e.preventDefault()

        const emailEl = form.querySelector('input[name="mail"]')
        const passEl = form.querySelector('input[name="pass"]')
        const dobEl = form.querySelector('input[name="ngaysinh"]')

        const email = (emailEl && emailEl.value || '').trim().toLowerCase()
        const password = (passEl && passEl.value || '')
        const dob = (dobEl && dobEl.value) || ''

        if (!email || !/^\S+@\S+\.\S+$/.test(email)) {
            alert('Vui lòng nhập email hợp lệ.')
            return
        }
        if (!password || password.length < 6) {
            alert('Mật khẩu phải có ít nhất 6 ký tự.')
            return
        }

        const users = JSON.parse(localStorage.getItem('users') || '[]')

        if (users.find(u => u.email === email)) {
            alert('Email này đã được đăng ký.')
            return
        }
        const newUser = {
            email,
            password,
            dob,
            createdAt: new Date().toISOString()
        }

        users.push(newUser)
        localStorage.setItem('users', JSON.stringify(users))
        localStorage.setItem('currentUser', JSON.stringify({ email }))

        alert('Đăng ký thành công.')
        window.location.href = 'TaiKhoan'
    })
})