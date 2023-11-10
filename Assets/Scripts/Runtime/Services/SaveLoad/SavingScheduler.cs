using System.Collections.Generic;

namespace SA.Runtime.Services.SaveLoad
{
    public class SavingScheduler
    {
        private List<IModel> _models = new List<IModel>();

        private const int _delay = 10;

        public void Push(IModel model)
        {
            if (!_models.Contains(model))
            {
                _models.Add(model);
            }
        }

        public void Tick()
        {
            if (_models.Count > 0)
            {
                for (var i = 0; i < _models.Count; i++)
                {
                    var model = _models[i];
                    model.Tick++;

                    if (model.Tick >= _delay && model.SaveLock == false) //200 ms
                    {
                        _models.RemoveAt(i);
                        i--;
                        model.Tick = 0;
                         
                        model.SaveAsync().WrapErrors();
                    }
                };
            }
        }
    }
}