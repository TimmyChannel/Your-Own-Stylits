using YOS.Models.EnumParams;
namespace YOS.Models
{
    public abstract class CloseItemCreator
    {
        public Styles Style { get; protected set; }
        public Weather Weather { get; protected set; }
        public Poses Pose { get; protected set; }
        public Somatotypes Somatotype { get; protected set; }
        public abstract IClosetItem CreateClosetItem(string name);
        public void SetParams(Somatotypes somatotype = Somatotypes.Mesomorphic,
            Weather weather = Weather.Indoor, Poses pose = Poses.Idle, Styles style = Styles.Casual)
        {
            Style = style;
            Weather = weather;
            Pose = pose;
            Somatotype = somatotype;
        }
    }
}
