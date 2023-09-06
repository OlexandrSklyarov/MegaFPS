
namespace SA.FPS
{
    public class InputService :  IService
    {
        public Controls Controls => _controls;
        private Controls _controls;

        public InputService()
        {
            _controls = new Controls();
        }
    }
}