# Feature: Player System

## 1. Mục tiêu
Quản lý toàn bộ vòng đời và hành vi của người chơi (Player) bao gồm: di chuyển, thu thập, bơi lội, chiến đấu, và quản lý túi đồ.

## 2. Kiến trúc (MVP)
Tách God Object `PlayerBehavior` thành 7 Presenter components:
- **Models**: `HealthModel`, `InventoryModel`.
- **Views**: `PlayerView`, `PlayerAnimationView`, `PlayerHealthBarView`.
- **Presenters**: `PlayerPresenter` (Root), `PlayerMovementPresenter`, `PlayerCombatPresenter`, `PlayerHarvestPresenter`, `PlayerSwimPresenter`, `PlayerResourcePresenter`, `PlayerInventoryPresenter`.
- **Configs**: `PlayerMovementConfig`, `PlayerCombatConfig`, `PlayerSwimmingConfig`, `PlayerHarvestConfig` (ScriptableObject).

## 3. Thành phần Code (Assets/_Game/Player)
- `Presenters/`: Các class logic điều khiển hành vi của người chơi.
- `Views/`: Các MonoBehaviour gắn trên model 3D (xử lý animation, particle, health bar UI trên đầu).

## 4. Dependencies
- Lắng nghe sự kiện từ: `EventBus` (ví dụ `UpgradeChangedEvent`).
- Dịch vụ sử dụng: `IUpgradeService` (để tính toán chỉ số tốc độ/sức mạnh).
