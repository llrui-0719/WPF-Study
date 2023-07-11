using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel: NavigationViewModel
    {
        public ToDoViewModel(IToDoService service, IContainerProvider container):base(container)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            AddCommand = new DelegateCommand(Add);
            this.service = service;
        }

        private bool isRightDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 添加待办
        /// </summary>
        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        public DelegateCommand AddCommand { get;private set; }

        private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IToDoService service;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value;RaisePropertyChanged(); }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataListAsync()
        {
            UpdateLoading(true);

            var todoresult=await service.GetAllAsync(new Shared.Parameter.QueryParameter()
            {
                PageIndex = 0,
                PageSize=100,
            });
            if (todoresult.Status)
            {
                ToDoDtos.Clear();
                foreach(var info in todoresult.Result.Items)
                {
                    ToDoDtos.Add(info);
                }
            }
            UpdateLoading(false);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDataListAsync();
        }
    }
}
