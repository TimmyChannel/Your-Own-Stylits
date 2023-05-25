using YOS.Models.Mannequin;

namespace YOS.Models.Algorithm
{
    interface IBuilderMannequin
    {
        void Reset();
        void SetHeadWear();
        void SetTop();
        void SetBottom();
        void SetShoes();
        void SetAccessories();
        MannequinSettings GetResult();
    }
}
