using Data;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class DeathZoneLogic : MonoBehaviour
    {
        public UnityEvent OnDeathZone;
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

            BallController ballController = other.gameObject.GetComponent<BallController>();
            ballController.enabled = false;

            if (other.gameObject.GetComponent<ParticleSystem>() != null)
                other.gameObject.GetComponent<ParticleSystem>().Play();

            other.gameObject.transform.position = new Vector3(0, 0.65f, -10);
            otherRenderer.enabled = true;
            ballController.enabled = true;
            OnDeathZone.Invoke();
            yield return new WaitForSeconds(0.5f);
            GameProgressManager.Instance.SaveProgress();
        }
    }
}
