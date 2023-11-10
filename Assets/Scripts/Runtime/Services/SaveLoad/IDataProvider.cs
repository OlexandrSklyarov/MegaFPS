using SA.FPS;

namespace SA.Runtime.Services.SaveLoad
{
    public interface IDataProvider : IService
    {
        public T Get<T>() where T : class, IModel;

        public void Set<T>(T item) where T : class, IModel;
    }
}