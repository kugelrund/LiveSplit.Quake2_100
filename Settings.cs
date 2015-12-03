using LiveSplit.UI;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace LiveSplit.Quake2_100
{
    partial class Settings : UserControl
    {
        public bool Display2Rows { get; set; }

        public System.Drawing.Color BackgroundColor1 { get; set; }
        public System.Drawing.Color BackgroundColor2 { get; set; }
        public GradientType BackgroundGradient { get; set; }
        public string GradientString
        {
            get { return BackgroundGradient.ToString(); }
            set { BackgroundGradient = (GradientType)Enum.Parse(typeof(GradientType), value); }
        }

        public string LeftText { get; set; }
        public string RightText { get; set; }

        public string ProcessName { get; set; }

        public int ListSize { get; set; }

        public LayoutMode Mode { get; set; }

        public Settings()
        {
            InitializeComponent();

            ProcessName = "";
            ListSize = 4;
            
            BackgroundColor1 = System.Drawing.Color.Transparent;
            BackgroundColor2 = System.Drawing.Color.Transparent;
            BackgroundGradient = GradientType.Plain;
            LeftText = "Text";
            RightText = "";
            
            cmbGradientType.DataBindings.Add("SelectedItem", this, "GradientString", false, DataSourceUpdateMode.OnPropertyChanged);
            btnBackgroundColor1.DataBindings.Add("BackColor", this, "BackgroundColor1", false, DataSourceUpdateMode.OnPropertyChanged);
            btnBackgroundColor2.DataBindings.Add("BackColor", this, "BackgroundColor2", false, DataSourceUpdateMode.OnPropertyChanged);
            txtLeftText.DataBindings.Add("Text", this, "LeftText");
            txtRightText.DataBindings.Add("Text", this, "RightText");
        }     
        
        private void ColorButtonClick(object sender, EventArgs e)
        {
            SettingsHelper.ColorButtonClick((Button)sender, this);
        }

        private void cmbGradientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnBackgroundColor1.Visible = cmbGradientType.SelectedItem.ToString() != "Plain";
            btnBackgroundColor2.DataBindings.Clear();
            btnBackgroundColor2.DataBindings.Add("BackColor", this, btnBackgroundColor1.Visible ? "BackgroundColor2" : "BackgroundColor1", false, DataSourceUpdateMode.OnPropertyChanged);
            GradientString = cmbGradientType.SelectedItem.ToString();
        }

        public void SetSettings(System.Xml.XmlNode node)
        {
            System.Xml.XmlElement element = (System.Xml.XmlElement) node;
            
            ProcessName = SettingsHelper.ParseString(element["ProcessName"]);
            leftTextOverrideControl.OverridingColor = SettingsHelper.ParseColor(element["LeftTextColor"]);
            leftTextOverrideControl.OverridingFont = SettingsHelper.GetFontFromElement(element["LeftTextFont"]);
            leftTextOverrideControl.OverrideColor = SettingsHelper.ParseBool(element["LeftTextOverrideColor"]);
            leftTextOverrideControl.OverrideFont = SettingsHelper.ParseBool(element["LeftTextOverrideFont"]);

            rightTextOverrideControl.OverridingColor = SettingsHelper.ParseColor(element["RightTextColor"]);
            rightTextOverrideControl.OverridingFont = SettingsHelper.GetFontFromElement(element["RightTextFont"]);
            rightTextOverrideControl.OverrideColor = SettingsHelper.ParseBool(element["RightTextOverrideColor"]);
            rightTextOverrideControl.OverrideFont = SettingsHelper.ParseBool(element["RightTextOverrideFont"]);
            
            BackgroundColor1 = SettingsHelper.ParseColor(element["BackgroundColor1"]);
            BackgroundColor2 = SettingsHelper.ParseColor(element["BackgroundColor2"]);
            GradientString = SettingsHelper.ParseString(element["BackgroundGradient"]);
            LeftText = SettingsHelper.ParseString(element["LeftText"]);
            RightText = SettingsHelper.ParseString(element["RightText"]);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            System.Xml.XmlElement parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        private int CreateSettingsNode(System.Xml.XmlDocument document, System.Xml.XmlElement parent)
        {
            int hash = SettingsHelper.CreateSetting(document, parent, "Version", "1.0.0");

            return hash ^
            SettingsHelper.CreateSetting(document, parent, "ProcessName", ProcessName) ^
            SettingsHelper.CreateSetting(document, parent, "LeftTextColor", leftTextOverrideControl.OverridingColor) ^
            SettingsHelper.CreateSetting(document, parent, "LeftTextFont", leftTextOverrideControl.OverridingFont) ^
            SettingsHelper.CreateSetting(document, parent, "LeftTextOverrideColor", leftTextOverrideControl.OverrideColor) ^
            SettingsHelper.CreateSetting(document, parent, "LeftTextOverrideFont", leftTextOverrideControl.OverrideFont) ^
            SettingsHelper.CreateSetting(document, parent, "RightTextColor", rightTextOverrideControl.OverridingColor) ^
            SettingsHelper.CreateSetting(document, parent, "RightTextFont", rightTextOverrideControl.OverridingFont) ^
            SettingsHelper.CreateSetting(document, parent, "RightTextOverrideColor", rightTextOverrideControl.OverrideColor) ^
            SettingsHelper.CreateSetting(document, parent, "RightTextOverrideFont", rightTextOverrideControl.OverrideFont) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundColor1", BackgroundColor1) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundColor2", BackgroundColor2) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundGradient", BackgroundGradient) ^
            SettingsHelper.CreateSetting(document, parent, "LeftText", LeftText) ^
            SettingsHelper.CreateSetting(document, parent, "RightText", RightText);
        }

        public static bool TryParseHex(string str, out int integer)
        {
            integer = 0;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            else
            {
                if (str.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                {
                    str = str.Substring(2);
                }

                return int.TryParse(str, NumberStyles.HexNumber, null, out integer);
            }
        }
    }
}
