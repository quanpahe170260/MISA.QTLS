<template>
    <div v-if="isOpen" class="modal-backdrop">
        <div class="modal-container d-flex flex-direction-row ">
            <div class="modal-icon d-flex align-items-center justify-content-center flex-direction-column">
                <i class="fa-solid fa-triangle-exclamation fa-lg" style="color: #FFD43B;"></i>
                <div class="nothing"></div>
            </div>

            <div class="d-flex flex-direction-column w-100">
                <div class="modal-message">
                    {{ message }}
                </div>

                <div class="modal-actions">
                    <slot></slot>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
//#region Props
defineProps({
    isOpen: { type: Boolean, default: false },
    message: { type: String, default: 'Bạn có muốn lưu các thay đổi này?' }
});
//#endregion

//#region Emis
defineEmits(['cancel', 'discard', 'save']);
//#endregion
</script>

<style scoped>
@import '@/assets/components/ms-modal/MsModalConfirm.css';
</style>
