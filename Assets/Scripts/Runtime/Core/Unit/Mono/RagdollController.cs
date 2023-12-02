using System.Collections.Generic;
using UnityEngine;

namespace SA.FPS
{
    public class RagdollController : MonoBehaviour
    {
        private IEnumerable<Rigidbody> Rigidbodies => _rigidbodies;
        
        private Rigidbody[] _rigidbodies;

        private void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
        }

        public void On() => SetActiveRB(true);
        
        public void Off() => SetActiveRB(false);       
       
        private void SetActiveRB(bool isActive)
        {
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                var r = _rigidbodies[i];

                if (isActive)
                {
                    r.isKinematic = false;
                    r.useGravity = true;
                    continue;
                }

                r.isKinematic = false;
                r.useGravity = false;
                
                //reset velocity
                r.velocity = Vector3.zero;
                r.angularVelocity = Vector3.zero;
            }
        }
    }
}