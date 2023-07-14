using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class RegisterDto:UserDto
    {
        private string newPassword;

        public string NewPassWord
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged(); }
        }
    }
}
