using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.plugin.QrTimer
{
    public class QrItem : INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; 
                OnPropertyChanged("Content"); }
        }

        private int _time;
        private int time;

        public int Time
        {
            get { return time; }
            set { time = value;
                OnPropertyChanged("Time");
            }
        }

        private bool running;

        public bool Running
        {
            get { return running; }
            set { running = value;
                OnPropertyChanged("Running");
            }
        }

        public void Reset()
        {
            Time = _time;
        }
        public QrItem(string content,int time)
        {
            this.Content = content;
            this._time = time;
            this.Time = time;
        }
    }
}
