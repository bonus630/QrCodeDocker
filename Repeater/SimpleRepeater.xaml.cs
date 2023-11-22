using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Corel.Interop.VGCore;
using br.corp.bonus630.QrCodeDocker;
using br.corp.bonus630.PluginLoader;
using System.Reflection;

namespace br.corp.bonus630.plugin.Repeater
{

    /// <summary>
    /// Interaction logic for SimpleRepeater.xaml
    /// </summary>
    public partial class SimpleRepeater : UserControl, IPluginMainUI
    {

        Corel.Interop.VGCore.Application app;

        RepeaterCore rCore;
        private ItemTuple<Shape> shapeContainer;
        private List<ItemTuple<Shape>> shapeContainerText = new List<ItemTuple<Shape>>();
        private List<ItemTuple<Shape>> shapeContainerEnumerator = new List<ItemTuple<Shape>>();
        private List<ItemTuple<Shape>> shapeContainerImageFile = new List<ItemTuple<Shape>>();




        public SimpleRepeater()
        {
            InitializeComponent();
            this.Loaded += SimpleRepeater_Loaded;

        }

        private void SimpleRepeater_Loaded(object sender, RoutedEventArgs e)
        {
            rCore = Core as RepeaterCore;
            this.app = rCore.App;
            rCore.LoadConfigEvent += RCore_LoadConfigEvent;
            FillButtons();
        }

        private void RCore_LoadConfigEvent()
        {
            LoadConfig();
        }


        public IPluginCore Core { get; set; }

        public void FillButtons()
        {
            if (rCore!=null && rCore.DataSource != null && rCore.DataSource.Count > 0)
            {

                try
                {
                    stackPanel_buttons.Children.Clear();
                    object[] firstLine = rCore.DataSource[0];

                    for (int i = 0; i < firstLine.Length; i++)
                    {
                        System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
                        System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                        System.Windows.Controls.ComboBox combo = new System.Windows.Controls.ComboBox();
                        grid.Width = 74;

                        label.Content = firstLine[i].ToString();
                        label.Height = 26;
                        label.Width = 70;
                        label.Margin = new Thickness(2, 0, 2, 26);
                        ToolTip toolTip = new ToolTip();
                        toolTip.Content = firstLine[i].ToString();
                        label.ToolTip = toolTip;
                        combo.Items.Add(new ItemTuple<ItemType>(i, ItemType.Code));
                        combo.Items.Add(new ItemTuple<ItemType>(i, ItemType.Text));
                        combo.Items.Add(new ItemTuple<ItemType>(i, ItemType.ImagePath));
                        combo.Margin = new Thickness(2, 26, 2, 0);
                        combo.Height = 26;
                        combo.Width = 70;
                        combo.DropDownClosed += Combo_DropDownClosed;
                        grid.Children.Add(label);
                        grid.Children.Add(combo);


                        stackPanel_buttons.Children.Add(grid);

                    }
                    stackPanel_buttons.Visibility = Visibility.Visible;
                    scrollViewer_buttons.Visibility = Visibility.Visible;
                }
                catch { }

            }
        }

        private void Combo_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            ItemTuple<ItemType> item = cb.SelectedItem as ItemTuple<ItemType>;
            if (item != null && this.app.ActiveDocument != null)
            {

                if (!GetContainer(item.Index, item.Item))
                {
                    (sender as ComboBox).SelectedIndex = -1;
                }
            }

        }

        private void btn_process_Click(object sender, RoutedEventArgs e)
        {
            Draw();

        }

        private bool GetContainer(int index, ItemType type)
        {
            bool preservSelection = app.ActiveDocument.PreserveSelection;
            app.ActiveDocument.PreserveSelection = true;
            double x = 0;
            double y = 0;
            int shift = 0;

            app.ActiveDocument.GetUserClick(out x, out y, out shift, 0, false, Corel.Interop.VGCore.cdrCursorShape.cdrCursorWinCross);


#if X7

            Shape shapes = app.ActiveDocument.ActivePage.SelectShapesAtPoint(x, y, false);
            Shape shape = shapes.Shapes[1];
            for (int i = 1; i <= shapes.Shapes.Count; i++)
            {
                if (shape.ZOrder > shapes.Shapes[i].ZOrder)
                    shape = shapes.Shapes[i];
            }
            

#else
            Shape shape = app.ActiveDocument.ActivePage.FindShapeAtPoint(x, y);
#endif
            app.ActiveDocument.PreserveSelection = preservSelection;
            if (shape == null)
                return false;




            if (shape.SizeWidth != shape.SizeHeight && type == ItemType.Code)
            {
                app.MsgShow(rCore.GetLocalizedString("MBOX_ERROR_PerfectSquare"));
                return false;
            }
            if (shape.Type != cdrShapeType.cdrTextShape && type == ItemType.Text)
            {
                app.MsgShow(rCore.GetLocalizedString("MBOX_ERROR_TextShape"));
                return false;
            }

            if (type == ItemType.Code)
            {
                Corel.Interop.VGCore.Color redColor = new Corel.Interop.VGCore.Color();
                redColor.RGBAssign(255, 0, 0);
                app.Optimization = true;
                app.ActiveDocument.BeginCommandGroup();
                shape.Fill.ApplyNoFill();
                shape.Outline.Color = redColor;
                shape.Outline.PenWidth = 0.5;


                //Shape line  = shape.Layer.CreateLineSegment(shape.RightX, shape.TopY, shape.LeftX, shape.BottomY);
                //line.Outline.Color = redColor;

                //Shape line2 = shape.Layer.CreateLineSegment(shape.RightX, shape.BottomY, shape.LeftX, shape.TopY);
                //line2.Outline.Color = redColor;

                //Shape we = shape.Weld(line);
                //shape = we.Weld(line2);
                shape.Name = "container";
                app.ActiveDocument.EndCommandGroup();
                app.Optimization = false;
                app.Refresh();
                shapeContainer = new ItemTuple<Shape>(index, shape);
            }
            if (type == ItemType.Text)
            {
                if (string.IsNullOrEmpty(shape.Name))
                    shape.Name = string.Format("Text-container {0}", index);
                shapeContainerText.Add(new ItemTuple<Shape>(index, shape));
            }
            if (type == ItemType.Enumerator)
            {
                if (string.IsNullOrEmpty(shape.Name))
                    shape.Name = string.Format("Enumetor-container {0}", index);
                shapeContainerEnumerator.Add(new ItemTuple<Shape>(index, shape));
            }
            if (type == ItemType.ImagePath)
            {
                if (string.IsNullOrEmpty(shape.Name))
                    shape.Name = string.Format("file-container {0}", index);
                shapeContainerImageFile.Add(new ItemTuple<Shape>(index, shape));
            }
            //  core.ShapeContainer = shapeContainer;
            // core.Size = shapeContainer.SizeWidth;
            if (app.MsgShow(rCore.GetLocalizedString("MBOX_ERROR_CorrectShape"), 
                rCore.GetLocalizedString("Warning"), QrCodeDocker.MessageBox.DialogButtons.YesNo) != QrCodeDocker.MessageBox.DialogResult.Yes)
            {
                this.app.ActiveDocument.Undo();
                if (type == ItemType.Code)
                    shapeContainer = null;
                if (type == ItemType.Text)
                    shapeContainerText.RemoveAt(shapeContainerText.Count - 1);
                if (type == ItemType.ImagePath)
                    shapeContainerImageFile.RemoveAt(shapeContainerText.Count - 1);
                return false;
            }
            this.Focus();
            return true;
        }



        public void Draw()
        {
            if (rCore.DataSource == null)
            {
                app.MsgShow(rCore.GetLocalizedString("MBOX_ERROR_ValidDataSource"));
                return;
            }
            if (rCore.DataSource.Count == 0)
            {
                app.MsgShow(rCore.GetLocalizedString("MBOX_ERROR_DataSourceEmpty"));
                return;
            }
            if (app.ActiveSelectionRange == null)
            {
                app.MsgShow(rCore.GetLocalizedString("MBOX_ERROR_NoShapes"));
                return;
            }
            if (app.ActiveSelection.Shapes.Count == 0)
            {
                app.MsgShow(rCore.GetLocalizedString("MBOX_ERROR_NoShapes"));
                return;
            }

            this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;

            rCore.ShapeContainerText = this.shapeContainerText;
            rCore.ShapeContainerImageFile = this.shapeContainerImageFile;
            rCore.ShapeContainerEnumerator = this.shapeContainerEnumerator;
            bool vector = (bool)cb_vector.IsChecked;
            rCore.ShapeContainer = this.shapeContainer;
            rCore.ModelShape = app.ActiveSelectionRange;


            //if (this.shapeContainer == null)
            //{
            //    rCore.ProcessVector();
            //    return;
            //}
            //rCore.ProcessVector(this.shapeContainer.Index,vector);

            if (this.shapeContainer == null)
            {
                rCore.Draw();
                return;
            }
            rCore.Draw((bool)ck_useThread.IsChecked,this.shapeContainer.Index, vector);


        }

        private void cb_fitToPage_Click(object sender, RoutedEventArgs e)
        {
            rCore.FitToPage = (bool)cb_fitToPage.IsChecked;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn_enumerator.IsEnabled = false;
            GetContainer(shapeContainerEnumerator.Count, ItemType.Enumerator);
            btn_enumerator.IsEnabled = true;
        }



        public void LoadConfig()
        {
            if (Properties.Settings1.Default.QrFormatVector)
                cb_vector.IsChecked = true;
            else
                cb_bitmap.IsChecked = true;
            txt_initialValue.Text = rCore.Enumerator.ToString();
            txt_increment.Text = rCore.EnumeratorIncrement.ToString();
            txt_mask.Text = rCore.Mask;
            cb_fitToPage.IsChecked = rCore.FitToPage;
            txt_startX.Text = rCore.StartX.ToString();
            txt_startY.Text = rCore.StartY.ToString();
            txt_gap.Text = rCore.Gap.ToString();
            ck_ignoreFirstLine.IsChecked = rCore.IgnoreFirstLine;

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            rCore.IgnoreFirstLine = (bool)ck_ignoreFirstLine.IsChecked;
        }
    }
}
