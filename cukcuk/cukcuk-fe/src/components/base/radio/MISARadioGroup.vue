<template>
    <div class="radio-group">
        <m-radio-item
            v-for="(radio, index) in radios"
            :key="radio.id"
            :radio=radio
            :checked="id == radio.id"
            :name="groupName"
            ref="radio"
            @emitUpdateValue="updateValue"
        >
        </m-radio-item>
    </div>
</template>
<script>
export default {
    name: 'MISARadioGroup',
    props: {
        /**
         * Array of select objects
         */
        radios: {
            type: Array
        },
        /**
         * Id of selected radio
         */
        id: {
            type: [Number, String]
        },
        /**
         * Name of selected radio
         */
        name: {
            type: [Number, String]
        },
        /**
         * Name of radio group
         */
        groupName: {
            type: String
        },
    },
    expose: ['focus'],
    methods: {
        /** 
         * Update selected radio
         * 
         * Author: nlnhat (01/07/2023)
         * @param {object} radio Selected radio object in radios
         */
        updateValue(radio) {
            this.$emit('update:id', radio.id);
            this.$emit('update:name', radio.name);
        },
        /**
         * Focus on first item
         * 
         * Author: nlnhat (05/07/2023)
         */
        focus() {
            if (this.$refs.radio[0])
                this.$refs.radio[0].focus();
        },
    }
}
</script>