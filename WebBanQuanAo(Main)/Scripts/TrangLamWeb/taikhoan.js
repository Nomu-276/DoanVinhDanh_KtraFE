document.addEventListener('DOMContentLoaded', () => {
    const raw = localStorage.getItem('currentUser')
    if (!raw) {
        window.location.href = 'DangNhap'
        return
    }

    const current = JSON.parse(raw)
    const users = JSON.parse(localStorage.getItem('users') || '[]')
    const user = users.find(u => u.email === current.email)

    if (!user) {
        localStorage.removeItem('currentUser')
        window.location.href = 'DangNhap'
        return
    }

    const emailEl = document.getElementById('acc-email')
    const dobEl = document.getElementById('acc-dob')
    const createdEl = document.getElementById('acc-created')

    if (emailEl) emailEl.textContent = user.email || '-'
    if (dobEl) dobEl.textContent = user.dob || '-'
    if (createdEl) createdEl.textContent = user.createdAt ? new Date(user.createdAt).toLocaleString() : '-'

    const logoutBtn = document.getElementById('btn-logout')
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            localStorage.removeItem('currentUser')
            alert('Bạn đã đăng xuất.')
            window.location.href = 'TrangChu'
        })
    }
})