using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SA.FPS
{
    public class RagdollController : MonoBehaviour
    {
        public IEnumerable<Rigidbody> Rigidbodies => _rigidbodies;
        
        [SerializeField, Min(1)] private int _solverIterations = 6;
        [SerializeField, Min(1)] private int _solverVelocityIterations = 1;
        [SerializeField, Min(0f)] private float _rbDrag = 1.5f;
        [SerializeField, Min(1f)] private float _rbAngularDrag = 1.5f;
        [Space, SerializeField] private Rigidbody _hipBoneRigidbody;

        private Rigidbody[] _rigidbodies;
        private CharacterJoint[] _characterJoints;
        private Animator _animator;

        private void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            Array.ForEach(_rigidbodies, r =>
            {
                r.solverIterations = _solverIterations;
                r.solverVelocityIterations = _solverVelocityIterations;
                r.drag = _rbDrag;
                r.angularDrag = _rbAngularDrag;
            });

            Array.ForEach(GetComponentsInChildren<CharacterJoint>(), joint =>
            {
                joint.enableProjection = true;
                joint.enablePreprocessing = true;
            });

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

        public void OnAndPushAtPosition(Vector3 direction, Vector3 point, float power)
        {
            On();

            var targetRB = _rigidbodies
                .OrderBy(x => Vector3.Distance(x.position, point))
                .First();
            targetRB.AddForceAtPosition(direction * power, point, ForceMode.Impulse);
        }    

        public void OnAndPushHipBone(Vector3 direction, float power)
        {
            On();
            
            _hipBoneRigidbody.AddForce(direction * power, ForceMode.Impulse);
        } 
       
        private void SetActiveRB(bool isActive)
        {
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                var r = _rigidbodies[i];

                r.isKinematic = !isActive;
                r.useGravity = isActive; 

                //reset velocity
                if (r.velocity != Vector3.zero) r.velocity = Vector3.zero;
                if (r.angularVelocity != Vector3.zero) r.angularVelocity = Vector3.zero;  
            }
        }        
    }
}