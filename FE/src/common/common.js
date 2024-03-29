
const MISACommon = {
    //hàm format tiền
  //CreadtedBy : NC Mạnh
  //CreatedDate "5/12/2023"
  formatCurrency(amount) {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    })
      .format(amount)
      .replace("₫", "");
  },

  //hàm format ngày tháng
  //CreadtedBy : NC Mạnh
  //CreatedDate "5/12/2023"
  formatDate(dateString) {
    if(dateString===null){
      return null;
    }
    let date = new Date(dateString);
    // Lấy ngày, tháng và năm từ đối tượng Date
    let day = date.getDate(); // Ngày
    let month = date.getMonth() + 1; // Tháng (0-11, cần cộng thêm 1)
    let year = date.getFullYear(); // Năm
    // Định dạng lại theo dd/mm/yyyy
    return (this.formattedDate = `${day < 10 ? "0" + day : day}/${
      month < 10 ? "0" + month : month
    }/${year}`);
  },
  //hàm mã hóa token
  //CreadtedBy : NC Mạnh
  //CreatedDate "5/12/2023"
   decodeJWT(token) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  }

}



export default MISACommon;