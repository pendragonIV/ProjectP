# Swordidle Project Knowledge Base

## Mục Đích Project
- Game Idle được phát triển bằng **Unity 6**
- Hỗ trợ toàn bộ hệ thống vũ khí (Weapon System), dungeon, skill upgrade
- Sử dụng **Spine** cho animations nhân vật
- Giao diện UI phức tạp với drag & drop
- Thư viện: **Odin Inspector**, **DOTween**

## Namespace Structure
```
SwordIdle.Core           - Infrastructure: DI, Events, Pooling, Tick, Save, TimeManagement
SwordIdle.Core.DI        - ServiceContainer
SwordIdle.Core.Events    - EventBus, IEventBus
SwordIdle.Core.Tick      - TickManager, ITickable, IFixedTickable, ITickManager
SwordIdle.Core.Pooling   - PoolManager, IPoolManager, PoolItem
SwordIdle.Core.Save      - FileSaveService, ISaveService
SwordIdle.Core.Debug     - IDebugLogger

SwordIdle.Game           - Game logic, UI, Combat
SwordIdle.Game.Bootstrap - CompositionRoot, GameServicesHost, IGameServices
SwordIdle.Game.UI        - UI Presenters + Views
SwordIdle.Game.Combat    - Weapon systems, combat events
SwordIdle.Game.Unit      - Player, Enemy, AI
SwordIdle.Game.Progression - Stage progression (State Machine)
SwordIdle.Game.Dungeon   - Dungeon system (State Machine)
SwordIdle.Game.Economy   - EconomyManager, LootDropService
SwordIdle.Game.Spawn     - SpawnWaveService (shared)
SwordIdle.Game.Skill     - SkillUpgradeManager
SwordIdle.Game.PlayerLevel - PlayerLevelManager
SwordIdle.Game.Save      - SaveManager, SaveData
```

## Architecture — Hybrid (MVP + Service + State Machine)

### Layer 1: UI (MVP Pattern)
- **View** — Passive MonoBehaviour: chỉ expose public methods, KHÔNG subscribe EventBus
- **Presenter** — Subscribe EventBus, resolve DI, gọi View methods

### Layer 2: Gameplay (Service + State Machine)
- **State Machine** — Enum state + TransitionTo(), delegate logic cho Services
- **Service (POCO)** — Business logic, dùng chung giữa nhiều system
- **Model (POCO)** — Data + pure functions, dễ unit test

### Layer 3: Infrastructure (Event-Driven + DI)
- **EventBus** — Communication giữa modules
- **ServiceContainer** — DI registration
- **GameServicesHost** — Singleton facade

## Key Systems
- **Weapon System**: Inventory (32 slots), Drag & Drop, Merge, Orbit Visual
- **Stage System**: State Machine (Spawning→Fighting→Clearing→Completed→Paused)
- **Dungeon System**: State Machine (Inactive→Spawning→Fighting→Clearing→Completing)
- **Economy**: EconomyManager (balance) + LootDropService (gold drop)
- **Tick System**: ITickable (Update), IFixedTickable (FixedUpdate)
- **Pool System**: Universal IPoolManager with automatic pool creation

## Technologies
- Unity 6
- Spine Animation
- TextMesh Pro (UI Text)
- DOTween (Tween animations)
- Odin Inspector (Editor UI)
- Custom Events System (EventBus)

## Key Patterns
- **MVP**: Passive View cho UI
- **State Machine**: enum + TransitionTo() cho Gameplay
- **Service (POCO)**: SpawnWaveService, LootDropService, StageModel
- **Dependency Injection**: Constructor injection + GameServicesHost facade
- **Events**: Typed event structs via IEventBus
- **Object Pooling**: IPoolManager (universal)
- **ITickable/IFixedTickable**: Centralized Update/FixedUpdate

## Important Knowledge
- Views chỉ render dữ liệu, không chứa business logic
- Presenters subscribe/unsubscribe trong OnEnable/OnDisable
- IWeaponHolder interface cho drag & drop components
- SpawnWaveService dùng chung cho cả Stage và Dungeon
- EnemyAIController dùng IFixedTickable (không dùng FixedUpdate trực tiếp)
- FastMobAnimator và UnitSpineView dùng ITickable (tránh Update overhead)
- LootDropService tách khỏi EconomyManager để tuân thủ SRP

## Recent Project Rules & Conventions
- **Terminology**: Bắt buộc dùng `Skill Point` (không dùng Book), `Artifact` (không dùng Apple, Ruby hay Kì vật).
- **UI Formatting**: Mọi chỉ số (Damage, HP, Stats) trên UI bắt buộc hiển thị số nguyên (dùng `NumberFormatter.Format(value)`) để tránh số thập phân khó nhìn (VD: 100.5), ngoại trừ các chỉ số % (phần trăm).
- **Unity Scene/Prefab Editing**: KHÔNG BAO GIỜ sửa trực tiếp các file `.unity` hoặc `.prefab` bằng text/bash (VD: `cat`, `sed`, `replace`). Bắt buộc phải thông qua MCP Server (`ai-game-developer`) để chỉnh sửa GameObject/Component.
- **Service Resolution Safety**: Trong `OnDestroy()` hoặc các hàm Cleanup, bắt buộc gọi `Core.DI.ServiceContainer.IsRegistered<T>()` trước khi `Resolve<T>()` để tránh lỗi `InvalidOperationException` khi `GameCompositionRoot` đã bị hủy trước đó.
- **Documentation Sync**: BẮT BUỘC viết tài liệu (docs) cho tính năng mới nếu chưa có. Nếu tính năng đã có docs, phải cập nhật docs ngay khi sửa code. Đồng thời, phải cập nhật các tài liệu chính (main docs) như `SystemArchitecture.md`, `DataStructure.md` nếu có thay đổi về luồng hoặc dữ liệu cốt lõi.
- **Git Commit Format**: Mọi commit message bắt buộc dùng định dạng `Đình Anh - <Mô tả ngắn gọn>` (không dùng Conventional Commits như `feat:`, `fix:`).

## Context Graph & Architecture Visualization (The Project's Brain)
- The project maintains a visual Architecture Graph in `.agent/context_graph.js`.
- **Perpetual Brain Loop**: After any code changes, the AI MUST use the `context-graph-generate` MCP tool to automatically rescan the C# code using Reflection.
- The AI uses this data to update `ONBOARDING.md` or architectural docs.
- The user can view this graph dynamically in a 3D interface by opening `SwordIdle -> Open Context Graph` from the Unity Menu, which supports Complexity Heatmaps and Diff Impact Analysis.
