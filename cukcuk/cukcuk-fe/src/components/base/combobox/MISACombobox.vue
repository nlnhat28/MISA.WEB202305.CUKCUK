<template>
    <div
        :class="{ 'combobox': true, 'disabled': isDisabled }"
        tabindex="0"
        v-click-outside="hideSelects"
        @keydown="handleKeyDown"
    >
        <div class="input-wrapper">
            <div :class="{ 'input-group': true, 'input--error': errorMessage }">
                <input
                    :placeholder="this.$resources['vn'].selectValue"
                    :readonly="isReadOnly"
                    v-model="inputValue.name"
                    ref="input"
                    @click="toggleDisplaySelects"
                />
                <!-- <span class="divider"></span> -->
                <div
                    class="input-dropdown"
                    @click="toggleDisplaySelects"
                >
                    <m-icon icon="dropdown--sm"></m-icon>
                </div>
                <div
                    class="input-action"
                    v-if="action"
                    v-tooltip="action.title"
                    @click="onAction()"
                >
                    <m-icon :icon="action.icon"></m-icon>
                </div>
            </div>
            <div
                class="p-relative"
                v-show="isShowSelects"
            >
                <div
                    class="select-list"
                    v-show="selects.length > 0"
                    ref="refSelects"
                >
                    <!-- <m-no-content v-if="selects.length == 0"></m-no-content> -->
                    <div
                        v-for="(select, index) in selects"
                        :key="select.id"
                        :class="{
                            'select-item': true,
                            'select-item--selected': select.id == inputValue.id,
                            'select-item--focused': index == focusedSelectIndex,
                        }"
                        @click="onClickSelect(select)"
                    >
                        {{ select.note ?? select.name }}
                        <div
                            class="selected-item__icon"
                            v-if="isShowCheckedIcon"
                            v-show="select.id == inputValue.id"
                        >
                            <m-icon icon="check--blue"></m-icon>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <m-icon-container
            v-if="errorMessage"
            :icon="'error'"
            class="cursor--default tooltip--light"
            v-tooltip="`⛔ ${errorMessage}`"
        ></m-icon-container>
    </div>
</template>
<script>
import enums from '@/constants/enums.js';
import { debounce } from '@/js/utils/debounce.js';
import { isNullOrWhiteSpace } from '@/js/utils/string.js'
const direction = enums.direction;

export default {
    name: 'MISACombobox',
    props: {
        /**
         * (Props) Id of object
         */
        id: {
            type: [String, Number],
            default: null,
        },
        /**
         * (Props) Name of object or input value
         */
        name: {
            type: [String, Number],
            default: null,
        },
        /**
         * Tên nhãn
         */
        label: {
            type: String,
        },
        /**
         * Độ dài tối đa
         */
        maxLength: {
            type: Number
        },
        /**
         * (Props) List select in dropdown
         */
        theSelects: {
            type: Array,
        },
        /**
         * Focused or not
         */
        isFocused: {
            type: Boolean,
            default: false,
        },
        /**
         * Readonly input or not
         */
        isReadOnly: {
            type: Boolean,
            default: false,
        },
        /**
         * Disabled
         */
        isDisabled: {
            type: Boolean,
            default: false,
        },
        /**
         * Function to validate
         */
        validate: {
            type: Function,
        },
        /**
         * Type of direction
         */
        direction: {
            type: Number,
            default: direction.down,
            validator: (value) => {
                return [
                    direction.down,
                    direction.up,
                ].includes(value);
            },
        },
        /**
         * Allow select value
         */
        canSearch: {
            type: Boolean,
            default: true,
        },
        /**
         * Allow un select
         */
        canUnselect: {
            type: Boolean,
            default: true,
        },
        /**
         * Show checked icon for selected item
         */
        isShowCheckedIcon: {
            type: Boolean,
            default: false,
        },
        /**
         * Hành động đi kèm
         */
        action: {
            type: Object,
            default: null
        },
    },
    data() {
        return {
            /**
             * (Data) Selected input object includes id and name
             */
            inputValue: {
                id: this.id ?? null,
                name: this.name ?? null,
            },
            /**
             * (Data) List of selects in dropdown
             */
            selects: this.theSelects,
            /**
             * Show or hide dropdown
             */
            isShowSelects: false,
            /**
             * Error message when check validate
             */
            errorMessage: null,
            /**
             * Index of select is focused
             */
            focusedSelectIndex: 0,
        }
    },
    created() {
        this.onChangeName = this.debounce(this.handleChangeName, this.canSearch ? 200 : 0);
    },
    mounted() {
        if (this.isFocused) this.focus();
        this.scrollToSelectedItem();
    },
    beforeUnmount() {
        this.blur();
        this.$refs.refSelects.scrollTop = 0;
    },
    expose: [
        'checkValidate',
        'focus',
        'errorMessage'],
    watch: {
        /**
         * Watch change of the selects, id, name received from parent components
         * 
         * Author: nlnhat (01/07/2023)
         */
        theSelects: {
            handler(newValue) {
                this.selects = newValue;
                this.scrollToSelectedItem();
            },
            deep: true
        },
        id() {
            this.inputValue.id = this.id;
            this.inputValue.name = this.getNameById;

        },
        name() {
            this.inputValue.name = this.name;
        },
        /**
         * Watch change of input name
         * 
         * Author: nlnhat (01/07/2023)
         */
        "inputValue.name": function () {
            this.onChangeName()
        },
        /**
         * Watch change of focused index to scroll
         * 
         * Author: nlnhat (01/07/2023)
         */
        focusedSelectIndex() {
            if (this.focusedSelectIndex >= 0 && this.$refs.refSelects) {
                this.$refs.refSelects.scrollTop = this.scrollTopComputed;
            }
        },
    },
    computed: {
        /**
         * Get id of select by its name
         * 
         * Author: nlnhat (01/07/2023)
         */
        getIdByName() {
            try {
                const matchingSelect = this.selects.find(select => {
                    return (select.name == this.inputValue.name)
                });
                if (matchingSelect) {
                    this.selects = this.theSelects;
                    return matchingSelect.id;
                }
            } catch (error) {
                console.error(error);
            }
            return null;
        },

        /**
         * Get name of select by id
         * 
         * Author: nlnhat (01/07/2023)
         */
        getNameById() {
            try {
                const matchingSelect = this.theSelects.find(select => {
                    return (select.id == this.inputValue.id)
                });
                if (matchingSelect) {
                    return matchingSelect.name;
                }
            } catch (error) {
                console.error(error);
            }
            return this.inputValue.name;
        },
        /**
         * Validate value
         * 
         * Author: nlnhat (01/07/2023)
         * @returns Error message if invalid, null if valid
         */
        validateComputed() {
            try {
                if (this.maxLength) {
                    if (this.inputValue?.length > this.maxLength)
                        return `${this.label} ${resources['vn'].mustLessThan} ${++this.maxLength} ${resources['vn'].char}`;
                }
                if (this.validate) {
                    return this.validate(this.label, this.inputValue.name, this.selects)
                }
            } catch (error) {
                console.error(error);
            }
            return null
        },
        /**
         * Search in list
         * 
         * Author: nlnhat (06/07/2023)
         * @returns Selects include input
         */
        searchInList() {
            if (isNullOrWhiteSpace(this.inputValue.name))
                return this.theSelects
            const matchingSelects = this.theSelects.filter(select => {
                return select.name.toString().toLowerCase().includes(this.inputValue.name.toString().toLowerCase())
            });
            if (matchingSelects.length > 0)
                return matchingSelects;
            return this.theSelects;
        },
        /**
         * Search in list
         * 
         * Author: nlnhat (06/07/2023)
         * @returns Selects include input
         */
        scrollTopComputed() {
            if (this.selects.length > 0)
                return this.focusedSelectIndex * 26
            return 0;
        }
    },
    methods: {
        debounce,
        /**
         * Focus on input
         * 
         * Author: nlnhat (01/07/2023)
         */
        focus() {
            this.$refs.input.focus();
        },
        /**
         * Blur input
         * 
         * Author: nlnhat (01/07/2023)
         */
        blur() {
            this.$refs.input.blur();
        },
        /**
         * Handle name change
         * 
         * Author: nlnhat (01/07/2023)
         */
        handleChangeName() {
            try {
                if (this.canSearch)
                    this.selects = this.searchInList;
                this.inputValue.id = this.getIdByName;
                this.$emit('update:id', this.inputValue.id);
                this.$emit('update:name', this.inputValue.name);
                this.checkValidate();
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Handle when press up, down, enter from keyboard
         */
        handleKeyDown(event) {
            try {
                const keyCode = event.keyCode
                switch (keyCode) {
                    // Đi xuống
                    case this.$enums.keyCode.down:
                        if (this.isShowSelects == false)
                            this.showSelects();
                        if (this.focusedSelectIndex < this.selects.length - 1)
                            this.focusedSelectIndex++;
                        else
                            this.focusedSelectIndex = 0
                        break;
                    // Đi lên
                    case this.$enums.keyCode.up:
                        if (this.focusedSelectIndex > 0)
                            this.focusedSelectIndex--;
                        else
                            this.focusedSelectIndex = this.selects.length - 1
                        break;
                    // Enter: chọn
                    case this.$enums.keyCode.enter:
                        if (this.isShowSelects && this.focusedSelectIndex > -1) {
                            const select = this.selects[this.focusedSelectIndex]
                            this.onClickSelect(select)
                        }
                        else {
                            this.$emit('emitEnter');
                        }
                        break;
                    // Tab: Đóng
                    case this.$enums.keyCode.tab:
                        this.isShowSelects = false;
                        break;
                    default:
                        break;
                }
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Check validate value
         * 
         * Author: nlnhat (01/07/2023)
         * @param {*} value Value to validate 
         */
        checkValidate() {
            try {
                this.errorMessage = this.validateComputed
                if (this.errorMessage != null) {
                    this.focus();
                }
                return this.errorMessage;
            } catch (error) {
                console.error(error);
                return null
            }
        },
        /**
         * Handle event click on a select
         * 
         * Author: nlnhat (01/07/2023)
         * @param {*} select Select on clicked
         */
        onClickSelect(select) {
            try {
                this.focus();
                // Un select
                if (select.id == this.inputValue.id) {
                    if (this.canUnselect) {
                        this.inputValue.name = null;
                    }
                }
                // Select
                else {
                    this.inputValue.name = select.name;
                    this.onSelected();
                }
                this.isShowSelects = false;
            } catch (error) {
                console.error(error);
            }
        },
        /**
         * Show select list 
         * 
         * Author: nlnhat (01/07/2023)
         */
        showSelects() {
            this.isShowSelects = true;
            this.scrollToSelectedItem();
        },
        /**
         * Hide select list 
         * 
         * Author: nlnhat (01/07/2023)
         */
        hideSelects() {
            this.isShowSelects = false;
        },
        /**
         * Toggle show or hide select list
         * 
         * Author: nlnhat (01/07/2023)
         */
        toggleDisplaySelects() {
            if (this.isShowSelects) {
                this.isShowSelects = false;
            }
            else {
                this.showSelects();
            }
            this.focus();
        },
        /**
         * Handle after selected
         * 
         * Author: nlnhat (01/07/2023)
         */
        onSelected() {
            this.$emit('emitSelected');
        },
        /**
         * Scroll to selected item
         * 
         * Author: nlnhat (01/07/2023)
         */
        scrollToSelectedItem() {
            const index = this.selects.findIndex(select => select.id == this.inputValue.id);
            this.focusedSelectIndex = index ?? 0;
            this.$nextTick(() => {
                this.$refs.refSelects.scrollTop = this.scrollTopComputed;
            })
        },
        /**
         * Execute action
         * 
         * Author: nlnhat (01/07/2023)
         */
        onAction() {
            this.action.method();
        }
    },
}
</script>
<style>
.select-list {
    max-height: 200px;
}

.select-list:has(.mask) {
    min-height: 200px;
}

.input-action:hover .icon {
    filter: brightness(1.2);
}

.input-action:active .icon {
    filter: brightness(0.8);
}
</style>