using System;

namespace Core.Money
{
    [Serializable]
    public struct MoneyData
    {
        public int Cash;
        public int BTC;
        public int Gems;

        public MoneyData(int cash, int btc, int gems)
        {
            Cash = cash;
            BTC = btc;
            Gems = gems;
        }
    }
    
    public class MoneySystem
    {
        private MoneyData _moneyData;

        public MoneySystem()
        {
            _moneyData = TryGetMoneyDataFromSave();
        }

        private MoneyData TryGetMoneyDataFromSave()
        {
            return MoneySaveSystem.LoadMoneyData();
        }
    }
}
