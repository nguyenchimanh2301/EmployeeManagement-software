<template>
  <div></div>
</template>

<script>
export default{
    name: "data-import",
    props:["data"],
    created(){
      this.employees = this.$emit('dataex');
      console.log(this.employees);
    },
    methods: {  
      
    },
    mounted(){
    },
    data() {
      return {
        employees:[]
      }
    },
} 
</script>