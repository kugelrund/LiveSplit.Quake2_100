using LiveSplit.UI.Components;
using System;

namespace LiveSplit.Quake2_100
{
    public class Factory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Quake2_100"; }
        }
        public ComponentCategory Category
        {
            get { return ComponentCategory.Information; }
        }
        public string Description
        {
            get { return "Shows Memory"; }
        }
        public IComponent Create(Model.LiveSplitState state)
        {
            return new Component(state);
        }
        public string UpdateName
        {
            get { return ComponentName; }
        }
        public string UpdateURL
        {
            get { return "https://raw.githubusercontent.com/kugelrund/LiveSplit.Quake2_100/master/"; }
        }
        public Version Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public string XMLURL
        {
            get { return UpdateURL + "Components/update.LiveSplit.Quake2_100.xml"; }
        }
    }
}