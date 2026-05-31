# Feature: Mission System (Nhiệm vụ)

## 1. Mục tiêu
Quản lý luồng nhiệm vụ tuần tự hoặc song song, trao phần thưởng và hướng dẫn người chơi.

## 2. Kiến trúc
- **Models**: `MissionProgressModel` (Nhiệm vụ đang active, tiến độ).
- **Configs**: `MissionDefinition` (ScriptableObject: Tên, phần thưởng, điều kiện hoàn thành, tọa độ Navigation).
- **Service**: `MissionService` (Quản lý danh sách nhiệm vụ tổng thể).
- **Presenter/View**: `MissionPresenter` (logic từng nhiệm vụ cụ thể), `MissionUIView` (hiển thị UI).

## 3. Thành phần Code
- Tách biệt logic và data từ `MissionsController` tĩnh cũ.
- Tách `Mission.cs` thành config tĩnh và presenter động.

## 4. Dependencies
- Kích hoạt phần thưởng thông qua `ICurrencyService` hoặc `IUpgradeService`.
- Phát `MissionStartedEvent` và `MissionCompletedEvent`.
