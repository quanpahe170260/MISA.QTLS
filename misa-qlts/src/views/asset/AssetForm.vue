<template>
    <ms-modal :is-open="isOpen" :title="modalTitle" @close="$emit('close')">
        <template #body>
            <div class="form-body">
                <div class="row d-flex flex-direction-row">
                    <div class="d-flex flex1 flex-direction-column">
                        <!-- Mã tài sản -->
                        <div class="form-group">
                            <ms-input label="Mã tài sản" v-model="generateCode" required placeholder="Mã tài sản" />
                        </div>
                        <!-- Mã bộ phận sử dụng -->
                        <div class="form-group">
                            <ms-select label="Mã bộ phận sử dụng" v-model="selectedDepartmentId" required
                                :options="lsDepartment" placeholder="Chọn mã bộ phận sử dụng">
                                <template #suffix>
                                    <i class="icon-default icon-chevrons-down"></i>
                                </template>
                            </ms-select>
                        </div>
                        <!-- Mã loại tài sản -->
                        <div class="form-group">
                            <ms-select label="Mã loại tài sản" v-model="selectedAssetTypeId" required
                                :options="lsAssetType" placeholder="Chọn mã loại tài sản">
                                <template #suffix>
                                    <i class="icon-default icon-chevrons-down"></i>
                                </template>
                            </ms-select>
                        </div>
                        <!-- Số lượng -->
                        <div class="form-group">
                            <ms-input label="Số lượng" v-model="assetCode" required="" type="number" />
                        </div>
                        <!-- Ngày mua -->
                        <div class="form-group">
                            <!-- <ms-date label="Ngày mua" required=""></ms-date> -->
                            <label for="purchase-date">Ngày mua<span class="required">*</span></label>
                            <input type="date" placeholder="dd/MM/yyyy" style="height: 32px;"
                                v-model="formData.datePurchase" />
                        </div>
                        <!-- Số năm sử dụng -->
                        <div class="form-group">
                            <ms-input label="Số năm sử dụng" v-model="yearOfUse" required="" type="number" />
                        </div>
                    </div>
                    <div class="d-flex flex2 flex-direction-column">
                        <!-- Tên tài sản -->
                        <div class="form-group">
                            <ms-input label="Tên tài sản" v-model="assetTypeName" required placeholder="Tên tài sản" />
                        </div>
                        <!-- Tên bộ phận sử dụng -->
                        <div class="form-group">
                            <ms-input label="Tên bộ phận sử dụng" v-model="departmentName" :disabled="true" />
                        </div>
                        <!-- Tên loại tài sản -->
                        <div class="form-group">
                            <ms-input label="Tên loại tài sản" v-model="assetTypeName" :disabled="true" />
                        </div>
                        <div class="d-flex flex-direction-row">
                            <div class="d-flex flex-direction-column flex1 mr-16 ">
                                <!-- Nguyên giá -->
                                <div class="form-group">
                                    <ms-input label="Nguyên giá" v-model="originalPrice" required="" />
                                </div>
                                <!-- Giá trị hao mòn năm -->
                                <div class="form-group">
                                    <ms-input label="Giá trị hao mòn năm" v-model="annualDepreciation" required="" />
                                </div>
                                <!-- Ngày mua -->
                                <div class="form-group">
                                    <!-- <ms-date label="Ngày mua" required=""></ms-date> -->
                                    <label for="purchase-date">Ngày bắt đầu sử dụng<span
                                            class="required">*</span></label>
                                    <input type="date" placeholder="dd/MM/yyyy" style="height: 32px;"
                                        v-model="formData.datePurchase" />
                                </div>
                            </div>
                            <div class="d-flex flex-direction-column flex1">
                                <!-- Tỷ lệ hao mòn -->
                                <div class="form-group">
                                    <ms-input label="Tỷ lệ hao mòn (%)" v-model="wareRate" required="" />
                                </div>
                                <!-- Năm theo dõi -->
                                <div class="form-group">
                                    <ms-input label="Năm theo dõi" v-model="currentYear" :disabled="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </template>

        <template #actions>
            <ms-button type="secondary" @click="$emit('cancel')" buttonComponentStyle="btn-modal">
                <p class="btn-text">Hủy</p>
            </ms-button>
            <ms-button type="primary" @click="saveAsset" buttonComponentStyle="btn-modal">
                <p class="btn-text">Lưu</p>
            </ms-button>
        </template>

    </ms-modal>
</template>

<script setup>
import { computed, ref, onMounted, watch } from 'vue';
import MsModal from '@/components/ms-modal/MsModal.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsInput from '@/components/ms-input/MsInput.vue';
import MsSelect from '@/components/ms-select/MsSelect.vue';
// import MsDate from '@/components/ms-date/MsDate.vue';
import AssetTypeApi from '@/apis/components/AssetTypeApi.js';
import DepartmentApi from '@/apis/components/DepartmentApi.js';
import AssetApi from '@/apis/components/AssetApi.js';

const lsAssetType = ref([]);
const lsDepartment = ref([]);
const generateCode = ref('');
const selectedDepartmentId = ref(null);
const selectedAssetTypeId = ref(null);
const departmentName = ref("");
const assetTypeName = ref("");
const yearOfUse = ref(0);
const wareRate = ref(0);
const currentYear = new Date().getFullYear();
const originalPrice = ref(0);
const annualDepreciation = ref(0);
const localData = ref({
    assetId: null,
    assetCode: '',
    assetName: '',
    datePurchase: new Date().toISOString().split("T")[0],
    quantity: 0,
    originalPrice: 0,
    depreciationValueYear: 0,
    assetTypeId: null,
    departmentId: null,
});
//#region Props
const props = defineProps({
    isOpen: {
        type: Boolean,
        required: true
    },
    mode: { type: String, default: 'add' },
    data: { type: Object, default: () => ({}) }
});
//#endregion

//#region Emits
const emit = defineEmits(['close', 'cancel', 'save']);
//#endregion

const formData = ref({
    datePurchase: new Date().toISOString().split("T")[0]
});

/**
 * Hàm lấy danh sách bộ phận
 * @return danh sách bộ phận
 * CreatedBy: QuanPA - 17/11/2025
 */
async function getAllDepartment() {
    try {
        const response = await DepartmentApi.getAll();
        return response.data.data.map(item => ({
            value: item.departmentId,
            label: item.departmentAbbreviated,
            nameDepart: item.departmentName
        }));
    }
    catch (error) {
        console.log("Lỗi lấy danh sách bộ phận", error);
    }
}

/**
 * Hàm tạo mã tài sản
 * @return mã tài sản
 * CreatedBy: QuanPA - 17/11/2025
 */
async function generateAssetCode() {
    try {
        const response = await AssetApi.generateAssetCode();
        return response.data.data;
    } catch (error) {
        console.log("Lỗi tạo mã tài sản", error);
    }
}

/**
 * Hàm lấy danh sách loại tài sản
 * @return danh sách loại tài sản
 * CreatedBy: QuanPA - 17/11/2025
 */
async function getAllAssetType() {
    try {
        const response = await AssetTypeApi.getAll()
        return response.data.data.map(item => ({
            value: item.assetTypeId,
            label: item.assetTypeAbbreviated,
            nameAssetType: item.assetTypeName,
            yearOfUse: item.yearOfUse,
            wareRate: item.wearRate
        }));
    }
    catch (error) {
        console.log("Lỗi lấy danh sách loại tài sản", error);
    }
}

onMounted(async () => {
    lsAssetType.value = await getAllAssetType();
    lsDepartment.value = await getAllDepartment();
    generateCode.value = await generateAssetCode();
});

/**
 * Hàm lưu tài sản
 * CreatedBy: QuanPA - 17/11/2025
 */
const saveAsset = async () => {
    try {
        const payload = {
            assetCode: generateCode.value,
            assetName: assetTypeName.value,
            datePurchase: formData.value.datePurchase,
            quantity: parseFloat(formData.value.quantity) || 1,
            originalPrice: parseFloat(originalPrice.value),
            depreciationValueYear: parseFloat(annualDepreciation.value),
            assetTypeId: selectedAssetTypeId.value,
            departmentId: selectedDepartmentId.value,
        };
        let response;
        if (props.mode === 'add') {
            response = await AssetApi.create(payload);
        } else if (props.mode === 'edit') {
            payload.assetId = localData.value.assetId;
            response = await AssetApi.update(localData.value.assetId, payload);
        }
        if (response && response.data && response.data.success) {
            alert("Tạo tài sản thành công!");
            emit('save', payload);
            emit('close');
        } else {
            alert("Tạo tài sản thất bại!");
        }
    }
    catch (error) {
        console.log("Lỗi tạo tài sản", error);
    }
}

// Logic format tiền tệ
// const formatCurrency = (value) => {
//     if (value === null || value === undefined || value === '') return '';
//     return new Intl.NumberFormat('vi-VN').format(value);
// };

// const formattedOriginalPrice = computed({
//     get() {
//         return formatCurrency(formData.value.originalPrice);
//     },
//     set(newValue) {
//         const numericValue = parseInt(newValue.replace(/\./g, ''), 10);
//         formData.value.originalPrice = isNaN(numericValue) ? 0 : numericValue;
//     }
// });

//#region Computed
const modalTitle = computed(() => {
    return props.mode === 'add' ? 'Thêm tài sản' : 'Sửa tài sản';
});
//#endregion

//#region watch
watch(selectedDepartmentId, (newVal) => {
    const found = lsDepartment.value.find(x => x.value === newVal);
    departmentName.value = found ? found.nameDepart : "";
});
watch(selectedAssetTypeId, (newVal) => {
    const found = lsAssetType.value.find(x => x.value === newVal);
    assetTypeName.value = found ? found.nameAssetType : "";
    yearOfUse.value = found ? found.yearOfUse : '';
    wareRate.value = found ? found.wareRate : '';
});
watch([originalPrice, wareRate], ([newPrice, newRate]) => {
    const price = parseFloat(newPrice);
    const rate = parseFloat(newRate);
    if (!isNaN(price) && !isNaN(rate)) {
        annualDepreciation.value = price * rate / 100;
    } else {
        annualDepreciation.value = '';
    }
});
watch(() => props.data, (newVal) => {
    if (props.mode === 'edit' && newVal) {
        localData.value = { ...newVal };
        generateCode.value = newVal.assetCode;
        assetTypeName.value = newVal.assetName;
        originalPrice.value = newVal.originalPrice;
        selectedAssetTypeId.value = newVal.assetTypeId;
        selectedDepartmentId.value = newVal.departmentId;
        formData.value.datePurchase = newVal.datePurchase;
        originalPrice.value = newVal.originalPrice;
        wareRate.value = newVal.wearRate;
    }
    console.log('localData', localData);
}, { immediate: true });

//#endregion
</script>

<style scoped>
.form-body {
    padding: 0px 16px 0px 14px;
}

.row {
    display: flex;
    gap: 16px;
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

label {
    font-size: 13px;
    color: #555;
    margin-bottom: 4px;
    display: block;
}

.required {
    color: red;
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

input:focus,
select:focus {
    border-color: #007bff;
    box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
    outline: none;
}
</style>