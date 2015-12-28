using System.Collections.Generic;
using System.Diagnostics;

namespace LiveSplit.Quake2_100
{
    class StatTracker
    {
        private string currMap;
        private List<string> currMaps;
        public List<MapInfoComponent> MapInfoComponents { get; set; }

        #region maps dictionary
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
            { "", MapInfo.Empty }
        };
        #endregion
        private const int MAX_MAP_LENGTH = 8;

        private static readonly DeepPointer mapAddress = new DeepPointer("gamex86.dll", 0x614C8);
        private static readonly DeepPointer killsAddress = new DeepPointer("gamex86.dll", 0x615A0);
        private static readonly DeepPointer maxKillsAddress = new DeepPointer("gamex86.dll", 0x6159C);
        private static readonly DeepPointer secretsAddress = new DeepPointer("gamex86.dll", 0x61590);
        private static readonly DeepPointer maxSecretsAddress = new DeepPointer("gamex86.dll", 0x6158C);

        public StatTracker(int length)
        {
            currMaps = new List<string>(length);
            MapInfoComponents = new List<MapInfoComponent>(length);

            for (int i = 0; i < length; i += 1)
            {
                currMaps.Add("");
                MapInfoComponents.Add(new MapInfoComponent(MapInfo.Empty));
            }
        }

        public void Update(Process gameProcess)
        {
            string map = mapAddress.DerefString(gameProcess, MAX_MAP_LENGTH, "");
            int kills = killsAddress.Deref(gameProcess, 0);
            int secrets = secretsAddress.Deref(gameProcess, 0);
            int maxKills = maxKillsAddress.Deref(gameProcess, 0);
            int maxSecrets = maxSecretsAddress.Deref(gameProcess, 0);

            if (!string.IsNullOrEmpty(map) && maps.ContainsKey(map))
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
    }
}
