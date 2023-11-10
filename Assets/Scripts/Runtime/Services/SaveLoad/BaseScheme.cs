using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SA.Runtime.Services.SaveLoad
{
    public abstract class BaseScheme : IScheme
    {
        protected List<string> DirtyKeys = new List<string>();


        public Dictionary<string, object> Serialize()
        {
            var data = SerializeProperties();
            if(DirtyKeys != null && DirtyKeys.Count > 0)
                data.Add("DirtyKeys", DirtyKeys);
            return data;
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            DeserializeProperties(data);

            DirtyKeys = GetValue<List<string>>("DirtyKeys", data) ?? new List<string>();
        }        

        protected T GetValue<T>(string key, IDictionary<string, object> data)
        {
            if (data == null)
            {
                Debug.LogError($"Data is null. {GetType()}");
                return default;
            }

            if (!data.TryGetValue(key, out var obj)) return default;

            if (obj == null)
                return default;

            if (obj is JObject jObject)
            {
                return jObject.ToObject<T>();
            }

            if (obj is JArray jArray)
            {
                return jArray.ToObject<T>();
            }

            return (T) Convert.ChangeType(obj, typeof(T));
        }

        public void AddDirtyKey(string key)
        {
            if (DirtyKeys.Contains(key))
                return;
            DirtyKeys.Add(key);
        }

        public void RemoveDirty(List<string> keys)
        {
            DirtyKeys.RemoveAll(keys.Contains);
        }

        protected abstract Dictionary<string, object> SerializeProperties();

        protected abstract void DeserializeProperties(Dictionary<string, object> data);
    }
}