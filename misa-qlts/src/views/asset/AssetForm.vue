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
                            <!-- <ms-input label="Số lượng" v-model="assetCode" required="" type="number" /> -->
                            <ms-input-number hasButton isRequired size="large" v-model="quantity" label="Số lượng" />
                        </div>
                        <!-- Ngày mua -->
                        <div class="form-group">
                            <ms-date label="Ngày mua" required="" v-model="datePurchase">
                                <template #icon>
                                    <i class="icon-default icon-calendar"></i>
                                </template>
                            </ms-date>
                            <!-- <label for="purchase-date">Ngày mua<span class="required">*</span></label>
                            <input type="date" placeholder="dd/MM/yyyy" style="height: 32px;"
                                v-model="formData.datePurchase" /> -->
                        </div>
                        <!-- Số năm sử dụng -->
                        <div class="form-group">
                            <!-- <ms-input label="Số năm sử dụng" v-model="yearOfUse" required="" type="number" /> -->
                            <ms-input-number hasButton isRequired size="large" v-model="yearOfUse"
                                label="Số năm sử dụng" />
                        </div>
                    </div>
                    <div class="d-flex flex2 flex-direction-column">
                        <!-- Tên tài sản -->
                        <div class="form-group">
                            <ms-input label="Tên tài sản" v-model="assetName" required placeholder="Nhập tên tài sản" />
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
                                    <ms-input label="Nguyên giá" v-model="originalPrice" :is-number="true" required=""
                                        align="right" />
                                </div>
                                <!-- Ngày mua -->
                                <div class="form-group">
                                    <ms-date label="Ngày bắt đầu sử dụng" required="" v-model="createdDate">
                                        <template #icon>
                                            <i class="icon-default icon-calendar"></i>
                                        </template>
                                    </ms-date>
                                    <!-- <label for="purchase-date">Ngày bắt đầu sử dụng<span
                                            class="required">*</span></label>
                                    <input type="date" placeholder="dd/MM/yyyy" style="height: 32px;"
                                        v-model="formData.datePurchase" /> -->
                                </div>
                                <!-- Giá trị hao mòn năm -->
                                <div class="form-group">
                                    <ms-input label="Giá trị hao mòn năm" :is-number="true" v-model="annualDepreciation"
                                        required="" align="right" />
                                </div>
                            </div>
                            <div class="d-flex flex-direction-column flex1">
                                <!-- Tỷ lệ hao mòn -->
                                <div class="form-group">
                                    <ms-input label="Tỷ lệ hao mòn (%)" v-model="wareRate" :is-number="true" required=""
                                        align="right" />
                                </div>
                                <!-- Năm theo dõi -->
                                <div class="form-group">
                                    <ms-input label="Năm theo dõi" v-model="currentYear" :disabled="true"
                                        align="right" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </template>

        <template #actions>
            <ms-button type="secondary" @click="handleCancel" buttonComponentStyle="btn-modal">
                <p class="btn-text">Hủy</p>
            </ms-button>
            <ms-button type="primary" @click="openConfirm" buttonComponentStyle="btn-modal">
                <p class="btn-text">Lưu</p>
            </ms-button>
        </template>

    </ms-modal>
    <ms-modal-confirm :is-open="showConfirmCancelEdit"
        message="Thông tin thay đổi sẽ không được cập nhật nếu bạn không lưu. Bạn có muốn lưu các thay đổi này?">
        <ms-button type="secondary" @click="closeCancelEdit" buttonComponentStyle="btn-modal border-1">Hủy
            bỏ</ms-button>
        <button class="btn btn-no-save" @click="dontSaveEdit">Không lưu</button>
        <ms-button type="primary" @click="confirmSave" buttonComponentStyle="btn-modal">
            <p class="btn-text">Lưu</p>
        </ms-button>
    </ms-modal-confirm>
    <ms-modal-confirm :is-open="showConfirmCancelAddCopy" message="Bạn có muốn hủy bỏ khai báo tài sản này?">
        <ms-button type="secondary" @click="closeCancelAddCopy"
            buttonComponentStyle="btn-modal border-1">Không</ms-button>
        <ms-button type="primary" @click="confirmCloseAddCopy" buttonComponentStyle="btn-modal">
            <p class="btn-text">Hủy bỏ</p>
        </ms-button>
    </ms-modal-confirm>
</template>

<script setup>
import { computed, ref, onMounted, watch } from 'vue';
import MsModal from '@/components/ms-modal/MsModal.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsInput from '@/components/ms-input/MsInput.vue';
import MsSelect from '@/components/ms-select/MsSelect.vue';
import MsDate from '@/components/ms-date/MsDate.vue';
import AssetTypeApi from '@/apis/components/AssetTypeApi.js';
import DepartmentApi from '@/apis/components/DepartmentApi.js';
import AssetApi from '@/apis/components/AssetApi.js';
import { openToast } from "@/utils/showToast.js";
import MsModalConfirm from '@/components/ms-modal/MsModalConfirm.vue';
import MsInputNumber from '@/components/ms-input/MsInputNumber.vue';
const lsAssetType = ref([]);
const lsDepartment = ref([]);
const generateCode = ref('');
const selectedDepartmentId = ref(null);
const selectedAssetTypeId = ref(null);
const departmentName = ref("");
const assetTypeName = ref("");
const assetName = ref("");
const yearOfUse = ref(0);
const wareRate = ref(0);
const currentYear = new Date().getFullYear();
const originalPrice = ref(0);
const annualDepreciation = ref(0);
const quantity = ref(1);
const datePurchase = ref(new Date().toISOString().split("T")[0]);
const createdDate = ref(new Date().toISOString().split("T")[0]);
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
const showConfirm = ref(false);
const showConfirmCancelEdit = ref(false);
const showConfirmCancelAddCopy = ref(false);
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

/**
 * Chuẩn hoá giá trị số nhận từ bảng (đã format chuỗi) về number
 * @param {*} value 
 * @param {*} defaultValue 
 * @returns {number}
 */
function toNumber(value, defaultValue = 0) {
    if (value === null || value === undefined || value === '') return defaultValue;
    if (typeof value === 'number') return value;
    if (typeof value === 'string') {
        const normalized = value.replace(/\./g, '').replace(',', '.');
        const parsed = parseFloat(normalized);
        return isNaN(parsed) ? defaultValue : parsed;
    }
    return defaultValue;
}

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
    const [assetTyps, department, generatedAssetCode] = await Promise.all([
        getAllAssetType(),
        getAllDepartment(),
        generateAssetCode()
    ])

    lsAssetType.value = assetTyps;
    lsDepartment.value = department;
    generateCode.value = generatedAssetCode;
});

/**
 * Hàm validate dữ liệu
 * CreatedBy: QuanPA - 22/11/2025
 */
const validateAsset = () => {
    if (!generateCode.value) {
        return { valid: false, message: "Cần phải nhập thông tin Mã tài sản" };
    }
    if (!assetName.value) {
        return { valid: false, message: "Cần phải nhập thông tin Tên tài sản" };
    }
    if (!selectedDepartmentId.value) {
        return { valid: false, message: "Cần chọn bộ phận sử dụng" };
    }
    if (!selectedAssetTypeId.value) {
        return { valid: false, message: "Cần chọn loại tài sản" };
    }
    if (!originalPrice.value || originalPrice.value <= 0) {
        return { valid: false, message: "Nguyên giá phải lớn hơn 0" };
    }
    if (!wareRate.value || wareRate.value <= 0) {
        return { valid: false, message: "Tỷ lệ hao mòn phải lớn hơn 0" };
    }
    if (!yearOfUse.value || yearOfUse.value <= 0) {
        return { valid: false, message: "Số năm sử dụng phải lớn hơn 0" };
    }
    if (!annualDepreciation.value || annualDepreciation.value <= 0) {
        return { valid: false, message: "Giá trị hao mòn năm phải lớn hơn 0" };
    }
    if (annualDepreciation.value > originalPrice.value) {
        return { valid: false, message: "Hao mòn năm phải nhỏ hơn hoặc bằng nguyên giá" };
    }
    const roundedWareRate = Number((wareRate.value / 100).toFixed(2));
    const roundedYearUse = Number((1 / yearOfUse.value).toFixed(2));

    if (roundedWareRate !== roundedYearUse) {
        return { valid: false, message: "Tỷ lệ hao mòn phải bằng 1 / Số năm sử dụng" };
    }

    return { valid: true };
};


/**
 * Hàm lưu tài sản
 * CreatedBy: QuanPA - 17/11/2025
 */
const saveAsset = async () => {
    try {
        const check = validateAsset();
        if (!check.valid) {
            openToast("error", "Thất bại", check.message);
            return;
        }

        const basePayload = {
            assetCode: generateCode.value,
            assetName: assetName.value,
            datePurchase: datePurchase.value,
            quantity: parseFloat(quantity.value) || 1,
            originalPrice: parseFloat(originalPrice.value),
            depreciationValueYear: parseFloat(annualDepreciation.value),
            assetTypeId: selectedAssetTypeId.value,
            departmentId: selectedDepartmentId.value,
            remaining: annualDepreciation.value
        };
        const payload = { ...basePayload };
        const currentUser = "Phan Anh Quân";
        if (props.mode === 'add' || props.mode === 'copy') {
            payload.createdDate = createdDate.value;
            payload.createdBy = currentUser;
            payload.modifiedBy = currentUser;
        } else if (props.mode === 'edit') {
            payload.modifiedDate = createdDate.value;
            payload.modifiedBy = currentUser;
            payload.createdBy = currentUser;
        }
        let response;
        if (props.mode === 'add' || props.mode === 'copy') {
            response = await AssetApi.create(payload);
        } else if (props.mode === 'edit') {
            payload.assetId = localData.value.assetId;
            response = await AssetApi.update(localData.value.assetId, payload);
        }
        if (response && response.data && response.data.success) {
            openToast("success", "Thành công", "Lưu dữ liệu thành công");
            emit('save', payload);
            emit('close');
        } else {
            openToast("error", "Thất bại", "Lưu dữ liệu thất bại");
        }
    }
    catch (error) {
        openToast("error", "Thất bại", error.response.data.message || "Lưu dữ liệu thất bại");
    }
}
/**
 * Hàm mở form confirm
 * CreatedBy: QuanPA - 18/11/2025
 */
function openConfirm() {
    saveAsset();
}

/**
 * Hàm đóng form confirm
 * CreatedBy: QuanPA - 18/11/2025
 */
function closeConfirm() {
    showConfirm.value = false
}

/**
 * Hàm hủy hành động
 * CreatedBy: QuanPA - 18/11/2025
 */
function handleCancel() {
    if (props.mode === 'edit') {
        showConfirmCancelEdit.value = true;
    } else {
        showConfirmCancelAddCopy.value = true;
    }
}

/**
 * Hàm đóng form edit
 * CreatedBy: QuanPA - 18/11/2025
 */
function closeCancelEdit() {
    showConfirmCancelEdit.value = false;
}

/**
 * Hàm không lưu dữ liệu chỉnh sửa
 * CreatedBy: QuanPA - 18/11/2025
 */
function dontSaveEdit() {
    showConfirmCancelEdit.value = false;
    emit('close');
}

/**
 * Hàm đóng form hủy thêm dữ liệu
 * CreatedBy: QuanPA - 18/11/2025
 */
function closeCancelAddCopy() {
    showConfirmCancelAddCopy.value = false;
}

/**
 * Hàm đóng form thêm dữ liệu
 * CreatedBy: QuanPA - 18/11/2025
 */
function confirmCloseAddCopy() {
    showConfirmCancelAddCopy.value = false;
    emit('close');
}

/**
 * Hàm reset form
 * CreatedBy: QuanPA - 18/11/2025
 */
async function resetForm() {
    generateCode.value = await generateAssetCode();
    assetName.value = "";
    selectedDepartmentId.value = null;
    selectedAssetTypeId.value = null;
    yearOfUse.value = 0;
    wareRate.value = 0;
    originalPrice.value = 0;
    annualDepreciation.value = 0;
    quantity.value = 1;
    datePurchase.value = new Date().toISOString().split("T")[0];
    createdDate.value = new Date().toISOString().split("T")[0];
}
/**
 * Hàm xác nhận lưu
 * CreatedBy: QuanPA - 18/11/2025
 */
function confirmSave() {
    closeConfirm()
    saveAsset()
}
//#region Computed
const modalTitle = computed(() => {
    return props.mode === 'edit' ? 'Sửa tài sản' : 'Thêm tài sản';
});

/**
 * Hàm load dữ liệu để chỉnh sửa
 * @param data 
 * CreatedBy: QuanPA - 18/11/2025
 */
function loadEditData(data) {
    localData.value = JSON.parse(JSON.stringify(data));
    generateCode.value = data.assetCode;
    assetName.value = data.assetName;
    selectedAssetTypeId.value = data.assetTypeId;
    selectedDepartmentId.value = data.departmentId;
    originalPrice.value = toNumber(data.originalPrice);
    wareRate.value = toNumber(data.wearRate);
    yearOfUse.value = toNumber(data.yearOfUse, yearOfUse.value);
    datePurchase.value = data.datePurchase;
    quantity.value = toNumber(data.quantity, 1);
    annualDepreciation.value = toNumber(data.depreciationValueYear);
    createdDate.value = data.createdDate || createdDate.value;
}

/**
 * Hàm load dữ liệu để nhân bản
 * @param data 
 * CreatedBy: QuanPA - 18/11/2025
 */
async function loadCopyData(data) {
    localData.value = {};
    generateCode.value = await generateAssetCode();
    assetName.value = data.assetName;
    selectedAssetTypeId.value = data.assetTypeId;
    selectedDepartmentId.value = data.departmentId;
    originalPrice.value = toNumber(data.originalPrice);
    wareRate.value = toNumber(data.wearRate);
    yearOfUse.value = toNumber(data.yearOfUse, yearOfUse.value);
    datePurchase.value = data.datePurchase;
    quantity.value = toNumber(data.quantity, 1);
    annualDepreciation.value = toNumber(data.depreciationValueYear);
    createdDate.value = new Date().toISOString().split("T")[0];
}
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

watch(() => props.isOpen, (val) => {
    if (val) {
        if (props.mode === 'add') {
            resetForm();
        }
        if (props.mode === 'copy') {
            loadCopyData(props.data);
        }
        if (props.mode === 'edit') {
            loadEditData(props.data);
        }
    }
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
    height: 70px;
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

.btn {
    flex: 1;
    padding: 8px 10px;
    border: none;
    border-radius: 2.5px;
    cursor: pointer;
}

.btn-no-save {
    background: #e8f6ff;
    color: #2a89d5;
    border: 1px solid #1aa4c8;
}
</style>