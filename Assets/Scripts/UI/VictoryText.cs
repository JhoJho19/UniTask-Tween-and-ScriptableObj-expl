using Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI victotyText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            victotyText.gameObject.SetActive(true);
        }
    }
}
