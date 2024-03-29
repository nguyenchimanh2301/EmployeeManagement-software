<template>
    <div class="login">
     <div class="language" @click="toggleOption">
        <i class="lang-icon-en" v-if="valuelanguage===this.MISAResource[state.language].Login.English"></i>
        <i class="lang-icon-vn" v-if="valuelanguage===this.MISAResource[state.language].Login.VietNam"> </i>
        <div class="selected-language">{{valuelanguage }}</div>
        <div class="sub-language selected-language" v-show="showOption" >
            <div class="sub-language-item" @click="ChangeCountry(this.MISAResource[state.language].Login.VN)">{{this.MISAResource[state.language].Login.VietNam }}</div>
            <div class="sub-language-item" @click="ChangeCountry(this.MISAResource[state.language].Login.EN)">{{this.MISAResource[state.language].Login.English }}</div>
        </div>
     </div>
     <div class="form-login">
       <div class="form-login-validate">
        <div class="form-login-title"></div>
        <div class="form-login-body">
            <div class="form-user">
              <MInput
                  v-model="state.account.username"
                  :hasError="v$.account.username.$error"
                  v-if="valuelanguage != this.MISAResource[this.state.language].Login.VietNam" 
                  placeholder="Phone number/Email"
                  ref="username"
             ></MInput>
             <MInput
                  v-model="state.account.username"
                  :hasError="v$.account.username.$error"
                  v-if="valuelanguage == this.MISAResource[this.state.language].Login.VietNam" 
                  placeholder="Số điện thoại/email"
                  ref="username"
             ></MInput>
            <span class="error-text-message" v-if="v$.account.username.$error">
                {{ v$.account.username.$errors[0].$message }}
            </span>
            </div>
            <div class="form-password" v-if="valuelanguage == this.MISAResource[this.state.language].Login.VietNam">
              <MInput
                  v-model="state.account.password"
                  :hasError="v$.account.password.$error"
                  v-if="!showPassword" 
                  placeholder="mật khẩu" 
                  type="password"
                  @keydown="inputOnkeyDown"
             ></MInput>
             <MInput
                   v-if="showPassword" 
                  v-model="state.account.password"
                  :hasError="v$.account.password.$error"
                  placeholder="mật khẩu" 
                  @keydown="inputOnkeyDown"
             >
            </MInput>
                 <i class="icon-toggle-password" @click="toggleShow"></i>
                 <span class="error-text-message" v-if="v$.account.password.$error">{{ v$.account.password.$errors[0].$message }}</span>
                 <span class="error-text-message" v-for="(item,index) in msgValidate" :key="index"><span v-if="v$.$errors.length === 0">{{item}}</span></span>
          </div>
          <div class="form-password" v-if="valuelanguage != this.MISAResource[this.state.language].Login.VietNam">
              <MInput
                  v-model="state.account.password"
                  :hasError="v$.account.password.$error"
                  v-if="showPassword" 
                  placeholder="password" 
             ></MInput>
             <MInput
                  v-model="state.account.password"
                  :hasError="v$.account.password.$error"
                  placeholder="password" 
             >
            </MInput>
            <i class="icon-toggle-password" @click="toggleShow"></i>
                 <span class="error-text-message" v-if="v$.account.password.$error">{{ v$.account.password.$errors[0].$message }}</span>
                 <span class="error-text-message" v-for="(item,index) in msgValidate" :key="index"> <span v-if="v$.$errors.length === 0">{{item}}</span> </span>
            <span class="error-text-message" v-if="v$.account.password.$error">
                {{ v$.account.password.$errors[0].$message }}
            </span>
          </div>
            <a href="" class="forgot-password">{{this.MISAResource[state.language].Login.ForgotPassword }}</a>
        </div>
        <button class="btn-login" @click="login">{{this.MISAResource[state.language].Login.Login }}</button>
        <div class="form-login-footer">
            <div class="method-login-title">
                <div class="method-line"></div>
                <div class="method-title">{{this.MISAResource[state.language].Login.LoginWith }}</div>
                <div class="method-line"></div>
            </div>
            <div class="method-login-icon">
                <div class="method-login-item method-icon-google"></div>
                <div class="method-login-item method-icon-apple"></div>
                <div class="method-login-item method-icon-microsoft"></div>
            </div>
        </div>
       </div>
        <div class="copy-right-text">{{this.MISAResource[state.language].Login.CopyWriterText }}</div>
     </div>
    </div>
    <MLoader v-if="loader"></MLoader>
    <the-toast-message :showToast="true" :iconToast="iconToast" :msgsToast="msgsToast"></the-toast-message>
</template>

<script>
import useValidate from "@vuelidate/core";
import router from '@/main';

import {
  required,
//   email,
  helpers,
} from "@vuelidate/validators";
import MISAResource from "../../js/helper/resource";
import { reactive, computed } from "vue";
import TheToastMessage from '../../components/toastmessage/TheToastMessage.vue';
export default {
  components: { TheToastMessage },
    created(){
    localStorage.clear();
    },
    mounted(){
      this.$refs.username.$el.focus();
    },
    setup() {
    //reactive validate
    const state = reactive({
        account: {
        username: "",
        password: "",
      },
      language : "VN"
    });

    const rules = computed(() => {
      return {
        account: {
          username: {
            required: helpers.withMessage(
              MISAResource["VN"].Login.UserNotEmpty,
              required
            ),
          },
          password: {
            required: helpers.withMessage(
              MISAResource["VN"].Login.PasswordNotEmpty,
              required
            ),
          },
         
        },
      };
    });
    const v$ = useValidate(rules, state);
    return { state, v$ };
  },
    methods: {
        /**
       * Hàm thao tác với phím tắt
       * CreatedBy NCManh(25/03/2024)
       */
      inputOnkeyDown(){
        const keyCode =  event.keyCode;
        if(keyCode == 13){
          this.login();
        }
    },
        /**
     * Hàm đăng nhập vào phần mềm
     * CreatedBy NC Manh(25/2/2024)
     * @
     */
     async login(){
      this.v$.$validate();
         if (this.v$.$errors.length > 0) {
       
            return; 
         }
         let response = await this.MISAApiService.login(this.state.account);
         if(response!==null && response!== undefined){
          this.msgValidate = this.MISAErrorService.GetErrorCode(
          response.response
          );
         if(response.status===200){
          this.loader= true;
          this.emitter.emit(this.MISAResource.EmitFunction.showToast,this.MISAResource.notice.success,this.msgsToast);
              setTimeout(()=>{
              this.loader= false;
              router.push('/layout')},1000)
         }
         }
        
      },
      ///Thay đổi ngôn ngữ
      ///CreatedBy : NCManh(13/3/2024)
      ChangeCountry(country){
        if(country === this.MISAResource[this.state.language].Login.EN){
          this.valuelanguage = this.MISAResource[this.state.language].Login.English;
          this.state.language = this.MISAResource[this.state.language].Login.EN;
          this.showOption = false;
        }
        if(country ===this.MISAResource[this.state.language].Login.VN){
          this.valuelanguage = this.MISAResource[this.state.language].Login.VietNam;
         this.state.language = this.MISAResource[this.state.language].Login.VN;
        }
        localStorage.setItem("language",country);
     },
     ///Toggle option chọn ngôn ngữ
    ///CreatedBy : NCManh(13/3/2024)
      toggleOption(){
       this.showOption = !this.showOption;
      },
    ///Hàm ẩn hiện mật khẩu
    ///CreatedBy : NCManh(20/1/2024)
      toggleShow() {
      this.showPassword = !this.showPassword;
    }
    },
    data(){
        return {
            validate : false,
            showOption : false,
            loader : false,
            showPassword: false,
            msgValidate:"",
            msgsToast : [this.MISAResource["VN"].LoginSuccess],
            valuelanguage : this.MISAResource[this.state.language].Login.VietNam,
            iconToast:  "success",
            isShowToast : true,
            msgBe : true,
        }
    }
}
</script>