<template>
    <div class="form__row">
        <slot></slot>
    </div>
</template>
<script>
export default {
    name: 'MISAFormRow',
    props: {
        // numberColumns: {
        //     type: Number,
        //     default: 1,
        // }
    }
}
</script>