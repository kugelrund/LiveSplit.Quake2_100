using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using LiveSplit.ComponentUtil;

namespace LiveSplit.Quake2_100
{
    class StatTracker
    {
        private string currMap;
        private List<string> currMaps;
        public List<MapInfoComponent> MapInfoComponents { get; set; }

        private IntPtr currentGameModuleAddress;
        private int killsAddress;
        private int maxKillsAddress;
        private int secretsAddress;
        private int maxSecretsAddress;
        private int mapAddress;
        private GameVersion gameVersion;

        #region maps dictionary
        private Dictionary<string, MapInfo> maps = new Dictionary<string, MapInfo>()
        {
            // baseq2
            { "base1", new MapInfo("Outer Base") },
            { "base2", new MapInfo("Installation") },
            { "base3", new MapInfo("Comm Center") },
            { "train", new MapInfo("Lost Station") },
            { "bunk1", new MapInfo("Ammo Depot") },
            { "ware1", new MapInfo("Supply Station") },
            { "ware2", new MapInfo("Warehouse") },
            { "jail1", new MapInfo("Main Gate") },
            { "jail2", new MapInfo("Detention Center") },
            { "jail3", new MapInfo("Security Complex") },
            { "jail4", new MapInfo("Torture Chambers") },
            { "jail5", new MapInfo("Guard House") },
            { "security", new MapInfo("Grid Control") },
            { "mintro", new MapInfo("Mine Entrance") },
            { "mine1", new MapInfo("Upper Mines") },
            { "mine2", new MapInfo("Borehole") },
            { "mine3", new MapInfo("Drilling Area") },
            { "mine4", new MapInfo("Lower Mines") },
            { "fact1", new MapInfo("Receiving Center") },
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
            { "space", new MapInfo("Comm Satellite") },
            { "lab", new MapInfo("Research Lab") },
            { "hangar2", new MapInfo("Inner Hangar") },
            { "command", new MapInfo("Launch Command") },
            { "strike", new MapInfo("Outlands") },
            { "city1", new MapInfo("Outer Courts") },
            { "city2", new MapInfo("Lower Palace") },
            { "city3", new MapInfo("Upper Palace") },
            { "boss1", new MapInfo("Inner Chamber") },
            { "boss2", new MapInfo("Final Showdown") },
            // xatrix (The Reckoning)
            { "xswamp", new MapInfo("The Swamps") },
            { "xsewer1", new MapInfo("The Sewers") },
            { "xsewer2", new MapInfo("Waste Sieve") },
            { "xcompnd1", new MapInfo("Outer Compound") },
            { "xcompnd2", new MapInfo("Inner Compound") },
            { "xreactor", new MapInfo("Core Reactor") },
            { "xware", new MapInfo("The Warehouse") },
            { "xintell", new MapInfo("Intelligence Center") },
            { "industry", new MapInfo("Industrial Facility") },
            { "outbase", new MapInfo("Outer Base") },
            { "refinery", new MapInfo("Refinery") },
            { "w_treat", new MapInfo("Water Treatment Plant") },
            { "badlands", new MapInfo("Badlands") },
            { "xhangar1", new MapInfo("Lower Hangars") },
            { "xhangar2", new MapInfo("The Hangars") },
            { "xship", new MapInfo("Strogg Freighter") },
            { "xmoon1", new MapInfo("Cargo Bay") },
            { "xmoon2", new MapInfo("Command Center") },
            // rogue (Ground Zero)
            { "rmine1", new MapInfo("Lower Mines") },
            { "rlava1", new MapInfo("Thaelite Mines") },
            { "rlava2", new MapInfo("Tectonic Stabilizer") },
            { "rmine2", new MapInfo("Mine Engineering") },
            { "rware1", new MapInfo("Eastern Warehouse") },
            { "rware2", new MapInfo("Waterfront Storage") },
            { "rbase1", new MapInfo("Logistics Complex") },
            { "rbase2", new MapInfo("Tactical Command") },
            { "rhangar1", new MapInfo("Research Hangar") },
            { "rsewer1", new MapInfo("Waste Processing") },
            { "rsewer2", new MapInfo("Waste Disposal") },
            { "rhangar2", new MapInfo("Maintenance Hangars") },
            { "rammo1", new MapInfo("Munitions Plant") },
            { "rammo2", new MapInfo("Ammo Depot") },
            { "rboss", new MapInfo("Widow's Lair") },
            // empty
            { "", MapInfo.Empty }
        };
        #endregion maps dictionary
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

        private IntPtr FindGameModuleBase(Process gameProcess)
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

        private IntPtr GetGameModuleBase(Process gameProcess)
        {
            var moduleInfo = new MODULEINFO();
            if (currentGameModuleAddress == IntPtr.Zero ||
                !GetModuleInformation(gameProcess.Handle, currentGameModuleAddress,
                                      out moduleInfo, (uint)Marshal.SizeOf(moduleInfo)))
            {
                currentGameModuleAddress = FindGameModuleBase(gameProcess);
                if (currentGameModuleAddress != IntPtr.Zero)
                {
                    GetModuleInformation(gameProcess.Handle, currentGameModuleAddress,
                                         out moduleInfo, (uint)Marshal.SizeOf(moduleInfo));
                    UpdateVersionAndSetupAddresses(gameProcess, moduleInfo.SizeOfImage);
                }
            }

            return currentGameModuleAddress;
        }

        public void UpdateVersionAndSetupAddresses(Process gameProcess, uint gameModuleSize)
        {
            switch (gameModuleSize)
            {
                case 499712:
                    gameVersion = GameVersion.v2018_10_13_baseq2;
                    break;
                case 565248:
                    gameVersion = GameVersion.v2018_10_13_xatrix;
                    break;
                case 679936:
                    gameVersion = GameVersion.v2018_10_13_rogue;
                    break;
                default:
                    MessageBox.Show("Unsupported game version", "LiveSplit.Quake2_100",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gameVersion = GameVersion.v2018_10_13_baseq2;
                    break;
            }

            switch (gameVersion)
            {
                case GameVersion.v2018_10_13_baseq2:
                    killsAddress = 0x62E40;
                    maxKillsAddress = 0x62E3C;
                    secretsAddress = 0x62E30;
                    maxSecretsAddress = 0x62E2C;
                    mapAddress = 0x62D68;
                    break;
                case GameVersion.v2018_10_13_xatrix:
                    killsAddress = 0x70EE0;
                    maxKillsAddress = 0x70EDC;
                    secretsAddress = 0x70ED0;
                    maxSecretsAddress = 0x70ECC;
                    mapAddress = 0x70E08;
                    break;
                case GameVersion.v2018_10_13_rogue:
                    killsAddress = 0x8C140;
                    maxKillsAddress = 0x8C13C;
                    secretsAddress = 0x8C130;
                    maxSecretsAddress = 0x8C12C;
                    mapAddress = 0x8C068;
                    break;
            }
        }

        public void Update(Process gameProcess)
        {
            IntPtr _base = GetGameModuleBase(gameProcess);
            if (_base == IntPtr.Zero)
            {
                return;
            }

            int kills = gameProcess.ReadValue<int>(_base + killsAddress);
            int secrets = gameProcess.ReadValue<int>(_base + secretsAddress);
            int maxKills = gameProcess.ReadValue<int>(_base + maxKillsAddress);
            int maxSecrets = gameProcess.ReadValue<int>(_base + maxSecretsAddress);
            string map = gameProcess.ReadString(_base + mapAddress, MAX_MAP_LENGTH, "");

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

    public enum GameVersion
    {
        v2018_10_13_baseq2,  // Q2PRO Speed, r1760, build from Oct 13 2018, baseq2
        v2018_10_13_xatrix,  // Q2PRO Speed, r1760, build from Oct 13 2018, xatrix (The Reckoning)
        v2018_10_13_rogue    // Q2PRO Speed, r1760, build from Oct 13 2018, rogue (Ground Zero)
    }
}