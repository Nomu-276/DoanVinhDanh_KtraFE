(function () {
    'use strict';

    var CART_KEY = 'cart';
    var appliedVoucher = { code: null, discount: 0 };
    var voucherPercent = 0;

    function getCart() {
        try {
            var raw = localStorage.getItem(CART_KEY);
            return raw ? JSON.parse(raw) : [];
        } catch (e) {
            console.error('checkout.js: Lỗi đọc cart từ localStorage', e);
            return [];
        }
    }

    function formatVND(n) {
        n = Number(n) || 0;
        return '₫' + n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }

    function parseNumber(v) {
        if (!v) return 0;
        return parseInt(String(v).replace(/[^0-9\-]/g, ''), 10) || 0;
    }

    function renderCart() {
        var cart = getCart();
        var tbody = document.getElementById('bdCart');
        var tongEl = document.getElementById('Tong');

        if (!tbody || !tongEl) return;

        tbody.innerHTML = '';
        if (!cart || cart.length === 0) {
            var tr = document.createElement('tr');
            var td = document.createElement('td');
            td.colSpan = 4;
            td.style.padding = '20px';
            td.textContent = 'Giỏ hàng trống.';
            tr.appendChild(td);
            tbody.appendChild(tr);
            tongEl.textContent = formatVND(0);
            updateOrderSummary();
            return;
        }

        var subtotal = 0;
        cart.forEach(function (item, idx) {
            var price = parseNumber(item.price);
            var qty = parseInt(item.quantity || 0, 10) || 0;
            var line = price * qty;
            subtotal += line;

            var tr = document.createElement('tr');

            var tdProd = document.createElement('td');
            tdProd.style.display = 'flex';
            tdProd.style.alignItems = 'center';
            tdProd.style.gap = '12px';

            var img = document.createElement('img');
            if (item.image) img.src = String(item.image).replace(/^[~]\//, '/');
            img.alt = item.name || '';
            img.style.width = '64px';
            img.style.height = '64px';
            img.style.objectFit = 'cover';
            img.style.border = '1px solid #eee';
            tdProd.appendChild(img);

            var nameWrap = document.createElement('div');
            var name = document.createElement('div');
            name.textContent = item.name || '';
            name.style.fontWeight = '600';
            var meta = document.createElement('div');
            meta.style.fontSize = '12px';
            meta.style.color = '#666';
            meta.textContent = (item.sku ? 'Mã: ' + item.sku + ' • ' : '') + (item.image ? '' : '');
            nameWrap.appendChild(name);
            nameWrap.appendChild(meta);
            tdProd.appendChild(nameWrap);
            tr.appendChild(tdProd);

            var tdQty = document.createElement('td');
            tdQty.style.textAlign = 'center';
            
            var minus = document.createElement('button');
            minus.textContent = '-';
            minus.className = 'qty-minus';
            minus.setAttribute('data-id', item.id);
            minus.style.marginRight = '6px';
            var input = document.createElement('input');
            input.type = 'number';
            input.min = '1';
            input.value = qty || 1;
            input.className = 'qty-input';
            input.setAttribute('data-id', item.id);
            input.style.width = '56px';
            input.style.textAlign = 'center';
            var plus = document.createElement('button');
            plus.textContent = '+';
            plus.className = 'qty-plus';
            plus.setAttribute('data-id', item.id);
            plus.style.marginLeft = '6px';

            tdQty.appendChild(minus);
            tdQty.appendChild(input);
            tdQty.appendChild(plus);
            tr.appendChild(tdQty);

            
            var tdPrice = document.createElement('td');
            tdPrice.style.whiteSpace = 'nowrap';
            tdPrice.textContent = formatVND(price);
            tr.appendChild(tdPrice);

           
            var tdLine = document.createElement('td');
            tdLine.textContent = formatVND(line);
            tr.appendChild(tdLine);

            tbody.appendChild(tr);
        });

        tongEl.textContent = formatVND(subtotal);
        updateOrderSummary();
    }


    function updateOrderSummary() {
        var cart = getCart();
        var subtotal = cart.reduce(function (acc, it) {
            return acc + (parseNumber(it.price) * (parseInt(it.quantity || 0, 10) || 0));
        }, 0);

        var orderValue = document.getElementById('orderValue');
        var deliveryFeeEl = document.getElementById('deliveryFee');
        var voucherDiscountEl = document.getElementById('voucherDiscount');
        var totalAmountEl = document.getElementById('totalAmount');

        if (orderValue) orderValue.textContent = formatVND(subtotal);
        var deliveryFee = 0; 
        if (deliveryFeeEl) deliveryFeeEl.textContent = formatVND(deliveryFee);

        var voucherDiscount = 0;
        if (voucherPercent && voucherPercent > 0) {
            voucherDiscount = Math.round(subtotal * (voucherPercent / 100));
        } else if (appliedVoucher && appliedVoucher.discount) {
            voucherDiscount = appliedVoucher.discount;
        }

        if (voucherDiscountEl) voucherDiscountEl.textContent = formatVND(voucherDiscount);

        var total = subtotal - voucherDiscount + deliveryFee;
        if (total < 0) total = 0;
        if (totalAmountEl) totalAmountEl.textContent = formatVND(total);
    }

    function setQuantity(id, qty) {
        var cart = getCart();
        var changed = false;
        cart = cart.map(function (it) {
            if (it.id === id) {
                it.quantity = Math.max(1, parseInt(qty, 10) || 1);
                changed = true;
            }
            return it;
        });
        localStorage.setItem(CART_KEY, JSON.stringify(cart));
        renderCart();
    }

    function onDocumentClick(e) {
        var t = e.target;
        if (t.matches('.qty-minus')) {
            var id = t.getAttribute('data-id');
            var cart = getCart();
            var item = cart.find(function (x) { return x.id === id; });
            if (!item) return;
            var newQty = Math.max(1, (parseInt(item.quantity || 0, 10) - 1));
            setQuantity(id, newQty);
        } else if (t.matches('.qty-plus')) {
            var id2 = t.getAttribute('data-id');
            var cart2 = getCart();
            var item2 = cart2.find(function (x) { return x.id === id2; });
            if (!item2) return;
            var newQty2 = (parseInt(item2.quantity || 0, 10) + 1);
            setQuantity(id2, newQty2);
        }
    }

    function onDocumentInput(e) {
        var t = e.target;
        if (t.matches('.qty-input')) {
            var id = t.getAttribute('data-id');
            var v = parseInt(t.value, 10) || 1;
            if (v < 1) v = 1;
            setQuantity(id, v);
        }
    }

    function sdVoucher() {
        var code = document.getElementById('VoucherCode').value.trim();
        var msgEl = document.getElementById('voucherMessage');
        if (!code) {
            if (msgEl) { msgEl.style.color = 'red'; msgEl.textContent = 'Nhập mã voucher.'; }
            return;
        }
      
        if (code.toUpperCase() === 'GIAM10') {
            voucherPercent = 10;
            appliedVoucher = { code: code, discount: 0 };
            if (msgEl) { msgEl.style.color = 'green'; msgEl.textContent = 'Áp dụng GIAM10: giảm 10%'; }
        } else if (code.toUpperCase() === 'GIAM10000') {
            voucherPercent = 0;
            appliedVoucher = { code: code, discount: 10000 };
            if (msgEl) { msgEl.style.color = 'green'; msgEl.textContent = 'Áp dụng GIAM10000: giảm ₫10,000'; }
        } else if (code.toUpperCase() === 'FREESHIP') {
            voucherPercent = 0;
            appliedVoucher = { code: code, discount: 0 };
            if (msgEl) { msgEl.style.color = 'green'; msgEl.textContent = 'Áp dụng FREESHIP'; }
        } else {
            voucherPercent = 0;
            appliedVoucher = { code: null, discount: 0 };
            if (msgEl) { msgEl.style.color = 'red'; msgEl.textContent = 'Mã voucher không hợp lệ.'; }
        }
        updateOrderSummary();
    }

  
    function togglePaymentFields() {
        var credit = document.getElementById('creditcard');
        var momo = document.getElementById('momo');
        var sel = document.querySelector('input[name="PtThanhToan"]:checked');
        if (!sel) return;
        if (sel.value === 'credit') {
            if (credit) credit.style.display = '';
            if (momo) momo.style.display = 'none';
        } else if (sel.value === 'momo') {
            if (credit) credit.style.display = 'none';
            if (momo) momo.style.display = '';
        }
    }

    function submitOrder(ev) {
        if (ev && ev.preventDefault) ev.preventDefault();

        var cart = getCart();
        if (!cart || cart.length === 0) {
            alert('Giỏ hàng trống.');
            return;
        }

        var fullName = document.querySelector('#ThongTin input[type="text"]') ? document.querySelector('#ThongTin input[type="text"]').value.trim() : '';
        var phone = document.getElementById('Đt') ? document.getElementById('Đt').value.trim() : '';
        var email = document.getElementById('email') ? document.getElementById('email').value.trim() : '';
        var city = document.getElementById('city') ? document.getElementById('city').value.trim() : '';
        var address = document.getElementById('địa chỉ') ? document.getElementById('địa chỉ').value.trim() : '';

        if (!fullName || !phone || !email || !city || !address) {
            alert('Vui lòng điền đầy đủ thông tin cá nhân và địa chỉ giao hàng.');
            return;
        }

        var pay = document.querySelector('input[name="PtThanhToan"]:checked');
        if (!pay) {
            alert('Vui lòng chọn phương thức thanh toán.');
            return;
        }

        var order = {
            id: 'ORD' + Date.now(),
            createdAt: new Date().toISOString(),
            status: 'Chờ giao',
            items: cart,
            customer: { name: fullName, phone: phone, email: email },
            shipping: { city: city, address: address },
            paymentMethod: pay.value,
            voucher: appliedVoucher && appliedVoucher.code ? appliedVoucher.code : null,
            totals: {
                subtotal: cart.reduce(function (acc, it) { return acc + (parseNumber(it.price) * (parseInt(it.quantity || 0, 10) || 0)); }, 0)
            }
        };

        var subtotal = order.totals.subtotal;
        var voucherDiscount = 0;
        if (voucherPercent && voucherPercent > 0) voucherDiscount = Math.round(subtotal * (voucherPercent / 100));
        else if (appliedVoucher && appliedVoucher.discount) voucherDiscount = appliedVoucher.discount;
        var delivery = 0;
        var grandTotal = Math.max(0, subtotal - voucherDiscount + delivery);

        order.totals.voucher = voucherDiscount;
        order.totals.delivery = delivery;
        order.totals.total = grandTotal;

        try {
            localStorage.setItem('lastOrder', JSON.stringify(order));

            var orders = [];
            try {
                var raw = localStorage.getItem('orders');
                orders = raw ? JSON.parse(raw) : [];
                if (!Array.isArray(orders)) orders = [];
            } catch (e) {
                orders = [];
            }
            orders.unshift(order);
            localStorage.setItem('orders', JSON.stringify(orders));

            localStorage.removeItem(CART_KEY);
        } catch (e) {
            console.error('Could not persist order', e);
        }

        renderCart();

       
        window.location.href = '/TrangChu/XacNhanDonHang';
    }

    function init() {
        document.addEventListener('click', onDocumentClick);
        document.addEventListener('input', onDocumentInput);

        window.sdVoucher = sdVoucher;
        window.togglePaymentFields = togglePaymentFields;
        window.submitOrder = submitOrder;

        renderCart();
        togglePaymentFields();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();
