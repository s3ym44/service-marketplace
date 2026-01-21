// offer-calculator.js

$(document).ready(function () {
    console.log("Offer Calculator Initialized");

    const state = {
        laborCost: 0,
        laborType: 'Fixed',
        area: parseFloat($('#listingArea').val()) || 0,
        materials: [],
        extras: []
    };

    initEvents();
    renderMaterialList();
    calculateTotal();

    function initEvents() {
        $('#LaborCost, #LaborCostType').on('change input', function () {
            state.laborCost = parseFloat($('#LaborCost').val()) || 0;
            state.laborType = $('#LaborCostType').val();
            calculateTotal();
        });

        $('#btnAddMaterial').click(function () {
            const type = $('#materialSourceType').val();
            if (type === 'Admin') {
                const selected = $('#adminMaterialSelect option:selected');
                if (!selected.val()) return;

                addMaterial({
                    name: selected.data('name'),
                    brand: selected.data('brand'),
                    quantity: parseFloat($('#matQty').val()) || 1,
                    unit: selected.data('unit'),
                    unitPrice: parseFloat(selected.data('price')),
                    source: 'AdminStock',
                    adminPriceId: parseInt(selected.val())
                });

                $('#adminMaterialSelect').val('');
                $('#matQty').val(1);
            } else {
                const name = $('#customMatName').val();
                const price = parseFloat($('#customMatPrice').val()) || 0;

                if (!name || price <= 0) {
                    alert('Please enter material name and price');
                    return;
                }

                addMaterial({
                    name: name,
                    brand: 'Custom',
                    quantity: parseFloat($('#matQty').val()) || 1,
                    unit: 'Unit',
                    unitPrice: price,
                    source: 'Custom',
                    adminPriceId: null
                });

                $('#customMatName').val('');
                $('#customMatPrice').val('');
                $('#matQty').val(1);
            }
        });

        // Template Load
        $('#btnLoadTemplate').click(function () {
            const templateId = $('#templateSelect').val();
            if (!templateId) {
                alert('Lütfen bir şablon seçin');
                return;
            }

            if (state.materials.length > 0) {
                if (!confirm('Mevcut malzeme listesi üzerine eklenecek. Devam edilsin mi?')) return;
            }

            // AJAX fetch Items
            $.get('/Offers/GetTemplateItems?templateId=' + templateId, function (data) {
                if (data && data.length > 0) {
                    data.forEach(item => {
                        addMaterial({
                            name: item.name,
                            brand: item.brand,
                            quantity: item.quantity,
                            unit: item.unit,
                            unitPrice: item.unitPrice,
                            source: 'AdminStock',
                            adminPriceId: item.adminPriceId
                        });
                    });
                    alert(data.length + ' kalem eklendi.');
                } else {
                    alert('Bu şablonda ürün bulunamadı.');
                }
            }).fail(function () {
                alert('Şablon yüklenirken hata oluştu.');
            });
        });

        // Catalog Search
        $('#catalogSearch').on('keyup', function () {
            const value = $(this).val().toLowerCase();
            $('#catalogTableBody tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            });
        });

        // Add Selected from Catalog
        $('#btnAddSelectedCatalog').click(function () {
            const selected = $('.catalog-check:checked');
            if (selected.length === 0) {

                return;
            }

            selected.each(function () {
                const $el = $(this);
                addMaterial({
                    name: $el.data('name'),
                    brand: $el.data('brand'),
                    quantity: 1, // Default 1
                    unit: $el.data('unit'),
                    unitPrice: parseFloat($el.data('price').toString().replace(',', '.')),
                    source: 'AdminStock',
                    adminPriceId: parseInt($el.val())
                });
                $el.prop('checked', false); // Reset checkbox
            });

            $('#catalogModal').modal('hide');
            // Show feedback (optional)
        });

        // FORM SUBMIT
        $('#offerForm').on('submit', function (e) {
            console.log('Form submitting...');

            const materialsJson = JSON.stringify(state.materials);
            $('#MaterialsJson').val(materialsJson);

            const servicesJson = JSON.stringify(state.extras);
            $('#AdditionalServicesJson').val(servicesJson);

            if (state.materials.length === 0) {
                e.preventDefault();
                alert('Lütfen en az bir malzeme ekleyin');
                return false;
            }

            if (state.laborCost <= 0) {
                e.preventDefault();
                alert('Lütfen işçilik maliyetini girin');
                return false;
            }
        });
    }

    window.addMaterial = function (material) {
        material.totalPrice = material.quantity * material.unitPrice;
        state.materials.push(material);
        renderMaterialList();
        calculateTotal();
    };

    window.removeMaterial = function (index) {
        state.materials.splice(index, 1);
        renderMaterialList();
        calculateTotal();
    };

    function renderMaterialList() {
        const tbody = $('#materialListBody');
        tbody.empty();

        state.materials.forEach((m, index) => {
            const html = `
                <tr>
                    <td>
                        <input type="text" class="form-control form-control-sm border-0 bg-transparent" value="${m.name}" readonly>
                        <small class="text-muted">${m.brand}</small>
                    </td>
                    <td style="width: 120px;">
                        <input type="number" step="0.01" class="form-control form-control-sm" value="${m.quantity}" onchange="updateQuantity(${index}, this.value)">
                        <small class="text-muted text-end d-block">${m.unit}</small>
                    </td>
                    <td>₺${m.unitPrice.toFixed(2)}</td>
                    <td>₺${m.totalPrice.toFixed(2)}</td>
                    <td>
                        <button type="button" class="btn btn-sm btn-outline-danger border-0" onclick="removeMaterial(${index})"><i class="bi bi-trash"></i></button>
                    </td>
                </tr>
            `;
            tbody.append(html);
        });

        $('#MaterialsJson').val(JSON.stringify(state.materials));
    }

    // New helper to update quantity live in table
    window.updateQuantity = function (index, confirmQty) {
        const qty = parseFloat(confirmQty);
        if (qty > 0) {
            state.materials[index].quantity = qty;
            state.materials[index].totalPrice = qty * state.materials[index].unitPrice;
            renderMaterialList();
            calculateTotal();
        }
    };

    function calculateTotal() {
        let laborTotal = 0;
        if (state.laborType === 'Fixed') {
            laborTotal = state.laborCost;
        } else {
            laborTotal = state.laborCost * state.area;
        }

        let materialTotal = state.materials.reduce((sum, m) => sum + m.totalPrice, 0);
        let extrasTotal = state.extras.reduce((sum, e) => sum + e.price, 0);
        const total = laborTotal + materialTotal + extrasTotal;

        $('#displayLaborTotal').text('₺' + laborTotal.toFixed(2));
        $('#displayMaterialTotal').text('₺' + materialTotal.toFixed(2));
        $('#displayGrandTotal').text('₺' + total.toFixed(2));
    }
});
