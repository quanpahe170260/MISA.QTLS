<template>
    <div class="modal-overlay" v-if="isOpen">
        <div class="modal-container" ref="modalRef">
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
import { onMounted, nextTick, watch, ref } from "vue";
//#region Props
const props = defineProps({
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

const modalRef = ref(null);

watch(() => props.isOpen, async (val) => {
    if (val) {
        await nextTick();

        const firstInput = modalRef.value?.querySelector("input, select, textarea");

        if (firstInput) {
            firstInput.focus();
        }
    }
});
</script>

<style scoped>
@import '@/assets/components/ms-modal/MsModal.css';
</style>