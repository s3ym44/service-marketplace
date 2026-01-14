// offer-calculator.js

$(document).ready(function() {
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
        $('#LaborCost, #LaborCostType').on('change input', function() {
            state.laborCost = parseFloat($('#LaborCost').val()) || 0;
            state.laborType = $('#LaborCostType').val();
            calculateTotal();
        });

        $('#btnAddMaterial').click(function() {
            const type = $('#materialSourceType').val();
            if(type === 'Admin') {
                const selected = $('#adminMaterialSelect option:selected');
                if(!selected.val()) return;
                
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
                
                if(!name || price <= 0) {
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
        
        $('#offerForm').on('submit', function(e) {
            console.log('Form submitting...');
            
            const materialsJson = JSON.stringify(state.materials);
            $('#MaterialsJson').val(materialsJson);
            console.log('Materials JSON:', materialsJson);
            
            const servicesJson = JSON.stringify(state.extras);
            $('#AdditionalServicesJson').val(servicesJson);
            console.log('Services JSON:', servicesJson);
            
            if(state.materials.length === 0) {
                e.preventDefault();
                alert('Please add at least one material');
                return false;
            }
            
            if(state.laborCost <= 0) {
                e.preventDefault();
                alert('Please enter labor cost');
                return false;
            }
            
            console.log('Form validation passed, submitting...');
        });
    }
    
    window.addMaterial = function(material) {
        material.totalPrice = material.quantity * material.unitPrice;
        state.materials.push(material);
        renderMaterialList();
        calculateTotal();
    };

    window.removeMaterial = function(index) {
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
                    <td>${m.name} <small class="text-muted d-block">${m.brand} - ${m.source}</small></td>
                    <td>${m.quantity} ${m.unit}</td>
                    <td>₺${m.unitPrice.toFixed(2)}</td>
                    <td>₺${m.totalPrice.toFixed(2)}</td>
                    <td>
                        <button type="button" class="btn btn-sm btn-danger" onclick="removeMaterial(${index})">&times;</button>
                    </td>
                </tr>
            `;
            tbody.append(html);
        });

        $('#MaterialsJson').val(JSON.stringify(state.materials));
    }

    function calculateTotal() {
        let laborTotal = 0;
        if(state.laborType === 'Fixed') {
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
