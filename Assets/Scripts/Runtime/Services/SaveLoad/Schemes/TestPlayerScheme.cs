using System.Collections.Generic;

namespace SA.Runtime.Services.SaveLoad.Schemes
{
    public class TestPlayerScheme : BaseScheme
    {
        public int Health;
        public Dictionary<int, string> Stats = new Dictionary<int, string>();


        protected override void DeserializeProperties(Dictionary<string, object> data)
        {
            Health = GetValue<int>("Health", data);
            Stats = GetValue<Dictionary<int, string>>("Stats", data);
        }

        protected override Dictionary<string, object> SerializeProperties()
        {
            return new Dictionary<string, object>
            {
                ["Health"] = Health,
                ["Stats"] = Stats
            };
        }
    }
}