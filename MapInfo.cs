namespace LiveSplit.Quake2_100
{
    class MapInfo
    {
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Secrets { get; set; }
        public int MaxKills { get; set; }
        public int MaxSecrets { get; set; }

        public string KillsString => Kills + "/" + MaxKills;
        public string SecretsString => Secrets + "/" + MaxSecrets;

        public static MapInfo Empty = new MapInfo("");

        public MapInfo(string name)
        {
            Name = name;

            Kills = 0;
            Secrets = 0;
            MaxKills = 0;
            MaxSecrets = 0;
        }
    }
}