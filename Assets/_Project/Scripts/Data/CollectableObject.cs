using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "CollectableObj", menuName = "CollectableObj Data", order = 52)]
    public class CollectableObjData : ScriptableObject
    {
        [SerializeField] private CollectableObjectType type;
        [SerializeField] private int scores;
        [SerializeField] private Material material;

        public CollectableObjectType Type { get { return type; } }

        public int Scores { get { return scores; } }

        public Material Material{ get { return material; } }
    }

    public enum CollectableObjectType
    {
        GoldCoin,
        SilverCoin
    }
}
