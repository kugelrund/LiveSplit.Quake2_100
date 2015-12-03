using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace LiveSplit.Quake2_100
{
    public class Component : IComponent
    {
        public ComponentRendererComponent InternalComponent { get; protected set; }

        private List<MapInfoComponent> MapInfoComponents { get; set; }
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

        private Dictionary<string, MapInfo> maps = new Dictionary<string, MapInfo>()
        {
            { "base1", new MapInfo("Outer Base") },
            { "base2", new MapInfo("Installation") },
            { "base3", new MapInfo("Comm Center") },
            { "train", new MapInfo("Lost Station") },
            { "bunk1", new MapInfo("Ammo Depot") },
            { "ware1", new MapInfo("Supply Station") },
            { "ware2", new MapInfo("Warehouse") },
            { "jail1", new MapInfo("Main Gate") },
            { "jail2", new MapInfo("Destination Center") },
            { "jail3", new MapInfo("Security Complex") },
            { "jail4", new MapInfo("Torture Chambers") },
            { "jail5", new MapInfo("Guard House") },
            { "security", new MapInfo("Grid Control") },
            { "mintro", new MapInfo("Mine Entrance") },
            { "mine1", new MapInfo("Upper Mines") },
            { "mine2", new MapInfo("Borehole") },
            { "mine3", new MapInfo("Drilling Area") },
            { "mine4", new MapInfo("Lower Mines") },
            { "fact1", new MapInfo("Receieving Center") },
            { "fact3", new MapInfo("Sudden Death") },
            { "fact2", new MapInfo("Processing Plant") },
            { "power1", new MapInfo("Power Plant") },
            { "power2", new MapInfo("The Reactor") },
            { "cool1", new MapInfo("Cooling Facility") },
            { "waste1", new MapInfo("Toxic Waste Dump") },
            { "waste2", new MapInfo("Pumping Station 1") },
            { "waste3", new MapInfo("Pumping Station 2") },
            { "biggun", new MapInfo("Big Gun") },
            { "hangar1", new MapInfo("Outer Hangar") },
            { "space", new MapInfo("Comm Satelite") },
            { "lab", new MapInfo("Research Lab") },
            { "hangar2", new MapInfo("Inner Hangar") },
            { "command", new MapInfo("Launch Command") },
            { "strike", new MapInfo("Outlands") },
            { "city1", new MapInfo("Outer Courts") },
            { "city2", new MapInfo("Lower Palace") },
            { "city3", new MapInfo("Upper Palace") },
            { "boss1", new MapInfo("Inner Chamber") },
            { "boss2", new MapInfo("Final Showdown") },
            { "", new MapInfo("") }
        };
        private string currMap;

        private List<string> currMaps;

        public Component(LiveSplitState state)
        {
            settings = new Settings();

            InternalComponent = new ComponentRendererComponent();
            MapInfoComponents = new List<MapInfoComponent>(settings.ListSize);
            currMaps = new List<string>(settings.ListSize);
            for (int i = 0; i < settings.ListSize; i += 1)
            {
                MapInfoComponents.Add(new MapInfoComponent(settings, maps[""]));
                currMaps.Add("");
            }
            InternalComponent.VisibleComponents = MapInfoComponents;

            state.OnReset += State_OnReset;
        }

        private void State_OnReset(object sender, TimerPhase value)
        {
            EmptyList();
        }

        private void EmptyList()
        {
            currMaps.Clear();
            for (int i = 0; i < MapInfoComponents.Count; i += 1)
            {
                currMaps.Add("");
                MapInfoComponents[i].MapInfo = maps[""];
            }            

            currMap = "";
        }

        private void PrepareDraw(LiveSplitState state, LayoutMode mode)
        {
        }

        private void DrawBackground(Graphics g, LiveSplitState state, float width, float height)
        {
            if (settings.BackgroundColor1.ToArgb() != Color.Transparent.ToArgb()
                || settings.BackgroundGradient != GradientType.Plain
                && settings.BackgroundColor2.ToArgb() != Color.Transparent.ToArgb())
            {
                bool horizontal = settings.BackgroundGradient == GradientType.Horizontal;
                bool plain = settings.BackgroundGradient == GradientType.Plain;
                LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    new PointF(0, 0),
                    horizontal ? new PointF(width, 0) : new PointF(0, height),
                    settings.BackgroundColor1,
                    plain ? settings.BackgroundColor1 : settings.BackgroundColor2);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
            }
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            DrawBackground(g, state, width, VerticalHeight);
            PrepareDraw(state, LayoutMode.Vertical);
            InternalComponent.DrawVertical(g, state, width, clipRegion);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            DrawBackground(g, state, HorizontalWidth, height);
            PrepareDraw(state, LayoutMode.Horizontal);
            InternalComponent.DrawHorizontal(g, state, height, clipRegion);
        }

        private static readonly DeepPointer mapAddress = new DeepPointer("gamex86.dll", 0x614C8);
        private static readonly DeepPointer killsAddress = new DeepPointer("gamex86.dll", 0x615A0);
        private static readonly DeepPointer maxKillsAddress = new DeepPointer("gamex86.dll", 0x6159C);
        private static readonly DeepPointer secretsAddress = new DeepPointer("gamex86.dll", 0x61590);
        private static readonly DeepPointer maxSecretsAddress = new DeepPointer("gamex86.dll", 0x6158C);

        private Process gameProcess = null;
        private const int MAX_MAP_LENGTH = 8;

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (gameProcess != null && !gameProcess.HasExited)
            {
                string map = mapAddress.DerefString(gameProcess, MAX_MAP_LENGTH, "");
                int kills = killsAddress.Deref(gameProcess, 0);
                int secrets = secretsAddress.Deref(gameProcess, 0);
                int maxKills = maxKillsAddress.Deref(gameProcess, 0);
                int maxSecrets = maxSecretsAddress.Deref(gameProcess, 0);

                if (!string.IsNullOrEmpty(map))
                {
                    if (map != currMap)
                    {
                        if (currMaps.Contains(map))
                        {
                            currMaps.Remove(map);
                            currMaps.Add(map);
                        }
                        else
                        {
                            currMaps.Add(map);
                            currMaps.RemoveAt(0);
                        }

                        for (int i = 0; i < currMaps.Count; i += 1)
                        {
                            MapInfoComponents[i].MapInfo = maps[currMaps[i]];
                        }

                        currMap = map;
                    }

                    MapInfo info = maps[map];

                    if (kills != info.Kills || secrets != info.Secrets ||
                        maxKills != info.MaxKills || maxSecrets != info.MaxSecrets)
                    {
                        int last = MapInfoComponents.Count - 1;

                        info.Kills = kills;
                        info.MaxKills = maxKills;
                        info.Secrets = secrets;
                        info.MaxSecrets = maxSecrets;

                        MapInfoComponents[last].MapInfo = info;
                    }
                }
            }
            else
            {
                gameProcess = Process.GetProcessesByName("q2pro").FirstOrDefault();
                EmptyList();
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
