using Cinemachine;
using FMODUnity;
using UnityEngine;

namespace SA.FPS
{    
    [RequireComponent(typeof(Camera), typeof(StudioListener), typeof(CinemachineBrain))]
    public class TPSCamera : MonoBehaviour   
    {
        public Camera TargetCamera => _camera;

        private Camera _camera;
        private CinemachineBrain _brain;
        private StudioListener _audioListener;

        private void Awake() 
        {
            _camera = GetComponent<Camera>();
            _brain = GetComponent<CinemachineBrain>();
            _audioListener = GetComponent<StudioListener>();
        }

        public void On()
        {
            _camera.enabled = true;
            _brain.enabled = true;
            _audioListener.enabled = true;
        }

        public void Off()
        {
            _camera.enabled = false;
            _brain.enabled = false;
            _audioListener.enabled = false;
        }
    }
}