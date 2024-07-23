using TMPro;
using UnityEngine;
using Data;

namespace UI
{
    public class ScoreField : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI goldCounter;
        [SerializeField] TextMeshProUGUI silverCounter;

        private void OnEnable()
        {
            UpdateCoinCounterText();
            if (GameProgressManager.Instance != null)
            {
                GameProgressManager.Instance.OnIncrementScore.AddListener(UpdateCoinCounterText);
                GameProgressManager.Instance.OnDecrementScore.AddListener(UpdateCoinCounterText);
            }
        }

        private void OnDisable()
        {
            if (GameProgressManager.Instance != null)
            {
                GameProgressManager.Instance.OnIncrementScore.RemoveListener(UpdateCoinCounterText);
                GameProgressManager.Instance.OnDecrementScore.RemoveListener(UpdateCoinCounterText);
            }
        }

        public void UpdateCoinCounterText()
        {
            if (GameProgressManager.Instance != null)
            {
                goldCounter.text = GameProgressManager.Instance.GoldCoinCounter.ToString();
                silverCounter.text = GameProgressManager.Instance.SilverCoinCounter.ToString();
            }
        }
    }
}