namespace SA.FPS
{
    public interface IPoolManager : IService 
    {
        Decal GetDecal(DecalType type);
    }
}
