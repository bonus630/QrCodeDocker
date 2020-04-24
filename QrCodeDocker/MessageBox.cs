using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.QrCodeDocker
{
    public static class MessageBox
    {
        public enum DialogButtons
        {
            Ok = 0,
            OkCancel = 1,
            AbortTryIgnor = 2,
            YesNoCancel = 3,
            YesNo = 4,
            TryAgainCancel = 5,
            CancelTryAgainContinue = 6,
            None = 7
        }
        public enum DialogResult
        {
            Ok = 1,
            CancelClose = 2,
            Abort = 3,
            TryAgain = 4,
            Ignor = 5,
            Yes = 6,
            No = 7,
            TryAgainContinue = 10
        }
        public static DialogResult MsgShow(this Corel.Interop.VGCore.Application corelApp, string message)
        {
            return corelApp.MsgShow(message, "", DialogButtons.Ok);
        }
        public static DialogResult MsgShow(this Corel.Interop.VGCore.Application corelApp, string message, string caption)
        {
            return corelApp.MsgShow(message, caption, DialogButtons.Ok);
        }
        public static DialogResult MsgShow(this Corel.Interop.VGCore.Application corelApp, string message, string caption,DialogButtons buttons)
        {
            
            try
            {
#if X7
                int result = (int)System.Windows.MessageBox.Show(message,caption,(System.Windows.MessageBoxButton)((int)buttons));

#elif X8
                int result = (int)System.Windows.MessageBox.Show(message,caption,(System.Windows.MessageBoxButton)((int)buttons));
#else
               int result = corelApp.FrameWork.ShowMessageBox(message, caption,(int)buttons);
               
#endif
                return (DialogResult)result;
            }
            catch { return DialogResult.Ignor; }
        }
       
       
    }
}
