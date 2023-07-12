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
    public class MemoViewModel: NavigationViewModel
    {

        public MemoViewModel(IMemoService service, IContainerProvider container) : base(container)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            ExcuteCommand = new DelegateCommand<String>(Excute);
            SelectCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);
            this.service = service;
        }

        private async void Delete(MemoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var result = await service.DeleteAsync(obj.Id);
                if (result.Status)
                {
                    var model = MemoDtos.FirstOrDefault(x => x.Id == obj.Id);
                    if (model != null)
                    {
                        MemoDtos.Remove(model);
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
                case "新增": Add(); break;
                case "查询": Query(); break;
                case "保存": Save(); break;
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
            set { search = value; RaisePropertyChanged(); }
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

        private MemoDto currentDto;

        /// <summary>
        /// 编辑选中/新增时的对象
        /// </summary>
        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 添加备忘录
        /// </summary>
        private void Add()
        {
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }

        private async void Selected(MemoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var result = await service.GetFirstorDefaultAsync(obj.Id);
                if (result.Status)
                {
                    IsRightDrawerOpen = true;
                    CurrentDto = result.Result;
                }

            }
            catch (Exception ex)
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
                if (string.IsNullOrWhiteSpace(currentDto.Title) || string.IsNullOrWhiteSpace(currentDto.Content))
                {
                    return;
                }
                UpdateLoading(true);
                if (currentDto.Id > 0)
                {
                    var updateresult = await service.UpdateAsync(currentDto);
                    if (updateresult.Status)
                    {
                        var todo = MemoDtos.FirstOrDefault(x => x.Id == currentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = currentDto.Title;
                            todo.Content = currentDto.Content;
                            IsRightDrawerOpen = false;
                        }
                    }
                }
                else
                {
                    var addresult = await service.AddAsync(currentDto);
                    if (addresult.Status)
                    {
                        MemoDtos.Add(addresult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
        private readonly IMemoService service;

        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataListAsync()
        {
            UpdateLoading(true);

            var memoresult = await service.GetAllAsync(new Shared.Parameter.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });
            if (memoresult.Status)
            {
                MemoDtos.Clear();
                foreach (var info in memoresult.Result.Items)
                {
                    MemoDtos.Add(info);
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
