using System.Collections.Generic;
using SA.Runtime.Services.SaveLoad.Models;

namespace SA.Runtime.Services.SaveLoad
{
    public class ModelRegister
    {
        private List<IModel> _allModels;

        public ModelRegister(SavingScheduler savingScheduler)
        {
            _allModels = new List<IModel>()
            {
                new TestPlayerModel(savingScheduler),
                new TestLevelModel(savingScheduler)
            };
        }

        public List<IModel> GetAllModels() => _allModels;
    }
}