<template>
    <!-- Sidebar -->
    <div class="sidebar d-flex flex-direction-column align-items-center justify-content-space-between p-12"
        :class="{ collapsed: isCollapsed }">
        <div class="sidebar-top w-100">
            <div class="d-flex flex-direction-row align-items-center">
                <div class="logo icon-default ml-6"></div>
                <p class="logo-text">MISA QLTS</p>
            </div>
            <div class="sidebar-list-item">
                <SidebarItem label="Tổng quan" icon="home" :active="current === 'home'" />

                <SidebarItem label="Tài sản" icon="asset" :active="current === 'asset'">
                    <template #suffix>
                        <div class="icon-mask icon-chevron-down"></div>
                    </template>
                </SidebarItem>

                <SidebarItem label="Tài sản HT-ĐB" icon="HTDB" :active="current === 'HTDB'">
                    <template #suffix>
                        <div class="icon-mask icon-chevron-down"></div>
                    </template>
                </SidebarItem>

                <SidebarItem label="Công cụ dụng cụ" icon="CCDC" :active="current === 'CCDC'">
                    <template #suffix>
                        <div class="icon-mask icon-chevron-down"></div>
                    </template>
                </SidebarItem>

                <SidebarItem label="Danh mục" icon="category" :active="current === 'category'" />

                <SidebarItem label="Tra cứu" icon="search" :active="current === 'search'">
                    <template #suffix>
                        <div class="icon-mask icon-chevron-down"></div>
                    </template>
                </SidebarItem>

                <SidebarItem label="Báo cáo" icon="report" :active="current === 'report'">
                    <template #suffix>
                        <div class="icon-mask icon-chevron-down"></div>
                    </template>
                </SidebarItem>

            </div>
        </div>
        <div class="sidebar-bottom d-flex flex-direction-column ">
            <hr class="opacity-54" />
            <div class="compact mt-12 mr-12 opacity-54 d-flex align-items-center justify-content-center j pointer align-self-flex-end"
                @click="toggleSidebar">
                <div class="icon-mask" :class="isCollapsed ? 'icon-chevron-right' : 'icon-chevron-left'"></div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { ref } from 'vue'
import SidebarItem from '@/components/ms-sidebar-item/MsSidebarItem.vue'

const current = ref('asset')
const isCollapsed = ref(false);

function toggleSidebar() {
    isCollapsed.value = !isCollapsed.value;
}
</script>

<style scoped>
.sidebar {
    width: 230px;
    background-color: #1c3048;
    box-shadow: inset 0 3px 6px rgba(0, 0, 0, .16);
    min-height: 100vh;
    color: white;
    /* transition: width 0.3s; */
}

.sidebar.collapsed :deep(p) {
    display: none;
}

.sidebar.collapsed .icon-chevron-down {
    display: none;
}

.sidebar.collapsed,
.sidebar.collapsed hr {
    width: 66px;
}

.sidebar.collapsed .sidebar-item {
    justify-content: flex-start;
}

.sidebar.collapsed .compact {
    align-self: center;
}

.sidebar.collapsed .sidebar-item {
    border-radius: 6px;
}

.logo {
    padding-right: 50px;
}

.logo-text {
    font-size: 25px;
    font-weight: bold;
}

.sidebar-list-item {
    margin-top: 20px;
    gap: 12px;
}

.sidebar-item {
    /* background-color: #1aa4c8; */
    border-radius: 2px;
    height: 40px;
    overflow: hidden;
    opacity: 0.54;
}

.active {
    background-color: #1aa4c8;
    opacity: 1;
}

hr {
    width: 230px;
}

.compact {
    border: 2px solid #ffffff;
    width: 22px;
    height: 22px;
    border-radius: 4px;
}
</style>