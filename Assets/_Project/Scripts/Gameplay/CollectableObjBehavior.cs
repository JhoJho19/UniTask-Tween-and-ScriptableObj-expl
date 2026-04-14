using UnityEngine.Events;
using Data;
using Game.Services;
using UnityEngine;

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

        private PlayerProgressService progressService;

        private void Awake()
        {
            progressService = FindFirstObjectByType<PlayerProgressService>();
        }

        private void OnEnable()
        {
            collectableObjRenderer.material = collectableObjData.Material;

            DeathZoneLogic deathZone = FindFirstObjectByType<DeathZoneLogic>();
            if (deathZone != null)
            {
                deathZone.OnDeathZone.AddListener(Restart);
            }
        }

        private void OnDisable()
        {
            DeathZoneLogic deathZone = FindFirstObjectByType<DeathZoneLogic>();
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

                if (progressService != null)
                {
                    progressService.Coins.Add(GetCoinType(), collectableObjData.Scores);
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
                if (progressService != null)
                {
                    progressService.Coins.TryRemove(GetCoinType(), collectableObjData.Scores);
                }
                else
                {
                    OnCoinReset.Invoke();
                }
                CoinAnimator.SetTrigger("Restart");
                _isCoinTaken = false;
            }
        }

        private CoinType GetCoinType()
        {
            return collectableObjData.Type == CollectableObjectType.GoldCoin ? CoinType.Gold : CoinType.Silver;
        }
    }
}
