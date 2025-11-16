<template>
    <AssetEditModal :is-open="isOpen" title="Sửa tài sản" @close="$emit('close')">
        <template #body>
            <div class="form-body">
                <div class="row d-flex flex-direction-row">
                    <div class="d-flex flex1 flex-direction-column">
                        <div class="form-group">
                            <ms-input label="Mã tài sản" v-model="assetCode" required />
                        </div>
                        <div class="form-group">
                            <ms-select label="Mã bộ phận sử dụng" require :options="assetTypeOptions">
                                <template #suffix>
                                    <i class="icon-default icon-chevrons-down"></i>
                                </template>
                            </ms-select>
                        </div>
                        <div class="form-group">
                            <ms-select label="Mã loại tài sản" require :options="assetTypeOptions">
                                <template #suffix>
                                    <i class="icon-default icon-chevrons-down"></i>
                                </template>
                            </ms-select>
                        </div>
                        <div class="form-group">
                            <ms-date label="Ngày mua" required=""></ms-date>
                        </div>
                    </div>
                    <div class="d-flex flex2 flex-direction-column">
                        <div class="form-group">
                            <ms-input label="Tên tài sản" v-model="assetCode" required />
                        </div>
                        <div class="form-group">
                            <ms-input label="Tên bộ phận sử dụng" v-model="assetCode" :disabled="true" />
                        </div>
                        <div class="form-group">
                            <ms-input label="Tên loại tài sản" v-model="assetCode" :disabled="true" />
                        </div>
                        <div class="d-flex flex-direction-row">
                            <div class="d-flex flex-direction-column flex1 mr-16 ">
                                <div class="form-group">
                                    <ms-input label="Nguyên giá" v-model="assetCode" required="" />
                                </div>
                                <div class="form-group">
                                    <ms-input label="Giá trị hao mòn năm" v-model="assetCode" required="" />
                                </div>
                            </div>
                            <div class="d-flex flex-direction-column flex1">
                                <div class="form-group">
                                    <ms-input label="Tỷ lệ hao mòn (%)" v-model="assetCode" required="" />
                                </div>
                                <div class="form-group">
                                    <ms-input label="Năm theo dõi" v-model="assetCode" :disabled="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </template>

        <template #actions>
            <button class="btn btn-secondary" @click="$emit('cancel')">Hủy</button>
            <button class="btn btn-primary" @click="saveAsset">Lưu</button>
        </template>

    </AssetEditModal>
</template>

<script setup>
import { reactive, computed } from 'vue';
import AssetEditModal from '@/components/ms-modal/MsModal.vue'; // Import component modal
import MsButton from '@/components/ms-button/MsButton.vue';
import MsInput from '@/components/ms-input/MsInput.vue';
import MsSelect from '@/components/ms-select/MsSelect.vue';
import MsDate from '@/components/ms-date/MsDate.vue';
// Khai báo props và emits
defineProps({
    isOpen: {
        type: Boolean,
        required: true
    }
});
const emit = defineEmits(['close', 'cancel', 'save']);
const assetTypeOptions = [
    { value: '0', label: 'Chọn giới tính', selected: 'true' },
    { value: '1', label: 'Nam' },
    { value: '2', label: 'Nữ' }
];
// Dữ liệu form
const formData = reactive({
    assetCode: 'TS00001',
    assetName: 'Laptop Dell 357GE Black 01',
    departmentCode: 'HCTH',
    roomName: 'Phòng Hành chính Tổng hợp',
    assetTypeCode: 'MTXT',
    assetTypeName: 'Máy tính xách tay',
    quantity: 1,
    originalPrice: 10000000,
    wearRate: 'TS00001',
});

// Logic format tiền tệ
const formatCurrency = (value) => {
    if (value === null || value === undefined || value === '') return '';
    return new Intl.NumberFormat('vi-VN').format(value);
};

const formattedOriginalPrice = computed({
    get() {
        return formatCurrency(formData.originalPrice);
    },
    set(newValue) {
        const numericValue = parseInt(newValue.replace(/\./g, ''), 10);
        formData.originalPrice = isNaN(numericValue) ? 0 : numericValue;
    }
});

// Xử lý sự kiện lưu
const saveAsset = () => {
    console.log('Dữ liệu được lưu:', formData);
    emit('save', formData);
};
</script>

<style scoped>
/* -------------------------------------- */
/* CSS Layout Form (từ hình ảnh) */
/* -------------------------------------- */
* {
    box-sizing: border-box;
    font-family: Arial, sans-serif;
}

.form-body {
    padding: 0px 16px 0px 14px;
    /* Mô phỏng padding trong hình */
}

.row {
    display: flex;
    gap: 16px;
    /* Khoảng cách giữa 2 cột */
}

.col {
    flex: 1;
    display: flex;
    flex-direction: column;
}

.form-group {
    margin-bottom: 8px;
    display: flex;
    flex-direction: column;
}

.inline-group {
    display: flex;
    gap: 16px;
}

.form-group-half {
    flex: 1;
}

/* Label */
label {
    font-size: 13px;
    color: #555;
    margin-bottom: 4px;
    display: block;
}

.required {
    color: red;
}

/* Input/Select Fields */
input[type="text"],
input[type="number"],
select {
    height: 36px;
    padding: 0 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 14px;
    color: #333;
    appearance: none;
    background-color: #fff;
    background-image: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="%23666" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-chevron-down"><polyline points="6 9 12 15 18 9"></polyline></svg>');
    background-repeat: no-repeat;
    background-position: right 8px center;
    cursor: pointer;
}

input[type="number"] {
    width: 80px;
    text-align: center;
    padding: 0 4px;
}

.input-right-align {
    text-align: right;
}

select {
    padding-right: 30px;
}

/* Focus State */
input:focus,
select:focus {
    border-color: #007bff;
    box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
    outline: none;
}

/* -------------------------------------- */
/* CSS Nút hành động */
/* -------------------------------------- */
.btn {
    padding: 8px 16px;
    border-radius: 4px;
    font-size: 14px;
    cursor: pointer;
    transition: background-color 0.2s;
    height: 36px;
}

.btn-primary {
    background-color: #007bff;
    color: white;
    border: 1px solid #007bff;
}

.btn-primary:hover {
    background-color: #0056b3;
}

.btn-secondary {
    background-color: #fff;
    color: #333;
    border: 1px solid #ccc;
}

.btn-secondary:hover {
    background-color: #e9ecef;
}
</style>