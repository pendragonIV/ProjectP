# Swordidle Copilot Agentic AI Setup

Hệ thống knowledge base và cấu hình cho Copilot AI trong dự án Swordidle.

## 📁 Cấu Trúc Thư Mục

```
.vscode/                    # VS Code configuration
├── settings.json          # Editor and Copilot settings
└── extensions.json        # Recommended extensions

.agent/                     # Knowledge Base for Copilot
├── knowledge.md           # Project overview, architecture, systems
├── conventions.md         # Coding standards and patterns
├── architecture.md        # MVC/MVVM layers and responsibilities
├── context.md             # Current project status and progress
└── quick-prompts.md       # Pre-written prompt templates

.copilot/                   # Copilot-specific configuration
└── instructions.md        # AI behavior guidelines and principles

.prompts/                   # Reusable prompt templates
├── analyze-code.md        # Code review prompts
└── refactor-architecture.md  # Architecture refactoring guide
```

## 🚀 Cách Sử Dụng

### 1. Cơ Bản - Hỏi Copilot
Mở Copilot Chat (Ctrl+I) và hỏi:

```
Analyze this code for architecture violations.
Reference `.agent/conventions.md` and `.agent/architecture.md`.
```

### 2. Sử Dụng Prompt Templates
Copy prompt từ `.agent/quick-prompts.md`:

```
Implement a [ComponentName] that:
- Follows the VIEW/ViewModel/Service pattern from `.agent/architecture.md`
- Matches conventions in `.agent/conventions.md`
```

### 3. Code Review
Chọn code → Copilot Chat → Paste prompt template từ `.prompts/analyze-code.md`

### 4. Refactoring
Chọn component → Copy prompt từ `.prompts/refactor-architecture.md` → Paste vào Copilot

## 📚 File Quan Trọng

| File | Mục Đích |
|------|---------|
| `.agent/knowledge.md` | Context dự án, systems, technologies |
| `.agent/conventions.md` | Naming, file structure, best practices |
| `.agent/architecture.md` | Layer diagram, responsibilities, patterns |
| `.copilot/instructions.md` | AI behavior, review guidelines, dos/don'ts |
| `.agent/quick-prompts.md` | Pre-written prompts cho các task thường gặp |

## 💡 Best Practices

### ✅ DO
- Tham khảo file kiến thức khi hỏi Copilot
- Sử dụng pre-written prompts từ `.agent/quick-prompts.md`
- Update `.agent/context.md` khi có progress mới
- Reference architecture file khi discussing design

### ❌ DON'T
- Không hỏi Copilot implement mà không reference architecture
- Không mix business logic vào Views
- Không forget event unsubscription
- Không use public fields (use [SerializeField])

## 🔧 Configuration

### VS Code Settings Included
- Copilot chat scope selection enabled
- C# formatter configured
- Roslyn analyzers enabled
- Search exclusions (Library, Temp folders)
- Inline suggestions with count limit

### Extensions Recommended
- GitHub Copilot
- GitHub Copilot Chat
- C# (ms-dotnettools)
- Unity Debugger
- GitLens (for blame, history)

## 📖 Architecture Overview

```
UI Views (WeaponSlotView)
    ↓ (user input)
ViewModel (WeaponInventoryViewModel)
    ↓ (requests)
Services (WeaponService)
    ↓ (events)
Back to ViewModel → Views (update UI)
```

**Key Rule**: Views display only, Services own state, ViewModels coordinate.

## 🎯 Workflow Example

1. **I want to add a new UI component**
   - Open `.agent/quick-prompts.md` → Find "Implement Component"
   - Copy prompt, paste into Copilot Chat
   - Copilot will follow architecture and conventions

2. **I found performance issue**
   - Select code → Copilot Chat
   - Use prompt from `.prompts/analyze-code.md`
   - Get suggestions aligned with project patterns

3. **I'm unsure about architecture**
   - Reference `.agent/architecture.md`
   - Ask Copilot about specific pattern
   - It will know the patterns used in Swordidle

## 🔄 Maintaining Knowledge Base

Update these files when:
- New architectural patterns introduced
- Convention changes
- New major system added
- Important decisions made

Location: `.agent/context.md` - Update "Last Updated" date

## 📝 Example: Using Copilot for Code Review

```bash
# 1. Select code in editor
# 2. Open Copilot Chat (Ctrl+I)
# 3. Paste this prompt:

Analyze this code using `.agent/conventions.md` and `.agent/architecture.md`. 
Check for:
- Architecture layer compliance
- Naming convention violations
- Memory leak risks
- Performance concerns

# 4. Copilot will review following YOUR project standards
```

## 🎓 Learning Path

1. **First Time**: Read `.agent/architecture.md` to understand layers
2. **Always**: Reference `.agent/conventions.md` when writing code
3. **Deep Dive**: Study `.copilot/instructions.md` to understand AI guidelines
4. **Efficient**: Use `.agent/quick-prompts.md` for common tasks

## 🔗 Related Files

- Project: `/c/Dev/swordidle`
- Scripts: `Assets/_Game/Scripts/`
- Weapon System: `Assets/_Game/Scripts/UI/Weapon/`
- Infrastructure: `Assets/_Core/`

---

**Created**: 2026-04-21  
**Last Updated**: 2026-04-21  
**Swordidle Team**
