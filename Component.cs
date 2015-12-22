using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace LiveSplit.Quake2_100
{
    public class Component : IComponent
    {
        public ComponentRendererComponent InternalComponent { get; protected set; }

        private Settings settings;

        public string ComponentName => "Quake2_100";

        public float PaddingTop => InternalComponent.PaddingTop;
        public float PaddingLeft => InternalComponent.PaddingLeft;
        public float PaddingBottom => InternalComponent.PaddingBottom;
        public float PaddingRight => InternalComponent.PaddingRight;

        public float VerticalHeight => InternalComponent.VerticalHeight;
        public float MinimumWidth => InternalComponent.MinimumWidth;
        public float HorizontalWidth => InternalComponent.HorizontalWidth;
        public float MinimumHeight => InternalComponent.MinimumHeight;

        public IDictionary<string, Action> ContextMenuControls => null;

        private StatTracker statTracker;

        public Component(LiveSplitState state)
        {
            settings = new Settings();
            ResetTracker();

            state.OnReset += State_OnReset;
            state.OnStart += State_OnStart;
        }

        private void State_OnStart(object sender, EventArgs e)
        {
            ResetTracker();
        }

        private void State_OnReset(object sender, TimerPhase value)
        {
            ResetTracker();
        }

        private void ResetTracker()
        {
            statTracker = new StatTracker(settings.ListSize);
            InternalComponent = new ComponentRendererComponent();
            InternalComponent.VisibleComponents = statTracker.MapInfoComponents;
        }
                
        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            InternalComponent.DrawVertical(g, state, width, clipRegion);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            InternalComponent.DrawHorizontal(g, state, height, clipRegion);
        }        

        private Process gameProcess = null;

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (gameProcess != null && !gameProcess.HasExited)
            {
                statTracker.Update(gameProcess);                
            }
            else
            {
                gameProcess = Process.GetProcessesByName("q2pro").FirstOrDefault();
            }

            if (invalidator != null)
            {
                InternalComponent.Update(invalidator, state, width, height, mode);
            }
        }

        public System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            settings.Mode = mode;
            return settings;
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            this.settings.SetSettings(settings);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return settings.GetSettings(document);
        }

        public int GetSettingsHashCode()
        {
            return settings.GetSettingsHashCode();
        }

        public void Dispose()
        {
            settings.Dispose();
        }
    }
}
