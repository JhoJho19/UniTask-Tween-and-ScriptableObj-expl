using Data;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class CollectableObjBehavior : MonoBehaviour
    {
        [SerializeField] CollectableObjData collectableObjData;
        [SerializeField] Renderer collectableObjRenderer;
        [SerializeField] AudioSource soundOfTakenCoin;
        [SerializeField] ParticleSystem takenGoldCoinEffect;
        [SerializeField] ParticleSystem takenSilverCoinEffect;
        public Animator CoinAnimator;
        public UnityEvent OnCoinTaken;
        public UnityEvent OnCoinReset;
        private bool _isCoinTaken;

        private GameProgressManager progressManager;

        private void Awake()
        {
            progressManager = FindObjectOfType<GameProgressManager>();
        }

        private void OnEnable()
        {
            collectableObjRenderer.material = collectableObjData.Material;

            DeathZoneLogic deathZone = FindObjectOfType<DeathZoneLogic>();
            if (deathZone != null)
            {
                deathZone.OnDeathZone.AddListener(Restart);
            }
        }

        private void OnDisable()
        {
            DeathZoneLogic deathZone = FindObjectOfType<DeathZoneLogic>();
            if (deathZone != null)
            {
                deathZone.OnDeathZone.RemoveListener(Restart);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isCoinTaken && other.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                CoinAnimator.SetTrigger("CoinTaked");
                soundOfTakenCoin.Play();

                if (collectableObjData.Type == CollectableObjectType.SilverCoin)
                {
                    takenSilverCoinEffect.Play();
                }
                else if (collectableObjData.Type == CollectableObjectType.GoldCoin)
                {
                    takenGoldCoinEffect.Play();
                }

                if (progressManager != null)
                {
                    if (collectableObjData.Type == CollectableObjectType.GoldCoin)
                        progressManager.IncrementGold();
                    else if (collectableObjData.Type == CollectableObjectType.SilverCoin)
                        progressManager.IncrementSilver();
                }
                else
                {
                    OnCoinTaken.Invoke();
                }

                _isCoinTaken = true;
            }
        }

        private void Restart()
        {
            if (_isCoinTaken)
            {
                if (progressManager != null)
                {
                    if (collectableObjData.Type == CollectableObjectType.GoldCoin)
                        progressManager.DecrementGold();
                    else if (collectableObjData.Type == CollectableObjectType.SilverCoin)
                        progressManager.DecrementSilver();
                }
                else
                {
                    OnCoinReset.Invoke();
                }
                CoinAnimator.SetTrigger("Restart");
                _isCoinTaken = false;
            }
        }
    }
}
