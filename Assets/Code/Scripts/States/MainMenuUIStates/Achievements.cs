using Achievements;
using UI;
using UI.Elements;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class Achievements : BaseState
    {
        private readonly AchievementStorage _achievementStorage;

        public Achievements(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet, AchievementStorage achievementStorage) : base(mainMenuUIManager, stateMachine)
        {
            _achievementStorage = achievementStorage;
            
            Root = CreateDocument(nameof(Settings), styleSheet);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            GenerateView();
        }

        protected sealed override void GenerateView()
        {
            base.GenerateView();
            
            VisualElement container = Root.CreateChild("container");
            
            StyledPanel achievementsContainer = new(MainMenuUIManager.SelectedTheme.PlayerTheme, "achievements-container");
            container.Add(achievementsContainer);
            
            foreach (IAchievement achievement in _achievementStorage.Achievements)
            {
                VisualElement achievementItem = achievementsContainer.CreateChild("achievement-item");
                VisualElement achievementInfo = achievementItem.CreateChild("achievement-info");
                achievementInfo.Add(new Label(achievement.Name));
                achievementInfo.Add(new Label(achievement.UnlockCondition));
                
                VisualElement achievementStatus = achievementItem.CreateChild("achievement-status");
                if (achievement.IsUnlocked) 
                    achievementStatus.Add(new Label("unlocked"));
            }
            
            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }

        protected override VisualElement Root { get; }
    }
}