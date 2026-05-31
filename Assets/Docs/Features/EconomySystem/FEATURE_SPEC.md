# Feature: Economy System (Tiền tệ & Tài nguyên)

## 1. Mục tiêu
Quản lý các loại tiền tệ (Wood, Stone, Coins...), lượng tài nguyên, và logic trao đổi, kiểm tra điều kiện tài chính.

## 2. Kiến trúc
- **Models**: `CurrencyModel` (trạng thái số lượng runtime cho từng loại).
- **Configs**: `CurrencyDefinition` (ScriptableObject định nghĩa ID, Icon, DropPrefab).
- **Service**: `CurrencyService` (triển khai `ICurrencyService` quản lý tổng kho tiền).
- **Events**: `CurrencyChangedEvent` { CurrencyId, Delta, NewAmount }.

## 3. Thành phần Code
- `Assets/_Game/Configs/Currency/CurrencyDefinition.cs`
- `Assets/_Game/Models/CurrencyModel.cs`
- `Assets/_Game/Services/Currency/CurrencyService.cs`

## 4. Dependencies
- Phụ thuộc `ISaveService` để tải và lưu trữ số dư.
- Được sử dụng rộng rãi bởi: Cửa hàng, Nâng cấp, Xây dựng.
