<template>
    <div class="sidebar-item d-flex flex-direction-row align-items-center justify-content-space-between p-12 pointer"
        :class="{ active }">
        <div class="d-flex flex-direction-row align-items-center">

            <div class="icon-default" :class="`icon-${icon}`" v-if="!active"></div>

            <div class="icon-default" :class="`icon-${icon}-active`" v-if="active"></div>

            <p class="opacity-54 ml-12">{{ label }}</p>
        </div>
        <slot name="suffix"></slot>
    </div>
</template>

<script setup>
defineProps({
    label: String,
    icon: String,
    active: Boolean
})
</script>

<style scoped>
p {
    font-weight: 600;
}

.active p {
    opacity: 1;
}
</style>
