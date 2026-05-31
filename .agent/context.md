# Project Context & Progress

Last Updated: 2026-05-20

## Current Status
- Completed Hybrid Architecture refactor (MVP + Service + State Machine)
- Project: SwordIdle (Idle game, Unity 6)
- Libraries: Odin Inspector, DOTween, Spine

## Architecture Summary
- **UI Layer (MVP)**: Passive View + Presenter for all HUDs and Popups
- **Gameplay Layer**: Service + State Machine (SpawnWaveService, StageState, DungeonState)
- **Core Layer**: EventBus + ServiceContainer + PoolManager + TickManager (ITickable + IFixedTickable)

## Key Systems Implemented
- ✓ Weapon Inventory (32 slots) — MVP: WeaponInventoryPresenter ↔ Panel ↔ System
- ✓ Weapon Drag & Drop (IWeaponHolder) — MVP: WeaponDragMergePresenter
- ✓ Universal Pooling (IPoolManager) — Integrated across all systems
- ✓ Stage Progression — State Machine (StageState enum) + SpawnWaveService
- ✓ Dungeon System — State Machine (DungeonState enum) + SpawnWaveService (shared)
- ✓ Economy — EconomyManager (balance) + LootDropService (gold drop logic)
- ✓ Skill Upgrade — SkillUpgradeManager + SkillUpgradePopup
- ✓ Player Level — PlayerLevelManager
- ✓ HUD System — Passive Views + Presenters (Gold, SP, Level, Stage)
- ✓ Save System — FileSaveService + SaveManager (dirty flag + debounce)
- ✓ IFixedTickable — Physics tick for Enemy movement
- ✓ Base Stats System — UnitStatsController + StatModifier (Data-driven formula)
- ✓ Enemy AI — Component-based FSM (EnemyAIBrain, EnemyMovement, EnemyAttackAction)

## Recent Refactors (2026-05-20)
1. Created `IFixedTickable` interface in TickManager
2. Migrated `EnemyAIController` from `FixedUpdate()` to `IFixedTickable`
3. Created `SpawnWaveService` — shared spawn logic for Stage + Dungeon
4. Created `StageState` + `DungeonState` enums — replaced boolean flags
5. Created `StageModel` — POCO data model for stage HP/count formulas
6. Refactored `LevelProgressionManager` → State Machine + SpawnWaveService
7. Refactored `DungeonManager` → State Machine + SpawnWaveService
8. Created Presenters: GoldHUD, SkillPointHUD, PlayerLevelHUD, StageHUD
9. Converted Views to Passive: GoldHUDView, SkillPointHUDView, PlayerLevelHUDView, StageHealthBarView
10. Created `LootDropService` — extracted gold drop logic from EconomyManager
11. Simplified `EconomyManager` — only manages balance + save

## Recent Refactors (Current Session)
1. Implemented `EnemyDataSO` for base stats, decoupling stats from Enemy prefab.
2. Implemented `EnemyStageStatModifierSO` to dynamically apply `StatModifier` based on current stage.
3. Replaced monolithic `EnemyAIController` with Component-based FSM (`EnemyAIBrain`, `EnemyMovement`, `EnemyMeleeAttack`).
4. Integrated Enemy AI with `UnitStatsController` for reading MoveSpeed, AttackRange, AttackSpeed dynamically.

## Knowledge Files
- Architecture: `.agent/architecture.md`
- Conventions: `.agent/conventions.md`
- Knowledge: `.agent/knowledge.md`
- Rules: `.agent/rules/` and `.Cursor/rules/`

## Quick Links
- Project Root: `c:\Dev\SwordIdle\`
- Scripts Root: `Assets\_Game\Scripts\`
- Core Infrastructure: `Assets\_Core\`
- Weapon UI: `Assets\_Game\Scripts\UI\Weapon\`
- Stage System: `Assets\_Game\Scripts\Game\Progression\`
- Dungeon System: `Assets\_Game\Scripts\Game\Dungeon\`

## Pending Work
- [ ] Migrate `Phu/Scripts` files (UnitFXView, FastMobAnimator, UnitSpineView) to `_Game/Scripts/Unit/View/`
- [ ] Tách UnitFXView thành các View nhỏ + UnitFXPresenter
- [ ] Unit tests cho StageModel, SpawnWaveService
