# ProjectP Project Structure

## 1) Mục tiêu
Tài liệu này là nguồn tham chiếu **bắt buộc** cho mọi thay đổi trong dự án.
Khi thêm/sửa bất kỳ phần nào, cần đối chiếu:
- Cấu trúc thư mục và trách nhiệm module trong tài liệu này.
- Tài liệu tính năng tương ứng trong `Assets/Docs/Features/` (nếu có).

Nếu chưa có tài liệu tính năng, phải tạo mới theo mục 6 trước khi hoặc cùng lúc implement.

## 2) Cấu trúc tài liệu chuẩn
```text
Assets/
└─ Docs/
   ├─ PROJECT_STRUCTURE.md                # Tài liệu cấu trúc tổng (file này)
   ├─ SETUP.md                            # Hướng dẫn thiết lập scene / prefab / inspector
   └─ Features/
      ├─ CoreGameplay/
      │  ├─ FEATURE_SPEC.md               # Unit, Weapon Orbit, Summon/Merge, UI Inventory
      │  └─ SETUP.md                      # Thiết lập scene core & unit combat
      ├─ HealthSystem/
      │  └─ FEATURE_SPEC.md               # HP Bar event-driven (IHealthBarView, ghost bar, Stage HP)
      ├─ Economy/
      │  ├─ FEATURE_SPEC.md               # Gold + Skill Point economy loop
      │  └─ SETUP.md                      # Thiết lập Economy UI HUD
      ├─ Progression/
      │  └─ FEATURE_SPEC.md               # Player Level, XP, Skill Point
      ├─ Save/
      │  ├─ FEATURE_SPEC.md               # Lưu trữ dữ liệu Event-Driven và Json DTO Schema
      │  └─ SETUP.md                      # Thiết lập WeaponRegistry & SaveManager
      ├─ Time/
      │  ├─ FEATURE_SPEC.md               # Hệ thống Quản lý Thời gian (chống cheat)
      │  └─ SETUP.md                      # Quy tắc sử dụng ITimeManager trong code
      ├─ SlotUpgrade/
      │  └─ FEATURE_SPEC.md               # Hệ thống nâng cấp ô vũ khí (12-Star System)
      ├─ Architecture/
      │  └─ FEATURE_SPEC.md               # asmdef, GameServicesHost, migration, tests
      ├─ Dungeon/Popup/SkillUpgrade/WeaponDragMerge/InfiniteLevel/
      │  └─ FEATURE_SPEC.md / SETUP.md
      └─ <FeatureName>/
         ├─ FEATURE_SPEC.md               # Mô tả yêu cầu + luồng
         ├─ SETUP.md                      # Quy trình cấu hình Unity Inspector
         └─ CHANGELOG.md (tuỳ chọn)       # Lịch sử chỉnh sửa ngắn gọn
```


## 3) Cấu trúc thư mục code (định hướng)

### Biên giới phụ thuộc (BẮT BUỘC)
- `_Core` **không phụ thuộc** `_Game` hay bất kỳ domain script nào.
- `_Game` được phép dùng `_Core`.
- Script trong `Editor/` không được nằm lẫn vào runtime path.

```text
Assets/
├─ _Core/                                  # Assembly: ProjectP.Core
│  ├─ ProjectP.Core.asmdef
│  ├─ DI/                                  # ServiceContainer, CompositionRoot (base)
│  ├─ Events/                              # IEventBus, EventBus
│  ├─ Tick/                                # ITickable, TickManager
│  ├─ Save/                                # ISaveService, FileSaveService
│  ├─ Time/                                # ITimeManager, TimeManager
│  ├─ Pooling/                             # ObjectPool<T>
│  ├─ Debug/                               # IDebugLogger, DebugLogger
│  ├─ Audio/                               # IAudioService
│  └─ Utils/                               # MathUtils
├─ _Game/                                  # Assembly: ProjectP.Game
│  ├─ ProjectP.Game.asmdef
│  ├─ Tests/                               # Assembly: ProjectP.Game.Tests (Edit Mode)
│  ├─ Scripts/
│  │  ├─ Bootstrap/                        # GameCompositionRoot, GameServicesHost
│  │  ├─ Unit/
│  │  ├─ Attributes/
│  │  ├─ Combat/
│  │  ├─ UI/
│  │  └─ Game/                             # GameEvents tập trung
│  ├─ Prefabs/
│  ├─ ScriptableObjects/                   # Toàn bộ Asset (.asset) và Script (.cs) của SO
│  │  ├─ BaseStats/
│  │  ├─ Modifiers/
│  │  ├─ Weapons/
│  │  ├─ Dungeons/
│  │  ├─ Popups/
│  │  └─ System/
│  └─ Scenes/
├─ Art/
├─ Audio/
├─ Resources/
├─ Scenes/
└─ Plugins/
```

### 3.1) Cấu trúc đề xuất cho CoreGameplay (phase khởi đầu)
```text
Assets/
├─ _Core/
│  ├─ DI/
│  │  ├─ ServiceContainer.cs
│  │  └─ CompositionRoot.cs
│  ├─ Events/
│  │  └─ EventBus.cs
│  ├─ Tick/
│  │  ├─ ITickable.cs
│  │  └─ TickManager.cs
│  ├─ Save/
│  │  ├─ ISaveService.cs       # Đọc ghi file cơ bản bằng string Key
│  │  └─ PlayerPrefsSaveService.cs
│  ├─ Pooling/
│  │  └─ ObjectPool.cs
│  ├─ Debug/
│  │  ├─ IDebugLogger.cs
│  │  └─ DebugLogger.cs
│  ├─ Audio/
│  │  └─ IAudioService.cs
│  └─ Utils/
│     └─ MathUtils.cs
└─ _Game/
   └─ Scripts/
      ├─ Bootstrap/
      │  ├─ GameCompositionRoot.cs          # Đăng ký ServiceContainer + GameServicesHost
      │  ├─ GameServicesHost.cs           # Facade gameplay (View/Presenter dùng)
      │  ├─ IGameServices.cs
      │  └─ SkillBootstrapper.cs
      ├─ Unit/
      │  ├─ Base/
      │  │  └─ Unit.cs
      │  ├─ Player/
      │  │  └─ PlayerUnit.cs
      │  ├─ Enemy/
      │  │  └─ EnemyUnit.cs
      │  └─ Structure/
      │     └─ StructureUnit.cs
      ├─ Attributes/
      │  ├─ Runtime/
      │  │  ├─ Stat.cs                      # Chỉ số cơ bản + OnValueChanged event
      │  │  ├─ StatModifier.cs              # Nguồn sửa đổi chỉ số
      │  │  ├─ StatType.cs                  # Enum 16 loại chỉ số
      │  │  ├─ StatBreakdown.cs             # Phân tích chỉ số theo Category
      │  │  ├─ ModifierSourceCategory.cs    # Enum nguồn gốc modifier
      │  │  ├─ UnitStatsController.cs       # Quản lý Stats + Health + Tags
      │  │  ├─ DamageCalculator.cs          # Tính sát thương
      │  │  ├─ VitalStat.cs                 # Chỉ số sinh tồn (HP/MP)
      │  │  ├─ GameplayTag.cs               # Nhãn phân cấp
      │  │  ├─ GameplayTagContainer.cs      # Container tập hợp tags
      │  │  ├─ GameplayEffectDurationType.cs # Enum Instant/Duration/Infinite
      │  │  ├─ ActiveGameplayEffect.cs      # Instance Effect đang hoạt động
      │  │  └─ GameplayEffectController.cs  # MonoBehaviour quản lý Effect
      ├─ ScriptableObjects/             # (Chỉ chứa các file Script .cs định nghĩa SO)
      │  ├─ BaseStats/
      │  │  ├─ UnitCombatStatsSO.cs
      │  │  ├─ UnitAttackStatsSO.cs
      │  │  ├─ UnitBaseStatsData.cs
      │  │  ├─ BaseStatDefinition.cs
      │  │  ├─ StatModifierDef.cs
      │  │  ├─ AttackPatternSO.cs
      │  │  └─ EnemyDataSO.cs
      │  ├─ Modifiers/
      │  │  ├─ GameplayEffectData.cs
      │  │  ├─ EnemyStageStatModifierSO.cs
      │  │  ├─ ArtifactConfigSO.cs
      │  │  └─ SkillConfigSO.cs
      │  ├─ Weapons/
      │  │  ├─ WeaponDataSO.cs
      │  │  └─ WeaponRegistrySO.cs
      │  ├─ Dungeons/
      │  │  ├─ DungeonConfigSO.cs
      │  │  └─ DungeonRegistrySO.cs
      │  ├─ Popups/
      │  │  └─ PopupRegistrySO.cs
      │  └─ System/
      │     └─ PreloadManifestSO.cs
      ├─ Combat/
      │  ├─ Orbit/
      │  │  ├─ WeaponOrbitSystem.cs
      │  │  ├─ WeaponOrbitSlot.cs
      │  │  └─ WeaponHitbox.cs
      │  ├─ Inventory/
      │  │  ├─ WeaponInventorySystem.cs
      │  │  └─ WeaponInstance.cs
      │  └─ Events/
      │     ├─ CombatEvents.cs
      │     └─ WeaponEvents.cs
      ├─ Save/
      │  ├─ SaveData.cs
      │  ├─ SaveSchemaVersions.cs
      │  ├─ ISaveDataMigrator.cs / SaveDataMigrator.cs
      │  ├─ SaveEvents.cs
      │  └─ ISaveManager.cs / SaveManager.cs
      ├─ Game/Dungeon/               # DungeonManager, DungeonRewardHandler
      ├─ Game/
      │  ├─ Economy/
      │  │  ├─ EconomyManager.cs          # SYSTEM: Ví tiền (Gold) + Skill Point
      │  │  ├─ IEconomyManager.cs         # Interface DI
      │  │  └─ EconomyEvents.cs           # GoldChangedEvent, SkillPointChangedEvent
      │  ├─ PlayerLevel/
      │  │  ├─ PlayerLevelManager.cs      # SYSTEM: XP, level-up, publish events
      │  │  ├─ IPlayerLevelManager.cs     # Interface DI
      │  │  └─ PlayerLevelEvents.cs       # PlayerXpChangedEvent, PlayerLevelUpEvent
      │  ├─ Progression/
      │  │  ├─ LevelProgressionManager.cs # SYSTEM: Stage loop (màn chơi vô hạn)
      │  │  └─ ProgressionEvents.cs       # StageProgressChangedEvent
      │  └─ GameEvents.cs                 # Placeholder / comment định hướng
      └─ UI/
         ├─ Weapon/
         │  ├─ WeaponInventoryPanel.cs      # VIEW: render 32 slot
         │  ├─ WeaponSlotView.cs            # VIEW: 1 ô slot
         │  ├─ WeaponSummonButton.cs        # VIEW: nút summon
         │  ├─ WeaponInventoryPresenter.cs  # PRESENTER: sync Inventory ↔ Panel
         │  └─ Drag/
         │     ├─ IWeaponHolder.cs
         │     ├─ WeaponSlotHolder.cs
         │     ├─ WeaponDragItem.cs
         │     └─ WeaponDragMergePresenter.cs
         ├─ Unit/
         │  └─ Health/
         │     ├─ IHealthBarView.cs           # Interface contract (Initialize, OnHpChanged, OnDied, OnRevived)
         │     ├─ HealthBarViewBase.cs        # Base MonoBehaviour subscribe EventBus → IHealthBarView
         │     ├─ UnitHealthBarBinder.cs      # Binder cho enemy/player HP bar cụ thể
         │     ├─ SliderHealthBarView.cs      # UI Slider + ghost delay bar (DOTween)
         │     └─ SpriteRendererHealthBarView.cs # SpriteRenderer bar (World Space)
         ├─ Stage/
         │  └─ StageHealthBarView.cs         # VIEW: Stage HP tổng − lắng nghe StageProgressChangedEvent
         ├─ Economy/
         │  ├─ GoldHUDView.cs               # VIEW: Hiển thị Gold
         │  └─ SkillPointHUDView.cs         # VIEW: Hiển thị Skill Point
         └─ PlayerLevel/
            └─ PlayerLevelHUDView.cs        # VIEW: XP bar + level label
```

Tài liệu chi tiết feature tương ứng: `Assets/Docs/Features/CoreGameplay/FEATURE_SPEC.md`.

> Lưu ý: Đây là định hướng chuẩn cho dự án mới. Nếu hiện trạng khác, khi refactor cần dịch chuyển dần theo hướng này và cập nhật lại tài liệu.

## 4) Quy tắc phụ thuộc
- `_Core` (**ProjectP.Core**) **không phụ thuộc** `_Game`.
- `_Game` (**ProjectP.Game**) chỉ reference `_Core` (enforce bởi asmdef).
- Script `Editor/` dùng assembly `ProjectP.Game.Editor`.
- Namespace: `ProjectP.Game.<Domain>` / `ProjectP.Core.<Module>`.
- **ServiceContainer**: chỉ `CompositionRoot` + `GameCompositionRoot` được `Register`/`Resolve`.
- **Gameplay**: Mọi MonoBehaviour lấy dependency qua `GameServicesHost.Instance.GetService<T>()` (đây là Proxy của ServiceContainer).
- **UI Architecture**: Bắt buộc dùng **MVP (Model-View-Presenter)**. View không được chứa logic, Presenter làm trung gian gọi GameServicesHost và xử lý EventBus bằng `EventBindingGroup`.
- **Không alloc trong hot path** (Update/Tick); cache reference; ưu tiên `ITickable`.
- Logic thuần mới → unit test trong `Assets/_Game/Tests/`.

## 5) Quy trình khi sửa mã nguồn
1. Xác định thay đổi thuộc feature nào.
2. Đọc `Assets/Docs/PROJECT_STRUCTURE.md`.
3. Tìm doc feature tại `Assets/Docs/Features/<FeatureName>/FEATURE_SPEC.md`.
4. Nếu chưa có: tạo folder feature + tạo `FEATURE_SPEC.md` theo mẫu ở mục 6.
5. Implement theo doc.
6. Cập nhật doc feature nếu phạm vi/luồng/class chính thay đổi.

## 6) Mẫu tối thiểu cho FEATURE_SPEC.md
```markdown
# <FeatureName>

## 1. Mục tiêu

## 2. Phạm vi

## 3. Luồng chính

## 4. File/Class chính

## 5. Tích hợp Scene/Inspector

## 6. Ràng buộc kỹ thuật
```

## 7) Quy ước đặt tên
- Feature folder: PascalCase, ví dụ `EnemySpawn`.
- Tài liệu feature chuẩn: `FEATURE_SPEC.md`.
- Nếu cần ghi thay đổi theo mốc: thêm `CHANGELOG.md` trong cùng feature folder.

## 8) Trách nhiệm đồng bộ
- Mọi thay đổi ảnh hưởng kiến trúc tổng: cập nhật lại file này.
- Mọi thay đổi ảnh hưởng feature cụ thể: cập nhật doc feature tương ứng.
- Không merge thay đổi lớn nếu tài liệu không còn phản ánh đúng code.

## 9) Danh mục feature doc (hiện có)

| Feature | Đường dẫn FEATURE_SPEC |
|---------|------------------------|
| CoreGameplay | `Features/CoreGameplay/FEATURE_SPEC.md` |
| HealthSystem | `Features/HealthSystem/FEATURE_SPEC.md` |
| Economy | `Features/Economy/FEATURE_SPEC.md` |
| Progression | `Features/Progression/FEATURE_SPEC.md` |
| Save | `Features/Save/FEATURE_SPEC.md` |
| Architecture | `Features/Architecture/FEATURE_SPEC.md` |
| Dungeon | `Features/Dungeon/FEATURE_SPEC.md` |
| Popup | `Features/Popup/FEATURE_SPEC.md` |
| SkillUpgrade | `Features/SkillUpgrade/FEATURE_SPEC.md` |
| WeaponDragMerge | `Features/WeaponDragMerge/FEATURE_SPEC.md` |
| InfiniteLevel | `Features/InfiniteLevel/FEATURE_SPEC.md` |
| Time | `Features/Time/FEATURE_SPEC.md` |
| Attributes | `Features/Attributes/FEATURE_SPEC.md` |
| GameplayEffect | `Features/Attributes/GameplayEffect/FEATURE_SPEC.md` |
| EnemyAI | `Features/EnemyAI/FEATURE_SPEC.md` |
| Rules | `Features/Rules/FEATURE_SPEC.md` |
| CardGacha | `Features/CardGacha/FEATURE_SPEC.md` |

## 10) Testing

- Framework: Unity Test Framework (`com.unity.test-framework`).
- Assembly: `ProjectP.Game.Tests` (Edit Mode only).
- Chạy: **Window > General > Test Runner > EditMode**.
- Phạm vi hiện tại: `DamageCalculator`, `EconomyManager`, `PlayerLevelManager`, `SaveDataMigrator` round-trip.
