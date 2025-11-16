<template>
    <div class="d-flex flex-direction-column">
        <!-- Main content -->
        <div class="main-content d-flex flex-direction-row mt-14 justify-content-space-between ">
            <!-- Header main content left -->
            <div class="d-flex flex-direction-row">
                <!-- Search -->
                <div class="search-content">
                    <ms-search icon="icon-default icon-lookup" placeholder="Tìm kiếm tài sản"
                        styleInput="border-none search-input" searchComponent="border-1" />
                </div>
                <!-- Filter asset type -->
                <div class="filter-asset-type">
                    <ms-select v-model="assetType" :options="assetTypeOptions">
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
                    <ms-select v-model="assetType" :options="assetTypeOptions">
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
                    @update:page="page = $event" @selection-change="selectedIds = $event" />
            </div>
        </div>
    </div>
    <AssetEditModal :is-open="isFormOpen" @close="isFormOpen = false" @cancel="isFormOpen = false" @save="handleSave" />
</template>
<script setup>
import MsSearch from '@/components/ms-search/MsSearch.vue';
import MsSelect from '@/components/ms-select/MsSelect.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsTable from '@/components/ms-table/MsTable.vue';
import AssetApi from '@/apis/components/AssetApi.js';
import AssetEditModal from '@/views/asset/AssetForm.vue';
import { ref, onMounted } from "vue";
const lsAssets = ref([]);
const assetTypeOptions = [
    { value: '0', label: 'Chọn giới tính', selected: 'true' },
    { value: '1', label: 'Nam' },
    { value: '2', label: 'Nữ' }
];

const search = ref("");
const department = ref("");
const assetType = ref("");
const page = ref(1);
const pageSize = ref(10);
const total = ref(200);
const isFormOpen = ref(false);

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

onMounted(async () => {
    lsAssets.value = await getAllAssets();
});

const columns = ref([
    { key: "stt", label: "STT", align: "left" },
    { key: "assetCode", label: "Mã tài sản" },
    { key: "assetName", label: "Tên tài sản" },
    { key: "assetTypeName", label: "Loại tài sản" },
    { key: "departmentName", label: "Bộ phận sử dụng" },
    { key: "quantity", label: "Số lượng", align: "right" },
    { key: "originalPrice", label: "Nguyên giá", align: "right" },
    { key: "depreciationValueYear", label: "HM/KM lũy kế", align: "right" },
    { key: "remaining", label: "Giá trị còn lại", align: "right" },
]);

const selectedIds = ref([]);
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