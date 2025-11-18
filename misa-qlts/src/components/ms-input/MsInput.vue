<template>
    <div class="input-group">
        <label class="input-label">
            {{ label }}
            <span v-if="required" class="required">*</span>
        </label>

        <input class="input-control" :class="[{ disabled: disabled }]" :placeholder="placeholder" :disabled="disabled"
            :value="modelValue" @input="onInput" @blur="onBlur" @focus="$emit('focus', $event)" :type="type"
            :style="{ textAlign: align }" />
        <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>
    </div>
</template>

<script setup>
import { ref } from 'vue';
const errorMessage = ref('')
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
    }
})
//#endregion

//#region Emits
const emit = defineEmits(["update:modelValue", "blur", "focus"])
//#endregion

function onInput(e) {
    let val = e.target.value
    if (props.type === "number") {
        val = val === "" ? null : Number(val)
    }
    emit("update:modelValue", val)
}

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
