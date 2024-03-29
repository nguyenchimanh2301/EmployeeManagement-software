<template>
    <div class="toast--container" v-if="showToast">
    <div class="form--toast" >
        <div class="toast__icon "
        :class="
        {
            'icon--info': icon == 'information',
            'icon--question': icon == 'question',
            'icon--danger': icon == 'error',
            'icon--warning': icon == 'warning',
            'icon--success': icon == 'success',
        }"></div>
        <div class="toast--content" >
        <ul v-for="(item, index) in msgToast" :key="index">
            <li >{{ item }}</li>
        </ul></div>
        <div class="toast--btn"></div>
    </div>
   </div>
</template>

<script>
export default {
    name :"MToast",
    props:{
        iconToast:{
            type: String,
            default: "",
        },
        msgsToast:{
            type:Array,
            default: ()=> [],
        }
    },
    created(){
      ///Sử dụng emitter để hiển thị toast và thông báo;
      ///CreatedBy NCManh(4/3/2024)
      this.emitter.on(this.MISAResource.EmitFunction.showToast,(icon,msgToast)=>{
      this.showToast = true;
      this.icon = icon;
      this.msgToast =msgToast;
      setTimeout(()=>this.showToast = false,2000);
     })
    },

    data(){
        return{
            showToast : false,
            icon : "information",
            msgToast : [],
        }
    }

    
}
</script>
<style scoped>
li{
    list-style: none;
}

</style>