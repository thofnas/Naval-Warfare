using AchievementSystem;
using Misc;
using UI.Elements;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.MainMenuUIStates
{
    public class Achievements : BaseState
    {
        private readonly AchievementStorage _achievementStorage;

        public Achievements(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet, AchievementStorage achievementStorage) : base(mainMenuUIManager, stateMachine)
        {
            _achievementStorage = achievementStorage;
            
            Root = VisualElementHelper.CreateDocument(nameof(Settings), styleSheet);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            ClearAndGenerateUI();
        }

        protected sealed override void GenerateUI()
        {
            VisualElement container = Root.CreateChild("container");

            ScrollView scrollView = new ()
            {
                mode = ScrollViewMode.Vertical,
                verticalScrollerVisibility = ScrollerVisibility.Hidden
            };
            
            container.Add(scrollView);
            StyledPanel achievementsContainer = new(MainMenuUIManager.SelectedTheme.PlayerTheme, "achievements-container");
            scrollView.Add(achievementsContainer);
            
            foreach (IAchievement achievement in _achievementStorage.Achievements)
            {
                StyledPanel achievementItem = new(MainMenuUIManager.SelectedTheme.PlayerTheme, "achievement-item");
                achievementsContainer.Add(achievementItem);
                VisualElement achievementInfo = achievementItem.CreateChild("achievement-info");
                achievementInfo.Add(new Label(achievement.Info.Name));
                achievementInfo.Add(new Label(achievement.Info.UnlockCondition));
                
                VisualElement achievementStatus = achievementItem.CreateChild("achievement-status");
                
                achievementStatus.Add(achievement.IsUnlocked
                    ? new Label(TextData.Unlocked)
                    : new Label($"{achievement.Reward.Amount} {(string)achievement.Reward.Name}"));
            }
            
            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = TextData.BackButton
            };
        }

        protected override VisualElement Root { get; }
    }
}