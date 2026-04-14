using TMPro;
using Game.Services;
using UnityEngine;

namespace UI
{
    public class ScoreField : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI goldCounter;
        [SerializeField] TextMeshProUGUI silverCounter;

        private PlayerProgressService progressService;

        private void OnEnable()
        {
            progressService = FindFirstObjectByType<PlayerProgressService>();
            if (progressService != null)
            {
                progressService.Coins.CoinsChanged += UpdateCoinCounterText;
            }

            UpdateCoinCounterText();
        }

        private void OnDisable()
        {
            if (progressService != null)
            {
                progressService.Coins.CoinsChanged -= UpdateCoinCounterText;
            }
        }

        public void UpdateCoinCounterText()
        {
            if (progressService != null)
            {
                goldCounter.text = progressService.Coins.GetBalance(CoinType.Gold).ToString();
                silverCounter.text = progressService.Coins.GetBalance(CoinType.Silver).ToString();
            }
        }
    }
}
