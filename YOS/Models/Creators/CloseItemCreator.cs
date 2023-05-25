using YOS.Models.EnumParams;
using YOS.Models.Items;

namespace YOS.Models.Creators
{
    public abstract class CloseItemCreator
    {
        public Poses Pose { get; protected set; }
        public GenderTypes Gender { get; protected set; }
        public abstract IClosetItem CreateClosetItem(string name);
        public void SetParams(Poses pose = Poses.Idle, 
            GenderTypes gender=GenderTypes.Male)
        {
            Pose = pose;
            Gender = gender;
        }
    }
}
