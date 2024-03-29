import axios from "axios";
import baseurl from './baseurl.js';
let urlEmployee =  `${baseurl.baseUrl}Employees`;

const ApiExportService={
   //Import Service
   //CreatedBy NCMANH(23/1/2024)
   async uploadFile(file) {
    // Tạo một FormData object để gửi dữ liệu là tệp tin
    try {
      const formData = new FormData();
      formData.append('file',file);
      // Gọi API với yêu cầu POST và FormData object
      return await axios.post(urlEmployee+'/Import', formData)
        .then(response => {
          // Xử lý phản hồi từ API
        return response.data;
        })
        .catch(error => {
          // Xử lý lỗi
          console.error(error);
        });
    } catch (error) {
      console.log(error);
    }},
     //Api Service Xuất tệp
      //CreatedBy NCMANH(23/1/2024)
  async exportFile(){
      let url =  urlEmployee +`/Export`;
     try {
      const token = localStorage.getItem('token'); // Lấy token từ localStorage
      if (!token) {
          throw new Error(this.MISAResource.Token.TokenNotExist);
      }
      return await axios
      .get(url,{
        headers: {
            Authorization: `Bearer ${token}`
        }
       })
      .then((response) => {
        console.log(response);
        window.open(url);
      });
     } catch (error) {
       console.log(error);
     }
    },
    //Hàm xuất tệp mẫu
    async exportEmptyFile(){
        let url =  urlEmployee +`/EmptyFile`;
       try {
        return await axios
        .get(url
          )
        .then((response) => {
          console.log(response);
          window.open(url);
        });
       } catch (error) {
         console.log(error);
       }
     },
       async exportData(object,type) {
        let url;
        type == null ? url = `${urlEmployee}/ExportData`:url = `${urlEmployee}/ExportEmployee`
        try {
            const response = await axios.post(url, object, { responseType: 'blob' });
            // Kiểm tra xem yêu cầu đã thành công hay không
            if (response.status === 200) {
                // Tạo một URL đặc biệt cho file blob
                const blobUrl = window.URL.createObjectURL(new Blob([response.data]));
                // Tạo một phần tử <a> ẩn để tải file
                const link = document.createElement('a');
                link.href = blobUrl;
                link.setAttribute('download', 'employee.xlsx'); // Đặt tên file tải về
                document.body.appendChild(link);
                link.click(); // Kích hoạt sự kiện click trên phần tử <a>
                document.body.removeChild(link); // Xóa phần tử <a> sau khi tải xong
            } else {
                console.error('error');
            }
        } catch (error) {
            console.error(error);
        }
    }
    
}

export default ApiExportService;
