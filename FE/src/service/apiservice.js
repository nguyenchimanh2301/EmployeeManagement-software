import axios from "axios";
import baseurl from './baseurl.js'
import MISAEnum from "@/js/helper/enum.js";
import route from "@/main";
import MISACommon from '../common/common.js';
import MISAResource from "@/js/helper/resource.js";
let urlEmployee =  `${baseurl.baseUrl}Employees`;
let urlLogin =`${baseurl.baseUrl}Authentications/login`;


// Hàm để thêm token vào tiêu đề Authorization của một yêu cầu Axios
async function addTokenToRequest(config) {
  let token = localStorage.getItem('token');
  if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
}

// Khởi tạo interceptor để thực hiện thêm token vào tiêu đề Authorization cho mỗi yêu cầu
axios.interceptors.request.use(addTokenToRequest);


const ApiService={
    //Api service lấy dữ liệu và phân trang
    //CreatedBy(23/1/2024)
  async FilterData(pageSize, numberPage,searchText) {
    let data = [];
    let record ;
    if (searchText.trim() === null) {
      console.log(searchText);
    } else {
      data = await ApiService.loadFilter(
       searchText.trim(),
       pageSize,
       numberPage
     );
     let urlRecord =  urlEmployee+`/getpagingdto?searchText=${searchText.trim()}`
     let countRecords = await ApiService.GetDataUrl(urlRecord);
     if (countRecords) {
       record = countRecords.length;
     }
     }
     return {data,record};
   },
    //Api service lọc dữ liệu
    //CreatedBy(23/1/2024)
  async loadFilter(text, pageSize, numberPage) {
  let token = localStorage.getItem('token');
    let api = urlEmployee + `/getpagingdto?searchText=${text}&pageSize=${pageSize}&numberPage=${numberPage}`;
    try {
        const response = await axios.get(api,{
          headers: {
              Authorization: `Bearer ${token}`
          }});
        
        // Trả về dữ liệu từ response
        return response.data;
    } catch (error) {
        console.log(error);
    }
},
    //Api Service lấy dữ liệu theo tên 
    //CreatedBy(23/1/2024)
  async GetDataName(name){
    try {
      return await axios
    .get(
      baseurl.baseUrl+name
    )
    .then((response) => {
      return response.data;
    })
    } catch (error) {
      console.log(error);
    }
   } ,
    //DeleteData Service
   //CreatedBy NCMANH(23/1/2024)
  async DeleteData(id){
    try {
      return await axios.delete(urlEmployee +`/${id}`)
      .then((response) => {
        response.data;
      })
    } catch (error) {
      console.log(error);
      
    }
   }, 
   async DeleteDataMultiple(data){
   try {
    return await axios.delete(urlEmployee,{data})
    .then((response) => {
      response.data;
    })
   } catch (error) {
    console.log(error);
    
   }
   }, 
    //Api Service lấy dữ liệu theo Url 
    //CreatedBy(23/1/2024)
   async GetDataUrl(url){
  try {
    return await axios
    .get(
      url
    )
    .then((response) => {
      return response.data;
    })
  } catch (error) {
    console.log(error);
    
  }
   } ,
   
    //GetData Service
   //CreatedBy NCMANH(23/1/2024)
   async GetData(){
    try {
      return await axios
      .get(
        urlEmployee
      )
      .then((response) => {
        return response.data;
      })
    } catch (error) {
      console.log(error);
    }
   } ,
   //Import Service
   //CreatedBy NCMANH(23/1/2024)
   async uploadFile(file,commit) {
    // Tạo một FormData object để gửi dữ liệu là tệp tin
    try {
      const formData = new FormData();
      formData.append('file',file);
      // Gọi API với yêu cầu POST và FormData object
      return await axios.post(urlEmployee+'/Import?commit='+commit, formData)
        .then(response => {
          // Xử lý phản hồi từ API
        return response;
        })
    } catch (error) {
      console.log(error);
      return error;
    }},
    //Lấy mã lớn nhất
   //CreatedBy NCMANH(23/1/2024)
   async GetMaxCode() {
    try {
      const res = await axios.get(urlEmployee+'/maxcode');
      return res.data;
    } catch (error) {
      console.log(error);
    }
  },
     //Api Service Xuất tệp
      //CreatedBy NCMANH(23/1/2024)
      async exportFile(){
        let url = urlEmployee + '/Export';  
        try {
          const token = localStorage.getItem('token'); 
          const response = await axios.get(url, {
            headers: {
              Authorization: `Bearer ${token}`
            },
            responseType: 'blob' 
          });
          const blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }); 
          const downloadUrl = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = downloadUrl;
          link.setAttribute(MISAResource.Download,MISAResource.FileName); 
          document.body.appendChild(link);
          link.click();
          link.remove(); 
        } catch (error) {
          console.log(error);
        }
      },
    async AddAndUpdateData(method,employee){
      try {
        if (method === MISAEnum.method.ADD) {
        return await axios.post(urlEmployee, employee)
            .then((response) => {
             return response;
            })
        } else {
          return await axios.put(
              urlEmployee+ "/" +employee.EmployeeId,
              employee
            )
            .then((response) => {
             return response;
            })
        }
      }catch (error) {
        return error;
     }
    },
    ///Api Service đăng nhập
    ///CreatedBy : NCManh(20/1/2024)
    async login(account){
        try {
           return await axios.post(urlLogin,account)
           .then(res=>{
            console.log(res);
            localStorage.setItem('token',res.data.Token);
            localStorage.setItem('refresh',res.data.RefreshToken);
            localStorage.setItem('expiration',res.data.Expiration);
            return res;
            })
        } catch (error) {
           return error;
        }
      },
     ///Api Service đăng nhập
    ///CreatedBy : NCManh(20/1/2024)
    async logout(){
      let token = localStorage.getItem('token');
      let username = MISACommon.decodeJWT(token) ;
      try {
         let urlLougt =`${baseurl.baseUrl}Authentications/revoke/${username[baseurl.urlUserName]} `;
          return await axios.post(urlLougt)
         .then(res=>{
          route.push("/");
          return res;
          })
      } catch (error) {
         return error;
      }
    }
}

export default ApiService;
