<template>
   <div class="dialog--background" >
    <div class="dialog">
        <div class="dialog--title">
          <h3>{{ title }}</h3>
          <div class="toast--btn" @click="hideDlg"></div>
        </div>
       <div class="dialog--content">
        <div class="toast__icon " 
        :class="{
        'icon--question': type =='question',
        'icon--warning': type =='warning',
        'icon--danger': type =='error',
        'icon--info': type =='information',
        }"></div>
        <div class="dialog--text">
          <ul v-for="(item, index) in msgError" :key="index">
            <li>{{item }}</li>
          </ul>
        </div>
       </div>
       <div class="dialog__btn" :class="{'position-item': classBinding===false,
       'position-item2': classBinding===true,
        }">
        <button class="btn-main btn-second btn-cancel" id="closedialog" v-if="classBinding===false"   @click="hideDlg">{{ this.MISAResource["VN"].Cancel }}</button>
         <div style="column-gap: 8px;display: flex;">
          <button class="button  btn-second btn-cancel" @click="hideForm">{{ textBtn2 }}</button>
           <button class="button btn-main"  @click="addData"  >{{ textBtn }}</button>
         </div>
       </div>
    </div> 
</div>
  
</template>
<script>
export default{
   name :"the-dialog",
   props:{
    title:{
        type : String,
        default : " ",
        required : true
      },
      msgError:{
        type :Array,
        default : ()=>[]
      },
      employeeIdRemove:{
        type : Object,
        default : ()=> {},
        required : true
      },
      type:{
        type : String,
        default :  "information",
        required : true
      },
      textBtn:{
        type : String,
        default : "Đồng ý",
        required : true
      },
      textBtn2:{
        type : String,
        default : "Không",
        required : true
      },
      button:{
        type : Boolean,
        default : false,
      },
      position:{
        type : String,
        default : "",
      },
      classBinding: {
        type : Boolean,
        default : true,
      }

   },
   created(){
      this.titleDialog = this.title;
      this.employeeId = this.employeeIdRemove;
      if(this.titleDialog!==this.MISAResource.NameMode.Delete){
        this.text = this.MISAResource.TextBtn.None;
      }
       else{
            this.text = this.employeeId.EmployeeCode;
      }
    window.addEventListener('keydown', this.checkCtrl); 
   },
   mounted(){

   },
   methods:{
    checkCtrl(event) {
        // Kiểm tra xem phím Ctrl và phím "e" đã được nhấn cùng nhau hay không
        if (event.ctrlKey && event.key === "d") {
            event.preventDefault();
            this.addData();
        }
    },
    //Hàm ẩn dialog
    //CreadtedBy : NC Manh (23/01/2024)
    hideDlg(){
        this.$emit(this.MISAResource.EmitFunction.hideDlg);
    },
     //Hàm ẩn form
    //CreadtedBy : NC Manh (23/01/2024)
    hideForm(){
      if(this.msgError[[0]]===this.MISAResource["VN"].HideDialogQuestion){
        this.$emit(this.MISAResource.EmitFunction.hideForm);
      }
      else{
        this.hideDlg();
      }
    },
     //Hàm chạy chức năng
    //CreadtedBy : NC Manh (23/01/2024)
    addData(){
       if(this.titleDialog===this.MISAResource.NameMode.Delete){
        this.$emit(this.MISAResource.EmitFunction.removeData, this.employeeId.EmployeeId);
       }
       if(this.titleDialog===this.MISAResource.NameMode.DeleteMultiple){
        this.$emit(this.MISAResource.EmitFunction.deleteMultiple);
       }
       if(this.type === this.MISAResource.notice.warning){
        this.hideDlg();
       }
       else{
        this.$emit(this.MISAResource.EmitFunction.addData);
        this.hideDlg();
       }
    },
    data() {
        return {
            titleDialog: '',
            employeeId  :{},
            text : "",
        }
    },
   }    
   
}
</script>
<style scoped>
.dialog--text li{
  list-style: none;
}
#btn-cancel{
  align-self: end;
}

.position-item{
  justify-content: space-between;
}
.position-item2{
  justify-content: flex-end
}
.dialog__btn .btn-cancel:hover{
   background-color: #E0E0E0;
}
</style>