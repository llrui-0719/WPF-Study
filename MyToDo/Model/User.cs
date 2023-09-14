using FreeSql.DataAnnotations;
using MyToDo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Model
{
    [Table(Name = "user")]
    public class User: BaseEntity
    {
        [Column(Name = "Account")]
        public string Account { get; set; }
        [Column(Name = "UserName")]
        public string UserName { get; set; }
        [Column(Name = "PassWord")]
        public string PassWord { get; set; }

    }
    [Table(Name = "registeruser")]
    public class RegisterUser : User
    {
        [Column(Name = "NewPassWord")]
        public string NewPassWord { get; set; }
    }
}
