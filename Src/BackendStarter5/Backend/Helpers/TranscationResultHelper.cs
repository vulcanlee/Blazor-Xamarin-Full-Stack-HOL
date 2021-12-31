using Backend.Events;
using Backend.ViewModels;
using Prism.Events;
using CommonDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Helpers
{
    public class TranscationResultHelper
    {
        public TranscationResultHelper(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public IEventAggregator EventAggregator { get; }
        public MessageBoxModel MessageBox { get; set; }
        /// <summary>
        /// 訊息對話窗設定
        /// </summary>
        public async Task CheckDatabaseResult(MessageBoxModel messageBox,
            VerifyRecordResult verifyRecordResult)
        {
            MessageBox = messageBox;
            if (verifyRecordResult.Success == false)
            {
                if(verifyRecordResult.MessageId == CommonDomain.Enums.ErrorMessageEnum.客製化文字錯誤訊息)
                {
                    string message;
                    if(verifyRecordResult.Exception==null)
                    {
                        message = $"{verifyRecordResult.Message}";
                    }
                    else
                    {
                        message = $"{verifyRecordResult.Message}，例外異常:{verifyRecordResult.Exception.Message}";
                    }
                    MessageBox.Show("400px", "200px", "發生例外異常", message, MessageBox.HiddenAsync);
                    EventAggregator.GetEvent<ToastEvent>().Publish(new ToastPayload()
                    {
                        Title = "重要通知",
                        Content = "系統發生例外異常，請查詢系統日誌系統來查看發生原因",
                        Timeout = 7000,
                        Color = "Red",
                    });

                }
            }
            await Task.Yield();
        }
    }
}
