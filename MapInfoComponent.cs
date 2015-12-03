using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Quake2_100
{
    class MapInfoComponent : IComponent
    {
        private bool textChanged;

        private MapInfo mapInfo;
        public MapInfo MapInfo
        {
            set
            {
                mapInfo = value;
                MapLabel.Text = mapInfo.Name;
                KillsLabel.Text = mapInfo.KillsString;
                SecretsLabel.Text = mapInfo.SecretsString;
                textChanged = true;
            }
        }

        protected SimpleLabel MapLabel { get; set; }
        protected SimpleLabel KillsLabel { get; set; }
        protected SimpleLabel SecretsLabel { get; set; }

        private Settings settings;

        public GraphicsCache Cache { get; set; }

        public float PaddingTop => 0f;
        public float PaddingLeft => 0f;
        public float PaddingBottom => 0f;
        public float PaddingRight => 0f;       

        public float VerticalHeight => 25;

        public float MinimumWidth { get; set; }

        public float HorizontalWidth => CalculateLabelsWidth();

        public float MinimumHeight { get; set; }

        public IDictionary<string, Action> ContextMenuControls => null;

        public MapInfoComponent(Settings settings, MapInfo mapInfo)
        {
            this.settings = settings;

            MapLabel = new SimpleLabel()
            {
                HorizontalAlignment = StringAlignment.Near,
                X = 5,
            };
            KillsLabel = new SimpleLabel()
            {
                HorizontalAlignment = StringAlignment.Near
            };
            SecretsLabel = new SimpleLabel()
            {
                HorizontalAlignment = StringAlignment.Far
            };

            MapInfo = mapInfo;
        }

        private void DrawGeneral(Graphics g, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            MapLabel.Font = state.LayoutSettings.TextFont;
            MapLabel.ShadowColor = state.LayoutSettings.ShadowsColor;
            MapLabel.SetActualWidth(g);
            MapLabel.ForeColor = state.LayoutSettings.TextColor;

            KillsLabel.Font = state.LayoutSettings.TextFont;
            KillsLabel.ShadowColor = state.LayoutSettings.ShadowsColor;
            KillsLabel.SetActualWidth(g);
            KillsLabel.ForeColor = (mapInfo.Kills == mapInfo.MaxKills) ? 
                                   state.LayoutSettings.AheadGainingTimeColor :
                                   state.LayoutSettings.BehindLosingTimeColor;

            SecretsLabel.Font = state.LayoutSettings.TextFont;
            SecretsLabel.ShadowColor = state.LayoutSettings.ShadowsColor;
            SecretsLabel.SetActualWidth(g);
            SecretsLabel.ForeColor = (mapInfo.Secrets == mapInfo.MaxSecrets) ?
                                     state.LayoutSettings.AheadGainingTimeColor :
                                     state.LayoutSettings.BehindLosingTimeColor;

            MinimumWidth = CalculateLabelsWidth() + 10;
            MinimumHeight = 0.85f * (g.MeasureString("A", state.LayoutSettings.TimesFont).Height + g.MeasureString("A", state.LayoutSettings.TextFont).Height);

            if (mode == LayoutMode.Vertical)
            {
                MapLabel.VerticalAlignment = StringAlignment.Center;
                MapLabel.Y = 0;
                MapLabel.Height = height;

                KillsLabel.VerticalAlignment = StringAlignment.Center;
                KillsLabel.Y = 0;
                KillsLabel.Height = height;

                SecretsLabel.VerticalAlignment = StringAlignment.Center;
                SecretsLabel.Y = 0;
                SecretsLabel.Height = height;
            }
            else
            {
                MapLabel.VerticalAlignment = StringAlignment.Near;
                MapLabel.Y = 0;
                MapLabel.Height = 50;

                KillsLabel.VerticalAlignment = StringAlignment.Near;
                KillsLabel.Y = 0;
                KillsLabel.Height = 50;

                SecretsLabel.VerticalAlignment = StringAlignment.Near;
                SecretsLabel.Y = 0;
                SecretsLabel.Height = 50;
            }

            var curX = width - 5;
            var nameX = width - 5;

            SecretsLabel.Width = SecretsLabel.ActualWidth + 40;
            curX -= SecretsLabel.ActualWidth + 5;
            SecretsLabel.X = curX - 35;
            SecretsLabel.Draw(g);

            KillsLabel.Width = KillsLabel.ActualWidth + 40;
            curX -= KillsLabel.ActualWidth + 5;
            KillsLabel.X = curX - 35;
            KillsLabel.Draw(g);

            nameX = curX + 5;
            MapLabel.Width = (mode == LayoutMode.Horizontal ? width - 10 : nameX);
            MapLabel.Draw(g);
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            if (settings.Display2Rows)
            {
                DrawGeneral(g, state, width, VerticalHeight, LayoutMode.Horizontal);
            }
            else
            {
                DrawGeneral(g, state, width, VerticalHeight, LayoutMode.Vertical);
            }
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            DrawGeneral(g, state, HorizontalWidth, height, LayoutMode.Horizontal);
        }

        public string ComponentName => "MapInfo";


        public Control GetSettingsControl(LayoutMode mode)
        {
            throw new NotSupportedException();
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            throw new NotSupportedException();
        }


        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            throw new NotSupportedException();
        }

        public string UpdateName
        {
            get { throw new NotSupportedException(); }
        }

        public string XMLURL
        {
            get { throw new NotSupportedException(); }
        }

        public string UpdateURL
        {
            get { throw new NotSupportedException(); }
        }

        public Version Version
        {
            get { throw new NotSupportedException(); }
        }
        
        protected float CalculateLabelsWidth()
        {
            return MapLabel.ActualWidth + KillsLabel.ActualWidth + SecretsLabel.ActualWidth;
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (invalidator != null && textChanged)
            {
                invalidator.Invalidate(0, 0, width, height);
                textChanged = false;
            }
        }

        public void Dispose()
        {
        }
    }
}
