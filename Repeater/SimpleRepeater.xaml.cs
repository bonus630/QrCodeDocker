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
using br.corp.bonus630.plugin.Repeater.Lang;

namespace br.corp.bonus630.plugin.Repeater
{

    /// <summary>
    /// Interaction logic for SimpleRepeater.xaml
    /// </summary>
    public partial class SimpleRepeater : UserControl, IPluginUI, IPluginDrawer
    {
        double size;
        Corel.Interop.VGCore.Application app;
        br.corp.bonus630.ImageRender.IImageRender imageRender;
        Ilang Lang;
        RepeaterCore core;
        private int qrcodeContentIndex = 0;
        private List<object[]> dataSource;
        private ItemTuple<Shape> shapeContainer;
        private List<ItemTuple<Shape>> shapeContainerText = new List<ItemTuple<Shape>>();
        private List<ItemTuple<Shape>> shapeContainerEnumerator = new List<ItemTuple<Shape>>();
        private List<ItemTuple<Shape>> shapeContainerImageFile = new List<ItemTuple<Shape>>();

        public event Action<object> FinishJob;
        public event Action<int> ProgressChange;
        public event Action<string> AnyTextChanged;
        public event Action UpdatePreview;
        public string PluginDisplayName { get{return RepeaterCore.PluginDisplayName;} }
        private ICodeGenerator codeGenerator;

        public ICodeGenerator CodeGenerator
        {
            get { return codeGenerator; }
            set { codeGenerator = value; }
        }


        public SimpleRepeater()
        {
            InitializeComponent();
            core = new RepeaterCore();
            core.FinishJob += Core_FinishJob;
            core.ProgressChange += Core_ProgressChange;
            
        }
        public void ChangeLang(LangTagsEnum langTag)
        {
            Lang = LangController.CreateInstance(Assembly.GetAssembly(typeof(br.corp.bonus630.plugin.Repeater.SimpleRepeater)), langTag) as Ilang;
            this.DataContext = Lang;
            (Lang as LangController).AutoUpdateProperties();
        }
        private void Core_ProgressChange(int obj)
        {
            OnProgressChange(obj);
        }

        public double Size
        {
            set { size = value; }
        }

        public Corel.Interop.VGCore.Application App
        {
            set
            {
                app = value;

            }
        }
        public br.corp.bonus630.ImageRender.IImageRender ImageRender
        {
            set { this.imageRender = value; }
        }

        public List<object[]> DataSource { set { this.dataSource = value; FillButtons(); } }

        public int Index { get ; set; }

        private void FillButtons()
        {
            if(this.dataSource != null && this.dataSource.Count > 0)
            {
                stackPanel_buttons.Visibility = Visibility.Visible;
                scrollViewer_buttons.Visibility = Visibility.Visible;
                stackPanel_buttons.Children.Clear();
                object[] firstLine = this.dataSource[0];
            
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
                    combo.Items.Add(new ItemTuple<ItemType>(i,ItemType.Code));
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
               
            }
        }

        private void Combo_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            ItemTuple<ItemType> item = cb.SelectedItem as ItemTuple<ItemType>;
            if(item != null && this.app.ActiveDocument != null)
            {
                
                if(!GetContainer(item.Index, item.Item))
                {
                    (sender as ComboBox).SelectedIndex = -1;
                }
            }
            
        }

        private void btn_process_Click(object sender, RoutedEventArgs e)
        {
            Draw();
            
        }

        private void Core_FinishJob(object obj)
        {
            if(FinishJob!=null)
                FinishJob(obj);
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
                app.MsgShow(Lang.MBOX_ERROR_PerfectSquare);
                return false;
            }
            if(shape.Type != cdrShapeType.cdrTextShape && type == ItemType.Text)
            {
                app.MsgShow(Lang.MBOX_ERROR_TextShape);
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
            if (app.MsgShow(Lang.MBOX_ERROR_CorrectShape, Lang.Warning, QrCodeDocker.MessageBox.DialogButtons.YesNo) != QrCodeDocker.MessageBox.DialogResult.Yes)
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

        public void OnProgressChange(int progress)
        {
            if (ProgressChange != null)
                ProgressChange(progress);
        }

        public void OnFinishJob(object obj)
        {
            if (FinishJob != null)
                FinishJob(obj);
        }

        public void Draw()
        {
            if (dataSource == null)
            {
                app.MsgShow(Lang.MBOX_ERROR_ValidDataSource);
                return;
            }
            if (dataSource.Count == 0)
            {
                app.MsgShow(Lang.MBOX_ERROR_DataSourceEmpty);
                return;
            }
            if (app.ActiveSelectionRange == null)
            {
                app.MsgShow(Lang.MBOX_ERROR_NoShapes);
                return;
            }
            if (app.ActiveSelection.Shapes.Count == 0)
            {
                app.MsgShow(Lang.MBOX_ERROR_NoShapes);
                return;
            }
            SetCoreValues();
            this.app.ActiveDocument.Unit = this.app.ActiveDocument.Rulers.HUnits;
            core.Size = this.size;
            core.App = this.app;
            core.CodeGenerator = this.codeGenerator;
            //core.ImageRender = this.imageRender;
            core.DataSource = dataSource;
            core.ShapeContainerText = this.shapeContainerText;
            core.ShapeContainerImageFile = this.shapeContainerImageFile;
            core.ShapeContainerEnumerator = this.shapeContainerEnumerator;
            bool vector = (bool)cb_vector.IsChecked;
            core.ShapeContainer = this.shapeContainer;
            core.ModelShape = app.ActiveSelectionRange;
         
            //Task t = new Task(() =>
            //{
            if(this.shapeContainer==null)
            {
                core.ProcessVector();
                return;
            }
            if (vector)
                    core.ProcessVector(this.shapeContainer.Index);

                else
                    core.ProcessVector(this.shapeContainer.Index,false);

            //});
            //t.Start();
        }
        private void SetCoreValues()
        {
            double val = 0;
            if (Double.TryParse(txt_gap.Text, out val))
                core.Gap = val;
            if (Double.TryParse(txt_startX.Text, out val))
                core.StartX = val;
            if (Double.TryParse(txt_startY.Text, out val))
                core.StartY = val;
            int val1 = 0;
            if (Int32.TryParse(txt_increment.Text, out val1))
                core.EnumeratorIncrement = val1;
            if (Int32.TryParse(txt_initialValue.Text, out val1))
                core.Enumerator = val1;
            if (!string.IsNullOrEmpty(txt_mask.Text))
                core.Mask = txt_mask.Text;
        }
        private void cb_fitToPage_Click(object sender, RoutedEventArgs e)
        {
            core.FitToPage = (bool)cb_fitToPage.IsChecked;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn_enumerator.IsEnabled = false;
            GetContainer(shapeContainerEnumerator.Count, ItemType.Enumerator);
            btn_enumerator.IsEnabled = true;
        }

        public void SaveConfig()
        {
            SetCoreValues();
            Properties.Settings1.Default.QrFormatVector = (bool)cb_vector.IsChecked;
            Properties.Settings1.Default.InitialValue = core.Enumerator;
            Properties.Settings1.Default.Increment = core.EnumeratorIncrement;
            Properties.Settings1.Default.Mask = core.Mask;
            Properties.Settings1.Default.FitToPage = core.FitToPage;
            Properties.Settings1.Default.StartX = core.StartX;
            Properties.Settings1.Default.Starty = core.StartY;
            Properties.Settings1.Default.Gap = core.Gap;
            Properties.Settings1.Default.Save();


        }

        public void LoadConfig()
        {
            if (Properties.Settings1.Default.QrFormatVector)
                cb_vector.IsChecked = true;
            else
                cb_bitmap.IsChecked = true;

            txt_initialValue.Text = Properties.Settings1.Default.InitialValue.ToString();
            txt_increment.Text = Properties.Settings1.Default.Increment.ToString();
            txt_mask.Text = Properties.Settings1.Default.Mask.ToString();
            cb_fitToPage.IsChecked = Properties.Settings1.Default.FitToPage;
            txt_startX.Text = Properties.Settings1.Default.StartX.ToString();
            txt_startY.Text = Properties.Settings1.Default.Starty.ToString();
            txt_gap.Text = Properties.Settings1.Default.Gap.ToString();
        }

        public void DeleteConfig()
        {
            Properties.Settings1.Default.Reset();
        }
    }
}
