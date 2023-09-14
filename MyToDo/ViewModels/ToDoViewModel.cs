using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Model;
using MyToDo.Model.Parameter;
using MyToDo.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
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
        private static IDialogHostService dialogHost;

        public ToDoViewModel(IToDoService service, IContainerProvider container):base(container)
        {
            ToDoDtos = new ObservableCollection<ToDo>();
            ExcuteCommand = new DelegateCommand<String>(Excute);
            SelectCommand = new DelegateCommand<ToDo>(Selected);
            DeleteCommand = new DelegateCommand<ToDo>(Delete);
            dialogHost = container.Resolve<IDialogHostService>();
            this.service = service;
        }

        private async void Delete(ToDo obj)
        {
            try
            {
                var dialogResult=await dialogHost.Question("温馨提示",$"确认删除待办事项：{obj.Title}?");
                if (dialogResult.Result != ButtonResult.OK) return;

                UpdateLoading(true);
                var result = await service.DeleteAsync(obj.Id);
                if (result.Status)
                {
                    var model = ToDoDtos.FirstOrDefault(x => x.Id == obj.Id);
                    if (model != null)
                    {
                        ToDoDtos.Remove(model);
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

        private int selectIndex;

        /// <summary>
        /// 搜索框，下拉列表选中状态值
        /// </summary>
        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
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

        private ToDo currentDto;

        /// <summary>
        /// 编辑选中/新增时的对象
        /// </summary>
        public ToDo CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value;RaisePropertyChanged(); }
        }


        /// <summary>
        /// 添加待办
        /// </summary>
        private void Add()
        {
            CurrentDto = new ToDo();
            IsRightDrawerOpen = true;
        }

        private async void Selected(ToDo obj)
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
                    var addresult = service.Add(currentDto);
                    if (addresult.Status)
                    {
                        ToDoDtos.Add(addresult.Result);
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
        public DelegateCommand<ToDo> SelectCommand { get; private set; }
        public DelegateCommand<ToDo> DeleteCommand { get; private set; }

        private ObservableCollection<ToDo> toDoDtos;
        private readonly IToDoService service;

        public ObservableCollection<ToDo> ToDoDtos
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

            int? status =SelectIndex == 0 ? null : SelectIndex == 2 ? 1 : 0;

            var todoresult = await service.GetAllFilterAsync(new ToDoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = status,
            }); 
            if (todoresult.Status)
            {
                ToDoDtos.Clear();
                foreach(var info in todoresult.Result)
                {
                    ToDoDtos.Add(info);
                }
            }
            UpdateLoading(false);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (navigationContext.Parameters.ContainsKey("Value"))
            {
                SelectIndex=navigationContext.Parameters.GetValue<int>("Value");
            }
            else
            {
                SelectIndex = 0;
            }

            GetDataListAsync();
        }
    }
}
