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
            ExcuteCommand = new DelegateCommand<String>(Excute);
            SelectCommand = new DelegateCommand<ToDoDto>(Selected);
            this.service = service;
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "新增":Add();break;
                case "查询":Query();break;
                case "保存":Save();break;
            }
        }

        

        private void Query()
        {
            GetDataListAsync();
        }

        private string search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value;RaisePropertyChanged(); }
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

        private ToDoDto currentDto;

        /// <summary>
        /// 编辑选中/新增时的对象
        /// </summary>
        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value;RaisePropertyChanged(); }
        }


        /// <summary>
        /// 添加待办
        /// </summary>
        private void Add()
        {
            CurrentDto = new ToDoDto();
            IsRightDrawerOpen = true;
        }

        private async void Selected(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var result=await service.GetFirstorDefaultAsync(obj.Id);
                if (result.Status)
                {
                    IsRightDrawerOpen = true;
                    CurrentDto = result.Result;
                }
                
            }
            catch(Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void Save()
        {
            try
            {
                if(string.IsNullOrWhiteSpace(currentDto.Title) || string.IsNullOrWhiteSpace(currentDto.Content))
                {
                    return;
                }
                UpdateLoading(true);
                if (currentDto.Id > 0)
                {
                    var updateresult=await service.UpdateAsync(currentDto);
                    if (updateresult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(x => x.Id == currentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = currentDto.Title;
                            todo.Content = currentDto.Content;
                            todo.Status = currentDto.Status;
                            IsRightDrawerOpen = false;
                        }
                    }
                }
                else
                {
                    var addresult = await service.AddAsync(currentDto);
                    if (addresult.Status)
                    {
                        toDoDtos.Add(addresult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch(Exception ex)
            {
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public DelegateCommand<string> ExcuteCommand { get;private set; }
        public DelegateCommand<ToDoDto> SelectCommand { get; private set; }

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
                Search= Search,
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
