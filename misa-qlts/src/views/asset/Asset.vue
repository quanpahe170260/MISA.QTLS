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
                    <ms-button type="primary" icon="icon-mask icon-plus" positionIcon="left" @click="openAddForm(null)">
                        <p class="btn-text">Thêm tài sản</p>
                    </ms-button>
                </div>
                <div class="excel d-flex justify-content-center align-items-center mr-10 pointer">
                    <div class="icon-default icon-excel"></div>
                </div>
                <div class="delete d-flex justify-content-center align-items-center pointer" @click="onDeleteClick">
                    <div class="icon-default icon-delete"></div>
                </div>
            </div>
        </div>
        <div class="flex1">
            <div style="padding: 20px">
                <ms-table :columns="columns" :rows="lsAssets" :page="page" :page-size="pageSize" :total="total"
                    :selected-keys="selectedIds" @update:page="page = $event" @update:pageSize="pageSize = $event"
                    @selection-change="handleSelectionChange" rowKey="assetId" @edit-row="openEditForm"
                    @open-add-form="openAddForm" @row-contextmenu="handleRowContextMenu" />
            </div>
        </div>
    </div>
    <div v-if="isContextMenuVisible" class="context-menu" :style="{
        top: contextMenuPosition.y + 'px',
        left: contextMenuPosition.x + 'px'
    }" @click.stop>
        <div class="context-menu-item" @click="handleContextMenuAction('add')">Thêm</div>
        <div class="context-menu-item" @click="handleContextMenuAction('edit')">Sửa</div>
        <div class="context-menu-item" @click="handleContextMenuAction('delete')">Xóa</div>
        <div class="context-menu-item" @click="handleContextMenuAction('duplicate')">Nhân bản</div>
    </div>
    <asset-form :is-open="isFormOpen" :mode="formMode" :data="selectedRow" @close="isFormOpen = false"
        @cancel="isFormOpen = false" @save="handleSave" />
    <ms-modal-confirm :is-open="showConfirm" message="Bạn có muốn xóa">
        <ms-button type="secondary" @click="handleCancel" buttonComponentStyle="btn-modal border-1">Không</ms-button>
        <ms-button type="primary" @click="confirmSave" buttonComponentStyle="btn-modal">
            <p class="btn-text">Xóa</p>
        </ms-button>
    </ms-modal-confirm>
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
import { ref, onMounted, onUnmounted, watch } from "vue";
import MsModalConfirm from '@/components/ms-modal/MsModalConfirm.vue';
import { openToast } from "@/utils/showToast.js";
import { formatNumber } from '@/utils/formatNumber';
const lsAssets = ref([]);
const search = ref("");
const department = ref("");
const assetType = ref("");
const page = ref(1);
const pageSize = ref(20);
const total = ref(200);
const isFormOpen = ref(false);
const selectedIds = ref([]);
const lsAssetType = ref([]);
const lsDepartment = ref([]);
const selectedRow = ref(null);
const formMode = ref("add");
const showConfirm = ref(false);
const isContextMenuVisible = ref(false);
const contextMenuRow = ref(null);
const contextMenuPosition = ref({ x: 0, y: 0 });
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
        const items = response.data.data.data.map(item => ({
            ...item,
            quantity: formatNumber(item.quantity),
            originalPrice: formatNumber(item.originalPrice),
            depreciationValueYear: formatNumber(item.depreciationValueYear),
            remaining: item.originalPrice - item.depreciationValueYear
        }));
        return items;
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
            label: item.departmentName
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
            label: item.assetTypeName,
            yearOfUse: item.yearOfUse,
            wareRate: item.wearRate
        }));
    }
    catch (error) {
        console.log("Lỗi lấy danh sách loại tài sản", error);
    }
}

onMounted(async () => {
    const [assets, assetTypes, departments] = await Promise.all([
        getAllAssets(),
        getAllAssetType(),
        getAllDepartment()
    ]);

    lsAssets.value = assets;
    lsAssetType.value = assetTypes;
    lsDepartment.value = departments;
});

onMounted(() => {
    document.addEventListener("click", hideContextMenu);
    document.addEventListener("scroll", hideContextMenu, true);
    window.addEventListener("resize", hideContextMenu);
    window.addEventListener("keydown", handleContextMenuKeydown);
});

onUnmounted(() => {
    document.removeEventListener("click", hideContextMenu);
    document.removeEventListener("scroll", hideContextMenu, true);
    window.removeEventListener("resize", hideContextMenu);
    window.removeEventListener("keydown", handleContextMenuKeydown);
});


/**
 * Mở modal xác nhận xóa
 * CreatedBy: QuanPA - 19/11/2025
 */
function onDeleteClick() {
    if (selectedIds.value.length === 0) {
        openToast("error", "Thất bại", "Chưa chọn tài sản nào");
        return;
    }
    showConfirm.value = true;
}

/**
 * Hàm hủy bỏ hành động
 * CreatedBy: QuanPA - 19/11/2025
 */
function handleCancel() {
    showConfirm.value = false;
}

/**
 * Hàm xác nhận hành động
 * CreatedBy: QuanPA - 19/11/2025
 */
async function confirmSave() {
    if (selectedIds.value.length === 0) {
        openToast("error", "Thất bại", "Chưa có tài sản nào để xóa");
        showConfirm.value = false;
        return;
    }

    try {
        const response = await AssetApi.deleteMultiple(selectedIds.value);
        openToast("success", "Thành công", "Xóa tài sản thành công");
        console.log('response', response);
        showConfirm.value = false;
        await loadData();
        selectedIds.value = [];
    } catch (error) {
        console.error("Lỗi khi xóa:", error);
        openToast("error", "Thất bại", error?.response?.data?.message || "Có lỗi xảy ra");
    }
}

/**
 * Hàm cập nhật danh sách tài sản được chọn
 * @param {Array} ids
 * CreatedBy: QuanPA - 20/11/2025
 */
function handleSelectionChange(ids) {
    selectedIds.value = ids;
}
const columns = ref([
    { key: "stt", label: "STT", align: "center", width: '50' },
    { key: "assetId", label: "", align: "left", hidden: true },
    { key: "assetCode", label: "Mã tài sản", width: '120' },
    { key: "assetName", label: "Tên tài sản", width: '170' },
    { key: "assetTypeName", label: "Loại tài sản", width: '170' },
    { key: "departmentName", label: "Bộ phận sử dụng", width: '150' },
    { key: "quantity", label: "Số lượng", align: "right", width: '80' },
    { key: "originalPrice", label: "Nguyên giá", align: "right", width: '130' },
    { key: "depreciationValueYear", label: "HM/KM lũy kế", align: "right", width: '130' },
    { key: "remaining", label: "Giá trị còn lại", align: "right", width: '130' },
]);

/**
 * Hàm dùng để load dữ liệu
 * CreatedBy: QuanPA - 17/11/2025
 */
async function loadData() {
    lsAssets.value = await getAllAssets();
}

/**
 * Hàm mở form add dữ liệu
 * CreatedBy: QuanPA - 17/11/2025
 */
function openAddForm(row) {
    formMode.value = row ? "copy" : "add";
    selectedRow.value = row ? row : null;
    isFormOpen.value = true;
}

/**
 * Hàm mở form edit dữ liệu
 * CreatedBy: QuanPA - 17/11/2025
 */
function openEditForm(row) {
    formMode.value = "edit";
    selectedRow.value = row;
    isFormOpen.value = true;
}

function handleContextMenuKeydown(event) {
    if (event.key === "Escape") {
        hideContextMenu();
    }
}

function hideContextMenu() {
    if (!isContextMenuVisible.value) return;
    isContextMenuVisible.value = false;
    contextMenuRow.value = null;
}

function setContextMenuPosition(x, y) {
    const MENU_WIDTH = 180;
    const MENU_HEIGHT = 160;
    const PADDING = 8;
    const maxX = window.innerWidth - MENU_WIDTH - PADDING;
    const maxY = window.innerHeight - MENU_HEIGHT - PADDING;
    contextMenuPosition.value = {
        x: Math.max(0, Math.min(x, maxX)),
        y: Math.max(0, Math.min(y, maxY)),
    };
}

function handleRowContextMenu({ row, clientX, clientY }) {
    if (!row) {
        return;
    }
    selectedRow.value = row;
    selectedIds.value = [row.assetId];
    contextMenuRow.value = row;
    setContextMenuPosition(clientX, clientY);
    isContextMenuVisible.value = true;
}

function handleContextMenuAction(action) {
    switch (action) {
        case "add":
            openAddForm(null);
            break;
        case "edit":
            if (contextMenuRow.value) {
                openEditForm(contextMenuRow.value);
            }
            break;
        case "delete":
            if (contextMenuRow.value) {
                selectedIds.value = [contextMenuRow.value.assetId];
                showConfirm.value = true;
            }
            break;
        case "duplicate":
            if (contextMenuRow.value) {
                openAddForm(contextMenuRow.value);
            }
            break;
        default:
            break;
    }
    hideContextMenu();
}

//#region Watch
watch([search, department, assetType, page, pageSize], () => {
    loadData();
});
//#endregion
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

.context-menu {
    position: fixed;
    min-width: 180px;
    background-color: #ffffff;
    border: 1px solid #d6d6d6;
    border-radius: 4px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    z-index: 9999;
    padding: 4px 0;
}

.context-menu-item {
    padding: 8px 16px;
    cursor: pointer;
    font-size: 13px;
    color: #1f1f1f;
}

.context-menu-item:hover {
    background-color: #f2f2f2;
}
</style>