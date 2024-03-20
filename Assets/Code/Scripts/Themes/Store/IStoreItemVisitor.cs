namespace Themes.Store
{
    public interface IStoreItemVisitor
    {
        void Visit(StoreItem storeItem);

        void Visit(IslandsThemeItem islandsThemeItem);
    
        void Visit(OceanThemeItem oceanThemeItem);
    }
}