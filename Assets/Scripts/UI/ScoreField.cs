using TMPro;
using UnityEngine;
using Data;

namespace UI
{
    public class ScoreField : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI goldCounter;
        [SerializeField] TextMeshProUGUI silverCounter;

        private GameProgressManager progressManager;

        private void Start()
        {
            progressManager = FindObjectOfType<GameProgressManager>();
            if (progressManager != null)
            {
                progressManager.OnIncrementScore.AddListener(UpdateCoinCounterText);
                progressManager.OnDecrementScore.AddListener(UpdateCoinCounterText);
            }
            UpdateCoinCounterText();
        }

        private void OnDisable()
        {
            if (progressManager != null)
            {
                progressManager.OnIncrementScore.RemoveListener(UpdateCoinCounterText);
                progressManager.OnDecrementScore.RemoveListener(UpdateCoinCounterText);
            }
        }

        public void UpdateCoinCounterText()
        {
            if (progressManager != null)
            {
                goldCounter.text = progressManager.GoldCoinCounter.ToString();
                silverCounter.text = progressManager.SilverCoinCounter.ToString();
            }
        }
    }
}
