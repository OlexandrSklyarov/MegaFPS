using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace SA.Runtime.Services.SaveLoad
{
    public interface IModel
    {
        string DataName { get; }
        bool SaveLock { get; }
        int Tick { get; set; }

        UniTask<bool> ReadAsync();
      
        UniTask SaveAsync();

        void DelaySave();
		
		UniTask Merge(Dictionary<string, object> inData);
       
        void Setup();
    }
}