# 🌊 Things — Game Design Document (GDD)

> **Codename**: Things  
> **Genre**: Peaceful Social MMO-lite / Life Simulation  
> **Inspiration**: Sky: Children of the Light × Animal Crossing × Stardew Valley  
> **Platform**: Mobile first (iOS + Android)  
> **Monetization**: F2P + Season Pass  
> **Players**: 20+ concurrent (MMO-lite)  

---

## 1. Tầm Nhìn (Vision Statement)

**"Một thế giới mở yên bình nơi mỗi người có một hòn đảo riêng, cùng bạn bè khám phá, xây dựng, và tạo nên những kỷ niệm đẹp."**

Things là game tập trung vào **trải nghiệm cảm xúc** chứ không phải chiến đấu. Người chơi sẽ:
- Xây dựng và trang trí hòn đảo riêng của mình
- Gặp gỡ người lạ, xây dựng tình bạn từ từ
- Chọn nghề nghiệp và hợp tác với nhau
- Khám phá các realm đa dạng với khí hậu khác nhau
- Tham gia sự kiện mùa và mini-game friendly

---

## 2. Core Gameplay Loop

```
┌─────────────────────────────────────────────────────────┐
│                    DAILY LOOP (30-60 phút)               │
│                                                          │
│   🌅 Đăng nhập → Check đảo → Tưới cây, cho pet ăn      │
│         │                                                │
│         ▼                                                │
│   🌲 Ra thế giới → Hub → Chọn realm → Thu thập/Khám phá │
│         │                                                │
│         ▼                                                │
│   👥 Gặp bạn bè → Co-op quest/mini-game/visit đảo       │
│         │                                                │
│         ▼                                                │
│   🏠 Về đảo → Craft → Xây dựng → Trang trí              │
│         │                                                │
│         ▼                                                │
│   📅 Hoàn thành daily → Nhận Season Pass reward          │
└─────────────────────────────────────────────────────────┘
```

### 2.1 Vòng lặp ngắn (5-10 phút)
- Thu thập tài nguyên (chặt cây, đào đá, hái hoa)
- Câu cá tại các điểm câu
- Craft 1 món đồ
- Trang trí 1 góc đảo

### 2.2 Vòng lặp trung (30-60 phút)
- Khám phá 1 realm mới
- Hoàn thành daily quest
- Visit đảo bạn bè + tặng quà
- Tham gia 1 mini-game/arena

### 2.3 Vòng lặp dài (tuần/tháng)
- Nâng cấp skill-tree nghề nghiệp
- Mở rộng đảo (thêm khu vực)
- Hoàn thành Season Pass
- Guild events + đảo guild

---

## 3. World Structure

```
                    ┌──────────────┐
                    │  WORLD MAP   │
                    └──────┬───────┘
                           │
            ┌──────────────┼──────────────┐
            │              │              │
    ┌───────▼───────┐ ┌────▼────┐ ┌───────▼───────┐
    │  PRIVATE      │ │  HUB    │ │  REALMS       │
    │  ISLANDS      │ │ (Shared)│ │  (Shared)     │
    │               │ │         │ │               │
    │ • My Island   │ │ • Plaza │ │ • Tropical 🌴 │
    │ • Friend's    │ │ • Market│ │ • Snow ❄️     │
    │ • Guild Island│ │ • Arena │ │ • Rain 🌧️    │
    │               │ │ • Port  │ │ • Desert 🏜️  │
    └───────────────┘ │ • Guild │ │ • Mystic 🌙   │
                      │   Hall  │ │ • Deep Sea 🌊 │
                      └─────────┘ └───────────────┘
```

### 3.1 Private Islands (Instanced — P2P/Host)
- **My Island**: Đảo riêng của mỗi người chơi. Tự do xây dựng, trang trí, trồng trọt.
- **Permissions**: Owner quyết định ai được visit, ai được xây/phá.
- **Khi offline**: Đảo tồn tại trên cloud save, bạn bè có thể xem (read-only) hoặc tưới cây giúp.
- **Mở rộng**: Bắt đầu với đảo nhỏ, mở rộng bằng tài nguyên.

### 3.2 Hub — Quảng Trường Trung Tâm (Dedicated Server)
- **Plaza**: Nơi gặp gỡ chính, NPC cửa hàng, bảng tin nhiệm vụ.
- **Market**: Giao dịch, trade, đấu giá vật phẩm giữa người chơi.
- **Arena**: Khu vực mini-game PvP friendly (đua thuyền, trượt, thi nấu ăn...).
- **Port**: Cổng đi đến các Realm khám phá.
- **Guild Hall**: Khu vực riêng cho Guild — meeting, quest board.
- **Max players**: 20-50 người cùng hub.

### 3.3 Realms — Vùng Khám Phá (Dedicated Server)
Mỗi realm là một vùng đất với khí hậu, tài nguyên, sinh vật riêng:

| Realm | Khí hậu | Tài nguyên đặc trưng | Hoạt động |
|-------|---------|---------------------|-----------|
| 🌴 Tropical Cove | Nắng, nóng | Dừa, san hô, cá nhiệt đới | Câu cá, lặn biển |
| ❄️ Frost Peaks | Tuyết, lạnh | Quặng, pha lê, gỗ thông | Đào mỏ, trượt tuyết |
| 🌧️ Everrain Woods | Mưa liên tục | Nấm, thảo dược, gỗ rêu | Hái thuốc, nấu ăn |
| 🏜️ Amber Dunes | Nóng, cát | Đá quý, xương rồng, dầu | Khảo cổ, khai thác |
| 🌙 Moonlit Hollow | Đêm vĩnh cửu | Hoa phát sáng, bọ lửa | Co-op puzzle, khám phá |
| 🌊 Abyssal Tide | Dưới nước | Ngọc trai, san hô hiếm | Lặn sâu, khám phá |

- Mỗi realm có **daily rotation** về tài nguyên hiếm.
- Một số vùng cần **co-op** để mở (cùng nhau giải puzzle/thắp sáng).
- **Max players per realm**: 8-16 người.

---

## 4. Hệ Thống Core

### 4.1 Xây Dựng & Trang Trí (Building & Decoration)

```
Nguyên liệu → Craft Blueprint → Đặt vào đảo → Trang trí
```

- **Snap Grid**: Hệ thống grid cho nhà/nền, free-place cho đồ trang trí.
- **Building Types**: Nhà ở, xưởng craft, kho chứa, vườn, bến thuyền, chuồng pet.
- **Decoration**: Đèn, hoa, tượng, bàn ghế, rèm cửa, thảm — hàng trăm items.
- **Style Sets**: Bộ sưu tập theo chủ đề (Tropical, Nordic, Oriental, Fantasy...).
- **Multiplayer**: Bạn bè có thể giúp xây (nếu owner cho phép).

### 4.2 Thu Hoạch Tài Nguyên (Resource Gathering)

- **Relaxing pace**: Không giới hạn thời gian, không áp lực. Animation đẹp, sound ASMR.
- **Tool-based**: Rìu → chặt cây, cuốc → đào đá, cần câu → câu cá, lưới → bắt bọ.
- **Tool upgrade**: Craft tool tốt hơn → hiệu quả hơn (thuộc skill-tree).
- **Realm-specific**: Mỗi realm có tài nguyên riêng → khuyến khích khám phá.

### 4.3 Craft / Chế Tạo

| Category | Ví dụ | Nguồn công thức |
|----------|-------|-----------------|
| Nội thất | Bàn, ghế, đèn, giường | Mua từ NPC / Tìm trong realm |
| Công cụ | Rìu, cuốc, cần câu, lưới | Skill-tree nghề nghiệp |
| Thức ăn | Bánh, súp, sushi, trà | Công thức nấu ăn (collectible) |
| Trang phục | Áo, mũ, giày, cánh | Season Pass / Event / Craft |
| Trang trí | Hoa, tượng, đài phun nước | Craft + mua + event |

### 4.4 Nông Trại / Vườn

- **Plot system**: Đặt ô vườn trên đảo, trồng hạt giống.
- **Growth cycle**: Mỗi loại cây có thời gian lớn khác nhau (real-time hours).
- **Seasons**: Cây trồng theo mùa (realm-inspired seasons trên đảo riêng).
- **Harvest**: Thu hoạch → bán / nấu ăn / craft / tặng bạn.
- **Auto-water**: Bạn bè visit có thể tưới cây giúp → tăng friendship.

### 4.5 Câu Cá

- **Mini-game**: Casting + timing mechanic (nhẹ nhàng, relaxing).
- **Fish catalog**: 50+ loài cá, phân bố theo realm + thời tiết + mùa.
- **Rare fish**: Cá hiếm xuất hiện ngẫu nhiên → collectible bragging rights.
- **Fish tank**: Trưng bày cá trên đảo trong bể cá trang trí.

### 4.6 Nấu Ăn

- **Recipe discovery**: Kết hợp nguyên liệu → khám phá công thức mới.
- **Buff system**: Món ăn cho buff nhẹ (chạy nhanh hơn, câu tốt hơn — 15 phút).
- **Share food**: Nấu cho bạn bè ăn → tăng friendship points.
- **Chef skill-tree**: Đầu bếp nấu được món buff mạnh hơn.

### 4.7 Nuôi Thú Cưng

- **Catch**: Bắt pet trong các realm (mỗi realm có pet riêng).
- **Care**: Cho ăn, tắm, chơi → tăng happiness.
- **Follow**: Pet đi theo trên đảo và khi khám phá.
- **Abilities**: Pet hỗ trợ nhẹ (giúp tìm tài nguyên, chỉ đường...).

---

## 5. Hệ Thống Social

### 5.1 Friendship Progression (Lấy cảm hứng từ Sky)

```
Stranger (Bóng mờ)
    │ ← Vẫy tay / Wave emote
    ▼
Acquaintance (Thấy rõ, tên hiển thị)
    │ ← Tặng quà nhỏ / Chơi cùng 30 phút
    ▼
Friend (Chat text, visit đảo)
    │ ← Tặng quà / Co-op quest / Visit đảo 5 lần
    ▼
Close Friend (Voice chat, teleport đến nhau)
    │ ← Chơi cùng tích lũy 10+ giờ
    ▼
Best Friend (Emote đặc biệt, buff khi chơi cùng)
```

### 5.2 Tương Tác Xã Hội

| Feature | Mô tả | Unlock tại |
|---------|-------|------------|
| Wave/Emote cơ bản | Vẫy tay, cúi đầu | Stranger |
| Xem profile | Tên, đảo, nghề nghiệp | Acquaintance |
| Chat text | Nhắn tin | Friend |
| Visit đảo | Đến thăm đảo bạn | Friend |
| Tặng quà | Gửi item | Friend |
| Trade | Giao dịch trực tiếp | Friend |
| Voice chat | Nói chuyện | Close Friend |
| Teleport | Bay đến vị trí bạn | Close Friend |
| Co-op emote | Ôm, high-five, nhảy cùng | Best Friend |
| Friendship buff | +10% XP khi chơi cùng | Best Friend |

### 5.3 Party System
- Tạo nhóm 2-4 người đi khám phá realm.
- Shared quest progress trong party.
- Party leader chọn realm, cả nhóm teleport cùng.

### 5.4 Guild / Clan

| Feature | Mô tả |
|---------|-------|
| Guild Island | Đảo chung cho cả guild, cùng xây dựng |
| Guild Rank | Leader, Officer, Member |
| Guild Quest | Nhiệm vụ tuần/tháng cho cả guild |
| Guild Market | Kho chung, chia sẻ tài nguyên |
| Max size | 20-30 thành viên |
| Guild Events | Sự kiện guild-only (xây công trình lớn, thi đấu liên guild) |

---

## 6. Progression — Skill Tree Nghề Nghiệp

Mỗi người chơi chọn **1 nghề chính** và có thể học **1-2 nghề phụ** (level thấp hơn):

```
                    ┌─────────────┐
                    │  EXPLORER   │ ← Mọi người bắt đầu từ đây
                    └──────┬──────┘
                           │ Level 5
           ┌───────────────┼───────────────┐
           │               │               │
    ┌──────▼──────┐ ┌──────▼──────┐ ┌──────▼──────┐
    │  GATHERER   │ │  BUILDER    │ │  ARTISAN    │
    │  Thu thập   │ │  Xây dựng  │ │  Nghệ nhân  │
    └──────┬──────┘ └──────┬──────┘ └──────┬──────┘
           │               │               │
    ┌──────┼──────┐  ┌─────┼─────┐  ┌──────┼──────┐
    │      │      │  │     │     │  │      │      │
    ▼      ▼      ▼  ▼     ▼     ▼  ▼      ▼      ▼
  Miner Fisher Farmer Architect Carpenter Chef Tailor Alchemist
```

### Ý nghĩa của nghề nghiệp
- **Miner** (Thợ mỏ): Đào quặng nhanh hơn, tìm được quặng hiếm.
- **Fisher** (Ngư dân): Câu cá lớn hơn, tỷ lệ cá hiếm cao.
- **Farmer** (Nông dân): Cây lớn nhanh hơn, năng suất cao hơn.
- **Architect** (Kiến trúc sư): Mở khóa blueprint đặc biệt, xây nhanh hơn.
- **Carpenter** (Thợ mộc): Craft nội thất chất lượng cao, ít nguyên liệu hơn.
- **Chef** (Đầu bếp): Nấu được món buff mạnh, nhiều công thức hơn.
- **Tailor** (Thợ may): Craft trang phục, nhuộm màu, thiết kế.
- **Alchemist** (Luyện đan): Tạo thuốc, phân bón đặc biệt, hiệu ứng.

> **Thiết kế cốt lõi**: Không ai tự cung tự cấp 100%. Thợ mỏ cần Đầu bếp nấu ăn để buff, Đầu bếp cần Nông dân cung cấp nguyên liệu → **Khuyến khích hợp tác & trade tự nhiên**.

---

## 7. PvE & PvP

### 7.1 PvE Nhẹ
- **Shadow Storms**: Bão bóng tối đổ bộ vào realm định kỳ — co-op thắp sáng để đẩy lùi.
- **Dungeon Puzzle**: Hang động ẩn trong realm — cần 2-4 người giải puzzle cùng nhau.
- **Boss Event**: Boss tuần/tháng xuất hiện — cả server hợp tác đánh (world boss).
- **Reward**: Cosmetic hiếm, blueprint đặc biệt, title.

### 7.2 PvP Friendly (Arena)

| Mini-game | Mô tả | Players |
|-----------|-------|---------|
| Boat Race | Đua thuyền qua đường đua trên biển | 2-8 |
| Cook-Off | Thi nấu ăn (tốc độ + chất lượng) | 2-4 |
| Build Battle | Thi xây theo chủ đề (voting) | 2-6 |
| Fish Derby | Thi câu cá lớn nhất trong thời gian | 2-8 |
| Snowball Fight | Ném tuyết (trong Snow realm) | 4-8 teams |
| Obstacle Course | Parkour/obstacle (kiểu Fall Guys nhẹ) | 4-16 |

- **Không mất gì khi thua** — chỉ nhận thưởng khi thắng.
- **Leaderboard**: Bảng xếp hạng tuần/mùa, reset mỗi season.

---

## 8. Season & Live Events

### 8.1 Season Pass (3 tháng/season)
- **Free Track**: Tài nguyên, blueprint cơ bản, currency.
- **Premium Track** (~$10): Cosmetic độc quyền, pet đặc biệt, emote, title.
- **50 tiers**: Unlock dần qua daily/weekly quest.

### 8.2 Seasonal Events

| Season | Theme | Nội dung đặc biệt |
|--------|-------|-------------------|
| 🌸 Spring | Blossom Festival | Hoa anh đào, trồng hoa đặc biệt, đèn lồng |
| ☀️ Summer | Ocean Carnival | Lễ hội biển, đua thuyền, pháo hoa |
| 🍂 Autumn | Harvest Moon | Lễ hội thu hoạch, nấu ăn cộng đồng |
| ❄️ Winter | Starlight Festival | Tuyết rơi, quà tặng, trang trí Giáng sinh |

---

## 9. Kiến Trúc Kỹ Thuật

### 9.1 Network Architecture (Hybrid)

```
┌─────────────────────────────────────────────────────────┐
│                    CLOUD SERVICES                        │
│                                                          │
│  ┌─────────────┐  ┌──────────┐  ┌─────────────────┐    │
│  │ Auth Server │  │ Database │  │ Matchmaking     │    │
│  │ (Firebase/  │  │ (Player  │  │ (Realm assign)  │    │
│  │  PlayFab)   │  │  Save,   │  │                 │    │
│  └─────────────┘  │  Guild,  │  └─────────────────┘    │
│                    │  Market) │                          │
│                    └──────────┘                          │
│                                                          │
│  ┌─────────────────────────────────────────────────┐    │
│  │           DEDICATED SERVERS (Scalable)           │    │
│  │                                                   │    │
│  │  ┌─────────┐  ┌─────────┐  ┌─────────┐          │    │
│  │  │ Hub #1  │  │ Hub #2  │  │ Hub #3  │  ...     │    │
│  │  │ (50 ppl)│  │ (50 ppl)│  │ (50 ppl)│          │    │
│  │  └─────────┘  └─────────┘  └─────────┘          │    │
│  │                                                   │    │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐         │    │
│  │  │Tropical  │ │Snow Realm│ │Rain Realm│  ...    │    │
│  │  │ (16 ppl) │ │ (16 ppl) │ │ (16 ppl) │         │    │
│  │  └──────────┘ └──────────┘ └──────────┘         │    │
│  └─────────────────────────────────────────────────┘    │
│                                                          │
│  ┌─────────────────────────────────────────────────┐    │
│  │           P2P / HOST (Islands)                   │    │
│  │  Owner = Host, Visitors = Clients                │    │
│  │  Max 4-8 visitors per island                     │    │
│  │  Unity Relay for NAT traversal                   │    │
│  └─────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────┘
```

### 9.2 Data Sync Strategy

| Data | Authority | Sync Method | Frequency |
|------|-----------|-------------|-----------|
| Player Position | Server | NetworkTransform | 15-30 Hz |
| Player Animation | Client (owner) | NetworkAnimator | On change |
| Inventory | Server | RPC on change | On change |
| Island Data | Cloud DB | Load on visit | On change |
| Currency | Server | RPC (validated) | On change |
| Chat | Server | Reliable RPC | Instant |
| Weather/Time | Server | Broadcast | Every 60s |
| Resource State | Server (realm) | NetworkVariable | On change |
| Building Placement | Server (validate) | RPC + confirm | On action |

### 9.3 Tech Stack

| Layer | Technology |
|-------|-----------|
| Game Engine | Unity 6 (URP) |
| Networking | Netcode for GameObjects (NGO) |
| Relay/NAT | Unity Relay |
| Matchmaking | Unity Lobby / Custom |
| Auth | Firebase Auth / Unity Authentication |
| Database | Firebase Firestore (player save, island, guild) |
| Dedicated Server | Unity Game Server Hosting / AWS GameLift |
| Voice Chat | Vivox (Unity built-in) / Agora |
| Analytics | Unity Analytics |
| Push Notification | Firebase Cloud Messaging |

### 9.4 Code Architecture Changes

```
HIỆN TẠI (Single-player)          →    MỤC TIÊU (Multiplayer)
─────────────────────────               ─────────────────────────
ServiceLocator (static)            →    NetworkServiceLocator (server/client aware)
EventBus (static, local)           →    NetworkEventBus (local + network events)
Model (pure C#)                    →    NetworkModel (NetworkVariable backed)
Presenter (MonoBehaviour)          →    NetworkPresenter (NetworkBehaviour)
PlayerPresenter (single)           →    NetworkPlayerPresenter (IsOwner check)
GameBootstrapper (local)           →    NetworkBootstrapper (server init → client sync)
```

---

## 10. Lộ Trình Phát Triển (Roadmap)

### Phase 0 — Foundation (2-3 tháng)
- Hoàn thiện single-player gameplay core (harvest, craft, build)
- Implement đầy đủ các service stubs
- Migrate legacy code cần thiết
- Art pipeline setup (stylized 3D)

### Phase 1 — Network Foundation (1-2 tháng)
- Cài đặt NGO + Relay + Lobby
- NetworkPlayer sync (position, animation)
- Server-authoritative resource system
- Basic Hub scene (multiplayer)

### Phase 2 — Island System (2-3 tháng)
- Private island save/load (cloud)
- Island visit system (P2P host)
- Building sync (multiplayer co-build)
- Permission system

### Phase 3 — Social (1-2 tháng)
- Friendship progression
- Chat (text)
- Emote system
- Gift/Trade system

### Phase 4 — World & Content (2-3 tháng)
- 3-4 Realms với tài nguyên đặc trưng
- Skill-tree nghề nghiệp
- Craft system đầy đủ
- Cooking system

### Phase 5 — Live Service (1-2 tháng)
- Season Pass framework
- Event system
- Mini-game arena (1-2 games)
- Guild system cơ bản

### Phase 6 — Polish & Launch (2-3 tháng)
- Performance optimization (mobile)
- Localization
- Tutorial / Onboarding
- Closed Beta → Open Beta → Soft Launch

> **Tổng thời gian ước tính**: 12-18 tháng cho MVP launch  
> **Team tối thiểu**: 3-5 người (1-2 programmer, 1 3D artist, 1 UI/UX, 1 game designer)

---

## 11. Rủi Ro & Giải Pháp

| Rủi ro | Xác suất | Giải pháp |
|--------|----------|-----------|
| Server cost cao với 20+ người | Cao | Bắt đầu P2P island + 1-2 hub nhỏ, scale dần |
| Content không đủ giữ chân | Cao | Season Pass + seasonal events tạo FOMO nhẹ |
| Multiplayer sync lag | Trung bình | Client prediction + interpolation, optimize packet |
| Cheat/hack | Trung bình | Server-authoritative cho economy, anti-cheat |
| Mobile performance | Trung bình | LOD, occlusion culling, object pooling, batching |
| Voice chat toxicity | Thấp | Report system, mute, friendship-gated |
