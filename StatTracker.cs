using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LiveSplit.Quake2_100
{
    class StatTracker
    {
        private string currMap;
        private List<string> currMaps;
        public List<MapInfoComponent> MapInfoComponents { get; set; }

        private int killsAddress;
        private int maxKillsAddress;
        private int secretsAddress;
        private int maxSecretsAddress;
        private DeepPointer mapAddress;
        private GameVersion gameVersion;
        private bool hasVersion = false;

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

        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumProcessModulesEx(IntPtr hProcess, [Out] IntPtr[] lphModule, uint cb,
            out uint lpcbNeeded, uint dwFilterFlag);

        [DllImport("psapi.dll")]
        private static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName,
            uint nSize);

        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, [Out] out MODULEINFO lpmodinfo,
          uint cb);

        [StructLayout(LayoutKind.Sequential)]
        public struct MODULEINFO
        {
            public IntPtr lpBaseOfDll;
            public uint SizeOfImage;
            public IntPtr EntryPoint;
        }

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

        private IntPtr GetGameModuleBase(Process gameProcess)
        {
            const int LIST_MODULES_ALL = 3;
            const int MAX_PATH = 260;
            var hModules = new IntPtr[1024];

            uint cb = (uint)IntPtr.Size * (uint)hModules.Length;
            uint cbNeeded;
            if (!EnumProcessModulesEx(gameProcess.Handle, hModules, cb, out cbNeeded, LIST_MODULES_ALL))
                throw new Win32Exception();
            uint numMods = cbNeeded / (uint)IntPtr.Size;

            var sb = new StringBuilder(MAX_PATH);
            for (int i = 0; i < numMods; i++)
            {
                sb.Clear();
                if (GetModuleBaseName(gameProcess.Handle, hModules[i], sb, (uint)sb.Capacity) == 0)
                    throw new Win32Exception();
                string baseName = sb.ToString();

                if (baseName.ToLower() == "gamex86.dll")
                {
                    var moduleInfo = new MODULEINFO();
                    if (!GetModuleInformation(gameProcess.Handle, hModules[i], out moduleInfo,
                                              (uint)Marshal.SizeOf(moduleInfo)))
                    {
                        throw new Win32Exception();
                    }
                    return moduleInfo.lpBaseOfDll;
                }
            }

            return IntPtr.Zero;
        }

        public void UpdateVersion(Process gameProcess)
        {
            if (gameProcess.MainModuleWow64Safe().ModuleMemorySize == 5029888)
            {
                gameVersion = GameVersion.v2014_12_03;
            }
            else if (gameProcess.MainModuleWow64Safe().ModuleMemorySize == 5033984)
            {
                gameVersion = GameVersion.v2016_01_12;
            }
            else
            {
                MessageBox.Show("Unsupported game version", "LiveSplit.Quake2_100",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                gameVersion = GameVersion.v2014_12_03;
            }

            switch (gameVersion)
            {
                case GameVersion.v2014_12_03:
                    killsAddress = 0x615A0;
                    maxKillsAddress = 0x6159C;
                    secretsAddress = 0x61590;
                    maxSecretsAddress = 0x6158C;
                    mapAddress = new DeepPointer(0x3086C4);
                    break;
                case GameVersion.v2016_01_12:
                    killsAddress = 0x625A0;
                    maxKillsAddress = 0x6259C;
                    secretsAddress = 0x62590;
                    maxSecretsAddress = 0x6258C;
                    mapAddress = new DeepPointer(0x33FF44);
                    break;
            }

            hasVersion = true;
        }

        public void Update(Process gameProcess)
        {
            if (!hasVersion)
            {
                UpdateVersion(gameProcess);
            }

            IntPtr _base = GetGameModuleBase(gameProcess);
            int kills = gameProcess.ReadValue<int>(_base + killsAddress);
            int secrets = gameProcess.ReadValue<int>(_base + secretsAddress);
            int maxKills = gameProcess.ReadValue<int>(_base + maxKillsAddress);
            int maxSecrets = gameProcess.ReadValue<int>(_base + maxSecretsAddress);
            string map = mapAddress.DerefString(gameProcess, MAX_MAP_LENGTH, "");

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
                    if (map == "lab")
                    {
                        info.MaxKills = 33;
                    }
                    else if (map == "city1")
                    {
                        info.MaxKills = 34;
                    }
                    else if (map == "city3")
                    {
                        info.MaxKills = 43;
                    }
                    else
                    {
                        info.MaxKills = maxKills;
                    }
                    info.Secrets = secrets;
                    info.MaxSecrets = maxSecrets;

                    MapInfoComponents[last].MapInfo = info;
                }
            }
        }
    }

    public enum GameVersion
    {
        v2014_12_03, // latest version of original Quake II Pro release, build from Dec 3 2014
        v2016_01_12  // first release of modified Q2PRO, build from Jan 12 2016
    }
}
