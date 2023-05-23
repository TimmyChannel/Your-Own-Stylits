using YOS.Models.EnumParams;
namespace YOS.Models
{
    public abstract class CloseItemCreator
    {
        public Poses Pose { get; protected set; }
        public Somatotypes Somatotype { get; protected set; }
        public abstract IClosetItem CreateClosetItem(string name);
        public void SetParams(Somatotypes somatotype = Somatotypes.Mesomorphic,
            Poses pose = Poses.Idle)
        {
            Pose = pose;
            Somatotype = somatotype;
        }
    }
}
