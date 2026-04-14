using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 10f;
        [SerializeField] private float offsetZ = -21;
        [SerializeField] private float currentY = 14f;
        private Transform _transform;

        void Start()
        {
            _transform = transform;
        }

        void LateUpdate()
        {
            Vector3 desiredPosition = new Vector3(target.position.x, currentY, target.position.z + offsetZ);
            _transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
