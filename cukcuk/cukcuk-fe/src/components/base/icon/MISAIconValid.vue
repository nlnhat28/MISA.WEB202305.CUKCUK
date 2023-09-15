<template>
    <div class="d-flex justify-content--center" v-if="isValid != null">
        <m-icon-container
            icon="check--blue"
            v-if="isValid"
        >
        </m-icon-container>
        <m-icon-container
            icon="cukcuk-xmark--red"
            v-if="!isValid"
        >
        </m-icon-container>
    </div>
</template>
<script>
export default {
    name: 'MISAIconValid',
    props: {
        /**
         * Valid icon
         */
        isValid: {
            type: Boolean,
        }
    }
}
</script>