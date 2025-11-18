<template>
    <div class="form-input d-flex flex1"
        :class="flexRow ? 'flex-direction-row align-items-center' : 'flex-direction-column'">
        <span class="text-primary">{{ label }}
            <span v-if="isRequired" class="required-icon">*</span>
        </span>

        <InputNumber class="input-text" :min="0" :mode="mode" :showButtons="hasButton"
            :maxFractionDigits="numType === 'decimal' ? 22 : 0" v-model="displayNumber"
            :pt="{ pcInputText: { root: { tabindex: tabindex } } }">
            <template #incrementicon>
                <span class="icon-chevrons-up icon-default" tabindex="-1"></span>
            </template>
            <template #decrementicon>
                <span class="icon-chevrons-down icon-default" tabindex="-1"></span>
            </template>
        </InputNumber>

        <small v-if="isRequired" :class="['error-text', error_message && 'show']">
            {{ error_message }}
        </small>
    </div>
</template>

<script setup>
import { computed } from 'vue'
import InputNumber from 'primevue/inputnumber'

const props = defineProps({
    label: String,
    type: String,
    name: String,
    placeholder: String,
    isRequired: Boolean,
    size: { type: String, default: 'medium' },
    modelValue: [String, Number],
    error_message: String,
    flexRow: Boolean,
    tabindex: Number,
    hasButton: Boolean,
    numType: String,
})

const emit = defineEmits(['update:modelValue'])

// Giá trị hiển thị với 2 chữ số
const displayNumber = computed({
    get() {
        if (props.modelValue === null || props.modelValue === undefined || props.modelValue === '')
            return ''
        return String(props.modelValue).padStart(2, '0')
    },
    set(val) {
        const numericVal = Number(val)
        emit('update:modelValue', isNaN(numericVal) ? null : numericVal)
    },
})
</script>

<style scoped>
@import '@/assets/components/ms-input/MsInputNumber.css';
</style>
