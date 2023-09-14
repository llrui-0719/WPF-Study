using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Model
{
    public class BaseEntity
    {
        [Column(IsIdentity = true, IsPrimary = true, Name = "Id")]
        public int Id { get; set; }
        [Column(Name = "CreateDate")]
        public DateTime CreateDate { get; set; }
        [Column(Name = "UpdateDate")]
        public DateTime UpdateDate { get; set; }
    }
}
