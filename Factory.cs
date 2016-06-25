using System;
using LiveSplit.UI.Components;

namespace LiveSplit.Quake2_100
{
    public class Factory : IComponentFactory
    {
        public string ComponentName => "Quake2_100";
        public ComponentCategory Category => ComponentCategory.Information;
        public string Description => "Quake II stattracker for 100% category";

        public string UpdateName => ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/kugelrund/LiveSplit.Quake2_100/master/";
        public Version Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => UpdateURL + "Components/update.LiveSplit.Quake2_100.xml";

        public IComponent Create(Model.LiveSplitState state)
        {
            return new Component(state);
        }
    }
}