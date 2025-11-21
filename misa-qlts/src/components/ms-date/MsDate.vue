<template>
    <div class="input-date-wrapper">
        <label v-if="label" class="input-label">
            {{ label }}
            <span v-if="required" class="required">*</span>
        </label>

        <div class="input-date-container">
            <Calendar ref="calendarRef" v-model="internalValue" appendTo="self" :placeholder="placeholder"
                :showIcon="false" :dateFormat="dateFormat" class="w-full" @blur="$emit('blur')"
                @focus="$emit('focus')" />

            <!-- Icon tùy chỉnh -->
            <span class="icon-container" @click="openCalendar">
                <slot name="icon">
                    <!-- Nếu không có slot thì dùng prop icon -->
                    <component v-if="icon" :is="icon" class="custom-icon" />
                    <i v-else class="pi pi-calendar"></i>
                </slot>
            </span>
        </div>
    </div>
</template>

<script setup>
import { ref, watch } from "vue";
import Calendar from "primevue/calendar";
const calendarRef = ref();
//#region Props
const props = defineProps({
    modelValue: {
        type: [String, Date, null],
        default: null
    },
    label: String,
    placeholder: String,
    required: Boolean,
    icon: [String, Object],
    dateFormat: {
        type: String,
        default: "dd/mm/yy"
    }
});
//#endregion

//#region Emits
const emit = defineEmits(["update:modelValue", "blur", "focus"]);
//#endregion

const internalValue = ref(
    props.modelValue ? new Date(props.modelValue) : new Date()
);


watch(
    () => props.modelValue,
    (val) => {
        internalValue.value = val;
    }
);

watch(internalValue, (val) => {
    emit("update:modelValue", val);
});

/**
 * Hàm mở popup calendar
 * CreatedBy: QuanPA - 18/11/2025
 */
function openCalendar() {
    if (calendarRef.value?.$el) {
        const input = calendarRef.value.$el.querySelector("input");
        if (input) input.click();
    }
}
</script>

<style scoped>
@import '@/assets/components/ms-date/MsDate.css';
</style>
