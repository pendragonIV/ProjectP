# SwordIdle — Agent Coding Standards

> Áp dụng tuyệt đối cho mọi file C# trong dự án.

## No-Fallback / Fail-Fast
- Throw exception khi config null/thiếu. KHÔNG dùng `?? defaultValue` cho ScriptableObject config.
- Exception message format: `[ClassName] Mô tả ngắn gọn. Gợi ý sửa.`

## Interface Segregation
- `IGameServices.Tick` → `ITickManager` (không `TickManager`)
- `IGameServices.WeaponRegistry` → `IWeaponRegistry` (không `WeaponRegistrySO`)
- Service mới: viết interface trước, implement sau.

## ITickable
- `OnEnable` → `services.Tick.Add(this)` | `OnDisable` → `services.Tick.Remove(this)`
- KHÔNG dùng Unity `Update()` trong gameplay MonoBehaviour khi đã có ITickable.

## Save / Dirty Flag
- `UnitDiedEvent` và sự kiện tần suất cao → `MarkDirty()` (không `Save()` ngay)
- `TickManager` hoặc `MonoBehaviour.Update` gọi `FlushIfDirty()` định kỳ.

## Logging
- `m_logger.Log(TAG, msg)` — KHÔNG `Debug.Log(msg)` trực tiếp.

## XML Docs
```csharp
/// <summary>
/// Mô tả ngắn gọn bằng tiếng Việt.
/// </summary>
```
- Bắt buộc cho mọi public class và public method.
