
using System.Collections.Generic;
using System.Linq;

namespace SA.Runtime.Services.SaveLoad.Schemes
{
    public class TestLevelScheme : BaseScheme
    {
        public int Level;
        public Dictionary<int, TestBuildScheme> Builds = new Dictionary<int, TestBuildScheme>();

        protected override void DeserializeProperties(Dictionary<string, object> data)
        {
            Level = GetValue<int>("Level", data);

            var list = data.GetJsonValue<Dictionary<int, Dictionary<string, object>>>("Builds");
            Builds.Clear();
            foreach (var item in list)
            {
                var f = new TestBuildScheme();
                f.Deserialize(item.Value);
                Builds.Add(item.Key, f);
            }
        }

        protected override Dictionary<string, object> SerializeProperties()
        {
            return new Dictionary<string, object>
            {
                ["Level"] = Level,
                ["Builds"] = Builds.ToDictionary(f => f.Key, f => f.Value.Serialize())
            };
        }
    }
    

    public class TestBuildScheme : BaseScheme
    {
        public int Windows;
        public int Doors;

        protected override void DeserializeProperties(Dictionary<string, object> data)
        {
            Windows = GetValue<int>("Windows", data);
            Doors = GetValue<int>("Doors", data);
        }

        protected override Dictionary<string, object> SerializeProperties()
        {
            return new Dictionary<string, object>
            {
                ["Windows"] = Windows,
                ["Doors"] = Doors
            };
        }
    }
}