<template>
    <div v-if="isOpen" class="modal-backdrop">
        <div class="modal-container d-flex flex-direction-row">
            <div class="modal-icon icon-default icon-warning d-flex align-items-center justify-content-center">
            </div>
            <div>
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
defineProps({
    isOpen: { type: Boolean, default: false },
    message: { type: String, default: 'Bạn có muốn lưu các thay đổi này?' }
});
defineEmits(['cancel', 'discard', 'save']);
</script>

<style scoped>
@import '@/assets/components/ms-modal/MsModalConfirm.css';
</style>
