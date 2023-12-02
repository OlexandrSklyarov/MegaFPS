using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SA.FPS
{
    public class RagdollController : MonoBehaviour
    {
        private IEnumerable<Rigidbody> Rigidbodies => _rigidbodies;
        
        private Rigidbody[] _rigidbodies;
        private Animator _animator;

        private void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void On() 
        {
            _animator.enabled = false;
            SetActiveRB(true);
        }
        
        public void Off() 
        {
            SetActiveRB(false);  
            _animator.enabled = true;
        }  

        public void OnAndPush(Vector3 direction, Vector3 point, float power)
        {
            On();

            var targetRB = _rigidbodies
                .OrderBy(x => Vector3.Distance(x.position, point))
                .First();
            
            targetRB.AddForceAtPosition(direction * power, point, ForceMode.Impulse);
        }    
       
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