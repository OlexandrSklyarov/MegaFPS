using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SA.Runtime.Services.SaveLoad.Parser;
using UnityEngine;

namespace SA.Runtime.Services.SaveLoad
{
    public abstract class BaseModel<T> : IModel where T : BaseScheme, new()
    {
        public abstract string DataName { get; }
        public bool SaveLock { get; private set; }
        public int Tick {get; set;}

        protected T Data;        
        protected SavingScheduler _savingScheduler;

        
        public BaseModel(SavingScheduler savingScheduler)
        {
            _savingScheduler = savingScheduler;
            SaveLock = false;
            Tick = 0;
        }


        public virtual async UniTask<bool> ReadAsync()
        {
            Data = new T();
            Dictionary<string, object> saveData = null;
            try
            {
                saveData = await DataParser.DeserializeAsync(ParserDataConfig.DataPath, DataName);
            }
            catch (Exception e)
            {
                Debug.LogError($"Private data - {DataName}: Read binary fail with exception - {e.Message}");
            }

            if (saveData == null)
            {
                return false;
            }

            Data.Deserialize(saveData);
            saveData.Clear();
            return true;
        }


        public virtual async UniTask SaveAsync()
        {
            if (Data == null) return;

            if (SaveLock)
            {
                await UniTask.WaitWhile(() => SaveLock);
            }

            SaveLock = true;

            await DataParser.SerializeAsync(ParserDataConfig.DataPath, DataName, Data.Serialize());
            
            SaveLock = false;   
        }


        public void DelaySave()
        {
            Tick = 0;
            _savingScheduler.Push(this);
        }


        public virtual void Setup()
        {
            Data = new T();
            SaveAsync().WrapErrors();
        }


        public virtual UniTask Merge(Dictionary<string, object> inData)
        {
            if (Data == null)
            {
                Debug.LogError($"Merge not inited document! {GetType()}");
                return default;
            }

            //предполагаем что нам приходят только измененные поля и заменяем их у нас
            Data.Deserialize(inData);

            SaveAsync().WrapErrors();
			return default;
        }

        protected void SetKeyDirty(string key)
        {
            Data.AddDirtyKey(key);
        }
    }
}