using Enemy;
using Enemy.Difficulties;
using Grid;
using Themes;
using UI;
using UnityEngine;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Infrastructure
{
    public class GameplayEssentialsInstaller : MonoInstaller, IValidatable
    {
        [SerializeField] private GameplayUIManager _gameplayUIManager;
        private ThemeSettings _playerThemeSettings;

        [Inject]
        private void Construct(ThemeSettings playerThemeSettings)
        {
            _playerThemeSettings = playerThemeSettings;
        }
        
        public override void InstallBindings()
        {
            InitExecutionOrder();

            Container.Bind<CharactersThemes>().AsSingle().WithArguments(_playerThemeSettings, GameResources.Instance.AIThemeSettings).NonLazy();
            
            StateMachine();
            TurnSystem();
            GameManager();
            ShipsManager();
            InteractionSystem();
            GridSystemSpawner();
            GridSystemFactory();
            GridSystemVisualFactory();
            GridCellVisualFactory();
            ShipFactory();
            ShipSpawner();
            Level();
            UIManager();
            AIDamagedShipSearcher();
            Difficulty();
            CameraController();
        }

        private void CameraController() => Container.BindInterfacesAndSelfTo<CameraController>().FromNew().AsSingle();

        private void StateMachine() => Container.Bind<StateMachine.StateMachine>().AsTransient();

        private void AIDamagedShipSearcher() => Container.Bind<AIDamagedShipSearcher>().AsSingle();

        private void GridSystemVisualFactory() =>
            Container
                .BindFactory<GridSystem, Vector3, ThemeSettings, GridSystemVisual, GridSystemVisual.Factory>()
                .FromNewComponentOnNewGameObject();

        private void GridCellVisualFactory() =>
            Container
                .BindFactory<GridCell, Vector2, Transform, ThemeSettings, Sprite, GridCellVisual, GridCellVisual.Factory>()
                .FromComponentInNewPrefab(GameResources.Instance.GridCellVisualPrefab);

        private void GridSystemFactory() =>
            Container
                .BindFactory<CharacterType, GridSystem, GridSystem.Factory>()
                .FromNew();

        private void InitExecutionOrder()
        {
            Container.BindExecutionOrder<TurnSystem>(-60);
            Container.BindExecutionOrder<GameManager>(-50);
            Container.BindExecutionOrder<Level>(-20);
        }

        private void InteractionSystem() =>
            Container.BindInterfacesAndSelfTo<InteractionSystem>().AsSingle().NonLazy();

        private void ShipsManager() => Container.BindInterfacesAndSelfTo<ShipsManager>().AsSingle();

        private void GameManager() => Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

        private void TurnSystem() =>
            Container.BindInterfacesAndSelfTo<TurnSystem>().AsSingle()
                .WithArguments(CharacterType.Player);

        private void Level() => Container.BindInterfacesAndSelfTo<Level>().AsSingle();

        private void Difficulty()
        {
            Container.Bind<IDifficulty>().To<Easy>().AsSingle();
            Container.Bind<EnemyAI>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void ShipFactory() =>
            Container
                .BindFactory<Ship.Ship, Ship.Ship.Factory>()
                .FromComponentInNewPrefab(GameResources.Instance.ShipPrefab);

        private void UIManager() => Container.Bind<GameplayUIManager>().FromInstance(_gameplayUIManager).AsSingle();

        private void ShipSpawner() => Container.Bind<ShipsSpawner>().AsSingle();

        private void GridSystemSpawner() => Container.Bind<GridSystemSpawner>().AsSingle();
        public void Validate() => throw new System.NotImplementedException();
    }
}