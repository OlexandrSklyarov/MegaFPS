using System.Collections.Generic;
using SA.Runtime.Services.SaveLoad.Schemes;

namespace SA.Runtime.Services.SaveLoad.Models
{
    public class TestLevelModel : BaseModel<TestLevelScheme>
    {
        public TestLevelModel(SavingScheduler savingScheduler) : base(savingScheduler)
        {
        }

        public override string DataName => "TestLevelModel";

        public int Level 
        { 
            get => Data.Level; 
            set 
            {
                Data.Level = value; 
            }
        }

        public Dictionary<int, TestBuildScheme> Builds 
        { 
            get => Data.Builds; 
            set 
            {
                Data.Builds = value; 
            }
        }
    }
}