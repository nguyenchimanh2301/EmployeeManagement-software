import {  createApp } from 'vue'
import App from './App.vue'


// import jwt_decode from "jwt-decode";
import excel from 'vue-excel-export'
 import TheToastMessage from './components/toastmessage/TheToastMessage.vue';
 import MInput from './components/input/MInput.vue';
 import MLoader from './components/loader/MLoader.vue';
 import TheDialog from './components/dialog/TheDialog.vue';

 import ThePagination from './components/paging/ThePagination.vue'
 import MCombobox from './components/combobox/MCombobox.vue'
 import  MDropdownList from './components/dropdown/MDropdownList.vue';
 import FormExcel from './view/import/FormImportExcel.vue'
 import TheLogin from './view/login/TheLogin.vue'
 import TableEmployee from './view/employee/TableEmployee.vue'
 import TheLayout from './view/layout/TheLayout.vue'
 import TheUnauthorize from './view/unauthorize/TheUnauthorize.vue'
 

 /* import font awesome icon component */

import MISAResource from './js/helper/resource';
import TheEmployee from './view/router/TheEmployee.vue';
import ChooseFile from './view/router/ChooseFile.vue';
import ImportResult from './view/router/ImportResult.vue';
import DataImport from './view/router/DataImport.vue';

import  axios  from 'axios';
import MISAEnum from './js/helper/enum';
import MISAErrorService from './service/errorservice';
import MISAApiService from './service/apiservice';
import DataService from './service/dataservice';
import FormEmployeeDetail from './view/employee/FormEmployeeDetail.vue';


import routers from './service/routerservice.js'
import './service/apiclientservice.js'
import ApiExportService from './service/exportservice.js'
import MISABaseUrl from './service/baseurl.js';
import MISACommon from './common/common.js';
import tinyEmitter from 'tiny-emitter/instance'
import "flatpickr/dist/flatpickr.css";


import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'


const app = createApp(App);
app.component('VueDatePicker', VueDatePicker);
app.component("MToast",TheToastMessage)
app.component("MInput",MInput)
app.component("MLoader",MLoader)
app.component("the-dialog",TheDialog)
app.component("the-employee",TheEmployee)
app.component("the-login",TheLogin)
app.component("the-result",ImportResult)
app.component("the-choosefile",ChooseFile)
app.component("the-data",DataImport)
app.component("the-layout",TheLayout)
app.component("the-unauthorize",TheUnauthorize)



app.component("ThePagination",ThePagination)
app.component("MCombobox",MCombobox)
app.component("MDropdownList",MDropdownList)
app.component("FormEmployeeDetail",FormEmployeeDetail)
app.component("TableEmployee",TableEmployee)
app.component("FormExcel",FormExcel)


app.use(excel)
app.use(routers);

app.config.globalProperties.api = axios;
app.config.globalProperties.MISAEnum = MISAEnum;
app.config.globalProperties.MISAResource = MISAResource;
app.config.globalProperties.MISAErrorService = MISAErrorService;
app.config.globalProperties.MISAApiService= MISAApiService;
app.config.globalProperties.MISADataService= DataService;
app.config.globalProperties.MISACommon= MISACommon;

app.config.globalProperties.ApiExportService= ApiExportService;
app.config.globalProperties.emitter = tinyEmitter;

app.config.globalProperties.MISAApi = "https://localhost:7096/api/v1/Employees";
app.config.globalProperties.MISAApiUrl = "https://localhost:7096/api/v1/";
app.config.globalProperties.MISABaseUrl = MISABaseUrl;


app.mount('#app')

export default routers;