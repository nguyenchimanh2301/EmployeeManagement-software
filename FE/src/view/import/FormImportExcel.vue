<template>
  <div class="dark--screen">
    <div
      class="form-excel"
      :class="{
        'close-form': showForm === false,
        'show-form': showForm === true,
      }"
    >
      <div class="form-header">
        <h3>{{ this.MISAResource["VN"].Import.Step }}{{ header }}</h3>
      </div>
      <div class="form-excel-content">
        <div class="form-sidebar">
          <div>
            <ul class="menu-import">
              <li
                v-for="(menu, index) in sidebar"
                :key="index"
                :class="{ 'menu-active': isShow === menu.value }"
              >
                {{ menu.name }}
              </li>
            </ul>
          </div>
        </div>
        <div class="form-table">
          <div class="form-excel-title" v-show="isShow === 0">
            <div class="title">
              {{ this.MISAResource["VN"].Import.ChoseFileImport }}
            </div>
            <div class="input-file">
              <input type="text" v-model="excelName" id="filename" @input="checkNullFile"  />
              <button
                class="button btn-second btn-add btn-second"
                @click="ImportFileClick"
              >
                Chọn
              </button>
              <input type="file" ref="fileInput" @change="handleFileChange" />
            </div>
            <div class="text">
              {{ this.MISAResource["VN"].Import.DownLoadEmptyFile }} <strong @click="DownloadEmptyFile">{{ this.MISAResource["VN"].Import.Here}} </strong>
            </div>
          </div>
          <div class="scroll__table-excel" v-show="isShow === 1">
            <div class="import-count">
              <strong
                >{{ valid }}/{{ length
                }}{{ this.MISAResource["VN"].Import.RowValid }}</strong
              >
              <span></span>
              <strong
                >{{ notValid }}/{{ length }}
                {{ this.MISAResource["VN"].Import.RowNotValid }}</strong
              >
            </div>
            <table id="table--excel">
              <thead>
                <tr>
                
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.EmployeeCode }}
                  </th>
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.EmployeeName }}
                  </th>
                  <th>{{ this.MISAResource["VN"].TableColumn.Gender }}</th>
                  <th>{{ this.MISAResource["VN"].TableColumn.DateOfBirth }}</th>
                  
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.DepartmentName }}
                  </th>
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.PositionName }}
                  </th>
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.CreditNumber }}
                  </th>
                  <th>{{ this.MISAResource["VN"].TableColumn.BankName }}</th>
                  <th>{{ this.MISAResource["VN"].TableColumn.BankAddress }}</th>
                  <th>{{ this.MISAResource["VN"].TableColumn.Status }}</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  style="position: relative"
                  v-for="(item, index) in employees"
                  :key="index"
                >
                  <td class="txt-left">{{ item.EmployeeCode }}</td>
                  <td class="txt-left">{{ item.FullName }}</td>
                  <td class="txt-left">
                    {{ this.MISAEnum.GenderName(item.Gender) }}
                  </td>
                  <td class="txt-center">
                    {{ this.MISACommon.formatDate(item.DateOfBirth) }}
                  </td>
                
                  <td class="txt-left">{{ item.DepartmentName }}</td>
                  <td class="txt-left">{{ item.PositionName }}</td>
                  <td class="txt-right">
                    {{ item.CreditNumber }}
                  </td>
                  <td class="txt-left">{{ item.BankName }}</td>
                  <td class="txt-left">
                    {{ item.BankAdress }}
                  </td >
                  <td  class="txt-left" >
                    <div
                      v-for="(error, index) in item.DTOImportErrors"
                      :key="index"
                    >
                      <p class="excel-text-error" 
                      v-if="item.DTOImportErrors.length >0"
                      > 
                        {{ error }}
                      </p>
                    </div>
                    <p v-if="item.DTOImportErrors.length ===0">{{ item.DTOImportSuccess }}</p>
                  </td>
        
                </tr>
              </tbody>
              <tfoot>
                <tr></tr>
              </tfoot>
            </table>
          </div>
          <div class="form-excel-result" v-show="isShow === 2">
            <div class="title">
              <h2>{{ this.MISAResource["VN"].Import.ImportResult }}</h2>
            </div>
            <div>
              <h4>{{ this.MISAResource["VN"].Import.DownLoadHere }} <strong @click="DownloadDataValid" >{{ this.MISAResource["VN"].Import.Here}} </strong> </h4>
            </div>
            <div class="text">
              <div>
                + {{ this.MISAResource["VN"].Import.RowImportValid }}<span>:</span>  <span></span> {{ valid }}
              </div>
              <div>
                +{{ this.MISAResource["VN"].Import.RowImportNotValid}}<span>:</span><span></span>{{ notValid }}
              </div>
            </div>
          </div>
        </div>
        <div v-show="isShow === 1" id="file-error">{{ this.MISAResource["VN"].Import.DownLoadFileError }} <strong @click="DownloadDataErrors" >{{ this.MISAResource["VN"].Import.Here}} </strong></div>
      </div>
      <div class="form--footer">
        <button class="button btn-second btn-cancel">
          <div class="icon--info"></div>
          <div>{{ this.MISAResource["VN"].Help }}</div>
        </button>
        <div>
          <button
            class="button btn-add"
            v-bind:disabled="isShow === 0 "
            v-show="isShow < 2"
            @click="onClickPreviousStep"
          >
          <div class="icon--previous-step"></div>
          <div>  {{ this.MISAResource["VN"].Previous }}</div>
          </button>
          <button
            class="button btn-add"
            v-show="isShow < 2"
            @click="onClickNextStep"
            v-bind:disabled="notImport"
          >
           <div>{{ this.MISAResource["VN"].Next }}</div>
            <div class="icon--continue"></div>
          </button>
          <button class="button btn-add" @click="CloseForm">
             <div><i class="fas fa-ban"></i></div>
             <div>{{ this.MISAResource["VN"].CancelBtn }}</div>
          </button>
        </div>
      </div>
    </div>
    <m-loader v-if="loader"></m-loader>
  </div>

</template>
  <script>
export default {
  props: [],
  created() {
    
  },
  watch:{
    
  },
  methods: {
    //Mở màn hình chọn tệp
    //CreatedBy NC Mạnh (25/3/2024)
    ImportFileClick() {
      this.$refs.fileInput.click();
    }, 
    /// Kiểm tra tên tệp có trống không có thì vô hiệu hóa bước tiếp
    //CreatedBy NC Mạnh (25/3/2024)
    checkNullFile(){
      if(this.excelName.length === 0 ) {
          this.notImport = true;
          this.selectedFile = null;
       }
       else{
          this.notImport = false;
       }
    },
    //Chọn tệp
    //CreatedBy NC Mạnh (25/3/2024)
    handleFileChange() {
      this.selectedFile = this.$refs.fileInput.files[0];
      if(this.selectedFile){
        this.excelName = this.selectedFile.name;
      }
      this.checkNullFile();
      this.employees = [];
    },
    //Hàm lấy về tệp  dữ liệu
    //CreatedBy NCMANH(24/1/2024)
    async handleFile(bool) {
      try {
        this.loader = true;
        if(this.selectedFile){
          this.response = await this.MISAApiService.uploadFile(
          this.selectedFile,bool
        );
        if(this.response.status===200){
        this.employeeImportValid = [];
        this.employeeImportErrors = [];
         this.employees = this.response.data;
         this.valid = 0;
         this.notValid=0;
         this.length = this.employees.length;
         this.employees.reduce((acc, x) => {
          if (x.DTOImportErrors.length < 1) {
            this.valid++;
            this.employeeImportValid.push(x);
          } else {
            this.employeeImportErrors.push(x);
            this.notValid++;
          }
        }, {});
        this.loader = false;
        this.$emit(this.MISAResource.EmitFunction.loadData);
        if(this.valid===0){
            this.notImport = true;
        }
        }else{
          this.msgToast = [];
          this.isShow=0;
          this.loader = false;
          this.notImport = true;
          this.msgToast = this.MISAErrorService.GetErrorCode(
          this.response.response
          );
          this.emitter.emit(this.MISAResource.EmitFunction.showToast,this.MISAResource.notice.error,
          this.msgToast);
        }
        }
      } catch (e) {
        console.log(e);
      }
    },
    //Hàm chạy đến bước tiếp theo
    //CreatedBy NCMANH(24/1/2024)
    onClickNextStep() {
 
      if (this.isShow < 2 ) {
        this.isShow += 1;
        if(this.isShow === 1 && this.employees.length ===0){
          this.handleFile(false);
        }
        let obj = this.sidebar.filter((x) => x.value === this.isShow);
        this.header = obj[0].name;
        }
      if(this.isShow===2 && this.valid>0){
          this.valid = 0;
          this.notValid=0;
          this.loader = true;
          this.handleFile(true);
      }
    },
      //Hàm tải về file Mẫu
    //CreatedBy NCMANH(01/03/2024)
    DownloadEmptyFile(){
       this.ApiExportService.exportEmptyFile();
    },
     //Hàm tải về file dữ liệu không hợp lệ
    //CreatedBy NCMANH(01/03/2024)
    DownloadDataErrors(){
      console.log(this.employeeImportErrors);
      this.ApiExportService.exportData(this.employeeImportErrors);
    },
    DownloadDataValid(){
      this.ApiExportService.exportData(this.employeeImportValid);
    },
    //Hàm chạy đến bước trước đó
    //CreatedBy NCMANH(24/1/2024)
    onClickPreviousStep() {
      if (this.isShow > 0) {
        this.isShow -= 1;
        let obj = this.sidebar.filter((x) => x.value === this.isShow);
        this.header = obj[0].name;
      }
    },
    //Thay đổi nội dung tiêu đề
    //CreatedBy NCMANH(24/1/2024)
    // ChangeContent(menu) {
    // try {
    //   this.isShow = menu.value;
    //   this.header = menu.name;
    // } catch (error) {
    //   console.log(error);
    // }
    // },
     //Đóng formExcel
    //CreatedBy NCMANH(24/1/2024)
    CloseForm() {
      this.$emit(this.MISAResource["VN"].Import.CloseFormExcel);
    },
  },
  mounted() {},
  data() {
    return {
      sidebar: this.MISAResource["VN"].Import.Sidebar,
      employees: [],
      employeeImportErrors: [],
      employeeImportValid: [],
      excelName: "",
      valid: 0,
      notValid: 0,
      length: 0,
      isShow: 0,
      loader: false,
      header: this.MISAResource["VN"].Import.Sidebar[0].name,
      notImport : true,
      response : {},
      msgToast : [],
      selectedFile : ""
    };
  },
};
</script>
  
  <style>

</style>


    
    