using UnityEngine.UIElements;
using Utilities;

namespace UI
{
    public interface IUIScreen : IComponent
    {
        public void Init(GameplayUIManager gameplayUIManager);

        public UIDocument GetUIDocument();
    }
}