using System;
using System.Windows.Forms;
using LiveSplit.UI;

namespace LiveSplit.Quake2_100
{
    partial class Settings : UserControl
    {
        public int ListSize { get; set; }
        public bool ShowKills
        {
            get { return MapInfoComponent.ShowKills; }
            set { MapInfoComponent.ShowKills = value; }
        }
        public bool ShowSecrets
        {
            get { return MapInfoComponent.ShowSecrets; }
            set { MapInfoComponent.ShowSecrets = value; }
        }

        public LayoutMode Mode { get; set; }

        public Settings()
        {
            InitializeComponent();

            // defaults
            ListSize = 4;

            // assign data bindings
            numListSize.DataBindings.Add("Value", this, "ListSize");
            chkShowKills.DataBindings.Add("Checked", this, "ShowKills");
            chkShowSecrets.DataBindings.Add("Checked", this, "ShowSecrets");
        }

        private void ColorButtonClick(object sender, EventArgs e)
        {
            SettingsHelper.ColorButtonClick((Button)sender, this);
        }

        public void SetSettings(System.Xml.XmlNode node)
        {
            System.Xml.XmlElement element = (System.Xml.XmlElement)node;

            ListSize = SettingsHelper.ParseInt(element["ListSize"]);
            ShowKills = SettingsHelper.ParseBool(element["ShowKills"], true);
            ShowSecrets = SettingsHelper.ParseBool(element["ShowSecrets"], true);
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
            return SettingsHelper.CreateSetting(document, parent, "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) ^
            SettingsHelper.CreateSetting(document, parent, "ListSize", ListSize) ^
            SettingsHelper.CreateSetting(document, parent, "ShowKills", ShowKills) ^
            SettingsHelper.CreateSetting(document, parent, "ShowSecrets", ShowSecrets);
        }
    }
}