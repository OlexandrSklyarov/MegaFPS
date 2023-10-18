using UnityEngine;

namespace SA.FPS
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyTargetTestGO : MonoBehaviour, IDamageable
    {
        public void ApplyDamage(int damage, Vector3 source)
        {
            Debug.Log($"damage {damage}");
            GetComponent<Rigidbody>().AddForce((transform.position - source).normalized * 10f);
        }
    }
}
