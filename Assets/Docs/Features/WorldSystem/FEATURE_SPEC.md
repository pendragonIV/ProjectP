# Feature: World System (Quản lý Scene & Môi trường)

## 1. Mục tiêu
Quản lý việc chuyển đổi giữa các World (Scene), tối ưu nạp cảnh, và tối ưu cập nhật các object trong thế giới (Distance Toggle / Spatial Partitioning).

## 2. Kiến trúc
- **Model**: `WorldStateModel` (World hiện tại, danh sách element).
- **Config**: `WorldDefinition` (ScriptableObject: Tên scene, environment preset).
- **Service**: `WorldService` (Cung cấp Async Scene Loading).
- **Hệ thống con**: `DistanceToggleSystem` kết hợp `SpatialGrid` (Grid-based spatial partitioning O(1) thay vì tuyến tính O(N)).

## 3. Thành phần Code
- Xóa bỏ `SceneManager.LoadScene` đồng bộ (gây giật) thay bằng Async Load.
- Cải tiến `EnvironmentController` thành `EnvironmentView`.

## 4. Dependencies
- Phát `WorldLoadStartedEvent`, `WorldLoadedEvent` trên `EventBus`.
