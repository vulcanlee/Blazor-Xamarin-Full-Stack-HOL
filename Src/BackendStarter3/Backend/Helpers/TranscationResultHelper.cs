using Backend.ViewModels;
using Prism.Events;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class TranscationResultHelper
    {
        public TranscationResultHelper(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public IEventAggregator EventAggregator { get; }

        /// <summary>
        /// 訊息對話窗設定
        /// </summary>
        public async Task CheckDatabaseResult(MessageBoxModel messageBox,
            VerifyRecordResult verifyRecordResult)
        {
            if(verifyRecordResult.Success == false)
            {
                if(verifyRecordResult.MessageId == ShareDomain.Enums.ErrorMessageEnum.客製化文字錯誤訊息)
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
                    messageBox.Show("400px", "200px", "發生例外異常", message);
                }
            }
            await Task.Yield();
        }
    }
}
