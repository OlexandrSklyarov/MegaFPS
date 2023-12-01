using System;
using UnityEngine;

namespace SA.FPS
{
    public class RagdollController : MonoBehaviour
    {
        private Collider[] _colliders;
        private Rigidbody[] _rigidbodies;
        private bool _isInit;

        public void Init()
        {
            if (_isInit) return;

            _colliders = GetComponentsInChildren<Collider>();
            _rigidbodies = GetComponentsInChildren<Rigidbody>();

            _isInit = true;
        }

        public void On()
        {
            Array.ForEach(_colliders, c => c.isTrigger = false);
            Array.ForEach(_rigidbodies, r => SetActiveRB(r, true));
        }
        
        public void Off()
        {
            Array.ForEach(_colliders, c => c.isTrigger = true);
            Array.ForEach(_rigidbodies, r => SetActiveRB(r, false));
        }

        private void SetActiveRB(Rigidbody rb, bool isActive)
        {
            if (isActive)
            {
                rb.isKinematic = false;
                return;
            }

            rb.isKinematic = false;
            
            //reset velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}