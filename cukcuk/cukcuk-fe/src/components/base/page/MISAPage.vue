<template>
    <div class="page">
        <div class="page__header">
            <div class="page__title">
                <div class="page__title--text">{{ title }}</div>
            </div>
            <div class="page__action">
                <slot name="pageAction"></slot>
            </div>
        </div>
        <div class="page__body">
            <slot name="pageBody"></slot>
        </div>
    </div>
</template>
<script>
export default {
    name: 'MISAPage',
    props: {
        /**
         * Title of page
         */
        title: {
            type: String
        }
    }
}
</script>