const MISAEnum = {
    //Enum phương thức thêm và sửa
    method:{
        ADD:0,
        UPDATE:1
    },
    //Enum Giới tính
    Gender:{
        MALE :0,
        FEMALE:1,
        OTHER:2,
    },
    EN:{

    },
    //Enum Giới tính
    GenderName(gender){
       if(gender===0){
          return "Nam";
       }
       if(gender===1){
        return "Nữ";
       }
       else{
        return "Khác";
       }
    }
}
 
export default  MISAEnum
