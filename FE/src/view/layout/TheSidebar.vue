<template>
    <div class="sidebar">
        <div class="sidebar__icon">
          <div class="icon--menu"></div>
          <div class="icon--logo">
            <img
              src="../../assets/img/Logo_Module_TiengViet_White.66947422.svg"
              alt="LOGO"
            />
          </div>
          <!-- <div><p>KẾ TOÁN</p></div> -->
        </div>
        <div class="list--item">
            <div class="item">
              <div class="icon--item icon--home"></div>
            <div>
              {{ this.MISAResource["VN"].LayoutSideBar.overview }}
            </div>
          </div>
          <div class="item">
            <div class="icon--item icon--cash"></div>
            <div>
              {{ this.MISAResource["VN"].LayoutSideBar.cash }}
            </div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--credit"></div>
            <div>    {{ this.MISAResource["VN"].LayoutSideBar.deposits }} </div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--shopping"></div>
            <div>   {{ this.MISAResource["VN"].LayoutSideBar.purchase }}</div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--sell"></div>
            <div>{{ this.MISAResource["VN"].LayoutSideBar.sell }}</div>
          </div>
          <div class="item" :class="{ 'sidebar-click': isClicked }" @click="toggleColor">
            <div class="icon--item icon--bill"></div>
            <router-link to="/layout/employees">{{ this.MISAResource["VN"].LayoutSideBar.employeeManager }} </router-link>
            <div>
          </div>
          </div>
          <div class="item">
            <div class="icon--item icon--warehouse"></div>
            <div> {{ this.MISAResource["VN"].LayoutSideBar.warehouse }} </div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--tool"></div>
            <div>{{ this.MISAResource["VN"].LayoutSideBar.tool }} </div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--assets"></div>
            <div> {{ this.MISAResource["VN"].LayoutSideBar.assets }}</div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--home"></div>
            <div> {{ this.MISAResource["VN"].LayoutSideBar.tax }}  </div>
          </div>
          <div class="item"> 
            <div>
              <div class="icon--item icon--price"></div>
            </div>
            <div>{{ this.MISAResource["VN"].LayoutSideBar.price }}</div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--synthetic"></div>
            <div> {{ this.MISAResource["VN"].LayoutSideBar.synthetic }} </div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--home"></div>
            <div>{{ this.MISAResource["VN"].LayoutSideBar.budget }} </div>
          </div>
          <div class="item">
            <div class="icon--item icon--report"></div>
            <div> {{ this.MISAResource["VN"].LayoutSideBar.report }}</div>
          </div>
          <div class="item"> 
            <div class="icon--item icon--analysis"></div>
            <div>{{ this.MISAResource["VN"].LayoutSideBar.analysis }}</div>
          </div>
        </div>
        <!-- <div class="btn--sidebar-footer">
          <div id="btn--hide-sidebar">
           <div id="previous" onclick="collapseSidebar()">
           </div>
           <p> Thu gọn</p>
          </div>
          <div id="btn--show-sidebar">
           <div  id="sibar__icon--show" onclick="openSidebar()">    
           </div>
           <p> Mở rộng</p>
          </div>
         </div> -->
      </div>
      
</template>

<script>
export default {
   
   methods:{
    /**
     * Đổi màu sidebar khi click
     * CreatedBy NCManh(25/3/2024)
     */
    toggleColor() {
      this.isClicked = !this.isClicked;
    },
    collapse(){ 

    },
    open(){

    },
    toggleSidebar() {
      this.isExpanded = !this.isExpanded; // Toggle the expanded state
    }
   },data() {
    return {
      isExpanded: false,
      isClicked: false
    };
  },

}
</script>
<style scoped>
a{
  text-decoration: none;
  color: #869AB8;
}
.side{
  width: 76px;
  transition: linear ;
  -webkit-transition: all 0.5s linear;
  animation: animation 2s;

}
.sidebar-click{
    background-color: #A7C4FF;
}
.item-collapse{
  transition: width 0.5s ease;
  width: 44px;
}
@keyframes animation{
  0% {transform: translateX(-0px);}
}
.width{
  transition: width 0.5s ease;
  width: 76px;
}
a{
  text-decoration: none;
}

</style>