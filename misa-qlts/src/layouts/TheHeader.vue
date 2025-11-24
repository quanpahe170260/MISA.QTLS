<template>
    <!-- Header -->
    <div class="header d-flex justify-content-space-between align-items-center">
        <!-- Header left -->
        <div class="header-left">
            <p class="header-left-text">Danh sách tài sản</p>
        </div>
        <!-- Header right -->
        <div class="header-right">
            <div class="d-flex flex-direction-row align-items-center just">
                <p class="header-right-text">Sở tài chính</p>
                <div
                    class="header-right-center d-flex flex-direction-row align-items-center justify-content-space-around">
                    <p>Năm</p>
                    <p><b>2021</b></p>
                    <div class="icon-navigation icon-default"></div>
                </div>
                <div class="d-flex flex-direction-row">
                    <div class="icon-item d-flex align-items-center justify-content-center mr-16">
                        <div class="icon-notification icon-default"></div>
                    </div>
                    <div class="icon-item d-flex align-items-center justify-content-center mr-16">
                        <div class="icon-default icon-menu"></div>
                    </div>
                    <div class="icon-item d-flex align-items-center justify-content-center mr-16">
                        <div class="icon-default icon-helper"></div>
                    </div>
                    <div class="icon-item d-flex align-items-center justify-content-center">
                        <div class="icon-default icon-avatar"> </div>
                    </div>
                    <div class="icon-item d-flex align-items-center justify-content-center">
                        <div class="icon-default icon-chevrons-down"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup></script>
<style scoped>
/* header */
.header {
    width: 100%;
    height: 44px;
    background-color: #ffffff;
    display: flex;
    align-items: center;
    padding: 0 1rem;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

/* header-left-text */
.header-left-text {
    font-weight: 700;
}

.header-right-text {
    font-weight: 500;
    color: #001031;
}

.header-right-center {
    margin: 0 26px;
    background-color: rgba(26, 164, 200, .2);
    border-radius: 3px;
    width: 112px;
    height: 30px;
    overflow: hidden;
}

.icon-item {
    width: 24px;
    height: 24px;
}
</style>