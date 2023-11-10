using System.Collections.Generic;
using SA.Runtime.Services.SaveLoad.Schemes;

namespace SA.Runtime.Services.SaveLoad.Models
{
    public class TestPlayerModel : BaseModel<TestPlayerScheme>
    {
        public TestPlayerModel(SavingScheduler savingScheduler) : base(savingScheduler)
        {
        }

        public override string DataName => "TestPlayerModel";

        public int Health 
        { 
            get => Data.Health; 
            set 
            {
                Data.Health = value; 
            }
        }

        public Dictionary<int, string> Stats
        { 
            get => Data.Stats; 
            set 
            {
                Data.Stats = value; 
            }
        }
    }
}