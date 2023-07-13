using MyToDo.Common;
using MyToDo.Common.Events;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Extensions
{
    public static class DialogExtensions
    {
        /// <summary>
        /// 询问窗口
        /// </summary>
        /// <param name="dialogHost">指定的dialoghost会话主机</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="dialoghostname">会话主机名称（唯一）</param>
        /// <returns></returns>
        public static async Task<IDialogResult> Question(this IDialogHostService dialogHost,string title,string content,string dialoghostname="Root")
        {
            DialogParameters param = new DialogParameters();
            param.Add("Title", title);
            param.Add("Content", content);
            param.Add("dialogHostName", dialoghostname);
            return await dialogHost.ShowDialog("MsgView",param,dialoghostname);
        }

        /// <summary>
        /// 推送等待消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="model"></param>
        public static void UpdateLoading(this IEventAggregator aggregator,UpdateModel model)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);
        }

        /// <summary>
        /// 注册等待消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="model"></param>
        public static void Resgiter(this IEventAggregator aggregator,Action<UpdateModel> action)
        {
            aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action);
        }

        /// <summary>
        /// 注册提示消息事件
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterMsg(this IEventAggregator aggregator,Action<string> action)
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(action);
        }

        /// <summary>
        /// 发送提示消息事件
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void SendMessage(this IEventAggregator aggregator, string message)
        {
            aggregator.GetEvent<MessageEvent>().Publish(message);
        }
    }
}
