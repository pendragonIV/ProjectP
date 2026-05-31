using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Configs.Currency;
using Things.Game.Services.Currency;
using Things.Game.Services.Audio;
using Things.Game.Services.Upgrade;
using Things.Game.Services.Mission;
using Things.Game.Services.Energy;
using Things.Game.Services.World;
using Things.Game.Services.Navigation;
using Things.Game.Services.NavMesh;

namespace Things.Game.Bootstrap
{
    [DefaultExecutionOrder(-100)]
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private CurrencyDatabase currencyDatabase;
        [SerializeField] private Things.Game.Configs.Upgrade.UpgradeDatabase upgradeDatabase;
        [SerializeField] private Things.Game.Configs.PlayerConfig.PlayerStatsConfig playerStatsConfig;
        [SerializeField] private Things.Game.Configs.Craft.CraftDatabase craftDatabase;
        [SerializeField] private Things.Game.Configs.Cooking.CookingDatabase cookingDatabase;
        [SerializeField] private Things.Game.Configs.Fishing.FishingDatabase fishingDatabase;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializeServices();
        }

        private void InitializeServices()
        {
            ServiceLocator.Reset();

            // Core Infrastructure Services
            ServiceLocator.Register<IPoolManager>(new PoolManager());
            ServiceLocator.Register<ISaveService>(new JsonSaveService());

            // Config-dependent services
            if (currencyDatabase != null)
            {
                currencyDatabase.Initialize();
                ServiceLocator.Register<ICurrencyService>(new CurrencyService(currencyDatabase));
            }

            // Game services
            ServiceLocator.Register<IAudioService>(new AudioService());
            ServiceLocator.Register<IUpgradeService>(new UpgradeService(upgradeDatabase));
            ServiceLocator.Register<IMissionService>(new MissionService());
            
            float maxEnergy = playerStatsConfig != null ? playerStatsConfig.MaxEnergy : 100f;
            ServiceLocator.Register<IEnergyService>(new EnergyService(maxEnergy));
            
            ServiceLocator.Register<IWorldService>(new WorldService());
            ServiceLocator.Register<INavigationService>(new NavigationService());
            ServiceLocator.Register<INavMeshService>(new NavMeshService());

            // Things MMO Systems
            ServiceLocator.Register<Things.Game.Services.Fishing.IFishingService>(new Things.Game.Services.Fishing.FishingService(fishingDatabase));
            ServiceLocator.Register<Things.Game.Services.Craft.ICraftService>(new Things.Game.Services.Craft.CraftService(craftDatabase));
            ServiceLocator.Register<Things.Game.Services.Cooking.ICookingService>(new Things.Game.Services.Cooking.CookingService(cookingDatabase));
            ServiceLocator.Register<Things.Game.Services.Pet.IPetService>(new Things.Game.Services.Pet.PetService());
            ServiceLocator.Register<Things.Game.Services.Social.IFriendshipService>(new Things.Game.Services.Social.FriendshipService());
            ServiceLocator.Register<Things.Game.Services.Island.IIslandService>(new Things.Game.Services.Island.IslandService());
            // Initialize legacy SaveController via Reflection to avoid Assembly-CSharp dependency
            System.Type saveControllerType = System.Type.GetType("Things.SaveController, Assembly-CSharp");
            if (saveControllerType != null)
            {
                var initMethod = saveControllerType.GetMethod("Init", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (initMethod != null)
                {
                    initMethod.Invoke(null, new object[] { 0.5f, false });
                }
            }

            Debug.Log("[GameBootstrapper] All Core Services Registered successfully!");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                // Auto-save when app goes to background (critical for mobile)
                if (ServiceLocator.TryGet<ISaveService>(out var saveService))
                {
                    saveService.Flush();
                }
            }
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.DisposeAll();
        }

        private void OnDestroy()
        {
            ServiceLocator.DisposeAll();
        }
    }
}
