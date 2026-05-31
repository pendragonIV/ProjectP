# 📐 ProjectP - Kế Hoạch Chuyển Đổi Kiến Trúc
# Hybrid MVP + Data-Driven + Event-Driven

> **Phiên bản**: 1.0  
> **Ngày tạo**: 31/05/2026  
> **Pattern**: MVP (Model-View-Presenter) + Data-Driven (ScriptableObject) + Event-Driven (EventBus)  
> **Ưu tiên**: Mở rộng → Hiệu năng → Testability → Dễ onboard → Data-driven  
> **Scope**: Viết lại kiến trúc toàn bộ, giữ nguyên gameplay logic  

---

## Mục Lục

1. [Tổng Quan Kiến Trúc Mới](#1-tổng-quan-kiến-trúc-mới)
2. [Nguyên Tắc Thiết Kế](#2-nguyên-tắc-thiết-kế)
3. [Cấu Trúc Folder Mới](#3-cấu-trúc-folder-mới)
4. [Phase 0 — Foundation Infrastructure](#4-phase-0--foundation-infrastructure)
5. [Phase 1 — Data Layer](#5-phase-1--data-layer)
6. [Phase 2 — Core Services Migration](#6-phase-2--core-services-migration)
7. [Phase 3 — Player System Decomposition](#7-phase-3--player-system-decomposition)
8. [Phase 4 — World & Game Systems](#8-phase-4--world--game-systems)
9. [Phase 5 — Performance & Polish](#9-phase-5--performance--polish)
10. [Mapping File Cũ → Mới](#10-mapping-file-cũ--mới)
11. [Quyết Định Đã Xác Nhận](#11-quyết-định-đã-xác-nhận)
12. [Verification Plan](#12-verification-plan)
13. [Timeline & Milestones](#13-timeline--milestones)

---

## 1. Tổng Quan Kiến Trúc Mới

### 1.1 Sơ Đồ Kiến Trúc Tổng Quan

```
┌──────────────────────────────────────────────────────────────────────┐
│                        BOOTSTRAP LAYER                              │
│  GameBootstrapper → đăng ký Services → khởi tạo theo priority       │
└──────────────┬───────────────────────────────────────────────────────┘
               │
┌──────────────▼───────────────────────────────────────────────────────┐
│                     INFRASTRUCTURE LAYER                             │
│  ┌──────────────┐ ┌──────────┐ ┌─────────────┐ ┌────────────────┐   │
│  │ServiceLocator│ │ EventBus │ │PoolManager  │ │ UpdateManager  │   │
│  └──────────────┘ └──────────┘ └─────────────┘ └────────────────┘   │
│  ┌──────────────┐                                                    │
│  │ SaveService  │                                                    │
│  └──────────────┘                                                    │
└──────────────┬───────────────────────────────────────────────────────┘
               │
┌──────────────▼───────────────────────────────────────────────────────┐
│                        DATA LAYER                                    │
│                                                                      │
│  ┌─────────────────────────┐    ┌───────────────────────────┐        │
│  │ ScriptableObject Config │    │   Pure C# Models          │        │
│  │ (static, designer-edit) │    │   (runtime state)         │        │
│  │                         │    │                           │        │
│  │ • PlayerMovementConfig  │    │ • HealthModel             │        │
│  │ • CurrencyDefinition   │    │ • InventoryModel          │        │
│  │ • EnemyDefinition      │    │ • CurrencyModel           │        │
│  │ • UpgradeDefinition    │    │ • EnergyModel             │        │
│  │ • WorldDefinition      │    │ • WorldStateModel         │        │
│  │ • MissionDefinition    │    │ • MissionProgressModel    │        │
│  │ • BuildingDefinition   │    │ • BuildingStateModel      │        │
│  │ • EnvironmentPreset    │    │ • UpgradeModel            │        │
│  └─────────────────────────┘    └───────────────────────────┘        │
└──────────────┬───────────────────────────────────────────────────────┘
               │
┌──────────────▼───────────────────────────────────────────────────────┐
│                       SERVICE LAYER                                  │
│                                                                      │
│  ┌─────────────────┐ ┌─────────────────┐ ┌────────────────────┐     │
│  │ CurrencyService │ │ UpgradeService  │ │  MissionService    │     │
│  ├─────────────────┤ ├─────────────────┤ ├────────────────────┤     │
│  │ EnergyService   │ │ AudioService    │ │  NavigationService │     │
│  ├─────────────────┤ ├─────────────────┤ ├────────────────────┤     │
│  │ WorldService    │ │ NavMeshService  │ │  ToolUnlockService │     │
│  └─────────────────┘ └─────────────────┘ └────────────────────┘     │
└──────────────┬───────────────────────────────────────────────────────┘
               │
┌──────────────▼───────────────────────────────────────────────────────┐
│                     PRESENTER LAYER (MVP)                            │
│                                                                      │
│  ┌──────────────────────────────────────────────────┐                │
│  │ PlayerPresenter (orchestrator)                    │                │
│  │  ├─ PlayerMovementPresenter                       │                │
│  │  ├─ PlayerCombatPresenter                         │                │
│  │  ├─ PlayerHarvestPresenter                        │                │
│  │  ├─ PlayerSwimPresenter                           │                │
│  │  ├─ PlayerResourcePresenter                       │                │
│  │  └─ PlayerInventoryPresenter                      │                │
│  └──────────────────────────────────────────────────┘                │
│  ┌───────────────┐ ┌──────────────┐ ┌────────────────┐              │
│  │WorldPresenter │ │EnemyPresenter│ │HelperPresenter │              │
│  └───────────────┘ └──────────────┘ └────────────────┘              │
│  ┌────────────────────┐ ┌─────────────────┐ ┌──────────────────┐    │
│  │BuildingPresenter   │ │MissionPresenter │ │EnergyPresenter   │    │
│  └────────────────────┘ └─────────────────┘ └──────────────────┘    │
└──────────────┬───────────────────────────────────────────────────────┘
               │
┌──────────────▼───────────────────────────────────────────────────────┐
│                        VIEW LAYER                                    │
│                                                                      │
│  ┌────────────┐ ┌──────────────┐ ┌──────────────┐ ┌──────────────┐  │
│  │ PlayerView │ │ BuildingView │ │  EnemyView   │ │ ResourceView │  │
│  └────────────┘ └──────────────┘ └──────────────┘ └──────────────┘  │
│  ┌─────────────────┐ ┌──────────────────┐ ┌──────────────────────┐  │
│  │EnvironmentView  │ │ UI Views (HUD,   │ │ MissionUIView        │  │
│  │                  │ │ Store, Pause...) │ │                      │  │
│  └─────────────────┘ └──────────────────┘ └──────────────────────┘  │
└──────────────────────────────────────────────────────────────────────┘
```

### 1.2 Luồng Dữ Liệu (Data Flow)

```
User Input → Presenter → Model (update state)
                │                │
                │                ├── EventBus.Publish(StateChangedEvent)
                │                │
                └── View.Render(newState) ◄── Presenter reads Model
```

**Quy tắc bất di bất dịch:**
- View **KHÔNG BAO GIỜ** truy cập Model trực tiếp
- Model **KHÔNG BIẾT** View/Presenter tồn tại
- Presenter là **cầu nối duy nhất** giữa Model ↔ View
- Systems giao tiếp qua **EventBus**, không direct reference

---

## 2. Nguyên Tắc Thiết Kế

| # | Nguyên tắc | Mô tả | Ví dụ |
|---|-----------|-------|-------|
| 1 | **Model = Pure C#** | Không `MonoBehaviour`, không Unity API. Chỉ data + business logic | `HealthModel`, `InventoryModel` |
| 2 | **View = Passive Display** | `MonoBehaviour` trên scene. Chỉ hiển thị, không logic. Expose events cho Presenter | `PlayerView.OnHitAnimationEnd` |
| 3 | **Presenter = Logic Hub** | `MonoBehaviour` cầu nối. Lắng nghe View events → cập nhật Model → push data về View | `PlayerCombatPresenter` |
| 4 | **Config = ScriptableObject** | Data tĩnh do designer chỉnh. Không chứa runtime state | `PlayerMovementConfig.asset` |
| 5 | **Service = Stateless Logic** | Đăng ký trong ServiceLocator. Accessible everywhere via interface | `ICurrencyService` |
| 6 | **EventBus = Decoupled Comm** | Systems giao tiếp qua typed events. Không direct coupling | `CurrencyChangedEvent` |
| 7 | **UpdateManager = 1 Loop** | Centralized update thay 50+ `Update()` riêng lẻ | `IUpdatable.OnUpdate()` |
| 8 | **Composition > Inheritance** | Ưu tiên compose behaviors từ nhỏ hơn thay vì kế thừa sâu | Player = 7 sub-presenters |

---

## 3. Cấu Trúc Folder Mới

```
Assets/
│
├── _Core/                                  # Framework layer — KHÔNG phụ thuộc game logic
│   ├── Infrastructure/
│   │   ├── ServiceLocator/
│   │   │   ├── IService.cs                 # [NEW] Interface cho mọi service
│   │   │   ├── ServiceLocator.cs           # [NEW] Registry & lifecycle manager
│   │   │   └── ServiceAttribute.cs         # [NEW] [Service(priority=10)] attribute
│   │   │
│   │   ├── EventBus/
│   │   │   ├── IGameEvent.cs               # [NEW] Marker interface cho events
│   │   │   ├── EventBus.cs                 # [NEW] Publish/Subscribe typed events
│   │   │   ├── EventBusListener.cs         # [NEW] MonoBehaviour auto-unsub on destroy
│   │   │   └── EventBinding.cs             # [NEW] Subscription handle for manual unsub
│   │   │
│   │   ├── Pool/
│   │   │   ├── IPoolManager.cs             # [NEW] Interface
│   │   │   └── PoolManager.cs              # [REFACTOR] Từ Core/Modules/Pool
│   │   │
│   │   ├── Save/
│   │   │   ├── ISaveService.cs             # [NEW] Interface
│   │   │   └── SaveService.cs              # [REFACTOR] Từ Core/Modules/Save
│   │   │
│   │   └── UpdateManager/
│   │       ├── IUpdatable.cs               # [NEW] interface { void OnUpdate(float dt); }
│   │       ├── UpdateManager.cs            # [NEW] Centralized update loop
│   │       └── UpdatePhase.cs              # [NEW] enum { Early, Normal, Late, Fixed }
│   │
│   ├── Extensions/                         # [MOVE] Từ Core/Utils + Extensions
│   │   └── ... (giữ nguyên)
│   │
│   ├── Tween/                              # [KEEP] Từ Core/Modules/Tween (không đổi)
│   │   └── ...
│   │
│   └── UI/                                 # [KEEP] Base UI framework
│       ├── UIController.cs
│       ├── UIPage.cs
│       └── ...
│
├── _Game/                                  # Game-specific code
│   │
│   ├── Bootstrap/
│   │   ├── GameBootstrapper.cs             # [NEW] Thay GameController.Awake()
│   │   ├── ServiceRegistration.cs          # [NEW] Đăng ký tất cả services
│   │   └── GameLifecycle.cs                # [NEW] OnGameStart, OnGameEnd, OnWorldTransition
│   │
│   ├── Configs/                            # ScriptableObject DEFINITIONS (code)
│   │   ├── Player/
│   │   │   ├── PlayerMovementConfig.cs     # [NEW] speed, acceleration, rotation
│   │   │   ├── PlayerCombatConfig.cs       # [NEW] health, regen, damage
│   │   │   ├── PlayerSwimmingConfig.cs     # [NEW] swim duration, speed mult
│   │   │   └── PlayerHarvestConfig.cs      # [NEW] hit radius, snap speed
│   │   │
│   │   ├── Currency/
│   │   │   ├── CurrencyDefinition.cs       # [NEW] SO — thay CurrencyType enum
│   │   │   └── CurrencyDatabase.cs         # [REFACTOR] Registry of all currencies
│   │   │
│   │   ├── Resource/
│   │   │   ├── ResourceSourceConfig.cs     # [NEW] hits, respawn, drop table
│   │   │   └── DropAnimationConfig.cs      # [NEW] drop physics settings
│   │   │
│   │   ├── Enemy/
│   │   │   └── EnemyDefinition.cs          # [NEW] SO — health, speed, drop, AI type
│   │   │
│   │   ├── Building/
│   │   │   └── BuildingDefinition.cs       # [NEW] SO — cost, construction hits
│   │   │
│   │   ├── Upgrade/
│   │   │   ├── UpgradeDefinition.cs        # [REFACTOR] Từ AbstactGlobalUpgrade
│   │   │   └── UpgradeStageData.cs         # [REFACTOR] Từ GlobalUpgradeStage
│   │   │
│   │   ├── Mission/
│   │   │   └── MissionDefinition.cs        # [NEW] SO — title, reward, navigation
│   │   │
│   │   ├── Energy/
│   │   │   └── EnergyConfig.cs             # [REFACTOR] Từ EnergySystemDatabase
│   │   │
│   │   ├── World/
│   │   │   └── WorldDefinition.cs          # [REFACTOR] Từ WorldData
│   │   │
│   │   └── Environment/
│   │       ├── EnvironmentPresetConfig.cs  # [REFACTOR] Từ EnvironmentPreset
│   │       └── WeatherPresetConfig.cs      # [REFACTOR]
│   │
│   ├── Models/                             # Pure C# runtime state — NO MonoBehaviour
│   │   ├── HealthModel.cs                  # [NEW] HP logic, events: OnDepleted, OnChanged
│   │   ├── InventoryModel.cs               # [NEW] Capacity, add/remove, full check
│   │   ├── CurrencyModel.cs                # [NEW] Per-currency runtime amount
│   │   ├── EnergyModel.cs                  # [NEW] Energy points, food consumption
│   │   ├── UpgradeModel.cs                 # [NEW] Current level, stage reference
│   │   ├── WorldStateModel.cs              # [NEW] Current world, loaded elements
│   │   ├── MissionProgressModel.cs         # [NEW] Active mission, stage tracking
│   │   └── BuildingStateModel.cs           # [NEW] isPurchased, isConstructed, hitsLeft
│   │
│   ├── Events/                             # Typed event definitions
│   │   ├── CurrencyEvents.cs               # [NEW] CurrencyChangedEvent { CurrencyId, Delta, NewAmount }
│   │   ├── PlayerEvents.cs                 # [NEW] PlayerSpawned, PlayerDied, PlayerDamaged
│   │   ├── BuildingEvents.cs               # [NEW] BuildingPurchased, BuildingConstructed
│   │   ├── MissionEvents.cs                # [NEW] MissionStarted, MissionCompleted, MissionRewardClaimed
│   │   ├── WorldEvents.cs                  # [NEW] WorldLoadStarted, WorldLoaded, WorldUnloaded
│   │   ├── UpgradeEvents.cs                # [NEW] UpgradePurchased { UpgradeType, NewLevel }
│   │   ├── EnergyEvents.cs                 # [NEW] EnergyChanged, EnergyDepleted, FoodConsumed
│   │   ├── ResourceEvents.cs               # [NEW] ResourceHarvested, ResourceDropped, ResourcePicked
│   │   └── AIEvents.cs                     # [NEW] HelperTaskStarted, HelperTaskCompleted
│   │
│   ├── Services/                           # Game services — registered in ServiceLocator
│   │   ├── Currency/
│   │   │   ├── ICurrencyService.cs         # [NEW] Interface
│   │   │   └── CurrencyService.cs          # [REFACTOR] Từ CurrencyController (static → instance)
│   │   ├── Audio/
│   │   │   ├── IAudioService.cs            # [NEW]
│   │   │   └── AudioService.cs             # [REFACTOR] Từ AudioController
│   │   ├── Upgrade/
│   │   │   ├── IUpgradeService.cs          # [NEW]
│   │   │   └── UpgradeService.cs           # [REFACTOR] Từ GlobalUpgradesController
│   │   ├── Mission/
│   │   │   ├── IMissionService.cs          # [NEW]
│   │   │   └── MissionService.cs           # [REFACTOR] Từ MissionsController
│   │   ├── Energy/
│   │   │   ├── IEnergyService.cs           # [NEW]
│   │   │   └── EnergyService.cs            # [REFACTOR] Từ EnergyController (static → instance)
│   │   ├── World/
│   │   │   ├── IWorldService.cs            # [NEW]
│   │   │   └── WorldService.cs             # [REFACTOR] Từ WorldController
│   │   ├── Navigation/
│   │   │   ├── INavigationService.cs       # [NEW]
│   │   │   └── NavigationService.cs        # [REFACTOR] Từ NavigationHelper
│   │   ├── NavMesh/
│   │   │   ├── INavMeshService.cs          # [NEW]
│   │   │   └── NavMeshService.cs           # [REFACTOR] Từ NavMeshController
│   │   ├── FloatingText/
│   │   │   ├── IFloatingTextService.cs     # [NEW]
│   │   │   └── FloatingTextService.cs      # [REFACTOR] Từ FloatingTextController
│   │   └── Tool/
│   │       ├── IToolUnlockService.cs       # [NEW]
│   │       └── ToolUnlockService.cs        # [REFACTOR] Từ UnlockableToolsController
│   │
│   ├── Player/
│   │   ├── Presenters/
│   │   │   ├── PlayerPresenter.cs              # [NEW] Root orchestrator
│   │   │   ├── PlayerMovementPresenter.cs      # [NEW] Input → NavMesh → position
│   │   │   ├── PlayerCombatPresenter.cs        # [NEW] Health, damage, death, regen
│   │   │   ├── PlayerHarvestPresenter.cs       # [NEW] Hit detection, snapping, IHitable
│   │   │   ├── PlayerSwimPresenter.cs          # [NEW] Water detection, swim state
│   │   │   ├── PlayerResourcePresenter.cs      # [NEW] Give/Take resources, flying res
│   │   │   └── PlayerInventoryPresenter.cs     # [NEW] Capacity, pick drops
│   │   └── Views/
│   │       ├── PlayerView.cs                   # [REFACTOR] Từ PlayerGraphics
│   │       ├── PlayerAnimationView.cs          # [REFACTOR] Từ PlayerAnimationHandler
│   │       └── PlayerHealthBarView.cs          # [REFACTOR] Từ HealthbarBehaviour
│   │
│   ├── AI/
│   │   ├── Presenters/
│   │   │   ├── HelperPresenter.cs              # [REFACTOR] Từ HelperBehavior
│   │   │   └── HelperStateMachine.cs           # [REFACTOR] Decouple
│   │   ├── Views/
│   │   │   └── HelperView.cs                   # [NEW]
│   │   ├── Tasks/
│   │   │   ├── IHelperTask.cs                  # [NEW] Clean interface
│   │   │   ├── BaseHelperTask.cs               # [REFACTOR]
│   │   │   ├── GatheringTask.cs                # [KEEP + refactor]
│   │   │   ├── ConstructionTask.cs             # [KEEP + refactor]
│   │   │   └── FishingTask.cs                  # [KEEP + refactor]
│   │   └── States/                             # Tách từ HelperStateMachine inner classes
│   │       ├── AbstractTaskState.cs            # [NEW] Template Method base
│   │       ├── WaitingForTaskState.cs          # [REFACTOR]
│   │       ├── GatheringState.cs               # [REFACTOR]
│   │       ├── FishingState.cs                 # [REFACTOR]
│   │       ├── StoringState.cs                 # [REFACTOR]
│   │       ├── BuildingState.cs                # [REFACTOR]
│   │       └── RestingState.cs                 # [REFACTOR]
│   │
│   ├── World/
│   │   ├── Presenters/
│   │   │   ├── WorldPresenter.cs               # [REFACTOR] Từ WorldBehavior + WorldController
│   │   │   └── SubworldPresenter.cs            # [REFACTOR] Từ SubworldHandler
│   │   ├── Views/
│   │   │   └── EnvironmentView.cs              # [REFACTOR] Từ EnvironmentController
│   │   └── Systems/
│   │       ├── DistanceToggleSystem.cs         # [REFACTOR] Từ DistanceToggle static
│   │       ├── WorldElementRegistry.cs         # [NEW] Thay IWorldElement pattern
│   │       └── SpatialGrid.cs                  # [NEW] Grid-based spatial partitioning
│   │
│   ├── Buildings/
│   │   ├── Presenters/
│   │   │   ├── BuildingPresenter.cs            # [REFACTOR] Từ AbstractComplexBehavior
│   │   │   ├── PurchasePresenter.cs            # [REFACTOR] Từ PurchasePoint logic
│   │   │   └── ConstructionPresenter.cs        # [REFACTOR] Từ ConstructionPointBehavior
│   │   ├── Views/
│   │   │   ├── BuildingView.cs                 # [REFACTOR] Visual unlock
│   │   │   ├── PurchaseView.cs                 # [REFACTOR] UI canvas
│   │   │   └── ConstructionView.cs             # [REFACTOR] Progress UI
│   │   └── Models/
│   │       └── BuildingStateModel.cs           # (shared with _Game/Models/)
│   │
│   ├── Resources/                              # Renamed from "Resources Forder"
│   │   ├── Presenters/
│   │   │   ├── ResourceSourcePresenter.cs      # [REFACTOR] Từ ResourceSourceBehavior
│   │   │   └── ResourceDropPresenter.cs        # [REFACTOR] Từ ResourceDropBehavior
│   │   └── Views/
│   │       ├── ResourceSourceView.cs           # [NEW] Visual stages, particles
│   │       └── ResourceDropView.cs             # [NEW] Drop animation visual
│   │
│   ├── Missions/
│   │   ├── Presenters/
│   │   │   └── MissionPresenter.cs             # [REFACTOR] Từ Mission.cs
│   │   ├── Views/
│   │   │   ├── MissionUIView.cs                # [REFACTOR] Từ MissionUIPanel
│   │   │   └── MissionRewardView.cs            # [REFACTOR] Từ UIMissionRewardPopUp
│   │   └── Types/                              # Mission-type specific implementations
│   │       ├── CollectMission.cs               # [REFACTOR]
│   │       ├── BuildMission.cs                 # [REFACTOR]
│   │       └── ...
│   │
│   ├── Enemies/
│   │   ├── Presenters/
│   │   │   ├── EnemyPresenter.cs               # [REFACTOR] Từ BaseEnemyBehavior
│   │   │   └── EnemyStateMachine.cs            # [REFACTOR]
│   │   └── Views/
│   │       └── EnemyView.cs                    # [NEW] Animation, particles, flash
│   │
│   ├── Upgrades/
│   │   ├── Presenters/
│   │   │   └── UpgradePresenter.cs             # [REFACTOR] Từ GlobalUpgradesController
│   │   └── Views/
│   │       └── UpgradeUIView.cs                # [REFACTOR]
│   │
│   ├── Energy/
│   │   ├── Presenters/
│   │   │   └── EnergyPresenter.cs              # [REFACTOR] Từ EnergyController UI binding
│   │   └── Views/
│   │       └── EnergyUIView.cs                 # [REFACTOR]
│   │
│   └── UI/                                     # Game-specific UI presenters
│       ├── GameUIPresenter.cs                  # [REFACTOR] Từ UIGame
│       ├── MainMenuPresenter.cs                # [REFACTOR]
│       └── StorePresenter.cs                   # [REFACTOR]
│
├── Data/                                       # ScriptableObject INSTANCES (assets)
│   ├── Configs/
│   │   ├── Player/
│   │   │   ├── DefaultMovement.asset
│   │   │   ├── DefaultCombat.asset
│   │   │   └── DefaultSwimming.asset
│   │   ├── Currencies/
│   │   │   ├── Wood.asset
│   │   │   ├── Stone.asset
│   │   │   ├── Coins.asset
│   │   │   └── ...
│   │   └── ...
│   └── Databases/
│       ├── CurrencyDatabase.asset
│       ├── WorldsDatabase.asset
│       └── ...
│
├── Doc/                                        # Tài liệu dự án
│   └── architecture_refactoring_plan.md        # File này
│
└── Scenes/                                     # Giữ nguyên
```

---

## 4. Phase 0 — Foundation Infrastructure

> **Mục tiêu**: Tạo các building block cơ bản mà mọi phase sau phụ thuộc vào  
> **Effort**: 2-3 ngày  
> **Files tạo mới**: ~10 files

### 4.1 ServiceLocator

**File**: `_Core/Infrastructure/ServiceLocator/ServiceLocator.cs`

```csharp
/// <summary>
/// Central service registry. Thay thế toàn bộ static singletons.
/// Thread-safe, lifecycle managed, scope-aware.
/// </summary>
public static class ServiceLocator
{
    // Register service bằng interface type
    public static void Register<TInterface>(TInterface service) where TInterface : class, IService;
    
    // Resolve service — throws nếu chưa register
    public static TInterface Get<TInterface>() where TInterface : class;
    
    // Try-resolve — null-safe
    public static bool TryGet<TInterface>(out TInterface service) where TInterface : class;
    
    // Dispose tất cả services (khi quit game)
    public static void DisposeAll();
    
    // Reset (cho unit tests)
    public static void Reset();
}
```

**Tại sao ServiceLocator thay vì DI Container?**
- Lightweight hơn Zenject/VContainer
- Team dễ hiểu hơn
- Đủ mạnh cho scale hiện tại của project
- Có thể migrate sang DI container sau nếu cần

### 4.2 EventBus

**File**: `_Core/Infrastructure/EventBus/EventBus.cs`

```csharp
/// <summary>
/// Typed pub/sub event system. Thay thế C# events rải rác + static delegates.
/// </summary>
public static class EventBus
{
    // Subscribe nhận event loại T
    public static EventBinding<T> Subscribe<T>(Action<T> handler) where T : IGameEvent;
    
    // Unsubscribe 
    public static void Unsubscribe<T>(EventBinding<T> binding) where T : IGameEvent;
    
    // Publish event tới tất cả subscribers
    public static void Publish<T>(T gameEvent) where T : IGameEvent;
    
    // Clear tất cả subscriptions (khi quit)
    public static void Clear();
}
```

**EventBusListener** (MonoBehaviour helper):
```csharp
/// <summary>
/// Đặt trên GameObject để auto-unsubscribe khi OnDestroy.
/// Giải quyết triệt để vấn đề event memory leak.
/// </summary>
public class EventBusListener : MonoBehaviour
{
    public EventBinding<T> Listen<T>(Action<T> handler) where T : IGameEvent;
    // Auto-unsubscribe trong OnDestroy
}
```

### 4.3 UpdateManager

**File**: `_Core/Infrastructure/UpdateManager/UpdateManager.cs`

```csharp
/// <summary>
/// 1 Update() loop duy nhất thay thế 50+ Update() riêng lẻ.
/// Giảm overhead Unity reflection-based Update dispatch.
/// </summary>
public class UpdateManager : MonoBehaviour, IService
{
    public static void Register(IUpdatable target, UpdatePhase phase = UpdatePhase.Normal);
    public static void Unregister(IUpdatable target);
    
    // Internals: iterate registered list, call OnUpdate(deltaTime)
}

public interface IUpdatable
{
    bool IsActive { get; }  // Skip update khi inactive
    void OnUpdate(float deltaTime);
}
```

### 4.4 GameBootstrapper

**File**: `_Game/Bootstrap/GameBootstrapper.cs`

```csharp
/// <summary>
/// Thay thế GameController.Awake(). 
/// Priority-based service registration + initialization.
/// </summary>
[DefaultExecutionOrder(-100)]
public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] GameConfig gameConfig;     // SO reference
    
    private void Awake()
    {
        // Phase 1: Register core infrastructure
        ServiceLocator.Register<ISaveService>(new SaveService());
        ServiceLocator.Register<IPoolManager>(new PoolManager());
        
        // Phase 2: Register game services
        ServiceLocator.Register<ICurrencyService>(new CurrencyService());
        ServiceLocator.Register<IUpgradeService>(new UpgradeService());
        ServiceLocator.Register<IAudioService>(new AudioService());
        ServiceLocator.Register<IEnergyService>(new EnergyService());
        ServiceLocator.Register<IMissionService>(new MissionService());
        ServiceLocator.Register<IWorldService>(new WorldService());
        // ...
        
        // Phase 3: Initialize all (respects priority order)
        ServiceLocator.InitializeAll();
        
        // Phase 4: Load first world
        ServiceLocator.Get<IWorldService>().LoadCurrentWorld();
    }
}
```

### 4.5 Folder Structure Setup

Tạo cấu trúc folder mới song song với code cũ, không xóa code cũ cho tới khi từng module migrate xong.

---

## 5. Phase 1 — Data Layer

> **Mục tiêu**: Tách toàn bộ game data thành SO configs + Pure C# Models  
> **Effort**: 3-4 ngày  
> **Files tạo mới**: ~20 files

### 5.1 ScriptableObject Configs

#### PlayerMovementConfig
```csharp
[CreateAssetMenu(menuName = "ProjectP/Player/Movement")]
public class PlayerMovementConfig : ScriptableObject
{
    [Header("Speed")]
    public float BaseMaxSpeed = 5f;
    public float BaseAcceleration = 10f;
    public float SwimmingSpeedMultiplier = 0.5f;
    
    [Header("Detection")]
    public float MovementInputThreshold = 0.025f;  // was magic number
    public float StationaryThresholdMultiplier = 0.5f;
    
    [Header("Rotation")]
    public float RotationLerpSpeed = 0.2f;
    
    [Header("Jump/Fall")]
    public float JumpHeightDifference = 0.5f;
    public int JumpCounterThreshold = 3;
    public float FallHitDelay = 0.1f;
}
```

#### CurrencyDefinition (thay CurrencyType enum)

```csharp
[CreateAssetMenu(menuName = "ProjectP/Currency/Definition")]
public class CurrencyDefinition : ScriptableObject
{
    [Header("Identity")]
    public string CurrencyId;            // "wood", "stone", "coins"...
    public string DisplayName;
    public Sprite Icon;
    
    [Header("Behavior")]
    public bool UseInventory = true;      // ảnh hưởng tới inventory capacity
    public bool IsFoodItem = false;       // cho Energy system
    
    [Header("Prefabs")]
    public GameObject DropPrefab;         // object rơi trên ground
    public GameObject FlyingPrefab;       // object bay tới target
    
    [Header("Audio")]
    public AudioClip PickUpSound;
    public AudioClip DropSound;
    
    [Header("Energy (nếu IsFoodItem)")]
    public int EnergyPointsRestoring;     // từ FoodItemData
}
```

#### CurrencyDatabase

```csharp
[CreateAssetMenu(menuName = "ProjectP/Currency/Database")]
public class CurrencyDatabase : ScriptableObject
{
    [SerializeField] List<CurrencyDefinition> currencies;
    
    // Runtime lookup table (built on Initialize)
    private Dictionary<string, CurrencyDefinition> lookupById;
    
    public CurrencyDefinition GetById(string id) => lookupById[id];
    public IReadOnlyList<CurrencyDefinition> All => currencies;
    public bool Exists(string id) => lookupById.ContainsKey(id);
}
```

### 5.2 Pure C# Models

#### HealthModel
```csharp
public class HealthModel
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsDepleted => CurrentHealth <= 0f;
    public bool IsFull => CurrentHealth >= MaxHealth;
    public float Percentage => MaxHealth > 0 ? CurrentHealth / MaxHealth : 0f;
    
    public event Action<float> OnChanged;       // (delta)
    public event Action OnDepleted;
    public event Action OnRestored;
    
    public void Initialize(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }
    
    public void Subtract(float amount)
    {
        float oldHealth = CurrentHealth;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnChanged?.Invoke(CurrentHealth - oldHealth);
        if (CurrentHealth <= 0) OnDepleted?.Invoke();
    }
    
    public void Add(float amount) { /* ... */ }
    public void AddPercent(float percent) { /* ... */ }
    public void Restore() { CurrentHealth = MaxHealth; OnRestored?.Invoke(); }
}
```

#### InventoryModel
```csharp
public class InventoryModel
{
    public int MaxCapacity { get; private set; }
    public int CurrentCapacity { get; private set; }
    public bool IsFull => CurrentCapacity >= MaxCapacity;
    public int FreeSpace => MaxCapacity - CurrentCapacity;
    
    public event Action OnCapacityChanged;
    public event Action OnFull;
    
    public void SetMaxCapacity(int capacity) { MaxCapacity = capacity; }
    
    public bool CanAdd(int amount) => CurrentCapacity + amount <= MaxCapacity;
    
    public int ClampToAvailable(int requestedAmount) 
        => Mathf.Min(requestedAmount, FreeSpace);
    
    public void Add(int amount)
    {
        CurrentCapacity += amount;
        OnCapacityChanged?.Invoke();
        if (IsFull) OnFull?.Invoke();
    }
    
    public void Remove(int amount)
    {
        CurrentCapacity = Mathf.Max(0, CurrentCapacity - amount);
        OnCapacityChanged?.Invoke();
    }
}
```

#### EnergyModel
```csharp
public class EnergyModel
{
    public float MaxEnergy { get; private set; }
    public float CurrentEnergy { get; private set; }
    public bool IsDepleted => CurrentEnergy <= 0f;
    public bool IsFull => CurrentEnergy >= MaxEnergy;
    
    public float LowEnergySpeedMultiplier { get; set; } = 1f;
    public float LowEnergyHarvestMultiplier { get; set; } = 1f;
    
    public event Action OnChanged;
    public event Action OnDepleted;
    
    public void Restore(float amount) { /* clamp to max, invoke events */ }
    public void Consume(float amount) { /* clamp to 0, invoke events */ }
}
```

Tương tự tạo: `CurrencyModel`, `UpgradeModel`, `WorldStateModel`, `MissionProgressModel`, `BuildingStateModel`.

---

## 6. Phase 2 — Core Services Migration

> **Mục tiêu**: Chuyển static controllers → instance services trong ServiceLocator  
> **Effort**: 4-5 ngày  
> **Files tạo mới**: ~22 files (11 interfaces + 11 implementations)

### 6.1 CurrencyService

```csharp
public interface ICurrencyService : IService
{
    int GetAmount(string currencyId);
    void Add(string currencyId, int amount);
    void Subtract(string currencyId, int amount);
    void Set(string currencyId, int amount);
    CurrencyDefinition GetDefinition(string currencyId);
    IReadOnlyList<CurrencyDefinition> GetAllDefinitions();
    bool HasEnough(string currencyId, int amount);
}
```

**Migration flow cũ → mới:**

```
// CŨ:
CurrencyController.Add(CurrencyType.Wood, 5);
CurrencyController.Get(CurrencyType.Wood);
Currency currency = CurrencyController.GetCurrency(CurrencyType.Wood);

// MỚI:
var currencyService = ServiceLocator.Get<ICurrencyService>();
currencyService.Add("wood", 5);
int amount = currencyService.GetAmount("wood");
CurrencyDefinition def = currencyService.GetDefinition("wood");

// Events (qua EventBus thay vì C# delegate):
EventBus.Subscribe<CurrencyChangedEvent>(OnCurrencyChanged);
```

### 6.2 AudioService

```csharp
public interface IAudioService : IService
{
    void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f);
    void PlaySFX(AudioClip clip, float volume = 1f);
    void PlayMusic(AudioClip clip);
    void StopMusic();
    // Sound throttling built-in
}
```

### 6.3 Bảng Migration Đầy Đủ

| Controller cũ (static) | Service mới (instance) | Interface | Priority |
|------------------------|----------------------|-----------|----------|
| `SaveController` | `SaveService` | `ISaveService` | 0 |
| `CurrencyController` | `CurrencyService` | `ICurrencyService` | 10 |
| `AudioController` | `AudioService` | `IAudioService` | 10 |
| `GlobalUpgradesController` | `UpgradeService` | `IUpgradeService` | 20 |
| `UnlockableToolsController` | `ToolUnlockService` | `IToolUnlockService` | 20 |
| `EnergyController` | `EnergyService` | `IEnergyService` | 30 |
| `MissionsController` | `MissionService` | `IMissionService` | 30 |
| `WorldController` | `WorldService` | `IWorldService` | 30 |
| `NavigationHelper` | `NavigationService` | `INavigationService` | 40 |
| `FloatingTextController` | `FloatingTextService` | `IFloatingTextService` | 40 |
| `NavMeshController` | `NavMeshService` | `INavMeshService` | 40 |
| `FoliageController` | `FoliageService` | `IFoliageService` | 40 |
| `EnvironmentController` | Refactor trực tiếp → `EnvironmentView` | N/A | Phase 4 |
| `DistanceToggle` | `DistanceToggleSystem` | N/A | Phase 4 |

---

## 7. Phase 3 — Player System Decomposition

> **Mục tiêu**: Tách God Object `PlayerBehavior` (1045 dòng, 36KB) thành 7 Presenter components  
> **Effort**: 5-7 ngày  
> **Quan trọng nhất** — ảnh hưởng tới mọi system khác

### 7.1 Mapping PlayerBehavior → Sub-Presenters

```
PlayerBehavior.cs (1045 lines)
│
├── L1-138    (Fields + Properties)  ──────→ PlayerPresenter.cs (shared refs)
├── L139-207  (Initialise + Unload)  ──────→ PlayerPresenter.cs (lifecycle)
├── L209-220  (Update)               ──────→ PlayerPresenter.cs (delegate)
│
├── L222-258  (Swimming region)      ──────→ PlayerSwimPresenter.cs
│   • IsSwimming, swimParticle
│   • OnSwimStarted, OnSwimEnded
│   • SwimmingUpdate()
│
├── L261-280  (Graphics region)      ──────→ PlayerView.cs
│   • PlayerGraphics reference
│   • OnSkinSelected callback
│
├── L282-290  (Upgrades region)      ──────→ PlayerPresenter.cs
│   • SpeedUpgrade, CapacityUpgrade
│   • OnUpgradeChanged → recalculate stats
│
├── L292-476  (Hitting region)       ──────→ PlayerHarvestPresenter.cs
│   • hitableObjectsInRange (→ HashSet)
│   • OnHittableInRange / OutOfRange
│   • PerformHit, snapping logic
│   • IHitter implementation
│
├── L479-657  (Movement region)      ──────→ PlayerMovementPresenter.cs
│   • NavMeshAgentBehaviour
│   • Speed calculation + upgrades
│   • Input handling
│   • Falling detection
│   • Ground snap
│
├── L687-771  (Combat region)        ──────→ PlayerCombatPresenter.cs
│   • HealthModel (new)
│   • TakeDamage(DamageSource)
│   • Health regen logic
│   • Death + respawn
│
├── L774-866  (Resource region)      ──────→ PlayerResourcePresenter.cs
│   • IResourceGiver, IResourceTaker
│   • RequiredResources (→ HashSet)
│   • GiveResource, TakeResource
│   • Flying resource handling
│
├── L868-991  (Drop region)          ──────→ PlayerInventoryPresenter.cs
│   • InventoryModel (new)
│   • OnResourcePickPerformed
│   • IsFull → reject
│   • Floating text
│
└── L1005-1037 (Cleanup)             ──────→ PlayerPresenter.cs (OnDestroy)
```

### 7.2 PlayerPresenter (Root Orchestrator)

```csharp
/// <summary>
/// Root player component. Orchestrates all sub-presenters.
/// Replaces the God Object PlayerBehavior.
/// </summary>
public class PlayerPresenter : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] PlayerMovementConfig movementConfig;
    [SerializeField] PlayerCombatConfig combatConfig;
    [SerializeField] PlayerSwimmingConfig swimmingConfig;
    [SerializeField] PlayerHarvestConfig harvestConfig;
    
    [Header("View")]
    [SerializeField] PlayerView view;
    
    // Sub-presenters (created as child components)
    public PlayerMovementPresenter Movement { get; private set; }
    public PlayerCombatPresenter Combat { get; private set; }
    public PlayerHarvestPresenter Harvest { get; private set; }
    public PlayerSwimPresenter Swim { get; private set; }
    public PlayerResourcePresenter Resource { get; private set; }
    public PlayerInventoryPresenter Inventory { get; private set; }
    
    // Shared Models
    public HealthModel HealthModel { get; private set; }
    public InventoryModel InventoryModel { get; private set; }
    
    // Static accessor (temporary — giữ cho backward compat trong quá trình migration)
    public static PlayerPresenter Instance { get; private set; }
    public static Vector3 Position => Instance.transform.position;
    
    public void Initialize()
    {
        Instance = this;
        
        HealthModel = new HealthModel();
        HealthModel.Initialize(combatConfig.MaxHealth);
        
        InventoryModel = new InventoryModel();
        
        // Init sub-presenters
        Movement = GetComponent<PlayerMovementPresenter>();
        Movement.Initialize(this, movementConfig);
        
        Combat = GetComponent<PlayerCombatPresenter>();
        Combat.Initialize(this, combatConfig, HealthModel);
        
        // ... etc
    }
}
```

### 7.3 Ví Dụ: PlayerMovementPresenter

```csharp
public class PlayerMovementPresenter : MonoBehaviour, IUpdatable
{
    private PlayerPresenter player;
    private PlayerMovementConfig config;
    private NavMeshAgentBehaviour navAgent;
    
    // State
    private float currentMaxSpeed;
    private bool isRunning;
    private bool isFalling;
    
    // Services (resolved from ServiceLocator)
    private IUpgradeService upgradeService;
    
    public bool IsActive => enabled;
    
    public void Initialize(PlayerPresenter player, PlayerMovementConfig config)
    {
        this.player = player;
        this.config = config;
        this.navAgent = GetComponent<NavMeshAgentBehaviour>();
        this.upgradeService = ServiceLocator.Get<IUpgradeService>();
        
        RecalculateSpeed();
        
        // Register cho centralized update
        UpdateManager.Register(this, UpdatePhase.Normal);
        
        // Listen events thay vì direct subscribe
        EventBus.Subscribe<UpgradeChangedEvent>(OnUpgradeChanged);
    }
    
    public void OnUpdate(float deltaTime)
    {
        HandleMovementInput(deltaTime);
        HandleFallingDetection(deltaTime);
    }
    
    private void RecalculateSpeed() { /* từ PlayerBehavior.CalculateMaxSpeed() */ }
    private void HandleMovementInput(float dt) { /* từ PlayerBehavior.MovementUpdate() */ }
    private void HandleFallingDetection(float dt) { /* từ PlayerBehavior.FallingUpdate() */ }
}
```

---

## 8. Phase 4 — World & Game Systems

> **Mục tiêu**: Refactor từng game system theo MVP pattern  
> **Effort**: 8-10 ngày  

### 8.1 World System

**Cũ**: `WorldController` (static) + `WorldBehavior` (MonoBehaviour) + `BaseWorldBehavior`  
**Mới**: `WorldService` (service) + `WorldPresenter` (scene) + `WorldStateModel` (model)

**Thay đổi quan trọng — Async Scene Loading:**
```csharp
// CŨ (sync — gây freeze):
SceneManager.LoadScene(worldData.Scene.Name, LoadSceneMode.Additive);

// MỚI (async — smooth):
public async void LoadWorld(WorldDefinition world)
{
    EventBus.Publish(new WorldLoadStartedEvent(world));
    
    var operation = SceneManager.LoadSceneAsync(world.SceneName, LoadSceneMode.Additive);
    operation.allowSceneActivation = false;
    
    while (operation.progress < 0.9f)
    {
        EventBus.Publish(new WorldLoadProgressEvent(operation.progress));
        await Task.Yield();
    }
    
    operation.allowSceneActivation = true;
    // WorldPresenter.Awake() sẽ tự gọi SetWorld khi scene activate
}
```

### 8.2 Building System

**Cũ**: `AbstractComplexBehavior<T,K>` (generic, 433 lines) → Purchase → Construction → Open  
**Mới**: 

| Component | Trách nhiệm |
|-----------|-------------|
| `BuildingPresenter` | Orchestrate flow: locked → purchased → constructed → open |
| `PurchasePresenter` | Handle resource collection from player |
| `ConstructionPresenter` | Handle hit-based building |
| `BuildingView` | Visual effects (spawn, unlock animation) |
| `BuildingStateModel` | Pure state (isPurchased, isConstructed, hitsLeft) |

### 8.3 AI/Bot System

**Tách state classes thành files riêng:**

Hiện tại `HelperStateMachine.cs` chứa ~10 inner classes (GatheringState, FishingState, StoringState...). Tất cả sẽ tách thành file riêng trong `_Game/AI/States/`.

**Extract duplicated code bằng Template Method:**

```csharp
// CŨ: GatheringState, FishingState có code gần giống nhau
// MỚI: Abstract base
public abstract class AbstractTaskState<TTask> : IStateBehavior where TTask : IHelperTask
{
    protected HelperPresenter helper;
    protected TTask currentTask;
    
    public void OnStart()
    {
        currentTask = FindTask();           // abstract
        SetWaypoint(currentTask.Position);  // shared
        helper.NavAgent.OnPathFinished += OnReachedTarget;
    }
    
    protected abstract TTask FindTask();
    protected abstract void OnReachedTarget();
    // ... shared cleanup logic
}
```

### 8.4 Mission System

**Fix critical bug**: Xóa `using UnityEditor` khỏi runtime code.

**Tách data khỏi logic:**
- `MissionDefinition` (ScriptableObject): title, reward config, navigation settings
- `MissionPresenter` (MonoBehaviour): runtime logic, stage transitions
- `MissionUIView`: progress display, reward popup

### 8.5 Enemy System

**Fix**: Guard `Update()` khi dead + register vào UpdateManager:

```csharp
// CŨ:
private void Update()
{
    // ❌ Chạy kể cả khi dead
    animator.SetFloat(MOVEMENT_HASH, agent.velocity.magnitude / agent.speed);
}

// MỚI:
public void OnUpdate(float deltaTime)
{
    if (IsDead || !IsActive) return;
    view.UpdateMovementAnimation(navAgent.NormalizedSpeed);
}
```

### 8.6 Energy System

**Cũ**: `EnergyController` — static, mixed UI + logic + events  
**Mới**:
- `EnergyService` (service): Pure logic — add/remove energy, food rules
- `EnergyModel` (model): Runtime state
- `EnergyPresenter` (presenter): Bind model → UI
- Events qua EventBus:
  - `EnergyChangedEvent`
  - `EnergyDepletedEvent`
  - `FoodConsumedEvent`

### 8.7 Environment System

**Fix**: Cache Light reference:
```csharp
// CŨ: FindFirstObjectByType<Light>() mỗi lần SetPreset
// MỚI: Cache trong Initialize, chỉ re-find khi world chuyển
```

### 8.8 Resource System

**Fix**: Xóa `Update()` trên ResourceSourceBehavior:
```csharp
// CŨ: Update() chạy mỗi frame chỉ để check respawn timer
// MỚI: Dùng delayed callback
public override void GetHit(...)
{
    // ...
    if (Health == 0)
    {
        IsActive = false;
        // Thay Update() poll bằng timed callback
        Tween.DelayedCall(respawnDuration, Respawn);
    }
}
// KHÔNG CẦN Update() nữa
```

---

## 9. Phase 5 — Performance & Polish

> **Mục tiêu**: Tối ưu hiệu năng toàn diện  
> **Effort**: 3-4 ngày

### 9.1 UpdateManager Integration

**Trước**: ~100+ MonoBehaviour.Update() riêng lẻ (Unity dispatch qua reflection)  
**Sau**: 1 UpdateManager.Update() iterate managed list

| System | Có Update() cũ | Sau refactor |
|--------|----------------|--------------|
| ResourceSourceBehavior (x100+) | ✅ mỗi frame | ❌ Removed — dùng timed callback |
| BaseEnemyBehavior (x10+) | ✅ mỗi frame | ✅ → UpdateManager (with dead guard) |
| HelperBehavior (x5+) | ✅ mỗi frame | ✅ → UpdateManager (with init guard) |
| AbstractStateMachine (x15+) | ✅ mỗi frame | ✅ → UpdateManager |
| DistanceToggle | ✅ coroutine | ✅ → UpdateManager + SpatialGrid |
| EnvironmentController | ✅ mỗi frame | ✅ → UpdateManager (only when transitioning) |
| ResourceTakingPoint | ✅ mỗi frame | ✅ → UpdateManager (only when givers > 0) |

**Ước tính giảm**: ~100+ Update() → ~30 managed updates (skip inactive)

### 9.2 Spatial Partitioning cho DistanceToggle

```csharp
/// <summary>
/// Grid-based spatial partitioning thay linear scan.
/// O(1) lookup thay O(n) mỗi frame.
/// </summary>
public class SpatialGrid<T> where T : ISpatialEntity
{
    private float cellSize;
    private Dictionary<Vector2Int, List<T>> cells;
    
    public void Insert(T entity) { /* add to correct cell */ }
    public void Remove(T entity) { /* remove from cell */ }
    public void Update(T entity) { /* move between cells if needed */ }
    
    public List<T> QueryRadius(Vector3 center, float radius)
    {
        // Chỉ check cells trong radius, không iterate ALL objects
    }
}
```

### 9.3 Collection Optimizations

```csharp
// CŨ: List<T> dùng cho lookup
private List<IHitable> hitableObjectsInRange = new List<IHitable>();
private List<CurrencyType> RequiredResources;

// MỚI: HashSet<T> cho O(1) Contains/Add/Remove
private HashSet<IHitable> hitableObjectsInRange = new HashSet<IHitable>();
private HashSet<string> requiredCurrencyIds = new HashSet<string>();
```

### 9.4 Object Pool Consolidation

| Object | Pooled hiện tại? | Sau refactor |
|--------|-------------------|--------------|
| Resource drops | ✅ Đã có | ✅ Giữ → PoolManager |
| Flying resources | ✅ Đã có | ✅ Giữ → PoolManager |
| Floating text | ❌ | ✅ Add pool |
| Tool objects | ❌ Instantiate/Destroy | ✅ Add pool |
| Hit particles | ❌ | ✅ Add pool |
| Enemy spawn VFX | ❌ | ✅ Add pool |

### 9.5 Fixes Nhỏ

- Xóa 30 dòng commented-out code trong `ResourceSourceBehavior.Update()`
- Fix typos: `geatheringTask` → `gatheringTask`, `IsFoorResource` → `IsFoodResource`...
- Extract magic numbers → config fields
- Fix double unsubscribe trong `EnergyController` (cả OnDisable lẫn Unload)
- Fix double init `RequiredResources` trong PlayerBehavior

---

## 10. Mapping File Cũ → Mới

### Controllers → Services

| File cũ | File mới | Thay đổi |
|---------|----------|----------|
| `Controllers/GameController.cs` | `Bootstrap/GameBootstrapper.cs` | Rewrite |
| `Level System/WorldController.cs` | `Services/World/WorldService.cs` | Static → instance |
| `Resources Forder/*.cs` | `Resources/*.cs` | Rename folder + MVP split |
| `Energy System/EnergyController.cs` | `Services/Energy/EnergyService.cs` + `Energy/Presenters/EnergyPresenter.cs` | Split logic/UI |
| `Missions/MissionsController.cs` | `Services/Mission/MissionService.cs` | Static → instance |
| `Upgrades/GlobalUpgradesController.cs` | `Services/Upgrade/UpgradeService.cs` | Static → instance |

### MonoBehaviour God Objects → MVP Split

| File cũ | Presenter(s) mới | View mới | Model mới |
|---------|-------------------|----------|-----------|
| `Player/PlayerBehavior.cs` (1045 lines) | `PlayerPresenter` + 6 sub-presenters | `PlayerView`, `PlayerAnimationView` | `HealthModel`, `InventoryModel` |
| `AI/Bot/HelperBehavior.cs` | `HelperPresenter` | `HelperView` | — |
| `Buildings/AbstractComplexBehavior.cs` | `BuildingPresenter` + `PurchasePresenter` + `ConstructionPresenter` | `BuildingView` | `BuildingStateModel` |
| `Enemies/BaseEnemyBehavior.cs` | `EnemyPresenter` | `EnemyView` | `HealthModel` (reuse) |
| `Missions/Mission.cs` | `MissionPresenter` | `MissionUIView` | `MissionProgressModel` |
| `Resources Forder/ResourceSourceBehavior.cs` | `ResourceSourcePresenter` | `ResourceSourceView` | — |
| `Environment/EnvironmentController.cs` | — | `EnvironmentView` | — |

### Events Migration

| Event cũ (C# delegate/static) | Event mới (EventBus) |
|-------------------------------|---------------------|
| `Currency.OnCurrencyChanged` | `CurrencyChangedEvent { CurrencyId, Delta, NewAmount }` |
| `EnergyController.OnEnergyChanged` | `EnergyChangedEvent { CurrentEnergy, MaxEnergy }` |
| `ResourceSourceBehavior.OnFirstTimeHit` | `ResourceHarvestedEvent { SourceId, CurrencyId }` |
| `PlayerBehavior.OnResourceWillBeReceived` | `ResourceIncomingEvent { CurrencyId, Amount }` |
| `MissionsController.OnNextMissionStarted` | `MissionStartedEvent { MissionId }` |
| `MissionsController.OnMissionFinished` | `MissionFinishedEvent { MissionId }` |
| `WorldController.OnWorldLoaded` | `WorldLoadedEvent { WorldId }` |
| `GlobalUpgradesEventsHandler.OnUpgraded` | `UpgradeChangedEvent { UpgradeType, NewLevel }` |
| `PurchasePoint.OnPurhcased` | `BuildingPurchasedEvent { BuildingId }` |
| `ConstructionPoint.OnConstructed` | `BuildingConstructedEvent { BuildingId }` |

---

## 11. Quyết Định Đã Xác Nhận

| # | Quyết định | Lý do |
|---|-----------|-------|
| 1 | **CurrencyType enum → CurrencyDefinition ScriptableObject** | Mở rộng tối đa — thêm currency mới chỉ cần tạo .asset, không sửa code |
| 2 | **Reset save data** khi chuyển kiến trúc mới | Không cần migration tool, clean start |
| 3 | **ServiceLocator** thay vì DI Container (Zenject/VContainer) | Lightweight, team dễ hiểu |
| 4 | **EventBus typed** thay vì ScriptableObject events | Truyền data kèm event, type-safe |
| 5 | **UpdateManager** centralized thay 50+ Update() | Giảm overhead, dễ profile |
| 6 | **Async scene loading** | Tránh freeze trên thiết bị yếu |
| 7 | **Tách PlayerBehavior** thành 7 sub-presenters | Maintainability, testability |

---

## 12. Verification Plan

### Automated Verification

| Phase | Kiểm tra | Command/Method |
|-------|----------|----------------|
| Mỗi phase | Unity compile thành công | Build → check 0 errors |
| Phase 1 | Model unit tests pass | NUnit: HealthModel, InventoryModel, EnergyModel |
| Phase 2 | Service unit tests pass | NUnit: CurrencyService, UpgradeService |
| Phase 3 | Player integration test | Play mode: spawn → move → hit → gather → die → respawn |
| Phase 4 | Full gameplay loop | Play mode: spawn → gather → build → upgrade → next world |
| Phase 5 | Performance benchmark | Unity Profiler: frame time, GC allocs, Update() count |

### Manual Verification Checklist

- [ ] Player di chuyển mượt, không giật
- [ ] Thu thập tài nguyên hoạt động (hit → drop → pick up)
- [ ] Mua và xây buildings đúng flow
- [ ] Helper bots AI pathfinding + task execution
- [ ] Missions activate/complete/reward đúng
- [ ] Energy system drain/restore đúng
- [ ] Day/night cycle + weather transitions
- [ ] World chuyển cảnh không freeze
- [ ] UI hiển thị đúng data
- [ ] Enemy spawn/combat/death/drop
- [ ] Save/load hoạt động (sau reset)

### Regression Strategy

```
main ─── Phase0 ─── Phase1 ─── Phase2 ─── Phase3 ─── Phase4 ─── Phase5
          │           │           │           │           │           │
          └─ verify   └─ verify   └─ verify   └─ verify   └─ verify   └─ final
```

- Mỗi phase tạo **git branch riêng**
- Merge vào main **chỉ khi verification pass**
- Tag version sau mỗi phase merge

---

## 13. Timeline & Milestones

| Phase | Tên | Effort | Dependencies | Milestone |
|-------|-----|--------|-------------|-----------|
| **0** | Foundation Infrastructure | 2-3 ngày | — | ServiceLocator + EventBus + UpdateManager biên dịch thành công |
| **1** | Data Layer | 3-4 ngày | Phase 0 | Tất cả SO configs + Models tạo xong, unit tests pass |
| **2** | Core Services | 4-5 ngày | Phase 0, 1 | Mọi static controller chuyển thành service, game compiles |
| **3** | Player Decomposition | 5-7 ngày | Phase 0, 1, 2 | Player đi, đánh, thu thập, bơi, chết/sống lại đúng |
| **4** | World & Game Systems | 8-10 ngày | Phase 0-3 | Full gameplay loop hoạt động |
| **5** | Performance & Polish | 3-4 ngày | Phase 0-4 | Profiler shows improvement, 0 memory leaks |
| | **TỔNG** | **~25-33 ngày** | | |

```
Tuần 1:       [Phase 0] [Phase 1]
Tuần 2-3:     [Phase 2] [Phase 3 bắt đầu]
Tuần 3-4:     [Phase 3 hoàn thành] [Phase 4 bắt đầu]
Tuần 4-5:     [Phase 4 hoàn thành]
Tuần 6:       [Phase 5] [Final verification]
```

---

*Tài liệu này là living document — sẽ được cập nhật khi có thay đổi trong quá trình thực thi.*
