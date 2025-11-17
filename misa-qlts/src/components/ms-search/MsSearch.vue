<template>
    <div :class="'d-flex align-items-center relative border-radius-4 ' + searchComponent">
        <div class="search relative d-flex align-items-center">
            <!-- Icon -->
            <div class="search-icon">
                <i :class="icon"></i>
            </div>
            <!-- Input -->
            <input :type="type" :placeholder="placeholder" :class="['search-input', styleInput]" @input="inputSearch"
                :value="props.modelValue" />
        </div>
    </div>
</template>

<script setup>
//#region Emits
const emit = defineEmits(['update:modelValue']);
//#endregion


//#region Props
const props = defineProps({
    icon: { type: String, default: 'icon-default icon-lookup' },
    placeholder: { type: String, default: 'Tìm kiếm...' },
    styleInput: { type: String, default: '' },
    searchComponent: { type: String, default: '' },
    type: { type: String, default: 'text' },
    modelValue: { type: String, default: '' },
});
//#endregion

function inputSearch(e) {
    emit('update:modelValue', e.target.value);
}
</script>
<style scoped>
@import '../../assets/components/ms-search/MsSearch.css';
</style>