<template>
  <div class="container">
    <div class="navbar">
      <div class="navbar__form--tool">
        <div class="navbar__icon">
          <div class="icon--navbar"></div>
          <div class="name--company">
            {{ this.MISAResource["VN"].CompanyName }}
          </div>
        </div>
        <div class="navbar--name"></div>
        <div class="navbar__input"></div>
        <div class="navbar__icon--user" @mouseleave="isLogout = false">
          <div
            :data-c-tooltip="tooltipText.Message"
            tooltip-position="left"
            class="icon--bell"
          ></div>
          <div class="icon--user"></div>
          <div class="name--user">{{ this.MISAResource["VN"].UserName }}</div>
          <div class="icon-dropdown" @click="isLogout = !isLogout">
            <div class="log-out" @click="logout" v-if="isLogout">
              <span> {{ this.MISAResource["VN"].Logout }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="content">
      <div class="layout-data">
        <div class="feature">
          <div class="title">
            <h2>{{ this.MISAResource["VN"].Employee }}</h2>
          </div>
          <div class="btn--feature">
            <button class="button btn-main btn-title" @click="showForm">
              {{ this.MISAResource["VN"].AddNewTitle }}
            </button>
          </div>
        </div>
        <div class="data">
          <div class="feature">
            <div class="selected-item" v-if="sum>0">
              <div class="count-row--select">
              <p>{{ this.MISAResource["VN"].Selected }}</p> <strong>  {{ sum }}</strong>
             </div>
             <div class="unselected-text"  @click="unSelected">
              <button
              class="unselected"
              :data-c-tooltip="unSelectedText"
              tooltip-position="top"
             >
              <div class="icon--close "></div>
            </button>
            </div>
            </div>
            <div
              @mouseleave="isShowDeleteMultiple = false"
              class="delete-multiple"
              @click="showDeleteMultiple"
              v-show="sum > 0"
            >
          
              <div class="delete-multiple-text">
                {{ this.MISAResource["VN"].MultipleCommand }}
              </div>
             
              <div class="icon-combobox">
                <div
                  v-show="isShowDeleteMultiple"
                  class="delete-text"
                  @click="showDlgDelete"
                >
                  {{ this.MISAResource.NameMode.Delete }}
                </div>
              </div>
            </div>
            <div class="box__input-icon">
              <input
                type="text"
                name="input"
                v-model="searchText"
                @input="OninputSearchData"
                :placeholder="placeholderText"
                ref="inputSeach"
              />
              <button
                class="btn-search"
                :data-c-tooltip="tooltipText.SearchData"
                @click="SearchData"
              >
                <div class="icon-search"></div>
              </button>
            </div>
            <button
              class="btn-reload"
              :data-c-tooltip="tooltipText.Reload"
              tooltip-position="left"
            >
              <div class="icon--reload" @click="reload"></div>
            </button>
           <div  class="export" 
           @mouseleave="menuExport=false"
           
           >
            <button
              @click="menuExport=true"
              class="btn-export"
              :data-c-tooltip="tooltipText.Export"
              tooltip-position="left"
            >
            <div class="icon--export"></div>
            </button>
            <div class="menu--export" v-if="menuExport">
                <div class="export-current"
              @click="ExportData"
                >{{this.MISAResource["VN"].ExportCurrent}}</div>
                <div class="export-all" 
              @click="ExportFile"
                >{{this.MISAResource["VN"].ExportAll}}</div>
            </div>
           </div>
            <button
              class="btn-import"
              :data-c-tooltip="tooltipText.Import"
              tooltip-position="left"
              @click="ShowFormImport"
            >
              <div class="icon--import"></div>
            </button>
          </div>
          <div class="scroll__table">
            <table id="table">
              <thead>
                <tr>
                  <th>
                    <div class="checked__box">
                      <input
                        id="check"
                        type="checkbox"
                        v-model="selectAll"
                        @change="toggleSelectAll"
                      />
                    </div>
                  </th>
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.EmployeeCode }}
                  </th>
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.EmployeeName }}
                  </th>
                  <th>{{ this.MISAResource["VN"].TableColumn.Gender }}</th>
                  <th>{{ this.MISAResource["VN"].TableColumn.DateOfBirth }}</th>
                  <th
                    :data-c-tooltip="tooltipText.IdentityNumber"
                    tooltip-position="bottom"
                  >
                    {{ this.MISAResource["VN"].TableColumn.IdentityNumber }}
                  </th>

                  <th>
                    {{ this.MISAResource["VN"].TableColumn.PositionName }}
                  </th>
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.DepartmentName }}
                  </th>
                  <th>
                    {{ this.MISAResource["VN"].TableColumn.CreditNumber }}
                  </th>
                  <th>{{ this.MISAResource["VN"].TableColumn.BankName }}</th>
                  <th>{{ this.MISAResource["VN"].TableColumn.BankAddress }}</th>
                  <th>{{ this.MISAResource["VN"].TableColumn.Feature }}</th>
                </tr>
              </thead>
              <tbody>
                <tr v-show="orderEmployee.length === 0">
                  <div>
                    {{ this.MISAResource["VN"].NotFoundRecord }}
                  </div>
                </tr>
                <tr
                  v-for="(item, index) in orderEmployee"
                  :key="index"
                  @keydown.delete.prevent="
                    showDlg(orderEmployee[selectedRowIndex])
                  "
                  @keydown.down.prevent="moveDown(index)"
                  @keydown.up.prevent="moveUp(index)"
                  @keydown.enter.prevent="
                    showData(orderEmployee[selectedRowIndex])
                  "
                  @dblclick="showData(item)"
                  @keydown.ctrl.1.prevent="showForm()"
                  :class="{ 'highlighted-row': item === selectedItems ||  isSelected(item) }"
                  @click="Select(index)"
                  tabindex="0"
                >
                  <td>
                    <div class="checked__box">
                      <input
                        id="check"
                        type="checkbox"
                        :value="item"
                        v-model="selectedItems"
                        @change="CountRowSelect"
                      />
                    </div>
                  </td>
                  <td class="txt-left">{{ item.EmployeeCode }}</td>
                  <td class="txt-left">{{ item.FullName }}</td>
                  <td class="txt-left">
                    {{ this.MISAEnum.GenderName(item.Gender) }}
                  </td>
                  <td class="txt-center">
                    {{ this.MISACommon.formatDate(item.DateOfBirth) }}
                  </td>
                  <td class="txt-left">
                    {{ item.IdentityNumber }}
                  </td>

                  <td class="txt-left">
                    {{ item.PositionName }}
                  </td>
                  <td class="txt-left">
                    {{ item.DepartmentName }}
                  </td>
                  <td class="txt-right">
                    {{ item.CreditNumber }}
                  </td>
                  <td class="txt-left">{{ item.BankName }}</td>
                  <td class="txt-left">
                    {{ item.BankAdress }}
                  </td>
                  <td>
                    <div
                      id="editRow"
                      @click="showTooltip(index)"
                      @mouseleave="hideTooltip(index)"
                    >
                      <div id="edit--detail">
                        {{ this.MISAResource["VN"].Edit }}
                      </div>
                      <div class="icon--drop">
                        <div id="edit--feature" v-show="index === tool">
                          <ul>
                            <li @click="copyData(item)">
                              {{ this.MISAResource["VN"].Copy }}
                            </li>
                            <li @click="showDlg(item)">
                              {{ this.MISAResource.NameMode.Delete }}
                            </li>
                            <li>{{ this.MISAResource["VN"].StopUsing }}</li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </td>
                </tr>
              </tbody>
              <tfoot>
                <tr></tr>
              </tfoot>
            </table>
          </div>
          <div class="footer">
            <div colspan="8">
              <div class="total-record">
                {{ this.MISAResource["VN"].Sum }}
                <strong> {{ records }} </strong>
                {{ this.MISAResource["VN"].Records }}
              </div>
            </div>
            <div colspan="6">
              <div class="paging">
                <m-dropdown-list
                  :dataApi="pageSizes"
                  propText="text"
                  propValue="value"
                  @ChangePageSize="changPageSize"
                ></m-dropdown-list>
                <the-pagination
                  v-model:pageNumber="pageNumber"
                  v-model:pageSize="pageSize"
                  :totalRecords="records"
                  @dataFilter="SearchData"
                ></the-pagination>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <the-dialog
    v-if="isShowDlg"
    @removeData="deleteData"
    @hideDlg="hideDlg"
    :title="title"
    :type="type"
    :msgError="msgDialog"
    :employeeIdRemove="employeeId"
    @loadData="reload"
    @deleteMultiple="DeleteMultiple"
  >
  </the-dialog>
  <FormExcel
    v-if="formExcel"
    @closeFormExcel="CloseFormImport"
    @loadData="reload"
  >
  </FormExcel>
  <form-employee-detail
    v-if="isShowForm"
    @hideForm="hideForm"
    :EmployeeSelected="employee"
    :methodP="method"
    @loadData="reload"
    @showForm="showForm"
  ></form-employee-detail>
  <MLoader v-if="loader"></MLoader>
  <!-- end toast-message -->
  <!-- this is dialog -->

  <!-- end dialog -->
</template>
<script>
import _ from "lodash";
import FormExcel from "../import/FormImportExcel.vue";
export default {
  components: { FormExcel },
  computed: {
    orderEmployee: function () {
      return _.orderBy(this.employees);
    },
  },
  created() {
    this.SearchData(this.pageSize, this.numberPage);
    window.addEventListener("keydown", this.checkCtrl);
  },
  watch: {},
  methods: {
    //Sử dụng phím tắt để thao tác chức năng
    checkCtrl(event) {
      if (event.key === this.MISAResource.Key.Escape) {
        event.preventDefault();
        this.hideForm();
      }
      if (event.ctrlKey && event.key === this.MISAResource.Key.KeyE) {
        event.preventDefault();
        if(!this.orderEmployee[this.selectedRowIndex]){
          return;
        }
        this.showData(this.orderEmployee[this.selectedRowIndex]);
      }
      if (event.ctrlKey && event.key === this.MISAResource.Key.KeyD) {
        event.preventDefault();
        if(!this.orderEmployee[this.selectedRowIndex]){
          return;
        } 
        this.showDlg(this.orderEmployee[this.selectedRowIndex]);
      }
      if (event.ctrlKey && event.key === this.MISAResource.Key.NUM1) {
        event.preventDefault();
        this.showForm();
      }
       if (event.ctrlKey && event.key === "F") {
        event.preventDefault();
         this.inputSearch.focus();
      }
    },
    //Hiển thị form Import
    ShowFormImport() {
      this.formExcel = true;
    },
    // Đóng form Import
    CloseFormImport() {
      this.formExcel = false;
    },
    //Lọc dữ liệu khi nhập
    //CreatedBy NCMANH(24/1/2024)
    OninputSearchData() {
      setTimeout(() => {
        this.SearchData(this.pageSize, this.numberPage);
      }, 500);
    },
    //Hàm phân trang dữ liệu
    //CreatedBy NCMANH(24/1/2024)
    async SearchData(pageSize, numberPage) {
      let res = await this.MISAApiService.FilterData(
        pageSize,
        numberPage,
        this.searchText
      );
      this.employees = res.data;
      this.records = res.record;
    },
    //Hàm xuất tệp các bản ghi hiện tại
    //CreatedBy NCMANH(24/1/2024)
    async ExportData(){
      if(this.selectedItems.length === 0){
        this.ApiExportService.exportData(this.employees,this.MISAResource["VN"].ExportCurrent);
      }
      else {
      this.ApiExportService.exportData(this.selectedItems,this.MISAResource["VN"].ExportCurrent);
      }
    },
 
    //Hàm xuất tệp tất cả bản ghi
    //CreatedBy NCMANH(24/1/2024)
    async ExportFile() {
      this.loader = true;
      await this.MISAApiService.exportFile();
      this.loader = false;
    },
    //Hiển thị dialog cảnh báo xóa nhiều
    //CreadtedBy : NC Mạnh(23/01/2024)
    showDlgDelete() {
      try {
        this.setDialog(
          this.MISAResource.notice.warning,
          this.msgDialog,
          true,
          this.MISAResource.NameMode.DeleteMultiple
        );
        this.msgDialog.push(this.MISAResource["VN"].DeleteMultipleQuestion);
      } catch (error) {
        console.log(error);
      }
    },
    //Hàm toggle tất cả bản ghi  với checkbox
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    toggleSelectAll() {
      // Đảo ngược giá trị của selectAll và cập nhật mảng selectedItems tương ứng
      try {
        if (this.selectAll) {
          this.selectedItems = [...this.orderEmployee];
        } else {
          this.selectedItems = [];
        }
        this.sum = this.selectedItems.length;
      } catch (error) {
        console.log(error);
      }
    },
     //Hàm đếm số bản ghi  với checkbox
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    CountRowSelect() {
      this.sum= 0;
      this.sum = this.selectedItems.length;
    },
    //Hàm bôi màu bản ghi  với checkbox
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    isSelected(item) {
        return this.selectedItems.includes(item);
    },
    //Hàm chọn bản ghi  với checkbox
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    Select(index) {
      try {
        this.selectedRowIndex = index;
        this.showTooltip(index);
        this.row = index;
        const selected = this.orderEmployee[index];
        const isSelected = this.selectedItems.includes(selected);
        if (!isSelected) {
          this.selectedItems.push(selected); 
          // Thêm vào mảng nếu chưa được chọn
        } else {
          this.selectedItems = this.selectedItems.filter(
            (item) => item !== selected
          ); // Loại bỏ nếu đã được chọn
          this.selectAll = false;
        }
        this.CountRowSelect();
      } catch (error) {
        console.log(error);
      }
    },
    //Hàm lên dòng trên bằng phím mũi lên trên
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    moveUp(index) {
      try {
        if (this.row > 0) {
          index = --this.row;
          this.selectedRowIndex = index;
          this.showTooltip(index);
        }
      } catch (error) {
        console.log(error);
      }
    },
    //Hàm xuống dòng dưới bằng phím mũi xuống dưới
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    moveDown(index) {
      try {
        if (this.row < this.orderEmployee.length - 1) {
          index = ++this.row;
          this.selectedRowIndex = index;
          this.showTooltip(index);
        }
      } catch (error) {
        console.log(error);
      }
    },
    //hàm hiển thị và đóng from toast
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    showFormToast() {
      this.emitter.emit(
        this.MISAResource.EmitFunction.showToast,
        this.typeToast,
        this.msgToast
      );
    },
    //hàm ẩn Dialog
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    hideDlg() {
      this.isShowDlg = false;
    },
    /**
     * Hàm Bỏ chọn tất cả checkbox
     */
    unSelected(){
         this.selectedItems = [];
         this.sum=0;
         this.selectAll = false;
    },
    //hàm show Dialog
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    showDlg(item) {
      try {
        this.setDialog(
          this.MISAResource.notice.warning,
          this.msgDialog,
          true,
          this.MISAResource.NameMode.Delete
        );
        if (item !== null && item !== undefined) {
          this.employeeId = item;
        }
        this.msgDialog.push(
          this.MISAResource["VN"].DeleteQuestion + `<${item.EmployeeCode}>`
        );
      } catch (error) {
        console.log(error);
      }
    },
    //Hàm xóa nhiều bản ghi
    //CreadtedBy : NC Mạnh(23/01/2024)
    async DeleteMultiple() {
      this.employeeIdArray = [];
      this.msgToast = [];
      this.selectedItems.map((x) => this.employeeIdArray.push(x.EmployeeId));
      this.msgToast.push(this.MISAResource.returnMessage.deleteComplete);
      this.typeToast = this.MISAResource.notice.success;
      await this.MISAApiService.DeleteDataMultiple(this.employeeIdArray);
      this.isShowDlg = false;
      this.showFormToast();
      await this.reload();
      this.sum = 0;
      this.selectedItems = [];
    },
    //hàm xóa employee
    //param : employeeId
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    async deleteData(id) {
      this.msgToast = [];
      this.msgToast.push(this.MISAResource.returnMessage.deleteComplete);
      this.typeToast = this.MISAResource.notice.success;
      await this.MISAApiService.DeleteData(id);
      this.isShowDlg = false;
      this.showFormToast();
      this.sum = 0;
      this.selectedItems = [];
      await this.reload();
    },
    //hàm show tool
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    showTooltip(index) {
      this.tool = index;
    },
    hideTooltip() {
      this.tool = [];
    },
    //hàm lấy về từng khách hàng
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    showData(item) {
      try {
        this.setUpFormInput(
          true,
          this.MISAEnum.method.UPDATE,
          this.MISAResource.returnMessage.updateComplete
        );
        this.employee = item;
        this.type = this.MISAResource.notice.question;
      } catch (error) {
        console.log(error);
      }
    },

    //hàm nhân bản
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    async copyData(item) {
      let maxCode = await this.MISAApiService.GetMaxCode();
      const { EmployeeCode, ...rest } = item;
      console.log(EmployeeCode);
      try {
        this.employee = Object.assign({}, rest, { EmployeeCode: maxCode });
        this.setUpFormInput(
          true,
          this.MISAEnum.method.ADD,
          this.MISAResource.returnMessage.addComplete
        );
      } catch (error) {
        console.log(error);
      }
    },
    //hàm mở form thông tin
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    async showForm() {
      let maxCode = await this.MISAApiService.GetMaxCode();
      this.employee = {};
      try {
        this.employee = {
          Gender: this.MISAEnum.Gender.MALE,
          EmployeeCode: maxCode,
        };
        this.setUpFormInput(
          true,
          this.MISAEnum.method.ADD,
          this.MISAResource.returnMessage.addComplete
        );
      } catch (error) {
        console.log(error);
      }
    },
    //hàm đóng form thông tin
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    hideForm() {
      this.isShowForm = false;
    },
    //hàm thay đổi số bản ghi
    //CreadtedBy : NC Mạnh
    //CreatedDate "23/01/2024"
    async changPageSize(pageSize) {
      this.pageSize = pageSize;
      this.loader = true;
      await this.SearchData(pageSize, this.numberPage);
      this.loader = false;
    },
    //hàm Load lại dữ liệu
    //CreadtedBy : NC Mạnh
    //CreatedDate "23/01/2024"
    async reload() {
      this.loader = true;
      await this.SearchData(this.pageSize, this.numberPage);
      this.loader = false;
    },
    //hàm đăng xuất
    //CreadtedBy : NC Mạnh
    //CreatedDate "23/02/2024"
    logout() {
      this.MISAApiService.logout();
    },
    //hàm show xóa nhiều
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    showDeleteMultiple() {
      this.isShowDeleteMultiple = !this.isShowDeleteMultiple;
    },
    //Hàm truyền dữ liệu cho FormInput trước khi hiển thị
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    setUpFormInput(isShowForm, method, content) {
      this.isShowForm = isShowForm;
      this.method = method;
      this.content = content;
    },
    //Hàm truyền dữ liệu cho dialog trước khi hiển thị
    //CreadtedBy : NC Mạnh
    //CreatedDate "5/12/2023"
    setDialog(type, msgDialog, isShowDlg, title) {
      msgDialog = [];
      this.msgDialog = [];
      this.isShowDlg = isShowDlg;
      this.title = title;
      this.type = type;
    },
  },
  data() {
    return {
      unSelectedText : "Bỏ chọn",
      isLogout: false,
      formExcel: false,
      isShowDeleteMultiple: false,
      employees: [],
      isShowForm: false,
      employee: {},
      showToast: false,
      typeToast: false,
      tool: [],
      content: "",
      method: 0,
      formattedDate: "",
      selectedRowIndex: "",
      row: 0,
      selectAll: false,
      selectedItems: [],
      sum: 0,
      loader: false,
      title: "",
      employeeId: {},
      isShowDlg: false,
      type: this.MISAResource.notice.question,
      msgToast: [],
      msgDialog: [],
      countPage: [],
      records: 0,
      pageSizes: this.MISAResource.PageSize,
      numberPage: 1,
      pageSize: 10,
      totalPage: 10,
      pageNumber: 1,
      response: [],
      employeeIdArray: [],
      searchText: "",
      titleDialog: "",
      token: "",
      menuExport : false,
      placeholderText: this.MISAResource["VN"].PlaceholderText,
      tooltipText :this.MISAResource["VN"].Tooltip,
      language : "VN",
    };
    // Thêm các dòng dữ liệu khác cần hiển thị
  },
};
</script>


<style>
</style>