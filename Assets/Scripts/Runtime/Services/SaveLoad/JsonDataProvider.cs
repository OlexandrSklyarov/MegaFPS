using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SA.Runtime.Services.SaveLoad
{
    public class JsonDataProvider : IDataProvider
    {
        private readonly ModelRegister _modelRegister;
        private readonly bool _isNewGame;
        private List<IModel> _dataModels;


        public JsonDataProvider(ModelRegister modelRegister,  bool isNewGame)
        {
            _modelRegister = modelRegister;
            _isNewGame = isNewGame; 
        }


        public async UniTask InitializeAsync()
        {
            _dataModels = _modelRegister.GetAllModels();

            if (_isNewGame)
            {
                SetupAll();
            }
            else
            {
                await ReadAll();
            }
        }


        private void SetupAll()
        {
            foreach (var dataModel in _dataModels)
            {
                dataModel.Setup();
            }
        }


        private async UniTask ReadAll()
        {
            await UniTask.WhenAll(_dataModels.Select(ReadModelAsync));
        }


        private async UniTask ReadModelAsync(IModel dataModel)
        {
            try
            {
                if (!await dataModel.ReadAsync())
                {
                    Debug.LogWarning($"Direct pull {dataModel.DataName}");                    
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Initializing model {dataModel.DataName} fail. {e.Message} \n {e.StackTrace}");
            }
        }

        
        public T Get<T>() where T : class, IModel
        {
            return _dataModels.Find(x => x is T) as T;
        }


        public void Set<T>(T item) where T : class, IModel
        {
            var index = _dataModels.FindIndex(x => x is T);
            if (index == -1)
                _dataModels.Add(item);
            else
                _dataModels[index] = item;
        }
    }
}