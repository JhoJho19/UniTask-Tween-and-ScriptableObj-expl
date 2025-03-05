using Data;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class DeathZoneLogic : MonoBehaviour
    {
        public UnityEvent OnDeathZone;
        private GameProgressManager progressManager;

        private void Start()
        {
            progressManager = FindObjectOfType<GameProgressManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                StartCoroutine(DeathZoneRoutine(other));
            }
        }

        private IEnumerator DeathZoneRoutine(Collider other)
        {
            MeshRenderer otherRenderer = other.GetComponent<MeshRenderer>();
            otherRenderer.enabled = false;

            BallController ballController = other.GetComponent<BallController>();
            ballController.enabled = false;

            ParticleSystem ps = other.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();

            other.transform.position = new Vector3(0, 0.65f, -10);
            otherRenderer.enabled = true;
            ballController.enabled = true;
            OnDeathZone.Invoke();
            yield return new WaitForSeconds(0.5f);

            //if (progressManager != null)
            //    progressManager.SaveProgress();
        }
    }
}
