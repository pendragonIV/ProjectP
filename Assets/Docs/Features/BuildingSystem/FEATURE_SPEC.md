# Feature: Building System (Công trình)

## 1. Mục tiêu
Xử lý vòng đời của một công trình, từ lúc khóa (Locked) -> Mua (Purchased bằng tài nguyên) -> Đang xây (Construction) -> Hoàn thành (Unlocked/Open).

## 2. Kiến trúc
- **Models**: `BuildingStateModel` (Lưu trạng thái isPurchased, isConstructed, hitsLeft).
- **Configs**: `BuildingDefinition` (ScriptableObject: chi phí mua, số hit cần để xây).
- **Views**: `BuildingView`, `PurchaseView` (Canvas giá tiền), `ConstructionView` (Thanh tiến trình đập búa).
- **Presenters**: `BuildingPresenter` (Điều phối vòng đời), `PurchasePresenter`, `ConstructionPresenter`.

## 3. Thành phần Code (Assets/_Game/Buildings)
- Được chia tách từ class khổng lồ `AbstractComplexBehavior` cũ.

## 4. Dependencies
- Sử dụng `ICurrencyService` để trừ tài nguyên khi mua.
- Người chơi tương tác thông qua `PlayerHarvestPresenter` (để đập búa xây dựng).
