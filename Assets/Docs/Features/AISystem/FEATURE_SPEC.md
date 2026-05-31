# Feature: AI Helper System (Bot tự động)

## 1. Mục tiêu
Quản lý các Helper Bot (đệ tử) phụ giúp người chơi thu thập tài nguyên, câu cá, xây nhà.

## 2. Kiến trúc
Sử dụng State Machine pattern được refactor thành từng file State riêng biệt thay vì gom chung trong 1 file.
- **Presenter**: `HelperPresenter` (Điều khiển NavMesh và Animation).
- **View**: `HelperView`.
- **States**: Kế thừa `AbstractTaskState` -> `GatheringState`, `FishingState`, `BuildingState`, `StoringState`.

## 3. Thành phần Code (Assets/_Game/AI)
- `HelperPresenter.cs`
- `States/AbstractTaskState.cs`

## 4. Dependencies
- Được đăng ký `UpdateManager` để không gọi Unity Update() khi đang rảnh.
