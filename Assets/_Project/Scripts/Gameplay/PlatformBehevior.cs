using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Gameplay 
{
    public class PlatformBehevior : MonoBehaviour
    {
        [SerializeField] private Platform platformData;
        [SerializeField] private Collider platformCollider;
        [SerializeField] private Renderer platformRenderer;
        [SerializeField] AudioSource failSound;

        private void OnEnable()
        {
            platformRenderer.material.color = platformData.Color;
            if (!platformData.IsColorAllowed)
            {
                platformCollider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ball") && !platformData.IsColorAllowed)
            {
                failSound.Play();
            }
        }
    }

}

