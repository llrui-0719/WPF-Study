using FreeSql.DataAnnotations;
using MyToDo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Model
{
    [Table(Name = "todo")]
    public class ToDo: BaseEntity
    {
        [Column(Name = "Title")]
        public string Title { get; set; }
        [Column(Name = "Content")]
        public string Content { get; set; }
        [Column(Name = "Status")]
        public int Status { get; set; }
    }
}
