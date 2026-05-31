# Feature: Infrastructure (Core)

## 1. Mục tiêu
Cung cấp các nền tảng kỹ thuật cơ sở (Core building blocks) độc lập với game logic, bao gồm ServiceLocator, EventBus, và UpdateManager.

## 2. Kiến trúc
- **ServiceLocator**: Registry tĩnh để đăng ký và lấy các service (thay thế singleton pattern).
- **EventBus**: Hệ thống Pub/Sub kiểu type-safe giúp các hệ thống giao tiếp lỏng lẻo.
- **UpdateManager**: Loop update tập trung để thay thế `MonoBehaviour.Update()` riêng lẻ nhằm tăng hiệu suất.

## 3. Thành phần Code (Assets/_Core/Infrastructure)
- `ServiceLocator.cs`, `IService.cs`
- `EventBus.cs`, `IGameEvent.cs`, `EventBinding.cs`, `EventBusListener.cs`
- `UpdateManager.cs`, `IUpdatable.cs`, `UpdatePhase.cs`
- `PoolManager.cs`, `IPoolManager.cs`
- `SaveService.cs`, `ISaveService.cs`

## 4. Dependencies
- Không phụ thuộc vào bất kỳ logic game nào.
- Thuộc `ProjectP.Core.asmdef`.
