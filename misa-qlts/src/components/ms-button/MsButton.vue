<template>
    <button :class="[
        'ms-button',
        `ms-button--${type}`,
        buttonComponentStyle
    ]">
        <i v-if="icon && positionIcon === 'left'" class="ms-button__icon ms-button__icon--left" :class="icon"
            :style="style">
        </i>
        <slot></slot>
        <i v-if="icon && positionIcon === 'right'" class="ms-button__icon ms-button__icon--right" :class="icon"
            :style="style">
        </i>
    </button>
</template>
<script setup>
import { defineProps } from 'vue';
//#region 
const props = defineProps({
    icon: {
        type: String,
        default: null,
    },
    positionIcon: {
        type: String,
        default: 'left',
        validator: (value) => ['left', 'right'].includes(value)
    },
    type: {
        type: String,
        default: 'null'
    },
    style: {
        type: Object,
        default: () => ({})
    },
    buttonComponentStyle: {
        type: Array,
        default: null
    }
})
//#endregion
</script>
<style scoped>
@import '@/assets/components/ms-button/MsButton.css';
</style>