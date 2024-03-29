<template>
    <input
      :class="{
        'border-error': hasError,
      }"
      type="text"
      :id="inputId"
      v-model="value"
      :ref="focusText"
      @input="onInput"
    />
  </template>
  
  <script>
    export default{
    name:"MInput",
    // props:['values','hasError'],
    created(){
         this.value = this.modelValue;
    },
    data() {
        return {
            value :" ",
        }
    },
    methods: {
      ///Cập nhật giá trị
        onInput(){
            this.$emit("update:modelValue",this.value);
        }
    },
    props: {
      modelValue: {
        type: String,
        required: false,
      },
      focusText: {
      type: String,
      default: ' ',
     },
      inputId: {
      type: String,
      default: ' ',
     },
      hasError: {
        type: Boolean,
        default: false,
      },
    },
  };
  </script>
  
  <style scoped>
    .border-error {
      /* CSS của lớp khi có lỗi */
      border: 1px solid red;
    }
  </style>
  

