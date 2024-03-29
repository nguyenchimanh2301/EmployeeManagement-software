    import axios from "axios";
    import baseurl from './baseurl.js'
    let urlRefreshToken = `${baseurl.baseUrl}Authentications/refresh-token`;
    axios.interceptors.request.use(async (config) => {
        let expiration = localStorage.getItem("expiration");
        // Kiểm tra xem yêu cầu có chứa Authorization header và token đã hết hạn hay không
        if (config.headers.Authorization && new Date() >= new Date(expiration)) {
            try {
                // Gọi hàm refresh token
                await reFreshToken();
                // Sau khi refresh token thành công, cập nhật lại Authorization header trong yêu cầu
                let newToken = localStorage.getItem('token');
                config.headers.Authorization = `Bearer ${newToken}`;
            } catch (error) {
                // Xử lý lỗi nếu có
                console.error(error);
                throw error; // Ném lỗi để yêu cầu gốc có thể xử lý
            }
        }
        return config; // Trả về config sau khi hoàn thành refresh token (hoặc không làm gì nếu không cần refresh token)
    });
    async function reFreshToken() {
        let token = localStorage.getItem('token');
        let refreshToken = localStorage.getItem('refresh')
        try {
            const response = await axios.post(
                urlRefreshToken,
                { AccessToken: token, RefreshToken: refreshToken }
            );
            console.log(response)
            // Cập nhật dữ liệu từ response
            localStorage.setItem("token", response.data.accessToken);
            localStorage.setItem("refresh", response.data.refreshToken);
            localStorage.setItem("expiration", response.data.expiration);
            localStorage.setItem("user", response.data.expiration); // Giả sử server trả về thông tin expiration mới
        } catch (error) {
            console.error(error);
            throw error;
        }
     }

   // axios.interceptors.request.use(async (config) => {
//   let expiration = localStorage.getItem("expiration");
//   console.log(new Date() >= new Date(expiration));
// //   console.log(new Date(expiration));
//   if (
//     config.headers.Authorization && // Kiểm tra xem yêu cầu có chứa Authorization header hay không
//     new Date() >= new Date(expiration) // Kiểm tra xem token đã hết hạn hay không
//   ) {
//     try {
//       await reFreshtoken();
//     } catch (error) {
//       // Xử lý lỗi nếu có
//       console.error("Lỗi khi thực hiện refresh token:", error);
//       throw error; // Ném lỗi để interceptor response có thể xử lý
//     }
//   }
//   return config; // Trả về config sau khi hoàn thành refresh token
// });

// Hàm để phân tích token JWT thành JSON
