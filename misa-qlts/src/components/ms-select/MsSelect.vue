<template>
    <div class="dropdown-container">
        <!-- Label -->
        <label v-if="label" class="dropdown-label-text">
            {{ label }}
            <span v-if="required" class="required">*</span>
        </label>

        <!-- Dropdown -->
        <div class="dropdown-wrapper" tabindex="0" @keydown="onKeydown" @click="toggleDropdown" ref="dropdownRef"
            :class="{ disabled: disabled }" :style="{ width: typeof width === 'number' ? width + 'px' : width }">
            <span class="prefix-icon">
                <slot name="icon"></slot>
            </span>

            <span class="dropdown-label">
                {{ selectedLabel || placeholder }}
            </span>

            <span class="suffix-icon">
                <slot name="suffix"></slot>
            </span>

            <div v-if="open" class="dropdown-list">
                <div class="dropdown-item" v-for="option in options" :key="option.value"
                    @click.stop="selectItem(option)" :class="{ selected: option.value === modelValue }">
                    <span class="check-icon" v-if="option.value === modelValue">✔</span>
                    <span class="option-label">{{ option.label }}</span>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from "vue";

//#region Props
const props = defineProps({
    modelValue: [String, Number],
    options: Array,
    placeholder: { type: String, default: "Chọn giá trị" },
    label: { type: String, default: null },
    required: { type: Boolean, default: null },
    disabled: { type: Boolean, default: false },
    width: { type: [String, Number], default: '100%' }
});
//#endregion

//#region Emits
const emit = defineEmits(["update:modelValue"]);
//#endregion
const open = ref(false);
const dropdownRef = ref(null);

const selectedLabel = computed(() => {
    const item = props.options.find(o => o.value === props.modelValue);
    return item ? item.label : "";
});

/**
 * Hàm mở dropdown
 * CreatedBy: QuanPA - 18/11/2025
 */
function toggleDropdown() {
    if (props.disabled) return;
    open.value = !open.value;
}

/**
 * Hàm lựa chọn option
 * @param option 
 * CreatedBy: QuanPA - 18/11/2025
 */
function selectItem(option) {
    emit("update:modelValue", option.value);
    open.value = false;
}

/**
 * Hàm khi click ngoài
 * @param e 
 * CreatedBy: QuanPA - 18/11/2025
 */
function clickOutside(e) {
    if (!dropdownRef.value.contains(e.target)) open.value = false;
}

/**
 * Hàm chọn value khi dùng bàn phím
 * @param e 
 * CreatedBy: QuanPA - 18/11/2025
 */
function onKeydown(e) {
    if (props.disabled) return;

    switch (e.key) {
        case "Enter":
        case " ":
            e.preventDefault();
            open.value = !open.value;
            break;

        case "ArrowDown":
            if (!open.value) open.value = true;
            navigate(1);
            break;

        case "ArrowUp":
            if (!open.value) open.value = true;
            navigate(-1);
            break;

        case "Escape":
            open.value = false;
            break;
    }
}

/**
 * Hàm di chuyển lựa chọn
 * @param dir 
 * CreatedBy: QuanPA - 18/11/2025
 */
function navigate(dir) {
    const list = props.options;
    const currentIndex = list.findIndex(o => o.value === props.modelValue);
    let next = currentIndex + dir;

    if (next < 0) next = list.length - 1;
    if (next >= list.length) next = 0;

    emit("update:modelValue", list[next].value);
}

onMounted(() => document.addEventListener("click", clickOutside));
onBeforeUnmount(() => document.removeEventListener("click", clickOutside));
</script>
<style scoped>
@import '../../assets/components/ms-select/MsSelect.css';
</style>
