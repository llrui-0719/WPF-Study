using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 备忘录
    /// </summary>
    public class MemoDto:BaseDto
    {

        private string title;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string content;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private int statue;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get { return statue; }
            set { statue = value; }
        }
    }
}
