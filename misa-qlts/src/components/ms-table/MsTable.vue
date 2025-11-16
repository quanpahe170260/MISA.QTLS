<template>
    <div class="table-wrapper" @keydown="onKey">
        <table class="m-table">
            <thead>
                <tr>
                    <th class="col-check">
                        <input type="checkbox" class="square-checkbox" v-model="selectAll" @change="toggleSelectAll" />
                    </th>

                    <th v-for="(col, i) in localCols" :key="col.key" :ref="el => { if (el) columnHeaders[i] = el }"
                        :style="{ width: col.width + 'px', textAlign: col.align || 'left' }" class="col-header">
                        {{ col.label }}

                        <!-- thanh resize -->
                        <div class="resize-handle" @mousedown="startResize($event, i)">
                        </div>
                    </th>
                    <th class="text-align-right fuction-head">
                        Chức năng
                    </th>
                </tr>
            </thead>

            <tbody>
                <tr v-for="(row, rindex) in rows" :key="row.id"
                    :class="{ selected: isSelected(row.id), focused: focusIndex === rindex }"
                    @click="onRowClick(row, rindex, $event)">

                    <td class="row-check"><input type="checkbox" class="square-checkbox"
                            :checked="isSelected(row.id)" /></td>

                    <td v-for="col in localCols" :key="col.key" :style="{ textAlign: col.align || 'left' }">
                        {{ row[col.key] }}
                    </td>
                    <td class="fuction d-flex align-items-center justify-content-space-evently">
                        <div class="icon-default icon-edit"></div>
                        <div class="icon-default icon-copy"></div>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- pagination -->
        <div class="pagination">
            <div class="d-flex align-items-center left-section">
                <span class="page-total">Tổng số: <b>200</b> bản ghi</span>

                <select v-model="localPageSize" class="page-select mr-20">
                    <option :value="20">20</option>
                    <option :value="50">50</option>
                    <option :value="100">100</option>
                </select>

                <div class="d-flex flex-direction-row justify-content-space-between align-items-center page-control">
                    <div @click="go(-1)" :disabled="page <= 1"
                        class="icon-mask icon-chevron-left-black page-nav-icon chevron-left">&lt;</div>
                    <div class="page-number-link">1</div>
                    <div class="page-number-link active-page">2</div>
                    <div class="page-number-link">3</div>
                    <div class="page-dots">...</div>
                    <div class="page-number-link">10</div>
                    <div @click="go(1)" :disabled="page >= maxPage"
                        class="icon-mask icon-chevron-right-black page-nav-icon chevron-right">&gt;</div>
                </div>
            </div>

            <div class="right-data-section" ref="rightDataSection">
                <span v-for="(total, index) in totals" :key="index" :style="getTotalStyle(index)" class="total-value">
                    {{ formatNumber(total) }}
                </span>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, watch, computed, onMounted, onUnmounted, nextTick } from "vue";
const props = defineProps({
    columns: Array,
    rows: Array,
    page: Number,
    pageSize: Number,
    total: Number,
    totals: {
        type: Array,
        default: () => [0, 0, 0, 0] // [qty, price, hm, remaining]
    }
});

const emit = defineEmits(["update:page", "selection-change"]);

const localCols = ref(JSON.parse(JSON.stringify(props.columns)));
const columnHeaders = ref({});
const rightDataSection = ref(null);
const columnOffsets = ref([0, 0, 0, 0]); // Offsets for 4 total columns

/// resize state
let resizing = false;
let colIndex = null;
let startX = 0;
let startWidth = 0;

const startResize = (e, index) => {
    resizing = true;
    colIndex = index;
    startX = e.clientX;
    startWidth = localCols.value[index].width;
    document.addEventListener("mousemove", resize);
    document.addEventListener("mouseup", stopResize);
};

const resize = (e) => {
    if (!resizing) return;
    const dx = e.clientX - startX;
    localCols.value[colIndex].width = Math.max(60, startWidth + dx);
    // Recalculate offsets after resize
    calculateColumnOffsets();
};

const stopResize = () => {
    resizing = false;
    document.removeEventListener("mousemove", resize);
    document.removeEventListener("mouseup", stopResize);
};

/// --- Selection logic ---
const selected = ref([]);
const focusIndex = ref(0);

const isSelected = (id) => selected.value.includes(id);

const onRowClick = (row, index, e) => {
    if (e.shiftKey) {
        const last = focusIndex.value;
        const [a, b] = [Math.min(last, index), Math.max(last, index)];
        selected.value = props.rows.slice(a, b + 1).map(r => r.id);
    } else if (e.ctrlKey || e.metaKey) {
        if (isSelected(row.id)) {
            selected.value = selected.value.filter(i => i !== row.id);
        } else {
            selected.value.push(row.id);
        }
    } else {
        selected.value = [row.id];
    }
    focusIndex.value = index;
    emit("selection-change", selected.value);
};

/// --- Keyboard navigation ---
const onKey = (e) => {
    if (e.key === "ArrowDown") {
        focusIndex.value = Math.min(focusIndex.value + 1, props.rows.length - 1);
        selected.value = [props.rows[focusIndex.value].id];
    }
    if (e.key === "ArrowUp") {
        focusIndex.value = Math.max(focusIndex.value - 1, 0);
        selected.value = [props.rows[focusIndex.value].id];
    }
    emit("selection-change", selected.value);
};

/// select all
const selectAll = ref(false);
watch(selectAll, (v) => {
    selected.value = v ? props.rows.map(r => r.id) : [];
    emit("selection-change", selected.value);
});

/// pagination
const localPageSize = ref(props.pageSize);
const maxPage = computed(() =>
    Math.ceil(props.total / localPageSize.value)
);

const go = (step) => {
    const newPage = props.page + step;
    if (newPage >= 1 && newPage <= maxPage.value) {
        emit("update:page", newPage);
    }
};

/// Calculate column positions for totals alignment
const calculateColumnOffsets = () => {
    nextTick(() => {
        // Find indices of columns: qty, price, hm, remaining
        const targetKeys = ['qty', 'price', 'hm', 'remaining'];
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

const getTotalStyle = (index) => {
    if (columnOffsets.value.length === 0) {
        return { marginLeft: index === 0 ? '0' : '30px' };
    }

    const rightSection = rightDataSection.value;
    const tableWrapper = document.querySelector('.table-wrapper');

    if (!rightSection || !tableWrapper) {
        return { marginLeft: index === 0 ? '0' : '30px' };
    }

    const wrapperRect = tableWrapper.getBoundingClientRect();
    const sectionRect = rightSection.getBoundingClientRect();

    // Calculate the left position of right-data-section relative to table wrapper
    const sectionLeftRelative = sectionRect.left - wrapperRect.left;

    // Get the target column position
    const targetOffset = columnOffsets.value[index] || 0;

    // Calculate margin-left to align with column
    const marginLeft = targetOffset - sectionLeftRelative;

    return {
        marginLeft: index === 0 ? `${Math.max(0, marginLeft)}px` : `${Math.max(0, marginLeft - (columnOffsets.value[index - 1] || 0) + sectionLeftRelative)}px`
    };
};

const formatNumber = (num) => {
    if (typeof num === 'number') {
        return num.toLocaleString('vi-VN');
    }
    return num || '0';
};

// Watch for column width changes
watch(() => localCols.value, () => {
    calculateColumnOffsets();
}, { deep: true });

// Recalculate on window resize
const handleResize = () => {
    calculateColumnOffsets();
};

onMounted(() => {
    calculateColumnOffsets();
    window.addEventListener('resize', handleResize);
});

onUnmounted(() => {
    window.removeEventListener('resize', handleResize);
});
</script>

<style scoped>
@import '../../assets/components/ms-table/MsTable.css';
</style>
