<template>
    <div class="p-relative">
        <table :class="{'select-table': true, 'table--noborder': !hasBorder}">
            <tbody>
                <tr>
                    <td>*</td>
                    <td>Chứa</td>
                </tr>
                <tr>
                    <td>=</td>
                    <td>Bằng</td>
                </tr>
                <tr>
                    <td>!</td>
                    <td>Không chứa</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
<script>
export default {
    name: 'MISASelectTable',
    props: {
        /**
         * Hiển thị border hay không
         */
        hasBorder: {
            type: Boolean,
            default: true,
        }
    },
    data() {
        return {

        }
    }
}
</script>