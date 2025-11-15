<template>
    <div :class="['input-wrapper', inputComponentStyle]">
        <label>
            <slot name="label"></slot>
        </label>

        <div class="select-container" v-if="type === 'select'">
            <slot name="icon"></slot>
            <select v-model="internalValue" :id="selectId" :class="['ms-input', selectClass || 'w-100']">
                <option v-for="(option, index) in options" :key="index" :value="option.value" :selected="selected">
                    {{ option.label }}
                </option>
            </select>
            <slot name="suffix"></slot>
        </div>

        <textarea v-else-if="type === 'textarea'" v-model="internalValue" class="ms-input"></textarea>

        <input v-else v-model="internalValue" :class="['ms-input', 'w-100', inputStyle]" :placeholder="placeholder"
            :type="type" :name="name" @input="$updateInputValid" :value="modelValue" />
    </div>
</template>


<script setup>
import { computed } from 'vue'
const props = defineProps({
    type: {
        type: String,
        default: 'text'
    },
    placeholder: {
        type: String,
        default: ''
    },
    name: {
        type: String,
        require: false,
        default: ""
    },
    modelValue: {
        type: [String, Number],
        default: ''
    },
    inputStyle: {
        type: String,
        default: null
    },
    inputComponentStyle: {
        type: String,
        default: null
    },
    selectClass: {
        type: String,
        default: null
    },
    selected: {
        type: Boolean,
        default: false
    },
    options: {
        type: Array,
        default: () => [],
    },
    selectId: {
        type: String,
        default: null
    },
    icon: {
        type: String,
        default: null
    }
})

const emit = defineEmits(['update:modelValue'])
const internalValue = computed({
    get: () => props.modelValue,
    set: (val) => emit('update:modelValue', val),
})
</script>
<style scope>
@import '@/assets/components/ms-input/MsInput.css'
</style>