(function () {
    function getQueryParam(name) {
        const params = new URLSearchParams(window.location.search)
        return params.get(name) || ''
    }

    function filterProducts(q) {
        q = q.trim().toLowerCase()
        const products = document.querySelectorAll('#trangsanpham .product li')
        products.forEach(li => {
            const nameEl = li.querySelector('.product-name')
            const text = nameEl ? nameEl.textContent.trim().toLowerCase() : ''
            if (q === '' || text.includes(q)) {
                li.style.display = ''
            } else {
                li.style.display = 'none'
            }
        })
    }

    document.addEventListener('DOMContentLoaded', () => {
        const q = getQueryParam('q')
       
        if (q) {
            const input = document.querySelector('.search .input')
            const searchDiv = document.querySelector('.search')
            if (input) input.value = q
            if (searchDiv) searchDiv.classList.add('active')
            filterProducts(q)
        }

        const headerInput = document.querySelector('.search .input')
        if (headerInput) {
            headerInput.addEventListener('input', (e) => {
                filterProducts(e.target.value)
            })
        }
    })
})()