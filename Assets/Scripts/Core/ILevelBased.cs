using System;

namespace Core
{
    public interface ILevelBased
    {
        void UpgradeLevel();
        void UpgradeLevel(int amount);
        event Action<int> OnLevelUpgraded;
    }
}
