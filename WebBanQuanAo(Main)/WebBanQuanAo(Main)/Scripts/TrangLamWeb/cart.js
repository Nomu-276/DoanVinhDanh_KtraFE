

(function () {
    'use strict';

    if (window.__cart_initialized) {
        console.warn('cart.js: already initialized, skipping duplicate init.');
        return;
    }
    window.__cart_initialized = true;

    var CART_KEY = 'cart';

    function getCart() {
        try {
            var raw = localStorage.getItem(CART_KEY);
            return raw ? JSON.parse(raw) : [];
        } catch (e) {
            console.error('Lỗi đọc cart từ localStorage', e);
            return [];
        }
    }

    function saveCart(cart) {
        try {
            localStorage.setItem(CART_KEY, JSON.stringify(cart));
            document.dispatchEvent(new CustomEvent('cartUpdated', { detail: { cart: cart } }));
        } catch (e) {
            console.error('Lỗi lưu cart vào localStorage', e);
        }
    }

    function findItem(cart, id) {
        return cart.find(function (it) { return it.id === id; });
    }

    function addToCart(product, qty) {
        qty = Math.max(1, parseInt(qty, 10) || 1);
        var cart = getCart();
        var existing = findItem(cart, product.id);
        if (existing) {
            existing.quantity = (existing.quantity || 0) + qty;
        } else {
            product.quantity = qty;
            cart.push(product);
        }
        saveCart(cart);
    }

    function setQuantity(id, qty) {
        qty = parseInt(qty, 10) || 0;
        var cart = getCart();
        var item = findItem(cart, id);
        if (!item) return;
        if (qty <= 0) {
            cart = cart.filter(function (it) { return it.id !== id; });
        } else {
            item.quantity = qty;
        }
        saveCart(cart);
    }

    function removeFromCart(id) {
        var cart = getCart();
        cart = cart.filter(function (it) { return it.id !== id; });
        saveCart(cart);
    }

    function formatVND(n) {
        try {
            return '₫' + n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        } catch (e) {
            return '₫0';
        }
    }

    function calculateTotals(cart) {
        var subtotal = cart.reduce(function (acc, it) {
            var p = parseInt(it.price, 10) || 0;
            var q = parseInt(it.quantity, 10) || 0;
            return acc + p * q;
        }, 0);
        return { subtotal: subtotal, shipping: 0, total: subtotal };
    }

    function normalizeImagePath(p) {
        return p ? String(p).replace(/^[~]\//, '/') : '';
    }

    function updateHeaderBadge(cart) {
        var countEl = document.getElementById('cart-count');
        if (!countEl) return;
        var totalQty = (cart || []).reduce(function (acc, it) { return acc + (parseInt(it.quantity, 10) || 0); }, 0);
        countEl.textContent = totalQty;
        countEl.style.display = totalQty > 0 ? 'inline-block' : 'none';
    }

    function renderHeaderPreview(cart) {
        var preview = document.getElementById('cart-preview');
        if (!preview) return;
        preview.innerHTML = '';
        if (!cart || cart.length === 0) {
            preview.innerHTML = '<div class="empty">Giỏ hàng trống</div>';
            return;
        }
        cart.slice(0, 5).forEach(function (item) {
            var div = document.createElement('div');
            div.className = 'item';
            var img = document.createElement('img');
            img.src = normalizeImagePath(item.image || '');
            img.alt = item.name || '';
            div.appendChild(img);
            var info = document.createElement('div');
            info.className = 'info';
            var name = document.createElement('div');
            name.className = 'name';
            name.textContent = item.name || '';
            var meta = document.createElement('div');
            meta.className = 'meta';
            meta.textContent = (item.quantity || 0) + ' × ' + formatVND(parseInt(item.price || 0, 10));
            info.appendChild(name);
            info.appendChild(meta);
            div.appendChild(info);
            preview.appendChild(div);
        });
        var more = document.createElement('div');
        more.style.marginTop = '8px';
        more.style.textAlign = 'right';
        more.innerHTML = '<a href="GioHang">Xem giỏ hàng</a>';
        preview.appendChild(more);
    }

    function renderCartPage() {
        var cart = getCart();

        var cartBody = document.getElementById('cart-body');
        var cartTable = document.getElementById('cart-table');
        var cartEmpty = document.getElementById('cart-empty');
        var cartSummary = document.getElementById('cart-summary');
        var subTotalEl = document.getElementById('sub-total');
        var grandTotalEl = document.getElementById('grand-total');

        var oldContainer = document.getElementById('cart-items');

        if (cartBody && cartTable && cartEmpty && cartSummary && subTotalEl && grandTotalEl) {
            if (!cart || cart.length === 0) {
                cartBody.innerHTML = '';
                cartTable.style.display = 'none';
                cartSummary.style.display = 'none';
                cartEmpty.style.display = 'block';
                updateHeaderTotals(0);
                return;
            }
            cartBody.innerHTML = '';
            cartTable.style.display = '';
            cartSummary.style.display = '';
            cartEmpty.style.display = 'none';
            cart.forEach(function (item) {
                var tr = document.createElement('tr');

                var tdP = document.createElement('td');
                tdP.style.display = 'flex';
                tdP.style.alignItems = 'center';
                var img = document.createElement('img');
                img.src = normalizeImagePath(item.image || '');
                img.style.width = '70px';
                img.style.height = '70px';
                img.style.objectFit = 'cover';
                img.style.marginRight = '12px';
                var nm = document.createElement('div');
                nm.innerHTML = '<div style="font-weight:600;">' + (item.name || '') + '</div>';
                tdP.appendChild(img);
                tdP.appendChild(nm);
                tr.appendChild(tdP);

                var tdPrice = document.createElement('td');
                tdPrice.textContent = formatVND(parseInt(item.price || 0, 10));
                tr.appendChild(tdPrice);

                var tdQty = document.createElement('td');
                tdQty.innerHTML = '<button class="qty-minus" data-id="' + item.id + '">-</button>' +
                    '<input class="qty-input" data-id="' + item.id + '" type="number" min="1" value="' + (item.quantity || 1) + '" style="width:60px;margin:0 6px;">' +
                    '<button class="qty-plus" data-id="' + item.id + '">+</button>';
                tr.appendChild(tdQty);

                var tdLine = document.createElement('td');
                tdLine.textContent = formatVND((parseInt(item.price || 0, 10) * parseInt(item.quantity || 0, 10)) || 0);
                tr.appendChild(tdLine);

                var tdAct = document.createElement('td');
                tdAct.innerHTML = '<button class="xoa-sp" data-id="' + item.id + '">Xóa</button>';
                tr.appendChild(tdAct);
                cartBody.appendChild(tr);
            });
            var totals = calculateTotals(cart);
            subTotalEl.textContent = formatVND(totals.subtotal);
            grandTotalEl.textContent = formatVND(totals.total);
            updateHeaderTotals(totals.subtotal);
            return;
        }
 
        if (oldContainer) {
            oldContainer.innerHTML = '';
            if (!cart || cart.length === 0) {
                oldContainer.innerHTML = '<h2 class="">Giỏ hàng của bạn đang trống.</h2><p class="">Đăng nhập để lưu hoặc truy cập các sản phẩm đã lưu trong giỏ hàng.</p>';
                updateHeaderTotals(0);
                return;
            }
            var table = document.createElement('table');
            table.className = 'cart-table';
            var thead = document.createElement('thead');
            thead.innerHTML = '<tr><th>Sản phẩm</th><th>Đơn giá</th><th>Số lượng</th><th>Thành tiền</th><th></th></tr>';
            table.appendChild(thead);
            var tbody = document.createElement('tbody');
            cart.forEach(function (item) {
                var tr = document.createElement('tr');
                tr.innerHTML = '<td><img src="' + normalizeImagePath(item.image || '') + '" style="width:80px;margin-right:10px;">' +
                    '<div>' + (item.name || '') + '</div></td>' +
                    '<td>' + formatVND(parseInt(item.price || 0, 10)) + '</td>' +
                    '<td><button class="qty-minus" data-id="' + item.id + '">-</button>' +
                    '<input class="qty-input" data-id="' + item.id + '" type="number" min="1" value="' + (item.quantity || 1) + '" style="width:60px;">' +
                    '<button class="qty-plus" data-id="' + item.id + '">+</button></td>' +
                    '<td>' + formatVND((parseInt(item.price || 0, 10) * parseInt(item.quantity || 0, 10)) || 0) + '</td>' +
                    '<td><button class="xoa-sp" data-id="' + item.id + '">Xóa</button></td>';
                tbody.appendChild(tr);
            });
            table.appendChild(tbody);
            oldContainer.appendChild(table);
            var totals = calculateTotals(cart);
            var subtotalEls = document.querySelectorAll('.cart-subtotal');
            subtotalEls.forEach(function (el) { el.textContent = formatVND(totals.subtotal); });
            var totalEls = document.querySelectorAll('.cart-total');
            totalEls.forEach(function (el) { el.textContent = formatVND(totals.total); });
            updateHeaderTotals(totals.subtotal);
        }
    }

    function updateHeaderTotals(subtotal) {
        var countEl = document.getElementById('cart-count');
        if (countEl) {
            var cart = getCart();
            var totalQty = (cart || []).reduce(function (acc, it) { return acc + (parseInt(it.quantity, 10) || 0); }, 0);
            countEl.textContent = totalQty;
            countEl.style.display = totalQty > 0 ? 'inline-block' : 'none';
        }
    }

    function onDocumentClick(e) {
        var t = e.target;

        var btn = t.closest && t.closest('button, .themgiohang') ? t.closest('button, .themgiohang') : t;

        if (btn && btn.matches('.qty-minus')) {
            var id = btn.getAttribute('data-id');
            var cart = getCart();
            var it = findItem(cart, id);
            if (!it) return;
            var newQty = Math.max(1, (parseInt(it.quantity, 10) || 1) - 1);
            setQuantity(id, newQty);
            renderCartPage();
            return;
        }

        if (btn && btn.matches('.qty-plus')) {
            var id2 = btn.getAttribute('data-id');
            var cart2 = getCart();
            var it2 = findItem(cart2, id2);
            if (!it2) return;
            var newQty2 = (parseInt(it2.quantity, 10) || 0) + 1;
            setQuantity(id2, newQty2);
            renderCartPage();
            return;
        }

        if (btn && btn.matches('.xoa-sp')) {
            var idRemove = btn.getAttribute('data-id');
            if (!idRemove) return;
            if (confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?')) {
                removeFromCart(idRemove);
                renderCartPage();
            }
            return;
        }

        if (btn && (btn.matches('.themgiohang') || (btn.closest && btn.closest('.themgiohang')))) {
            var addBtn = btn.matches('.themgiohang') ? btn : btn.closest('.themgiohang');
            if (!addBtn) return;

            if (addBtn.getAttribute('data-processing') === '1') return;
            addBtn.setAttribute('data-processing', '1');
            setTimeout(function () { addBtn.removeAttribute('data-processing'); }, 300);

            var id = addBtn.getAttribute('data-id') || '';
            var name = addBtn.getAttribute('data-name') || '';
            var price = addBtn.getAttribute('data-price') || '0';
            var image = addBtn.getAttribute('data-image') || '';

            if (!id) {
                alert('Sản phẩm không hợp lệ (thiếu id).');
                return;
            }

            var qty = 1;

            addToCart({ id: id, name: name, price: price, image: image }, qty);

            try {
                var original = addBtn.innerHTML;
                addBtn.innerHTML = 'Đã thêm';
                setTimeout(function () { addBtn.innerHTML = original; }, 900);
            } catch (e) { }

            return;
        }
    }

    function onDocumentInput(e) {
        var t = e.target;
        if (t && t.matches('.qty-input')) {
            var id = t.getAttribute('data-id');
            var v = parseInt(t.value, 10) || 1;
            if (v < 1) v = 1;
            setQuantity(id, v);
            renderCartPage();
        }
    }

    function init() {

        var cart = getCart();
        updateHeaderBadge(cart);
        renderHeaderPreview(cart);

        renderCartPage();

        document.removeEventListener('click', onDocumentClick); 
        document.addEventListener('click', onDocumentClick);

        document.removeEventListener('input', onDocumentInput);
        document.addEventListener('input', onDocumentInput);

        document.removeEventListener('cartUpdated', onCartUpdated);
        document.addEventListener('cartUpdated', onCartUpdated);
    }

    function onCartUpdated(e) {
        var cart = (e && e.detail && e.detail.cart) ? e.detail.cart : getCart();
        updateHeaderBadge(cart);
        renderHeaderPreview(cart);
        renderCartPage();
    }

    window.cart = window.cart || {};
    window.cart.get = getCart;
    window.cart.add = addToCart;
    window.cart.remove = removeFromCart;
    window.cart.setQuantity = setQuantity;

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();