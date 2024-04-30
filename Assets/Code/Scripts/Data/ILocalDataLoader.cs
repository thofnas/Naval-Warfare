namespace Data
{
    public interface ILocalDataLoader
    {
        public bool TryLoad(out PersistentData loadedData);
    }
}