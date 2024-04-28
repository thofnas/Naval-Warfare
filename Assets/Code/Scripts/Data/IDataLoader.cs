namespace Data
{
    public interface IDataLoader
    {
        public bool TryLoad(out PersistentData loadedData);
    }
}