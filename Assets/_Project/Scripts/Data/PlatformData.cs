using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Platform", menuName = "Platform Data", order = 51)]
    public class Platform : ScriptableObject
    {
        [SerializeField] private bool isColorAllowed;
        [SerializeField] private Color color;

        public bool IsColorAllowed { get { return isColorAllowed; } }
        public Color Color { get { return color; } }
    }
}