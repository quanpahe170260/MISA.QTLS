<template>
    <div class="modal-overlay" v-if="isOpen">
        <div class="modal-container">
            <div class="modal-header">
                <p class="modal-title">{{ title }}</p>
                <button class="close-btn" @click="$emit('close')">
                    <div class="icon-default icon-close"></div>
                </button>
            </div>

            <div class="modal-body">
                <slot name="body"></slot>
            </div>

            <div class="modal-footer">
                <slot name="actions"></slot>
            </div>
        </div>
    </div>
</template>

<script setup>
//#region Props
defineProps({
    isOpen: {
        type: Boolean,
        required: true
    },
    title: {
        type: String,
        default: 'Modal Title'
    }
});

//#endregion
//#region Emits
defineEmits(['close']);
//#endregion
</script>

<style scoped>
@import '@/assets/components/ms-modal/MsModal.css';
</style>