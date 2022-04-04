using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.PlaceHere
{
    public class BackgroundTask //: ICUIRunningTask, ICUIBackgroundTask
    {
        public bool Running { get; set; }
        private Application app;
        private Action drawFunction;

        public string Name
        {
            get
            {
                return "Export Task";
            }
        }
        public BackgroundTask(Application app,Action drawFunction)
        {
            this.app = app;
            this.drawFunction = drawFunction;
        }

        public void FinalizeTask()
        {
            Running = false;
        }

        public void FreeTask()
        {
            Running = false;
        }

        public void QuitTask()
        {
            Running = false;
        }

        public void TryAbort()
        {
            Running = false;
        }
        public void RunTask()
        {
            Running = true;
            drawFunction.Invoke();
        }
    }
}
