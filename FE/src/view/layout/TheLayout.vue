<template>
    <div>
      <the-header></the-header>
      <div class="layout">
       <the-sidebar></the-sidebar>
      <!-- end sidebar -->
       <router-view></router-view>
       <the-content></the-content>
       <the-toast-message ></the-toast-message>
    </div>
    </div>
  </template>
  
  <script>
  import TheToastMessage from '../../components/toastmessage/TheToastMessage.vue';
  import TheSidebar from './TheSidebar.vue';
  export default {
    name: 'App',
    components: {
      TheSidebar,
        TheToastMessage,
      // TheContent,
    },
    data() {
    return {
      isLogout : false,
    }},
    methods:{
      
    },
  }
  </script>
  
  <style>
  </style>
  