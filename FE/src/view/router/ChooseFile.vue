<template>
  <div class="form-excel-title">
   
  </div>
</template>
<script>
export default {
  name: "the-choosefile",
  methods: {
    //Hiển thị màn hình chọn tệp
   
      //   this.loader = false;
  },
  data() {
    return {
      employees: [],
      excelName: "",
      valid: 0,
      notValid: 0,
      length: 0,

    };
  },
};
</script>