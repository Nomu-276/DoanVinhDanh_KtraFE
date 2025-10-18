function updateUserLink() {
    let userLink = document.getElementById('user-link')
    if (!userLink) {
        const icon = document.querySelector('.fa-user') || document.querySelector('.fa-regular.fa-user') || document.querySelector('.fa-solid.fa-user')
        if (icon) userLink = icon.closest('a')
    }
    if (!userLink) return

    const raw = localStorage.getItem('currentUser')
    if (!raw) {
        userLink.href = 'DangNhap'
        userLink.title = 'Đăng nhập'
        return
    }

    const current = JSON.parse(raw)
    if (current && current.email) {
        userLink.href = 'TaiKhoan'
        userLink.title = `Tài khoản (${current.email})`
    } else {
        userLink.href = 'DangNhap'
        userLink.title = 'Đăng nhập'
    }
}

document.addEventListener('DOMContentLoaded', () => {
    updateUserLink()

    document.addEventListener('auth-changed', updateUserLink)

    window.addEventListener('storage', (e) => {
        if (e.key === 'currentUser') updateUserLink()
    })
})