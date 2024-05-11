using UI;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.GameplayUIStates
{
    public class Battle : BaseState
    {
        public Battle(GameplayUIManager gameplayUIManager, StyleSheet styleSheet) : base(gameplayUIManager)
        {
            Root = CreateDocument(nameof(Battle), styleSheet);

            GenerateView();
        }

        protected sealed override VisualElement Root { get; }

        public sealed override void GenerateView()
        {
        }
    }
}