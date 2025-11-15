<template>
    <div :class="'d-flex align-items-center relative border-radius-4 ' + searchComponent">
        <div class="search relative d-flex align-items-center">
            <!-- Icon -->
            <div class="search-icon">
                <i :class="icon"></i>
            </div>
            <!-- Input -->
            <input v-model="keySearch" :type="type" :placeholder="placeholder" :class="['search-input', styleInput]"
                @input="inputSearch" />
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';

const keySearch = ref('');

// Emits
const emit = defineEmits(['search']);

// Props
defineProps({
    icon: { type: String, default: 'icon-default icon-lookup' },
    placeholder: { type: String, default: 'Tìm kiếm...' },
    styleInput: { type: String, default: '' },
    searchComponent: { type: String, default: '' },
    type: { type: String, default: 'text' }
});

function inputSearch() {
    emit('search', keySearch.value);
}
</script>
<style scoped>
@import '../../assets/components/ms-search/MsSearch.css';
</style>