
const search = document.querySelector('.search')
const btn = document.querySelector('.btn')
const input = document.querySelector('.input')

if (btn && search && input) {
    btn.addEventListener('click', (e) => {
        
        if (!search.classList.contains('active')) {
            e.preventDefault()
            search.classList.add('active')
            input.focus()
            return
        }
        
    })

    
    input.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            search.classList.remove('active')
            input.blur()
        }
    })
}