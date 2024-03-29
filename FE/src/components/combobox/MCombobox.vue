<template>
  <div class="combobox" :class="{ 'border-error': hasError }">
    <input type="text" class="combobox_input" @input="FilterData" v-model="itemSelected" @keydown="inputOnkeyDown">
    <button class="combobox__button" @click="showDataClick">
      <div class="icon-combobox"></div>
    </button>
    <div class="combobox_data" v-if="showData">
      <a class="combobox-item" v-for="(item, index) in arrays" :key="index" 
         :class="{'item-selected': item[propText] === itemSelected ,'combobox-item--hover':index === indexHover}"
         @click="Selectitem(item)">
        <div class='icon-combobox-select' v-show="item[propText] === itemSelected"></div>
        <div>{{ item[propText] }}</div>
      </a>
    </div>
  </div>
</template>

<script>
export default {
  props: ["dataApi", "propText", "propValue", "hasError", "modelValue"],
  created() {
  this.showDataDetail();
  },
  watch:{
   propValue:function(newValue){
    console.log(newValue);
   }
  },
  methods: {
    /**
     * Sử dụng các phím tắt
     * CreatedBy NC Manh (25/03/2024)
     */
    inputOnkeyDown(){
        const keyCode =  event.keyCode;
        if(keyCode ==40){
          if(this.indexHover<this.arrays.length){
            this.indexHover++;
          }
        }
        if(keyCode == 38){
          if(this.indexHover>0 ){
            this.indexHover--;
          }
        }
        if(keyCode == 13){
           this.Selectitem(this.arrays[this.indexHover]);
        }
    },
    ///Hiển thị dữ liệu khi component render
    //createdBy : NC Mạnh (10/3/2024)
     async showDataDetail(){
       this.arrays = await this.MISAApiService.GetDataName(this.dataApi);
       if(this.arrays){
         const indexItem = this.arrays.findIndex(i=>i[this.propValue] == this.modelValue);
          if(indexItem>=0){
         this.itemSelected = this.arrays[indexItem][this.propText];      
         }
       }
    },
    ///Hiển thị dữ liệu combobox
    //createdBy : NC Mạnh (10/3/2024)
    async showDataClick() {
      this.showData = !this.showData;
      this.arrays = await this.MISAApiService.GetDataName(this.dataApi); 
    },
    ///Chọn dữ liệu từ combobox
    //createdBy : NC Mạnh (10/3/2024)
    Selectitem(item) {
      this.itemSelected = item[this.propText];
      this.showData = false;
      this.$emit('update:modelValue', item[this.propValue]);
    },
    ///Lọc các trường dữ liệu
    //createdBy : NC Mạnh (10/3/2024)
   async FilterData() {
      this.arrays = await this.MISAApiService.GetDataName(this.dataApi);
      this.showData = true;
      this.arrays = this.arrays.filter(item => item[this.propText].toLowerCase().includes(this.itemSelected.toLowerCase()));
    },
  },
  data() {
    return {
      showData: false,
      arrays: [],
      itemSelected: "",
      indexHover :0
    };
  },
};
</script>

