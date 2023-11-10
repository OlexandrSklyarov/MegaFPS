using System.Collections.Generic;

namespace SA.Runtime.Services.SaveLoad
{
    public interface IScheme
    {
        void AddDirtyKey(string key);
        void RemoveDirty(List<string> keys);
        Dictionary<string, object> Serialize();
        void Deserialize(Dictionary<string, object> data);
    }
}