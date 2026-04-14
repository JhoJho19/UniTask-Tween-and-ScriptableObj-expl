using System;

namespace Game.Services
{
    public class CoinService
    {
        private readonly PlayerProgressData progressData;

        public event Action CoinsChanged;

        public CoinService(PlayerProgressData progressData)
        {
            this.progressData = progressData;
        }

        public int GetBalance(CoinType coinType)
        {
            switch (coinType)
            {
                case CoinType.Gold:
                    return progressData.GoldCoinCounter;
                case CoinType.Silver:
                    return progressData.SilverCoinCounter;
                default:
                    return 0;
            }
        }

        public void Add(CoinType coinType, int amount = 1)
        {
            if (amount <= 0)
            {
                return;
            }

            SetBalance(coinType, GetBalance(coinType) + amount);
            CoinsChanged?.Invoke();
        }

        public bool TryRemove(CoinType coinType, int amount = 1)
        {
            if (amount <= 0)
            {
                return false;
            }

            int currentBalance = GetBalance(coinType);
            if (currentBalance < amount)
            {
                return false;
            }

            SetBalance(coinType, currentBalance - amount);
            CoinsChanged?.Invoke();
            return true;
        }

        private void SetBalance(CoinType coinType, int amount)
        {
            switch (coinType)
            {
                case CoinType.Gold:
                    progressData.GoldCoinCounter = amount;
                    break;
                case CoinType.Silver:
                    progressData.SilverCoinCounter = amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(coinType), coinType, null);
            }
        }
    }

    public enum CoinType
    {
        Gold,
        Silver
    }
}
