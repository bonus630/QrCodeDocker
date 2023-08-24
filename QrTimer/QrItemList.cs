using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.QrTimer
{
    public class QrItemList : ObservableCollection<QrItem>
    {
        public void SetRunning(QrItem item, bool running)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Running = false;
            }
            item.Running = running;
        }

        public void Reorder()
        {
            if (this.Count < 2)
                return;
            QrItem item = this[0];
            this[0] = this[this.Count - 1];
            this[this.Count - 1] = item;
            item.Reset();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    
    }
}
