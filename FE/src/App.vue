<template>
  <div>
    <!-- <the-header></the-header> -->
    <!-- <div class="layout">
     <the-sidebar></the-sidebar> -->
    <!-- end sidebar -->
    <!-- <table-employee></table-employee> -->
     <!-- <the-content></the-content> -->

     <router-view></router-view>
  <!-- </div> -->
  </div>
</template>

<script>
// import TheSidebar from './view/layout/TheSidebar.vue';
export default {
  name: 'App',
  components: {
    // TheSidebar,
    // TheContent,
  }
}
</script>

<style>
   @import url(./css/main.css);
</style>
