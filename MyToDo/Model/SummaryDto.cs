using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Model
{
    /// <summary>
    /// 汇总
    /// </summary>
    public class SummaryDto : BaseDto
    {
        private int sum { get; set; }
        private int completedCount { get; set; }
        private int memoCount { get; set; }
        private string completedRadio { get; set; }
        /// <summary>
        /// 汇总
        /// </summary>
        public int Sum
        {
            get { return sum; }
            set { sum = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 完成数量
        /// </summary>
        public int CompletedCount
        {
            get { return completedCount; }
            set { completedCount = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 备忘录数
        /// </summary>
        public int MemoCount
        {
            get { return memoCount; }
            set { memoCount = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 完成比例
        /// </summary>
        public string CompletedRadio
        {
            get { return completedRadio; }
            set { completedRadio = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ToDo> todoList;
        private ObservableCollection<Memo> memoList;

        /// <summary>
        /// 待办事项列表
        /// </summary>
        public ObservableCollection<ToDo> ToDoList
        {
            get { return todoList; }
            set { todoList = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 备忘录列表
        /// </summary>
        public ObservableCollection<Memo> MemoList
        {
            get { return memoList; }
            set { memoList = value; OnPropertyChanged(); }
        }

    }
}
