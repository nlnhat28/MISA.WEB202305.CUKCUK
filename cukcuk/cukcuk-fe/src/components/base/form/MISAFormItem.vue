<template>
    <div class="form__item">
        <tr style="width: 100%;">
            <td>
                <div class="form__label">
                    <label
                        class="label"
                        v-tooltip="title"
                    > {{ label }} </label>
                    <div
                        class="required"
                        v-if="isRequired"
                    >(*)</div>
                </div>
            </td>
            <td style="width: 100%;">
                <slot></slot>
            </td>
        </tr>

    </div>
</template>
<script>
export default {
    name: 'MISAFormItem',
    props: {
        /**
         * Label of form item
         */
        label: {
            type: String,
        },
        /**
         * Description about item
         */
        title: {
            type: String,
        },
        /**
         * Required of not
         */
        isRequired: {
            type: Boolean,
            default: false,
        }
    },
}
</script>
<style>
.form__item:has(textarea) td:has(.form__label) {
    padding-top: 6px;
    vertical-align: top;
}
.form__item td:has(textarea) {
  height: 60px;
}
</style>