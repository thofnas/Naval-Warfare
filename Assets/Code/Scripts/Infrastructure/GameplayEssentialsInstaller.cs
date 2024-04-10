using Audio;
using Data;
using Enemy;
using Enemy.Difficulties;
using Grid;
using Map;
using Themes;
using UI;
using UnityEngine;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Infrastructure
{
    public class GameplayEssentialsInstaller : MonoInstaller
    {
        [SerializeField] private GameplayUIManager _gameplayUIManager;

        [Inject]
        private void Construct(PersistentData persistentData, MapLibrary mapLibrary)
        {
            Container.Bind<Map.Map>().FromInstance(mapLibrary.Maps[persistentData.PlayerData.SelectedMapType]).AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSfxManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleMusicManager>().AsSingle();
        }

        public override void InstallBindings()
        {
            InitExecutionOrder();

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
            BackgroundAnimator();
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
            Container.BindExecutionOrder<GameManager>(-50);
            Container.BindExecutionOrder<TurnSystem>(-40);
            Container.BindExecutionOrder<LevelManager>(-20);
        }

        private void InteractionSystem() =>
            Container.BindInterfacesAndSelfTo<InteractionSystem>().AsSingle().NonLazy();

        private void ShipsManager() => Container.BindInterfacesAndSelfTo<ShipsManager>().AsSingle();

        private void GameManager() => Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

        private void TurnSystem() =>
            Container.BindInterfacesAndSelfTo<TurnSystem>().AsSingle()
                .WithArguments(CharacterType.Player);

        private void Level() => Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();

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
    }
}