using Enemy;
using Enemy.Difficulties;
using Grid;
using Themes;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Infrastructure
{
    public class GameplayEssentialsInstaller : MonoInstaller, IValidatable
    {
        [SerializeField] private GameplayUIManager _gameplayUIManager;
        private Theme _playerTheme;

        [Inject]
        private void Construct(Theme playerTheme)
        {
            _playerTheme = playerTheme;
        }
        
        public override void InstallBindings()
        {
            InitExecutionOrder();

            BackgroundAnimator();
            
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

        private void BackgroundAnimator() => Container.BindInterfacesAndSelfTo<BackgroundAnimator>().FromComponentInNewPrefab(GameResources.Instance.BackgroundPrefab).AsSingle().NonLazy();

        private void CameraController() => Container.BindInterfacesAndSelfTo<CameraController>().FromNew().AsSingle();

        private void AIDamagedShipSearcher() => Container.Bind<AIDamagedShipSearcher>().AsSingle();

        private void GridSystemVisualFactory() =>
            Container
                .BindFactory<GridSystem, Vector3, Theme, GridSystemVisual, GridSystemVisual.Factory>()
                .FromNewComponentOnNewGameObject();

        private void GridCellVisualFactory() =>
            Container
                .BindFactory<GridCell, Vector2, Transform, Theme, Sprite, GridCellVisual, GridCellVisual.Factory>()
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