using UnityEngine;

namespace SA.FPS
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;

        void Awake()
        {
            Util.DebugUtil.PrintColor("Boot game started...", Color.green);

            ServicesPool.Instance.Init(_gameConfig);            
        }

        private void OnDestroy() 
        {
            ServicesPool.Instance.Dispose();
        }
    }
}
