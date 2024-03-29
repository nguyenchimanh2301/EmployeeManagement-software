<template>
    <div class="dropdown-box" >
    <input type="text" class="dropdown_input" 
     :value="itemSelected" 
     >
    <button class="dropdown__button" @click="showDataClick"><div class="icon-dropdown"></div></button>
    <div class="dropdown_data"  v-if="showData">
    <a class="dropdown-item" 
    v-for="(item,index) in dataApi" :key="index" 
    :class="{'item-selected':item[propText] ==itemSelected}"
    @click="Selectitem(item)"
    > {{ item[propText] }}</a>
    </div>
    </div>
</template>
<script>
export default {
props:["dataApi","propText","propValue","hasError"],
methods:{
  
  created() {
    console.log(this.dataApi);
  },
  mounted() {
    this.arrays = this.dataApi;
  },
  showDataClick(){
    this.showData=!this.showData;
  },
  Selectitem(item){
    this.itemSelected = item[this.propText];
     this.showData=false;
    this.$emit('update:modelValue',item[this.propValue])
    this.$emit('ChangePageSize',item[this.propValue])
  }
},
data(){
    return{
       showData:false,
       arrays : [],
       itemSelected: this.dataApi[0].text,
    }
},
}
</script>
<style>

</style>