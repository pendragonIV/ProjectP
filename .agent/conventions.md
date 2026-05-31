# Coding Conventions - Swordidle

## Naming Conventions
- **Classes**: PascalCase (e.g., `WeaponSlotView`, `GoldHUDPresenter`)
- **Methods**: PascalCase (e.g., `RefreshDisplay()`, `HandleWeaponClick()`)
- **Properties**: PascalCase (e.g., `Weapon`, `SlotIndex`, `IsEmpty`)
- **Private Fields**: camelCase with `m_` prefix (e.g., `m_weaponData`, `m_eventBus`)
- **Local Variables**: camelCase (e.g., `slotIndex`, `weaponInstance`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_WEAPON_LEVEL`, `STAGE_PROGRESS_DEBOUNCE`)
- **Enum**: PascalCase (e.g., `StageState`, `DungeonState`, `SkillType`)
- **Interface**: Prefix `I` (e.g., `ITickable`, `IFixedTickable`, `IPoolManager`)
- **Events (struct)**: PascalCase + `Event` suffix (e.g., `GoldChangedEvent`, `UnitDiedEvent`)

## Architecture Patterns — Khi nào dùng gì

### MVP (UI Layer)
```
Presenter → subscribe EventBus (OnEnable/OnDisable)
         → gọi View.PublicMethod(data)
View     → Passive: chỉ expose public method, KHÔNG subscribe event
```
**Dùng cho**: Popup, HUD, Panel, bất kỳ UI component nào.

### Service + State Machine (Gameplay Layer)
```
StateMachine → enum State + TransitionTo()
            → delegate logic xuống Service
Service     → POCO, dùng chung giữa nhiều system
```
**Dùng cho**: Stage, Dungeon, và bất kỳ gameplay loop phức tạp nào.

### ITickable / IFixedTickable (Update Pattern)
```
OnEnable:  m_tickManager.Add(this)     hoặc m_tickManager.AddFixed(this)
OnDisable: m_tickManager.Remove(this)  hoặc m_tickManager.RemoveFixed(this)
```
**ITickable**: Logic frame-by-frame (animation, UI update).
**IFixedTickable**: Logic physics (Rigidbody, AI movement).

## File Organization
```
Assets/_Game/Scripts/
├── UI/
│   ├── {Feature}/
│   │   ├── {Feature}View.cs         (Passive View)
│   │   └── {Feature}Presenter.cs    (Presenter)
├── Game/
│   ├── {Feature}/
│   │   ├── {Feature}Manager.cs      (State Machine)
│   │   ├── {Feature}State.cs        (Enum)
│   │   ├── {Feature}Model.cs        (POCO data)
│   │   └── {Feature}Service.cs      (Shared logic)
├── Unit/
│   └── {Type}/{Type}Unit.cs
└── Combat/
    └── ...
```

## Class Structure
```csharp
public sealed class ClassName : MonoBehaviour/Interface
{
    // 1. Inspector fields
    [Header("Category")]
    [SerializeField] private Type field;

    // 2. Private fields
    private Type m_privateField;

    // 3. Properties
    public Type PublicProperty => m_privateField;

    // 4. Unity lifecycle
    private void Awake() { }
    private void OnEnable() { }
    private void OnDisable() { }

    // 5. Public methods
    public void PublicMethod() { }

    // 6. Private methods
    private void PrivateMethod() { }
}
```

## Documentation Standards & Comments
- **XML Comments**: Bắt buộc viết XML comments cho tất cả các public methods, properties và classes.
- **Inline Comments**: Giải thích business logic phức tạp (WHY, không phải WHAT).
- **Ngôn ngữ**: Tên biến, class, phương thức viết bằng Tiếng Anh; comment giải thích và summary viết bằng Tiếng Việt.
- **Tooltip**: Mỗi `[SerializeField]` cần có `[Tooltip("Mô tả chức năng, đơn vị nếu có")]`.
- **Docs-First**: Bắt buộc đọc hoặc tạo mới tài liệu thiết kế (`FEATURE_SPEC.md` và `SETUP.md`) trước khi bắt đầu code tính năng mới (theo rule `10_feature-documentation-first.mdc`). Mọi thay đổi code đều phải được cập nhật đồng thời vào tài liệu trong cùng commit (theo `11_feature-documentation-sync.mdc`).

## MonoBehaviour Best Practices
- Sử dụng `[SerializeField]` thay vì `public` fields
- Gán null checks trong `Awake()` — throw exception nếu thiếu
- Unsubscribe events trong `OnDisable()` hoặc `OnDestroy()`
- Sử dụng `sealed` classes nếu không extend
- Không dùng `Update()`/`FixedUpdate()` trực tiếp — dùng `ITickable`/`IFixedTickable`

## Performance
- Sử dụng `sealed` classes nếu không cần inherit
- Cache Transform, Rigidbody, etc. trong fields
- Sử dụng object pooling (IPoolManager) cho frequent instantiation
- Avoid LINQ in Update/LateUpdate
- Prefer `for` loop over `foreach` cho hot paths (no enumerator alloc)

## Version Control
- Commit message: `feat/fix/refactor: mô tả ngắn bằng tiếng Việt`
- Branch naming: `feature/weapon-system`, `fix/ui-bug`
- Mỗi commit = 1 logical change
