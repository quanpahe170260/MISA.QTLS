<template>
    <div class="dropdown-wrapper" @click="toggleDropdown" ref="dropdownRef">
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
            <div class="dropdown-item" v-for="option in options" :key="option.value" @click.stop="selectItem(option)"
                :class="{ selected: option.value === modelValue }">
                <span class="check-icon" v-if="option.value === modelValue">✔</span>
                {{ option.label }}
            </div>
        </div>
    </div>
</template>
<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from "vue";

const props = defineProps({
    modelValue: [String, Number],
    options: Array,
    placeholder: { type: String, default: "Chọn giá trị" }
});
const emit = defineEmits(["update:modelValue"]);

const open = ref(false);
const dropdownRef = ref(null);

const selectedLabel = computed(() => {
    const item = props.options.find(o => o.value === props.modelValue);
    return item ? item.label : "";
});

function toggleDropdown() {
    open.value = !open.value;
}

function selectItem(option) {
    emit("update:modelValue", option.value);
    open.value = false;
}

function clickOutside(e) {
    if (!dropdownRef.value.contains(e.target)) open.value = false;
}

onMounted(() => document.addEventListener("click", clickOutside));
onBeforeUnmount(() => document.removeEventListener("click", clickOutside));
</script>
<style scoped>
@import '../../assets/components/ms-select/MsSelect.css';
</style>
