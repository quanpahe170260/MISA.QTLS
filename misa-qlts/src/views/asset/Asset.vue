<template>
    <div class="d-flex flex-direction-column">
        <!-- Main content -->
        <div class="main-content d-flex flex-direction-row mt-14 justify-content-space-between ">
            <!-- Header main content left -->
            <div class="d-flex flex-direction-row">
                <!-- Search -->
                <div class="search-content">
                    <ms-search icon="icon-default icon-lookup" placeholder="Tìm kiếm tài sản"
                        styleInput="border-none search-input" searchComponent="border-1" v-model="search" />
                </div>
                <!-- Filter asset type -->
                <div class="filter-asset-type">
                    <ms-select v-model="assetType" :options="lsAssetType">
                        <template #icon>
                            <i class="icon-default icon-filter"></i>
                        </template>
                        <template #suffix>
                            <i class="icon-default icon-chevrons-down"></i>
                        </template>
                    </ms-select>
                </div>
                <!-- Filter department -->
                <div class="filter-department">
                    <ms-select v-model="department" :options="lsDepartment">
                        <template #icon>
                            <i class="icon-default icon-filter"></i>
                        </template>
                        <template #suffix>
                            <i class="icon-default icon-chevrons-down"></i>
                        </template>
                    </ms-select>
                </div>
            </div>
            <!-- Header main content right -->
            <div class="d-flex flex-direction-row ">
                <div class="add mr-10">
                    <ms-button type="primary" icon="icon-mask icon-plus" positionIcon="left" @click="isFormOpen = true">
                        <p class="btn-text">Thêm tài sản</p>
                    </ms-button>
                </div>
                <div class="excel d-flex justify-content-center align-items-center mr-10">
                    <div class="icon-default icon-excel"></div>
                </div>
                <div class="delete d-flex justify-content-center align-items-center">
                    <div class="icon-default icon-delete"></div>
                </div>
            </div>
        </div>
        <div class="flex1">
            <div style="padding: 20px">
                <ms-table :columns="columns" :rows="lsAssets" :page="page" :page-size="pageSize" :total="total"
                    @update:page="page = $event" @update:pageSize="pageSize = $event"
                    @selection-change="selectedIds = $event" rowKey="assetId" />
            </div>
        </div>
    </div>
    <asset-form :is-open="isFormOpen" @close="isFormOpen = false" @cancel="isFormOpen = false" @save="handleSave" />
</template>
<script setup>
import MsSearch from '@/components/ms-search/MsSearch.vue';
import MsSelect from '@/components/ms-select/MsSelect.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsTable from '@/components/ms-table/MsTable.vue';
import AssetApi from '@/apis/components/AssetApi.js';
import AssetForm from '@/views/asset/AssetForm.vue';
import DepartmentApi from '@/apis/components/DepartmentApi.js';
import AssetTypeApi from '@/apis/components/AssetTypeApi';
import { ref, onMounted, watch } from "vue";
const lsAssets = ref([]);
const search = ref("");
const department = ref("");
const assetType = ref("");
const page = ref(1);
const pageSize = ref(10);
const total = ref(200);
const isFormOpen = ref(false);
const selectedIds = ref([]);
const lsAssetType = ref([]);
const lsDepartment = ref([]);
/**
 * Hàm lấy tất cả tài sản
 * return Danh sách tài sản
 * CreatedBy: QuanPA - 17/11/2025
 */
async function getAllAssets() {
    try {
        const response = await AssetApi.getAllAssetPaging(
            search.value,
            department.value,
            assetType.value,
            page.value,
            pageSize.value
        );
        total.value = response.data.data.totalCount;
        return response.data.data.data;
    } catch (error) {
        console.log("Lỗi lấy danh sách tài sản", error);
    }
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
    lsAssets.value = await getAllAssets();
    lsAssetType.value = await getAllAssetType();
    lsDepartment.value = await getAllDepartment();
});

const columns = ref([
    { key: "stt", label: "STT", align: "left" },
    { key: "assetId", label: "", align: "left", hidden: true },
    { key: "assetCode", label: "Mã tài sản" },
    { key: "assetName", label: "Tên tài sản" },
    { key: "assetTypeName", label: "Loại tài sản" },
    { key: "departmentName", label: "Bộ phận sử dụng" },
    { key: "quantity", label: "Số lượng", align: "right" },
    { key: "originalPrice", label: "Nguyên giá", align: "right" },
    { key: "depreciationValueYear", label: "HM/KM lũy kế", align: "right" },
    { key: "remaining", label: "Giá trị còn lại", align: "right" },
]);
async function loadData() {
    lsAssets.value = await getAllAssets();
}
watch([search, department, assetType, page, pageSize], () => {
    loadData();
});
</script>
<style scoped>
.main-content {
    padding: 0 20px;
}

.search-content {
    width: 180px;
    height: 36px;
    margin-right: 10px;
}

.filter-department,
.filter-asset-type {
    width: 220px;
    height: 36px;
    margin-right: 10px;
}

.add {
    width: 120px;
    height: 36px;
}

.excel,
.delete {
    background-color: #ffffff;
    border-radius: 3px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, .16);
    width: 36px;
    height: 36px;
    overflow: hidden;
}
</style>