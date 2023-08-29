<template>
    <i :class="['icon', `icon--${icon}`, { 'icon-btn': isBtn }]"></i>
</template>
<script>
export default {
    name: 'MISAIcon',
    props: {
        /**
         * Icon name
         */
        icon: {
            type: String
        },
        /**
         * Is a button icon or not
         */
        isBtn: {
            type: Boolean,
            default: false
        }
    }
}
</script>