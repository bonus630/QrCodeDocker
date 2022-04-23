using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.corp.bonus630.plugin.ZxingQrCodeConfigurator.Lang;
using br.corp.bonus630.PluginLoader;
using br.corp.bonus630.QrCodeDocker;
using br.corp.bonus630.QrCodeDocker.Enums;
using Corel.Interop.VGCore;

namespace br.corp.bonus630.plugin.ZxingQrCodeConfigurator
{
    public class ZxingConfiguratorCore : PluginCoreBase<ZxingConfiguratorCore>, IPluginConfig
    {
        public const string PluginDisplayName = "Qrcode Configuration";
        public event Action<object, Type> GetCodeGenerator;
        public override string GetPluginDisplayName { get { return ZxingConfiguratorCore.PluginDisplayName; } }
        private bool weld;

        public bool Weld
        {
            get { return weld; }
            set { weld = value; 
                (CodeGenerator as QrCodeGenerator).Weld = value;
                base.OnNotifyPropertyChanged("Weld");
                OnUpdatePreview(); }
        }
        private bool noBorder;

        public bool NoBorder
        {
            get { return noBorder; }
            set { noBorder = value; 
                (CodeGenerator as QrCodeGenerator).NoBorder = value;
                base.OnNotifyPropertyChanged("NoBorder");
                OnUpdatePreview();
            }
        }


        private ColorSystem selectedBorderColor;
        public ColorSystem SelectedBorderColor
        {
            get { return selectedBorderColor; }
            set
            {
                
                if (value != null)
                {
                    (CodeGenerator as QrCodeGenerator).BorderColor = value.CorelColor;
                    selectedBorderColor = value;
                    OnUpdatePreview();
                    OnNotifyPropertyChanged("SelectedBorderColor");
                }
            }
        }
        private ColorSystem selectedDotColor;
        public ColorSystem SelectedDotColor
        {
            get { return selectedDotColor; }
            set
            {
                if (value != null)
                {
                    (CodeGenerator as QrCodeGenerator).DotFillColor = value.CorelColor;
                    selectedDotColor = value;
                    OnUpdatePreview();
                    OnNotifyPropertyChanged("SelectedDotColor");
                }
            
            }
        }
        private ColorSystem selectedDotBorderColor;
        public ColorSystem SelectedDotBorderColor
        {
            get { return selectedDotBorderColor; }
            set
            {
                if (value != null)
                {
                    (CodeGenerator as QrCodeGenerator).DotOutlineColor = value.CorelColor;
                    selectedDotBorderColor = value;
                    OnUpdatePreview();
                    OnNotifyPropertyChanged("SelectedDotBorderColor");
                }
            }
        }
        private double dotBorderSize;

        public double DotBorderSize
        {
            get { return dotBorderSize; }
            set { dotBorderSize = value; (CodeGenerator as QrCodeGenerator).DotBorderSize = value; OnUpdatePreview(); }
        }
        private DotShape dotShape;

        public DotShape DotShapeType
        {
            get { return dotShape; }
            set { dotShape = value; (CodeGenerator as QrCodeGenerator).DotShapeType = value; OnUpdatePreview(); }
        }


        private Corel.Interop.VGCore.Application app;
        public Corel.Interop.VGCore.Application App { get { return this.app; } set { this.app = value; } }

        private ICodeGenerator codeGenerator;
        public ICodeGenerator CodeGenerator
        {
            get { return codeGenerator; }
            set { codeGenerator = value; }
        }

        public void OnGetCodeGenerator(object sender, Type type)
        {
            if (CodeGenerator == null)
            {
                if (GetCodeGenerator != null)
                    GetCodeGenerator(sender, type);
            }
        }
        public override void LoadConfig()
        {
            NoBorder = Properties.Settings.Default.NoBorder;
            Weld = Properties.Settings.Default.Weld;
            DotShapeType = (DotShape)Properties.Settings.Default.DotShape;
            DotBorderSize = Properties.Settings.Default.DotBordeSize;
            string paletter = Properties.Settings.Default.PaletteIndentifier;
            try
            {
                Palette palette = app.PaletteManager.GetPalette(paletter);
                int index = palette.FindColor(Properties.Settings.Default.BorderColor);
                Corel.Interop.VGCore.Color bColor = palette.Color[index];
                SelectedBorderColor = new ColorSystem(bColor.HexValue, bColor.Name, bColor);
                bColor = palette.Color[palette.FindColor(Properties.Settings.Default.DotFillColor)];
                SelectedDotColor = new ColorSystem(bColor.HexValue, bColor.Name, bColor);
                bColor = palette.Color[palette.FindColor(Properties.Settings.Default.DotBorderColor)];
                SelectedDotBorderColor = new ColorSystem(bColor.HexValue, bColor.Name, bColor);
            }
            catch { }
            base.LoadConfig();
        }
        public override void SaveConfig()
        {
            QrCodeGenerator code = (CodeGenerator as QrCodeGenerator);
            Properties.Settings.Default.Weld = code.Weld;
            Properties.Settings.Default.NoBorder = code.NoBorder;
            Properties.Settings.Default.DotShape = (ushort)code.DotShapeType;
            Properties.Settings.Default.DotBordeSize = code.DotBorderSize;
            if (SelectedBorderColor != null)
                Properties.Settings.Default.BorderColor = SelectedBorderColor.CorelColorName;
            if (SelectedDotColor != null)
                Properties.Settings.Default.DotFillColor = SelectedDotColor.CorelColorName;
            if (SelectedDotBorderColor != null)
                Properties.Settings.Default.DotBorderColor = SelectedDotBorderColor.CorelColorName;
            Properties.Settings.Default.PaletteIndentifier = app.ActivePalette.Name;
            Properties.Settings.Default.Save();
            base.SaveConfig();
        }
        public override void DeleteConfig()
        {
            Properties.Settings.Default.Reset();
        }
    }
}
