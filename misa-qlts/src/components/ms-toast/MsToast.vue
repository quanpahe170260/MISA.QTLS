<template>
    <div class="ms-toast" :class="toastClass">
        <div class="ms-toast-icon">
            <span :class="icon"></span>
        </div>

        <div class="ms-toast-content">
            <p class="ms-toast-message">{{ message }}</p>
        </div>
    </div>
</template>

<script setup>
import { computed } from "vue";

//#region Props
const props = defineProps({
    type: String,
    title: String,
    message: String,
    icon: String
});
//#endregion

const toastClass = computed(() => props.type || "success");
</script>

<style scoped>
@import '@/assets/components/ms-toast/MsToast.css';
</style>
