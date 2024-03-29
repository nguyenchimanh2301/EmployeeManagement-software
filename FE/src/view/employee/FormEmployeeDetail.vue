<template>
  <div class="dark--screen" v-if="true">
    <div
      class="form"
      :class="{
        'close-form': showForm === false,
        'show-form': showForm === true,
      }"
    >
      <div class="form--content">
        <div class="form--title">
          <div class="form--title check-box">
            <h3>{{ this.MISAResource["VN"].EmployeeInformation }}</h3>
            <div>
              <label for="" id="object"
                ><input type="checkbox" name="object" />{{
                  MISAResource["VN"].IsCustomer
                }}</label
              >
              <label for="" id="object"
                ><input type="checkbox" name="object" />{{
                  MISAResource["VN"].IsSupplier
                }}</label
              >
            </div>
          </div>
          <div class="form--title-icon">
            <div class="icon--question"></div>
            <div
              class="icon--close"
              :data-c-tooltip="tooltipText.Close"
              tooltip-position="left"
              @click="closeForm"
            ></div>
          </div>
        </div>
        <div class="form--input">
          <div class="form__input--left">
            <div class="form-input-code">
              <div class="label-input">
                <label id="label" for=""
                  >{{ this.MISAResource["VN"].EmployeeCode }}
                  <span class="text--required">*</span>
                </label>
                <MInput
                  title="Mã không được để trống"
                  v-model="state.EmployeeSelect.EmployeeCode"
                  input-id="empcode"
                  :hasError="v$.EmployeeSelect.EmployeeCode.$error"
                  ref="focusText"
                ></MInput>
                <div
                  class="error-text"
                  v-if="v$.EmployeeSelect.EmployeeCode.$error"
                >
                  {{ v$.EmployeeSelect.EmployeeCode.$errors[0].$message }}
                </div>
              </div>
              <div class="label-input">
                <label id="label" for=""
                  >{{ this.MISAResource["VN"].FullName
                  }}<span class="text--required">*</span>
                </label>
                <MInput
                  title="Tên không được để trống"
                  v-model="state.EmployeeSelect.FullName"
                  :hasError="v$.EmployeeSelect.FullName.$error"
                  input-id="name"
                  ref="FullName"
                ></MInput>
                <div
                  class="error-text"
                  v-if="v$.EmployeeSelect.FullName.$error"
                >
                  {{ v$.EmployeeSelect.FullName.$errors[0].$message }}
                </div>
              </div>
            </div>
            <div class="label-input">
              <label id="label" for=""
                >Đơn vị<span class="text--required">*</span>
                <m-combobox
                  :dataApi="department"
                  title="Hãy chọn phòng ban"
                  propText="DepartmentName"
                  propValue="DepartmentId"
                  v-model="state.EmployeeSelect.DepartmentId"
                  :hasError="v$.EmployeeSelect.DepartmentId.$error"
                ></m-combobox>
              </label>
              <div
                class="error-text"
                v-if="v$.EmployeeSelect.DepartmentId.$error"
              >
                {{ v$.EmployeeSelect.DepartmentId.$errors[0].$message }}
              </div>
            </div>
            <div class="label-input">
              <label id="label" for=""
                >{{ this.MISAResource["VN"].PositionName }}
                <m-combobox
                  :dataApi="position"
                  title="Hãy chọn chức vụ"
                  propText="PositionName"
                  propValue="PositionId"
                  v-model="state.EmployeeSelect.PositionId"
                ></m-combobox>
              </label>
              <div
                class="error-text"
                v-if="v$.EmployeeSelect.DepartmentId.$error"
              >
                <!-- {{ v$.EmployeeSelect.DepartmentId.$errors[0].$message }} -->
              </div>
            </div>
          </div>
          <div class="form__input--right">
            <div>
              <div class="label-input">
                <label id="label" for=""
                  >{{ this.MISAResource["VN"].DateOfBirth }}
                </label>
                <div class="format-date">
                  <flat-pickr
                    id="dateDisplay"
                    :config="datePickerConfig"
                    v-model="state.EmployeeSelect.DateOfBirth"
                    placeholder="dd/mm/yyyy"
                  ></flat-pickr>
                  <span class="date-icon" @click="openPicker">
                    <i class="far fa-calendar-alt"></i>
                  </span>
                </div>
              </div>

              <label id="label" for=""
                >{{ this.MISAResource["VN"].Gender }}
                <div class="box__input--radio">
                  <label id="gender" for=""
                    ><input
                      name="gender"
                      type="radio"
                      v-model="state.EmployeeSelect.Gender" 
                      :value="0"
                    />{{ this.MISAResource["VN"].GenderName.Male }}</label
                  >
                  <label id="gender" for=""
                    ><input
                      name="gender"
                      type="radio"
                      v-model="state.EmployeeSelect.Gender"
                      :value="1"
                    />{{ this.MISAResource["VN"].GenderName.Female }}</label
                  >
                  <label id="gender" for=""
                    ><input
                      name="gender"
                      type="radio"
                      v-model="state.EmployeeSelect.Gender"
                      :value="2"
                    />{{ this.MISAResource["VN"].GenderName.Other }}</label
                  >
                </div>
              </label>
            </div>
            <div class="input-identity">
              <div class="label-input">
                <label
                  id="label"
                  for=""
                  :data-c-tooltip="tooltipText.IdentityNumber"
                  tooltip-position="left"
                  >{{ this.MISAResource["VN"].IdentityNumber }}
                </label>
                <input
                  type="text"
                  id="idcard"
                  v-model="state.EmployeeSelect.IdentityNumber"
                  ref="IdentityNumber"
                />
                <div></div>
              </div>

              <div class="label-input">
                <label id="label" for=""
                  >{{ this.MISAResource["VN"].IdentityDate }}
                </label>
                <div class="format-date">
                  <flat-pickr
                    id="dateDisplay"
                    :config="datePickerConfig"
                    v-model="state.EmployeeSelect.IdentityDate"
                    placeholder="dd/mm/yyyy"
                  ></flat-pickr>
                  <span class="date-icon" @click="openPicker">
                    <i class="far fa-calendar-alt"></i>
                  </span>
                </div>
              </div>
            </div>
            <label id="label" for=""
              >{{ this.MISAResource["VN"].IdentityPlace }}
              <input
                type="text"
                id="identity-place"
                v-model="state.EmployeeSelect.IdentityPlace"
                ref="IdentityPlace"
              />
            </label>
          </div>
        </div>
        <div class="form__input--under">
          <label id="label" for=""
            >{{ this.MISAResource["VN"].Address }}
            <input
              type="text"
              v-model="state.EmployeeSelect.Address"
              ref="Address"
            />
          </label>
          <div class="form--info">
            <label id="label" for=""
              >{{ this.MISAResource["VN"].PhoneNumber }}
              <input
                type="text"
                v-model="state.EmployeeSelect.PhoneNumber"
                ref="PhoneNumber"
              />
            </label>
            <label id="label" for=""
              >{{ this.MISAResource["VN"].Fax }}
              <input type="text" ref="reset" />
            </label>
            <div class="label-input">
              <label id="label" for=""
                >Email
                <MInput
                  title="Email chưa đúng định dạng"
                  v-model="state.EmployeeSelect.Email"
                  :hasError="v$.EmployeeSelect.Email.$error"
                  input-id="email"
                  ref="Email"
                ></MInput>
                <!-- <span class="error-text" v-if="v$.EmployeeSelect.Email.$error">
                  {{ v$.EmployeeSelect.Email.$errors[0].$message }} -->
                <!-- </span> -->
              </label>
            </div>
            <label id="label" for=""
              >{{ this.MISAResource["VN"].CreditNumber }}
              <input
                type="text"
                v-model="state.EmployeeSelect.CreditNumber"
                ref="CreditNumber"
              />
            </label>
            <label id="label" for=""
              >{{ this.MISAResource["VN"].BankName }}
              <input
                type="text"
                v-model="state.EmployeeSelect.BankName"
                ref="BankName"
              />
            </label>
            <label id="label" for=""
              >{{ this.MISAResource["VN"].BankAddress }}
              <input
                type="text"
                v-model="state.EmployeeSelect.BankAdress"
                ref="BankAdress"
              />
            </label>
          </div>
        </div>
        <div class="form--footer">
          <button class="button btn-second btn-cancel" @click="closeForm">
            {{ this.MISAResource["VN"].Cancel }}
          </button>
          <div>
            <button
              class="button btn-second btn-add btn-second"
              @click="addNew"
            >
              {{ this.MISAResource["VN"].Add }}
            </button>
            <button class="button btn-add btn-main" @click="addAndNew">
              {{ this.MISAResource["VN"].AddAndNew }}
            </button>
          </div>
        </div>
      </div>
    </div>
    <the-dialog
      v-if="isShowDlg"
      @addData="addData"
      @hideDlg="hideDlg"
      :type="type"
      :title="title"
      :msgError="msgError"
      :textBtn="textBtn"
      :classBinding="binding"
      @hideForm="hideForm"
    >
    </the-dialog>
  </div>
</template>
<script>
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import useValidate from "@vuelidate/core";
import isEqual from "lodash/isEqual";
import {
  required,
  minLength,
  maxLength,
  numeric,
  email,
  helpers,
} from "@vuelidate/validators";
import { reactive, computed } from "vue";
import MISAResource from "../../js/helper/resource";
export default {
  props: ["EmployeeSelected", "methodP"],
  created() {
    this.setupData();
    window.addEventListener("keydown", this.checkCtrl);

  },
  setup() {
    //reactive validate
    const state = reactive({
      EmployeeSelect: {
      },
    });
    const rules = computed(() => {
      return {
        EmployeeSelect: {
          EmployeeCode: {
            required: helpers.withMessage(
              MISAResource["VN"].EmployeeNotEmpty,
              required
            ),
            // minLength: minLength(6),
          },
          DepartmentId: {
            required: helpers.withMessage(
              MISAResource["VN"].DepartmentNotEmpty,
              required
            ),
            // minLength: minLength(6),
          },
          Email: {
            // required: helpers.withMessage(MISAResource["VN"].EmailNotEmpty, required),
            email: helpers.withMessage(MISAResource["VN"].EmailNotValid, email),
          },
          FullName: {
            required: helpers.withMessage(
              MISAResource["VN"].FullNameNotEmpty,
              required
            ),
            // minLength: minLength(10),
          },
          PhoneNumber: {
            // required: helpers.withMessage(MISAResource["VN"].PhoneIsNotEmpty, required),
            numeric: helpers.withMessage(
              MISAResource["VN"].PhoneIsNumeric,
              numeric
            ),
            minLength: helpers.withMessage(
              MISAResource["VN"].PhoneIsValid,
              minLength(10)
            ),
            maxLength: helpers.withMessage(
              MISAResource["VN"].PhoneIsValid,
              maxLength(10)
            ),
          },
        },
      };
    });
    const v$ = useValidate(rules, state);
    const resetForm = () => {
      const { EmployeeCode, Gender, DepartmentId } = state.EmployeeSelect; // Lưu giữ giá trị của trường cần giữ lại
      state.EmployeeSelect = {
        EmployeeCode,
        Gender,
        DepartmentId,
      };
    };
    return { state, v$, resetForm };
  },
  methods: {
    /**
     * 
     * @param { } event 
     * Thao tác các chức năng với phím tắt
     * CreatedBy : NC Manh(25/03/2024 )
     */
    checkCtrl(event) {
      // Kiểm tra xem phím Ctrl và phím "e" đã được nhấn cùng nhau hay không
      if (event.ctrlKey && event.key === this.MISAResource.Key.KeyS) {
        event.preventDefault();
        this.addData(this.MISAResource.TypeAdd.Add);
      }
      if (event.ctrlKey && event.shiftKey && event.key === this.MISAResource.Key.KeyS) {
        event.preventDefault();
        this.addAndNew();
      }
     
    },
    //Chuẩn bị các dữ liệu khi component render
    //createdBy : NC Mạnh
    //CreatedAt : 10/03/2024
    setupData() {
      this.state.EmployeeSelect = JSON.parse(
        JSON.stringify(this.EmployeeSelected)
      );
      ///Xác định kiểu của form chi tiết là thêm mới hay thay đổi
      this.method = this.methodP;
      if (this.methodP === this.MISAEnum.method.ADD) {
        this.title = this.MISAResource.NameMode.AddNew;
      } else {
        this.title = this.MISAResource.NameMode.Change;
      }
    },
    //Hàm Cất và thêm mới
    //createdBy : NC Mạnh
    //CreatedAt : 03/3/2024
    async addAndNew() {
      this.v$.$validate();
      await this.addData();
      this.resetData();
      let maxCode = await this.MISAApiService.GetMaxCode();
      this.state.EmployeeSelect.EmployeeCode = maxCode;
      this.$refs.focusText.value = maxCode;
    },
    //Hàm Cất Dữ liệu
    //createdBy : NC Mạnh
    //CreatedAt : 03/3/2024
    async addNew() {
      this.v$.$validate();
      await this.addData(this.MISAResource.TypeAdd.Add);
    },
    //Hàm hiển thị dialog
    //createdBy : NC Mạnh
    //CreatedAt : 5/12/2023
    showDlg() {
      try {
        this.type =
          this.methodP === this.MISAEnum.method.ADD
            ? this.MISAResource.notice.information
            : this.MISAResource.notice.question;
        this.isShowDlg = true;
      } catch (error) {
        console.log(error);
      }
    },
    //Hàm ẩn dialog
    //createdBy : NC Mạnh
    //CreatedAt : 5/12/2023
    hideDlg() {
      this.isShowDlg = false;
      this.$refs.focusText.$el.focus();
    },
    //hàm thêm,thay đổi dữ liệu
    //createdBy : NC Mạnh
    //CreatedAt : 5/12/2023
    async addData(typeAdd) {
      this.msgError = [];
      this.employee = Object.assign({}, this.state.EmployeeSelect);
      //Chạy hàm validate
        this.v$.$errors.forEach((x) => this.msgError.push("-" + x.$message));
      //Nếu cảnh báo ở validate ở UI >0
      if (this.msgError.length > 0) {
        this.setType(
          "",
          this.MISAResource.notice.error,
          this.MISAResource.TextBtn.Accept
        );
        this.isShowDlg = true;
      } else {
        if (this.method === this.MISAEnum.method.ADD) {
          this.response = await this.MISAApiService.AddAndUpdateData(
            this.MISAEnum.method.ADD,
            this.employee
          );
        } else {
          this.response = await this.MISAApiService.AddAndUpdateData(
            this.MISAEnum.method.UPDATE,
            this.employee
          );
        }
        if (this.response.status === 201 || this.response.status === 200) {
          this.loadForm(this.response);
          this.closeToast();
          this.textBtn = this.MISAResource.TextBtn.Close;
          if (typeAdd === this.MISAResource.TypeAdd.Add) {
            this.hideForm();
          }
        } else {
          this.MsgValidate = this.MISAErrorService.GetErrorCode(
            this.response.response
          );
          this.msgError = this.MsgValidate;
          this.textBtn = this.MISAResource.TextBtn.Close;
          this.loadForm(this.response.response);
        }
      }
    },
    //Hiển thị dialog
    showDialog() {
      try {
        this.msgError = [];
        this.isShowDlg = true;
        this.setType(
          "",
          this.MISAResource.notice.info,
          this.MISAResource["VN"].Yes
        );
        this.binding = false;
        this.msgError.push(this.MISAResource["VN"].HideDialogQuestion);
      } catch (error) {
        console.log(error);
      }
    },
    //hàm loadForm
    //createdBy : NC Mạnh
    //CreatedAt : 5/12/2023
    loadForm(error) {
      // window.location.reload();
      try {
        this.msgToast = [];
        this.isShowDlg = true;
        let message =
          error.status === 201
            ? this.MISAResource.returnMessage.addComplete
            : this.MISAResource.returnMessage.updateComplete;
        if ((error.status === 201) | (error.status === 200)) {
          this.setType(
            this.MISAResource.notice.success,
            this.MISAResource.notice.success
          );
        } else {
          this.setType(
            this.MISAResource.notice.success,
            this.MISAResource.notice.warning,
            this.MISAResource["VN"].Accept
          );
        }
        this.msgToast.push(message);
      } catch (error) {
        console.log(error);
      }
    },
    //hàm đóng form
    //createdBy : NC Mạnh
    //CreatedAt : 5/12/2023
    closeForm() {
      this.dataChange = JSON.parse(JSON.stringify(this.EmployeeSelected));
      if (isEqual(this.dataChange, this.state.EmployeeSelect)) {
        this.$emit(this.MISAResource.EmitFunction.hideForm);
        // Đối tượng có cùng giá trị
      } else {
        this.showDialog();
        // Đối tượng khác nhau
      }
    },
    //hàm đóng form
    //createdBy : NC Mạnh
    //CreatedAt : 5/12/2023
    hideForm() {
      this.$emit(this.MISAResource.EmitFunction.hideForm);
    },
    //hàm đóng form
    //createdBy : NC Mạnh
    //CreatedAt : 20/12/2023
    closeToast() {
      // this.showToast=true
      try {
        this.isShowDlg = false;
        this.emitter.emit(
          this.MISAResource.EmitFunction.showToast,
          this.typeToast,
          this.msgToast
        );
        this.$emit(this.MISAResource.EmitFunction.loadData);
      } catch (error) {
        console.log(error);
      }
    },

    setType(typeToast, typeDialog, textBtn) {
      this.typeToast = typeToast;
      this.type = typeDialog;
      this.textBtn = textBtn;
    },
    //hàm xóa lữ diệu form input
    //createdBy : NC Mạnh
    //CreatedAt : 03/03/2024
    resetData() {
      this.resetForm();
      const refs = this.MISAResource.Refs;
      refs.forEach((ref) => (this.$refs[ref].value = " "));
      this.$refs.focusText.$el.focus();
    },
  },

  computed: {

  },
  mounted() {
    this.$refs.focusText.$el.focus();
  },

  data() {
    return {
      binding: true,
      EmployeeSelect: {},
      isShowDlg: false,
      showToast: false,
      method: 0,
      title: " ",
      formattedValue: 0,
      type: "",
      employee: {},
      dataChange: {},
      msgError: [],
      msgToast: [],
      typeToast: this.MISAResource.notice.information,
      closeCss: false,
      MsgValidate: [],
      position: this.MISAResource.Table.Positions,
      department: this.MISAResource.Table.Departments,
      showForm: true,
      textBtn: "",
      showDob: "",
      identityDate: "",
      selectedItemId: "",
      tooltipText :this.MISAResource["VN"].Tooltip,
      response: {},
      datePickerConfig: {
        altFormat: "d/m/Y",
        altInput: true,
        enableTime: false,
        defaultHour:0,
       allowInput : true
      },
    
    };
  },
  components: {
    flatPickr,
  },
};
</script>

<style>
.form-control input active {
  width: 210px !important;
    /* Quy tắc CSS của bạn */
}
</style>