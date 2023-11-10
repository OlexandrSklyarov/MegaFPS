using SA.Runtime.Services.SaveLoad;
using SA.Runtime.Services.SaveLoad.Models;
using SA.Runtime.Services.SaveLoad.Schemes;
using UnityEngine;

namespace SA.FPS
{
    public class TestSaveLoadService : MonoBehaviour
    {
        private IDataProvider _dataProvider;
        private SavingScheduler _savingScheduler;

        async void Start()
        {
            _savingScheduler = new SavingScheduler();

            var register = new ModelRegister(_savingScheduler);

            var gameValue = PlayerPrefs.GetInt("IsNewGame" , 1);
            
            var prov = new JsonDataProvider(register, IsNewGame(gameValue));
            await prov.InitializeAsync();
            _dataProvider = prov;

            PlayerPrefs.SetInt("IsNewGame", 0);

            PrintData();

        }

        private void PrintData()
        {
            Util.DebugUtil.PrintColor($"PlayerModel:  {_dataProvider.Get<TestPlayerModel>().Health}", Color.green);
            foreach(var x in _dataProvider.Get<TestPlayerModel>().Stats)
            {
                Util.DebugUtil.PrintColor($"PlayerModel stat: {x.Key} {x.Value}", Color.green);
            }

             Util.DebugUtil.Print("***********************************************");

            Util.DebugUtil.PrintColor($"TestLevelModel:  {_dataProvider.Get<TestLevelModel>().Level}", Color.green);
            foreach(var x in _dataProvider.Get<TestLevelModel>().Builds)
            {
                Util.DebugUtil.PrintColor($"Build: {x.Key} w {x.Value.Windows} d {x.Value.Doors}", Color.green);
            }
        }

        private bool IsNewGame(int value) => value > 0;

        void Update()
        {
            _savingScheduler.Tick();

            if (Input.GetKeyDown(KeyCode.A))
            {
                var model = _dataProvider.Get<TestPlayerModel>();
                model.Health++;
                model.Stats.Add(model.Health, $"{Time.time}");
                model.SaveAsync().WrapErrors();

                Util.DebugUtil.PrintColor($"Change PlayerModel:  {_dataProvider.Get<TestPlayerModel>().Health}", Color.yellow);

                foreach(var x in _dataProvider.Get<TestPlayerModel>().Stats)
                {
                    Util.DebugUtil.PrintColor($"Change PlayerModel stat: {x.Key} {x.Value}", Color.yellow);
                }  

                var model2 = _dataProvider.Get<TestLevelModel>();
                model2.Level++;
                model2.Builds.Add(model2.Level + 1, new TestBuildScheme(){Windows = UnityEngine.Random.Range(2,8), Doors = UnityEngine.Random.Range(2,3)});
                model2.DelaySave();

                Util.DebugUtil.PrintColor($"Change PlayerModel:  {_dataProvider.Get<TestLevelModel>().Level}", Color.yellow);
            }
        }
    }
}
