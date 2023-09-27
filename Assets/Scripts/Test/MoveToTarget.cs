using UnityEngine;

namespace SA.FPS
{
    public class MoveToTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        void Update()
        {
            transform.position = Vector3.Lerp
            (
                transform.position,
                _target.position,
                100 * Time.deltaTime
            );
        }
    }
}
