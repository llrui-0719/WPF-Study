using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Singletons
{
    public class DataBaseConnect
    {

        private static readonly Lazy<IFreeSql> lazyFreeSqlInstance = new Lazy<IFreeSql>(() =>
        {
            return new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, "Data Source=D:\\Study\\Company_Study\\C_Sharp_Company_Study\\wpf\\MyToDo\\mydb.db")
                .UseAutoSyncStructure(true) // 自动同步实体结构到数据库
                .Build();
        });

        private DataBaseConnect()
        {
            // 私有构造函数，防止在类外部实例化该类。
        }

        public static IFreeSql GetFreeSqlInstance()
        {
            return lazyFreeSqlInstance.Value;
        }
    }

}   
