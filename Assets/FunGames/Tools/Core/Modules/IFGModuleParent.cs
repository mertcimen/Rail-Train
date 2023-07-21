namespace FunGames.Tools
{
    public interface IFGModuleParent
    {
        public void AddModule(IFGModule childModule);
        
        public void RemoveModule(IFGModule childModule);

        public void ChildModuleInitialized(IFGModule childModule);
    }
}