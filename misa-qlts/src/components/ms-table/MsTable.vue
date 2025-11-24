<template>
    <div class="table-wrapper d-flex flex-direction-column" tabindex="0" @keydown="onKey" ref="tableWrapperRef">
        <table class="m-table fixed-table">
            <thead class="d-flex">
                <tr>
                    <th class="col-check">
                        <input type="checkbox" class="square-checkbox" :checked="isAllSelected"
                            @change="toggleSelectAll" />
                    </th>

                    <th v-for="(col, i) in visibleCols" :key="col.key" :ref="el => { if (el) columnHeaders[i] = el }"
                        :style="{ width: getColumnWidth(col, i) + 'px', textAlign: col.align || 'left' }"
                        class="col-header">
                        {{ col.label }}

                        <!-- thanh resize -->
                        <div class="resize-handle" @mousedown="startResize($event, i)">
                        </div>
                    </th>
                    <th class="text-align-right fuction-head pr-20">
                        Chức năng
                    </th>
                </tr>
            </thead>

            <tbody>
                <tr v-for="(row, rindex) in rows" :key="row[props.rowKey]"
                    :class="{ selected: isSelected(row[props.rowKey]), focused: focusIndex === rindex }"
                    @click="onRowClick(row, rindex, $event)"
                    @contextmenu.prevent="onRowContextMenu(row, rindex, $event)">

                    <td class="row-check">
                        <input type="checkbox" class="square-checkbox" :checked="isSelected(row[props.rowKey])"
                            @click.stop @change="onCheckboxClick(row[props.rowKey], rindex, $event)" />
                    </td>

                    <td v-for="(col, cindex) in visibleCols" :key="col.key"
                        :style="{ width: getColumnWidth(col, cindex) + 'px', textAlign: col.align || 'left' }">
                        <span v-if="col.key === 'stt'">
                            {{ (page - 1) * pageSize + rindex + 1 }}
                        </span>
                        <span v-else>
                            {{ row[col.key] }}
                        </span>
                    </td>
                    <td class="fuction d-flex align-items-center justify-content-space-evently">
                        <div class="icon-default icon-edit pointer" @click="openEditForm(row)"></div>
                        <div class="icon-default icon-copy pointer" @click="openAddForm(row)"></div>
                    </td>
                </tr>
            </tbody>
        </table>

        <tfoot>
            <tr class="footer-row">

                <!-- Cột 1: Tổng số bản ghi -->
                <td class="footer-record-count">
                    Tổng số: <b>{{ total }}</b> bản ghi
                </td>

                <!-- Cột 2: PageSize -->
                <td class="footer-page-size">
                    <select v-model="localPageSize" class="page-select">
                        <option :value="20">20</option>
                        <option :value="50">50</option>
                        <option :value="100">100</option>
                    </select>
                </td>

                <!-- Cột 3: Phân trang -->
                <td class="footer-pagination">
                    <div class="page-control">
                        <div @click="go(-1)" :disabled="page <= 1" class="page-nav-icon">&lt;</div>

                        <div v-for="p in visiblePages" :key="p" @click="changePage(p)" class="page-number-link"
                            :class="{ 'active-page': p === page, 'page-dots': p === '...' }"
                            :style="{ cursor: p === '...' ? 'default' : 'pointer' }">
                            {{ p }}
                        </div>

                        <div @click="go(1)" :disabled="page >= maxPage" class="page-nav-icon">&gt;</div>
                    </div>
                </td>

                <!-- Các cột không có totals (cột 4 → cột 6) -->
                <td v-for="n in 3" :key="'empty' + n" :style="{ width: (348 / 3) + 'px' }"></td>

                <td v-for="(value, index) in totals" :key="'total' + index" class="footer-total-cell"
                    :style="{ textAlign: 'right', width: totalWidths[index] }">
                    <b> {{ formatNumber(value) }}</b>
                </td>
                <td :style="{ width: '94.4px' }"></td>
            </tr>
        </tfoot>

    </div>
</template>

<script setup>
import { ref, watch, computed, onMounted, onUnmounted, nextTick } from "vue";

const columnHeaders = ref([]);
const columnOffsets = ref([0, 0, 0, 0]);
const selected = ref([]);
const lastSelectedIndex = ref(null);
const focusIndex = ref(0);
const totalWidths = ["80px", "130px", "130px", "130px"];
const tableWrapperRef = ref(null);
const tableWidth = ref(0);

//#region Props
const props = defineProps({
    columns: Array,
    rows: {
        type: Array,
        default: () => []
    },
    page: Number,
    pageSize: Number,
    total: Number,
    totals: {
        type: Array,
        default: () => [0, 0, 0, 0]
    },
    rowKey: {
        type: String,
        default: "id"
    },
    selectedKeys: {
        type: Array,
        default: () => []
    },
    baseTableWidth: {
        type: Number,
        default: null // Nếu có baseTableWidth, sẽ scale width theo tỷ lệ
    }
});
//#endregion
const localCols = ref(JSON.parse(JSON.stringify(props.columns)));

//#region Emits
const emit = defineEmits(["update:page", "update:pageSize", "selection-change", "edit-row", "open-add-form", "row-contextmenu"]);
//#endregion

/**
 * Hàm mở form edit
 * @param row 
 * CreatedBy: QuanPA - 18/11/2025
 */
const openEditForm = (row) => {
    emit("edit-row", row);
};

/**
 * Hàm mở form thêm (copy từ row)
 * CreatedBy: QuanPA - 18/11/2025
 */
const openAddForm = (row) => {
    emit("open-add-form", row);
};

/**
 * Hàm tính số trang
 * CreatedBy: QuanPA - 18/11/2025
 */
const visiblePages = computed(() => {
    const pages = [];
    const total = maxPage.value;
    const current = props.page;

    // Ít trang → show hết
    if (total <= 7) {
        for (let i = 1; i <= total; i++) pages.push(i);
        return pages;
    }

    // Trang đầu
    if (current <= 3) {
        pages.push(1, 2, 3, 4, "...", total);
        return pages;
    }

    // Trang cuối
    if (current >= total - 2) {
        pages.push(1, "...", total - 3, total - 2, total - 1, total);
        return pages;
    }

    // Trang ở giữa
    pages.push(1, "...", current - 1, current, current + 1, "...", total);
    return pages;
});

/// resize state
let resizing = false;
let colIndex = null;
let startX = 0;
let startWidth = 0;
const visibleCols = computed(() => localCols.value.filter(col => !col.hidden));

/**
 * Tính width của cột dựa trên table width hiện tại
 * Nếu có baseTableWidth và baseWidth, sẽ scale theo tỷ lệ
 * Nếu không, sử dụng width cố định
 * @param {Object} col - Column object
 * @param {Number} index - Column index
 * @returns {Number} - Width tính bằng pixel
 * CreatedBy: QuanPA - 22/11/2025
 */
const getColumnWidth = (col, index) => {
    const localCol = localCols.value[index] || col;

    // Nếu đã được resize, dùng width đã resize (không scale lại)
    if (localCol.isResized && localCol.width !== undefined) {
        return typeof localCol.width === 'number' ? localCol.width : parseFloat(localCol.width) || 100;
    }

    // Lấy baseWidth hoặc width để tính toán
    const baseWidth = col.baseWidth !== undefined ? col.baseWidth : (typeof col.width === 'number' ? col.width : parseFloat(col.width));

    // Nếu có baseWidth/baseTableWidth, scale theo tỷ lệ table width
    if (props.baseTableWidth && baseWidth !== undefined && !isNaN(baseWidth) && tableWidth.value > 0) {
        const ratio = tableWidth.value / props.baseTableWidth;
        return Math.round(baseWidth * ratio);
    }

    // Nếu width là string phần trăm
    if (typeof col.width === 'string' && col.width.endsWith('%')) {
        if (tableWidth.value > 0) {
            return (tableWidth.value * parseFloat(col.width)) / 100;
        }
        return 100;
    }

    // Trả về width cố định
    if (typeof baseWidth === 'number' && !isNaN(baseWidth)) {
        return baseWidth;
    }

    return 100; // default width
};
/**
 * Bắt đầu resize một cột
 * CreatedBy: QuanPA - 18/11/2025
 */
const startResize = (e, index) => {
    e.preventDefault(); // tránh chọn text
    e.stopPropagation(); // tránh row click

    resizing = true;
    colIndex = index;
    startX = e.clientX;

    // Lấy width hiện tại từ columnHeaders DOM element để chính xác
    const colEl = columnHeaders.value[index];
    startWidth = colEl ? colEl.offsetWidth : localCols.value[index].width || 100;

    document.addEventListener("mousemove", resize);
    document.addEventListener("mouseup", stopResize);
};

/**
 * Xử lý khi đang kéo resize
 * CreatedBy: QuanPA - 18/11/2025
 */
const resize = (e) => {
    if (!resizing) return;

    const dx = e.clientX - startX;
    // Giữ width >= 60px
    const newWidth = Math.max(60, startWidth + dx);
    localCols.value[colIndex].width = newWidth;
    // Đánh dấu cột đã được resize để không scale lại
    localCols.value[colIndex].isResized = true;

    // Cập nhật offsets cho totals alignment
    calculateColumnOffsets();
};


/**
 * Kết thúc hành động resize
 * CreatedBy: QuanPA - 18/11/2025
 */
const stopResize = () => {
    resizing = false;
    document.removeEventListener("mousemove", resize);
    document.removeEventListener("mouseup", stopResize);
};

/**
 * Kiểm tra một row có được chọn hay không
 * CreatedBy: QuanPA - 18/11/2025
 */
const isSelected = (id) => selected.value.includes(id);

/**
 * So sánh hai mảng selection
 * CreatedBy: QuanPA - 18/11/2025
 */
const arraysEqual = (a, b) => {
    if (a.length !== b.length) return false;
    return a.every((item, index) => item === b[index]);
};

/**
 * Cập nhật danh sách selection và emit event
 * CreatedBy: QuanPA - 18/11/2025
 */
const updateSelection = (newSelection, shouldEmit = true) => {
    const unique = Array.from(new Set(newSelection));
    if (arraysEqual(unique, selected.value)) {
        return;
    }
    selected.value = unique;
    if (shouldEmit) {
        emit("selection-change", [...selected.value]);
    }
};

/**
 * Lấy ID của một row
 * CreatedBy: QuanPA - 18/11/2025
 */
const getRowId = (row) => row[props.rowKey];

/**
 * Lọc selection để đảm bảo chỉ lấy những ID còn tồn tại trong rows
 * CreatedBy: QuanPA - 18/11/2025
 */
const getValidSelection = (selection) => {
    if (!Array.isArray(selection)) {
        return [];
    }
    if (!Array.isArray(props.rows)) {
        return [];
    }
    const validIds = new Set(props.rows.map(row => getRowId(row)));
    return selection.filter(id => validIds.has(id));
};

/**
 * Tạo selection theo range (Shift + click)
 * CreatedBy: QuanPA - 18/11/2025
 */
const buildRangeSelection = (startIndex, endIndex) => {
    if (!Array.isArray(props.rows) || props.rows.length === 0) return [];
    const start = Math.min(startIndex, endIndex);
    const end = Math.max(startIndex, endIndex);
    return props.rows.slice(start, end + 1).map(row => getRowId(row));
};

/**
 * Xử lý khi click vào row
 * CreatedBy: QuanPA - 18/11/2025
 */
const onRowClick = (row, index, event) => {
    const rowId = getRowId(row);
    if (event.shiftKey && lastSelectedIndex.value !== null) {
        const rangeSelection = buildRangeSelection(lastSelectedIndex.value, index);
        updateSelection(rangeSelection);
    } else if (event.ctrlKey || event.metaKey) {
        if (isSelected(rowId)) {
            updateSelection(selected.value.filter(item => item !== rowId));
        } else {
            updateSelection([...selected.value, rowId]);
        }
        lastSelectedIndex.value = index;
    } else {
        updateSelection([rowId]);
        lastSelectedIndex.value = index;
    }
    focusIndex.value = index;
};

/**
 * Xử lý click chuột phải lên row
 * CreatedBy: QuanPA - 18/11/2025
 */
const onRowContextMenu = (row, index, event) => {
    const rowId = getRowId(row);
    if (!isSelected(rowId)) {
        updateSelection([rowId]);
    }
    focusIndex.value = index;
    emit("row-contextmenu", {
        row,
        index,
        clientX: event.clientX,
        clientY: event.clientY
    });
};

/**
 * Hàm kiểm tra xem có phải đang select-all hay không
 * CreatedBy: QuanPA - 18/11/2025
 */
const isAllSelected = computed(() => {
    return selected.value.length === props.rows.length && props.rows.length > 0;
});

/**
 * Hàm chọn tất cả
 * @param e 
 * CreatedBy: QuanPA - 17/11/2025
 */
const toggleSelectAll = (e) => {
    if (e.target.checked) {
        updateSelection(props.rows.map(r => getRowId(r)));
    } else {
        updateSelection([]);
    }
};

/**
 * Hàm checked row
 * @param id 
 * @param e 
 * CreatedBy: QuanPA - 17/11/2025
 */
const onCheckboxClick = (id, index, e) => {
    if (e.shiftKey && lastSelectedIndex.value !== null) {
        const rangeSelection = buildRangeSelection(lastSelectedIndex.value, index);
        updateSelection(rangeSelection);
        lastSelectedIndex.value = index;
        return;
    }

    if (e.target.checked) {
        updateSelection([...selected.value, id]);
    } else {
        updateSelection(selected.value.filter(item => item !== id));
    }
    lastSelectedIndex.value = index;
};

/**
 * Hàm di chuyển bằng bàn phím
 * @param e 
 * CreatedBy: QuanPA - 17/11/2025
 */
const onKey = (e) => {
    if (!Array.isArray(props.rows) || props.rows.length === 0) {
        return;
    }
    if (e.key === "ArrowDown") {
        focusIndex.value = Math.min(focusIndex.value + 1, props.rows.length - 1);
        updateSelection([getRowId(props.rows[focusIndex.value])]);
    } else if (e.key === "ArrowUp") {
        focusIndex.value = Math.max(focusIndex.value - 1, 0);
        updateSelection([getRowId(props.rows[focusIndex.value])]);
    }
};



/// pagination
const localPageSize = ref(props.pageSize);
const maxPage = computed(() =>
    Math.ceil(props.total / localPageSize.value)
);
watch(localPageSize, (newSize) => {
    emit("update:pageSize", newSize);
    emit("update:page", 1);
});

/**
 * Hàm chuyển trang
 * CreatedBy: QuanPA - 18/11/2025
 */
const go = (step) => {
    const newPage = props.page + step;
    if (newPage >= 1 && newPage <= maxPage.value) {
        emit("update:page", newPage);
    }
};

/**
 * Hàm chuyển đến trang cụ thể
 * @param {Number|String} pageNumber - Số trang muốn chuyển đến
 * CreatedBy: AI Assistant - 22/11/2025
 */
const changePage = (pageNumber) => {
    // Không làm gì nếu click vào dấu "..."
    if (pageNumber === '...' || typeof pageNumber !== 'number') {
        return;
    }

    // Kiểm tra pageNumber hợp lệ
    if (pageNumber >= 1 && pageNumber <= maxPage.value && pageNumber !== props.page) {
        emit("update:page", pageNumber);
    }
};

/**
 * Hàm tính lại vị trí cột để align totals
 * CreatedBy: QuanPA - 18/11/2025
 */
const calculateColumnOffsets = () => {
    nextTick(() => {
        const targetKeys = ['assetCode', 'assetName', 'assetTypeName', 'departmentName'];
        const targetIndices = targetKeys.map(key =>
            localCols.value.findIndex(col => col.key === key)
        ).filter(idx => idx !== -1);

        if (targetIndices.length === 0) return;

        const offsets = [];
        const tableWrapper = document.querySelector('.table-wrapper');
        if (!tableWrapper) return;

        // Get the table header row
        const headerRow = tableWrapper.querySelector('thead tr');
        if (!headerRow) return;

        // Get checkbox column width
        const checkboxCol = headerRow.querySelector('.col-check');
        const checkboxWidth = checkboxCol ? checkboxCol.offsetWidth : 40;

        targetIndices.forEach((colIndex) => {
            // Calculate cumulative width from start to this column
            let cumulativeWidth = checkboxWidth;

            // Add widths of all columns before this target column
            for (let i = 0; i < colIndex; i++) {
                const col = localCols.value[i];
                const colElement = columnHeaders.value[i];
                if (colElement) {
                    cumulativeWidth += colElement.offsetWidth;
                } else {
                    cumulativeWidth += col.width || 100;
                }
            }

            // Get the target column element
            const colElement = columnHeaders.value[colIndex];
            if (colElement) {
                const colWidth = colElement.offsetWidth;
                const computedStyle = window.getComputedStyle(colElement);
                const paddingLeft = parseFloat(computedStyle.paddingLeft) || 0;
                const paddingRight = parseFloat(computedStyle.paddingRight) || 0;

                // For right-aligned columns, position at the right edge
                const align = localCols.value[colIndex].align || 'left';
                if (align === 'right') {
                    // Position at right edge of column
                    offsets.push(cumulativeWidth + colWidth - paddingRight);
                } else {
                    // Position at left edge with padding
                    offsets.push(cumulativeWidth + paddingLeft);
                }
            } else {
                offsets.push(cumulativeWidth);
            }
        });

        columnOffsets.value = offsets;
    });
};

/**
 * Format số theo locale VN
 * CreatedBy: QuanPA - 18/11/2025
 */
const formatNumber = (num) => {
    if (typeof num === 'number') {
        return num.toLocaleString('vi-VN');
    }
    return num || '0';
};

// Watch for column width changes
//#region Watch
watch(() => props.columns, (newColumns) => {
    // Cập nhật localCols khi props.columns thay đổi, nhưng giữ lại width đã resize
    const newLocalCols = JSON.parse(JSON.stringify(newColumns));
    newLocalCols.forEach((newCol, index) => {
        const oldCol = localCols.value[index];
        if (oldCol && oldCol.isResized && oldCol.width !== undefined) {
            // Giữ lại width đã resize
            newCol.width = oldCol.width;
            newCol.isResized = true;
        }
        // Đảm bảo baseWidth được copy
        if (newColumns[index] && newColumns[index].baseWidth !== undefined) {
            newCol.baseWidth = newColumns[index].baseWidth;
        }
    });
    localCols.value = newLocalCols;
}, { deep: true });

watch(() => localCols.value, () => {
    calculateColumnOffsets();
}, { deep: true });

watch(() => props.rows, () => {
    updateSelection(getValidSelection(selected.value), false);
}, { deep: true });

watch(() => props.selectedKeys, (newKeys) => {
    updateSelection(getValidSelection(newKeys), false);
}, { immediate: true, deep: true });

watch(() => tableWidth.value, () => {
    // Khi table width thay đổi, cập nhật lại width nếu chưa resize
    nextTick(() => {
        calculateColumnOffsets();
    });
});
//#endregion

/**
 * Hàm cập nhật table width
 * CreatedBy: AI Assistant - 22/11/2025
 */
const updateTableWidth = () => {
    nextTick(() => {
        if (tableWrapperRef.value) {
            tableWidth.value = tableWrapperRef.value.offsetWidth;
        }
    });
};

/**
 * Hàm xử lý khi window được resize
 * CreatedBy: QuanPA - 18/11/2025
 */
const handleResize = () => {
    updateTableWidth();
    calculateColumnOffsets();
};

onMounted(() => {
    updateTableWidth();
    calculateColumnOffsets();
    window.addEventListener('resize', handleResize);

    // Watch cho table wrapper để update width khi có thay đổi
    if (tableWrapperRef.value) {
        const resizeObserver = new ResizeObserver(() => {
            updateTableWidth();
        });
        resizeObserver.observe(tableWrapperRef.value);
    }
});

onUnmounted(() => {
    window.removeEventListener('resize', handleResize);
});
</script>

<style scoped>
@import '../../assets/components/ms-table/MsTable.css';
</style>
