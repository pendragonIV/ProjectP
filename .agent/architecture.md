# Swordidle Architecture — Hybrid (MVP + Service + State Machine)

## High-Level Architecture

```
┌─────────────────────────────────────────────────────┐
│                 UI LAYER (MVP)                       │
│  View ← Presenter → Model/Service                   │
│  Passive View: chỉ render, không biết EventBus      │
│  Presenter: subscribe event, gọi View.Update(data)  │
└────────────────────┬────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────┐
│          GAMEPLAY LAYER (Service + State Machine)    │
│  SpawnWaveService, LootDropService, CombatService    │
│  StageStateMachine (Spawning→Fighting→Clearing→Done) │
│  DungeonStateMachine (Spawning→Fighting→Clearing)    │
│  StageModel, DungeonModel (POCO data)                │
└────────────────────┬────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────┐
│          CORE LAYER (Event-Driven + DI)              │
│  EventBus, ServiceContainer, PoolManager, TickManager│
│  SaveManager, TimeManager, DebugLogger               │
│  ITickable (Update), IFixedTickable (FixedUpdate)    │
└─────────────────────────────────────────────────────┘
```

## Khi nào dùng Pattern nào?

| Tình huống | Pattern | Lý do |
|---|---|---|
| UI mới (popup, HUD, panel) | **MVP** | View gắn MonoBehaviour, cần tách logic rõ ràng |
| Gameplay phức tạp (Stage, Dungeon) | **Service + State Machine** | State rõ ràng, dùng chung Service |
| Data/Model | **POCO** | Dễ test, không phụ thuộc Unity |
| Liên module communication | **EventBus** | Giải ghép, publish/subscribe |
| Physics update (AI movement) | **IFixedTickable** | Centralized FixedUpdate loop |
| Frame update (animation, UI) | **ITickable** | Centralized Update loop |

## Weapon System Architecture (MVP mẫu)

```
WeaponInventoryPresenter (Presenter)
    │
    ├─► WeaponInventoryPanel (View)
    │        │
    │        └─► WeaponSlotView x32 (View)
    │                 │
    │                 └─► WeaponDragItem (Drag Handler)
    │
    └─► WeaponInventorySystem (Model/Service)
         │
         ├─► WeaponData (ScriptableObject)
         └─► Events System
```

## Stage System Architecture (State Machine mẫu)

```
LevelProgressionManager (State Machine)
    │
    ├─► StageState enum (Idle, Spawning, Fighting, Clearing, Completed, Paused)
    ├─► StageModel (POCO: HP formula, stage data)
    └─► SpawnWaveService (POCO: spawn/despawn/target, dùng chung với Dungeon)
```

## Component Responsibilities

### Views (UI Layer — Passive)
- **Là**: MonoBehaviour components
- **Làm**: Render data, expose public methods (UpdateLabel, Refresh, SetData)
- **KHÔNG được**: Subscribe EventBus, resolve DI, chứa business logic
- **Ví dụ**: `GoldHUDView`, `WeaponSlotView`, `PlayerLevelHUDView`

### Presenters (Presentation Layer)
- **Là**: MonoBehaviour hoặc plain C# class
- **Làm**: Subscribe EventBus, resolve DI trong Awake, gọi View.Method()
- **KHÔNG được**: Chứa render logic, truy cập UI component trực tiếp
- **Ví dụ**: `GoldHUDPresenter`, `WeaponInventoryPresenter`, `StageHUDPresenter`

### Services (Gameplay/Domain Layer)
- **Là**: Plain C# class (POCO), registered trong DI container
- **Làm**: Handle business logic, state transitions, data persistence
- **KHÔNG được**: Reference Views/Presenters trực tiếp
- **Ví dụ**: `SpawnWaveService`, `LootDropService`, `EconomyManager`

### State Machines (Gameplay Layer)
- **Là**: MonoBehaviour chứa enum state + TransitionTo() method
- **Làm**: Điều phối Services dựa trên state transitions
- **KHÔNG được**: Chứa logic spawn/HP/loot trực tiếp (delegate cho Service)
- **Ví dụ**: `LevelProgressionManager` (StageState), `DungeonManager` (DungeonState)

### Models (Data Layer)
- **Là**: Plain C# classes, ScriptableObjects, Data Transfer Objects
- **Làm**: Hold data, define data structures, chứa pure functions
- **KHÔNG được**: Reference UnityEngine types (ngoài Vector/Color nếu cần)
- **Ví dụ**: `StageModel`, `WeaponInstance`, `WeaponData`, `SaveData`

## Dependency Injection Pattern

```csharp
// CompositionRoot đăng ký service
ServiceContainer.Register<IEconomyManager>(economyManager);

// MonoBehaviour lấy qua Facade (Awake)
var services = GameServicesHost.Instance;
m_eventBus = services.EventBus;
m_economyManager = services.Economy;
```

## Communication Flow

### 1. User Input → View → Presenter → Service
```csharp
// View: Nhận click (Passive — chỉ forward)
slotButton.onClick.AddListener(() => m_onClickCallback?.Invoke(SlotIndex));

// Presenter: Process click
HandleSlotClicked(slotIndex);

// Presenter → Service
weaponService.SelectWeapon(slotIndex);
```

### 2. Service → EventBus → Presenter → View
```csharp
// Service: Publish event
m_eventBus.Publish(new GoldChangedEvent(old, newAmount));

// Presenter: Subscribe (OnEnable/OnDisable)
m_eventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);

// Presenter → View
goldView.UpdateLabel(evt.CurrentAmount);
```

## File Organization Strategy

```
Assets/_Game/Scripts/
│
├── Bootstrap/
│   ├── GameCompositionRoot.cs    — DI registration
│   ├── GameServicesHost.cs       — Service facade
│   └── IGameServices.cs          — Interface
│
├── UI/
│   ├── Economy/
│   │   ├── GoldHUDView.cs        — Passive View
│   │   ├── GoldHUDPresenter.cs   — Subscribe + gọi View
│   │   ├── SkillPointHUDView.cs
│   │   └── SkillPointHUDPresenter.cs
│   │
│   ├── PlayerLevel/
│   │   ├── PlayerLevelHUDView.cs
│   │   └── PlayerLevelHUDPresenter.cs
│   │
│   ├── Stage/
│   │   ├── StageHealthBarView.cs
│   │   └── StageHUDPresenter.cs
│   │
│   ├── Weapon/
│   │   ├── WeaponInventoryPanel.cs  — View
│   │   ├── WeaponInventoryPresenter.cs
│   │   ├── WeaponSummonButton.cs
│   │   └── Drag/
│   │
│   └── Popup/
│       ├── PopupManager.cs
│       └── PopupView.cs
│
├── Game/
│   ├── Progression/
│   │   ├── LevelProgressionManager.cs  — State Machine
│   │   ├── StageState.cs                — Enum
│   │   └── StageModel.cs               — POCO data
│   │
│   ├── Dungeon/
│   │   ├── DungeonManager.cs           — State Machine
│   │   └── DungeonState.cs             — Enum
│   │
│   ├── Spawn/
│   │   └── SpawnWaveService.cs          — Shared Service
│   │
│   └── Economy/
│       ├── EconomyManager.cs           — Balance service
│       └── LootDropService.cs          — Gold drop logic
│
├── Unit/
│   ├── Base/Unit.cs
│   ├── Enemy/
│   │   ├── EnemyUnit.cs
│   │   ├── EnemyAIBrain.cs         — IFixedTickable (FSM)
│   │   ├── EnemyMovement.cs        — Physics movement & avoidance
│   │   └── EnemyAttackAction.cs    — Abstract attack strategy
│   └── Player/PlayerUnit.cs
│
├── Combat/
│   ├── Inventory/WeaponInventorySystem.cs
│   └── Orbit/WeaponOrbitSystem.cs
│
└── Services/
    └── ISaveManager, IDebugLogger...

Assets/_Core/
├── DI/ServiceContainer.cs
├── Events/EventBus.cs
├── Tick/TickManager.cs         — ITickable + IFixedTickable
├── Pooling/PoolManager.cs
├── Save/
└── TimeManagement/
```

## Event System Pattern

```csharp
// Define event (readonly struct cho performance)
public readonly struct GoldChangedEvent
{
    public readonly int OldAmount;
    public readonly int CurrentAmount;
}

// Presenter subscribe trong OnEnable/OnDisable
private void OnEnable() => m_eventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
private void OnDisable() => m_eventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
```

## Testing Strategy
- Unit tests cho Models/Services (StageModel, SpawnWaveService, EconomyManager)
- Integration tests cho State Machines (mock Services)
- Presenter tests: mock View interface, verify gọi đúng method
- PlayMode tests cho full integration
