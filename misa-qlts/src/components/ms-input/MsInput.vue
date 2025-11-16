<template>
    <div class="input-group">
        <label class="input-label">
            {{ label }}
            <span v-if="required" class="required">*</span>
        </label>

        <input class="input-control" :class="{ disabled: disabled }" :placeholder="placeholder" :disabled="disabled"
            :value="modelValue" @input="onInput" @blur="$emit('blur', $event)" @focus="$emit('focus', $event)"
            :type="type" />
    </div>
</template>

<script setup>
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
    }
})

const emit = defineEmits(["update:modelValue", "blur", "focus"])

function onInput(e) {
    let val = e.target.value
    if (props.type === "number") {
        val = val === "" ? null : Number(val)
    }
    emit("update:modelValue", val)
}

</script>

<style scoped>
@import '@/assets/components/ms-input/MsInput.css';
</style>
