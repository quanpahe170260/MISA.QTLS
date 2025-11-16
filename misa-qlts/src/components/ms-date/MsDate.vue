<template>
    <div class="date-input-wrapper">
        <label :for="id" class="date-label">
            {{ label }}
            <span v-if="required" class="required">*</span>
        </label>

        <DatePicker v-model="model" inputMode="icon" showIcon iconDisplay="input" dateFormat="dd/mm/yy"
            class="date-picker" />
    </div>
</template>

<script setup>
import { defineProps, defineEmits, computed } from "vue";
import DatePicker from "primevue/datepicker";

const props = defineProps({
    modelValue: null,
    label: String,
    id: String,
    required: Boolean
});

const emit = defineEmits(["update:modelValue"]);

const model = computed({
    get: () => props.modelValue,
    set: (val) => emit("update:modelValue", val)
});
</script>
<style scoped>
@import '../../assets/components/ms-date/MsDate.css';
</style>