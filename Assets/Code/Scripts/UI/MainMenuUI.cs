using Themes;
using UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utilities.Extensions;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private StyleSheet _styleSheet;
        private VisualElement _root;
        private ThemeSettings _themeSettings;

        private void Awake()
        {
            var uiDocument = new GameObject(nameof(MainMenuUI)).AddComponent<UIDocument>();
            uiDocument.panelSettings = GameResources.Instance.UIDocumentPrefab.panelSettings;
            uiDocument.visualTreeAsset = GameResources.Instance.UIDocumentPrefab.visualTreeAsset;
            _root = uiDocument.rootVisualElement;

            _root.styleSheets.Add(_styleSheet);

            GenerateUI();
        }

        [Inject]
        private void Construct(ThemeSettings themeSettings) => _themeSettings = themeSettings;

        private void GenerateUI()
        {
            VisualElement container = _root.CreateChild("container");

            VisualElement titleContainer = container.CreateChild("title-container");
            titleContainer.Add(new Label("Battleship"));

            VisualElement buttonsContainer = container.CreateChild("buttons-container");

            StyledButton startGameButton = new(_themeSettings, buttonsContainer,
                () => SceneManager.LoadScene("Gameplay"), "start-button")
            {
                text = "Start Game"
            };

            StyledButton optionsButton = new(_themeSettings, buttonsContainer, "options-button")
            {
                text = "Options"
            };
        }
    }
}