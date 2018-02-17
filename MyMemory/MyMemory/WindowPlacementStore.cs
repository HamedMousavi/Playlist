using System;


namespace MyMemory
{

    public class WindowPlacementStore : IWindowPlacementStore
    {

        public event EventHandler UserStoreChanged;


        public void Save(Windowplacement place)
        {
            SaveToUserSettings(place);
        }


        public Windowplacement? Placement => LoadFromUserSettings();


        public bool PlacementIsValid
        {
            get
            {
                var placement = Placement;
                if (placement == null) return false;

                return placement.Value.length > 0
                    && !(placement.Value.minPosition.X <= 0
                            && placement.Value.minPosition.Y <= 0
                            && placement.Value.maxPosition.X <= 0
                            && placement.Value.maxPosition.Y <= 0
                            && placement.Value.normalPosition.Left <= 0
                            && placement.Value.normalPosition.Right <= 0
                            && placement.Value.normalPosition.Bottom <= 0
                            && placement.Value.normalPosition.Top <= 0);
            }
        }


        private static void SaveToUserSettings(Windowplacement place)
        {
            Properties.Settings.Default.WindowPlacement =
                Newtonsoft.Json.JsonConvert.SerializeObject(place);
            Properties.Settings.Default.Save();
        }


        private static Windowplacement? LoadFromUserSettings()
        {
            var str = Properties.Settings.Default.WindowPlacement;
            return string.IsNullOrWhiteSpace(str)
                ? (Windowplacement?)null
                : Newtonsoft.Json.JsonConvert.DeserializeObject<Windowplacement>(str);
        }
    }
}
