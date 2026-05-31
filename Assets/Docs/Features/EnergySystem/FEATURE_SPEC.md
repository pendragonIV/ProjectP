# Feature: Energy System

## 1. Mục tiêu
Quản lý thể lực (Energy) của người chơi: tiêu hao khi làm việc và phục hồi khi ăn thức ăn. Hệ thống ảnh hưởng đến tốc độ làm việc/di chuyển khi cạn kiệt.

## 2. Kiến trúc
- **Model**: `EnergyModel` (Lưu lượng max/current, phạt tốc độ khi low energy).
- **Service**: `EnergyService` (Logic add/remove energy, tiêu thụ thức ăn).
- **Presenter/View**: `EnergyPresenter` và `EnergyUIView` để hiển thị thanh thể lực trên UI.
- **Events**: `EnergyChangedEvent`, `EnergyDepletedEvent`, `FoodConsumedEvent`.

## 3. Thành phần Code
- `Assets/_Game/Services/Energy/EnergyService.cs`
- `Assets/_Game/Models/EnergyModel.cs`
- `Assets/_Game/Energy/Presenters/EnergyPresenter.cs`
- `Assets/_Game/Energy/Views/EnergyUIView.cs`

## 4. Dependencies
- Giao tiếp qua `EventBus`.
- Tác động lên `PlayerMovementPresenter` (giảm tốc độ khi cạn energy).
