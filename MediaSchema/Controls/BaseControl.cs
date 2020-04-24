using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace br.corp.bonus630.plugin.MediaSchema.Controls
{
    public partial class BaseControl : UserControl
    {
        public Schemes CurrentScheme { get; set; }
        public BaseControl(Schemes scheme)
        {
            InitializeComponent();
            this.CurrentScheme = scheme;
        }
        private void OnAnyTextChange(object sender)
        {
            string attributeName = (sender as Control).Tag.ToString();
            if(sender is TextBox)
            {

            }
            if(sender is RadioButton)
            {

            }
            if(sender is CheckBox)
            {

            }
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            OnAnyTextChange(sender);
        }
    }
}
