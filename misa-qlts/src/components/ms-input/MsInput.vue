<template>
    <div class="input-group">
        <label class="input-label">
            {{ label }}
            <span v-if="required" class="required">*</span>
        </label>

        <input class="input-control" :class="[{ disabled: disabled }]" :placeholder="placeholder" :disabled="disabled"
            :value="displayValue" @input="onInput" @blur="onBlur" @focus="$emit('focus', $event)" :type="type"
            :style="{ textAlign: align }" />
        <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>
    </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import { formatNumber } from '@/utils/formatNumber';
const errorMessage = ref('')
const displayValue = ref("");
//#region Props
const props = defineProps({
    modelValue: {
        type: [String, Number],
        default: ''
    },
    label: {
        type: String,
        default: ''
    },
    placeholder: {
        type: String,
        default: ''
    },
    required: {
        type: Boolean,
        default: false
    },
    disabled: {
        type: Boolean,
        default: false
    },
    type: {
        type: String,
        default: 'text'
    },
    align: {
        type: String,
        default: 'left'
    },
    isNumber: {
        type: Boolean,
        default: false
    }
})
//#endregion

//#region Emits
const emit = defineEmits(["update:modelValue", "blur", "focus"])
//#endregion
watch(() => props.modelValue, (val) => {
    if (props.isNumber) {
        displayValue.value = formatNumber(val);
    } else {
        displayValue.value = val;
    }
}, { immediate: true });

/**
 * Hàm validate dữ liệu số 
 * @param e 
 * CreatedBy: QuanPA - 18/11/2025
 */
function onInput(e) {
    let raw = e.target.value;

    if (props.isNumber) {
        if (/[^0-9.]/.test(raw)) {
            errorMessage.value = "Chỉ được nhập số";
        } else {
            errorMessage.value = "";
        }
        raw = raw.replace(/\D/g, "");
        displayValue.value = formatNumber(raw);
        emit("update:modelValue", raw ? Number(raw) : "");
    } else {
        displayValue.value = raw;
        emit("update:modelValue", raw);
    }
}

/**
 * Hàm hiển thị lỗi
 * @param e 
 * CreatedBy: QuanPA - 18/11/2025
 */
function onBlur(e) {
    if (props.required && (props.modelValue === '' || props.modelValue === null)) {
        errorMessage.value = 'Trường này không được để trống'
    } else {
        errorMessage.value = ''
    }
    emit("blur", e)
}
</script>

<style scoped>
@import '@/assets/components/ms-input/MsInput.css';
</style>
