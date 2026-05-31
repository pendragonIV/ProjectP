# Workflow Guide - Sử Dụng Copilot Agentic AI

## Quy Trình Làm Việc

### 📋 Trước Khi Bắt Đầu (Bắt buộc chạy Quy trình Audit Tài liệu)
1. Mở project trong VS Code.
2. Kiểm tra tài liệu cấu trúc tổng quát tại `Assets/Docs/PROJECT_STRUCTURE.md`.
3. Kiểm tra xem thư mục `Assets/Docs/Features/<FeatureName>/` và file `FEATURE_SPEC.md` đã tồn tại chưa.
   - **Nếu CHƯA CÓ**: Dừng code ngay lập tức! Tiến hành tạo mới thư mục và file tài liệu thiết kế theo mẫu trong rule `10_feature-documentation-first.mdc`.
   - **Nếu ĐÃ CÓ**: Review tài liệu đó và kiểm tra độ khớp thực tế với scene hiện tại và codebase. Nếu tài liệu bị lệch, tiến hành cập nhật/đồng bộ hóa tài liệu trước khi viết code.
4. Chạy script `.agent/tools/AuditDocs.ps1` (nếu cần) để kiểm tra xem có những feature folder nào đang bị thiếu tài liệu.

### 🎯 Workflow Chính

#### 1. **Code Review / Analysis**
```
File cần review → Select code → Ctrl+I (Copilot Chat) 
→ Paste prompt từ `.prompts/analyze-code.md`
→ Copilot phân tích theo conventions + architecture
```

#### 2. **Document First (Review & Create Docs)**
```
Có tính năng cần làm/sửa → Check Assets/Docs/Features/<FeatureName>/
→ Nếu CHƯA CÓ: Tạo folder feature + file FEATURE_SPEC.md & SETUP.md trước
→ Nếu ĐÃ CÓ: Đọc kĩ, kiểm tra xem có khớp scene hierarchy/codebase không.
             Nếu tài liệu bị lệch, tiến hành cập nhật tài liệu trước khi code.
→ Cập nhật đồng bộ các file tài liệu liên quan chéo (PROJECT_STRUCTURE.md, SETUP.md)
```

#### 3. **Implement New Feature**
```
Know what to build (docs ready) → Ctrl+I 
→ Copy prompt từ `.agent/quick-prompts.md` ("Implement Component")
→ Customize prompt
→ Copilot generates code following architecture
```

#### 3. **Refactor Component**
```
Messy component → Select all → Ctrl+I
→ Copy prompt từ `.prompts/refactor-architecture.md`
→ Copilot refactors using MVC/MVVM pattern
```

#### 4. **Ask Architecture Question**
```
Ctrl+I → Ask question → Reference ".agent/architecture.md"
→ Copilot answers with project context
```

### 🔍 Example Workflows

#### Scenario A: Code Review
```
I have WeaponSlotView.cs and want to check quality

1. Select all code
2. Ctrl+I (Copilot Chat)
3. Paste:
   "Analyze this code using `.agent/conventions.md` and `.agent/architecture.md`. 
    Check for architecture layer compliance, naming violations, memory leaks, 
    and performance concerns."
4. Get detailed feedback aligned with project standards
```

#### Scenario B: Implement New Slot Type
```
I need to create SpecialWeaponSlotView

1. Ctrl+I
2. Paste:
   "Implement a SpecialWeaponSlotView that:
    - Follows VIEW/ViewModel/Service pattern from `.agent/architecture.md`
    - Matches conventions in `.agent/conventions.md`
    - Inherits from appropriate base
    - Has XML documentation"
3. Copilot generates complete component
4. Copy-paste into project
```

#### Scenario C: Debug Memory Issue
```
Events aren't unsubscribing properly

1. Select suspect code
2. Ctrl+I
3. Paste:
   "Check this code for memory leaks.
    Reference best practices from `.agent/conventions.md`.
    Ensure proper cleanup in OnDestroy()."
4. Get specific fixes
```

#### Scenario D: Understand Pattern
```
Why do we use IWeaponHolder interface?

1. Ctrl+I
2. Ask:
   "Explain the IWeaponHolder interface pattern in this codebase.
    Reference `.agent/architecture.md`. Why use it instead of inheritance?"
3. Get detailed explanation with project context
```

## 📱 Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Open Copilot Chat | Ctrl+I |
| Inline suggestion | Ctrl+\ |
| Accept suggestion | Tab |
| Dismiss | Esc |

## 📂 File Reference Quick Map

```
Need to understand...          → Read file...
├── Overall architecture       → `.agent/architecture.md`
├── Naming/coding rules       → `.agent/conventions.md`
├── What Copilot should do    → `.copilot/instructions.md`
├── Project status            → `.agent/context.md`
├── Common prompts            → `.agent/quick-prompts.md`
├── Code review prompts       → `.prompts/analyze-code.md`
└── Refactor guide            → `.prompts/refactor-architecture.md`
```

## 🎓 Tips & Tricks

### Tip 1: Add Context to Prompts
❌ Bad: "Implement a new component"
✅ Good: "Implement a UI component for weapon selection that follows `.agent/architecture.md` pattern"

### Tip 2: Reference Knowledge Files
Always reference `.agent/` files when asking Copilot
```
"Reference `.agent/conventions.md` for naming and structure"
"Following `.agent/architecture.md` MVC pattern"
```

### Tip 3: Use Specific Prompts
Copy exact prompts from `.agent/quick-prompts.md` instead of winging it

### Tip 4: Iterate & Improve
- Ask for alternatives if first suggestion not perfect
- Ask Copilot to explain decisions
- Request specific improvements (testability, performance, etc.)

### Tip 5: Update Context
After major work, update `.agent/context.md` with progress
```
## Recently Worked On
- WeaponSlotView refactoring (2026-04-21)

## Current Challenges
- [ ] Performance optimization needed
```

## 🔧 Advanced Usage

### Generate Multiple Options
```
"Generate 3 different approaches to implement [Feature].
For each, show pros/cons and which follows `.agent/architecture.md` best."
```

### Compare Patterns
```
"Compare these 2 implementations against `.agent/conventions.md`.
Which better follows Swordidle patterns?"
```

### Performance Analysis
```
"Audit this code for performance using `.agent/conventions.md` best practices.
Suggest optimizations for hot paths."
```

### Test Generation
```
"Write unit tests for this Service class.
Reference testability notes from `.agent/architecture.md`."
```

## 🚨 Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| Copilot ignores conventions | Reference `.agent/conventions.md` in prompt |
| Generated code not matching style | Include specific style examples in prompt |
| Copilot suggests tight coupling | Ask it to reference `.agent/architecture.md` |
| Memory leak suggestions missed | Use `.agent/conventions.md` memory leak checklist |

## 📊 Effectiveness Tips

**Do This More:**
- ✅ Reference knowledge files explicitly
- ✅ Use pre-written prompts from `.agent/quick-prompts.md`
- ✅ Ask for explanations, not just code
- ✅ Request multiple iterations
- ✅ Update context.md regularly

**Do This Less:**
- ❌ Generic prompts without context
- ❌ Asking Copilot design without architecture reference
- ❌ Implementing without review
- ❌ Ignoring AI suggestions on best practices

## 🎯 Weekly Routine

```
Monday:   Review `.agent/architecture.md` changes
Tuesday:  Use Copilot for feature implementation
Wednesday: Code review with Copilot
Thursday: Refactor with architecture in mind
Friday:   Update `.agent/context.md` with progress
```

## 📞 When to Use Copilot vs Manual

| Task | Use Copilot? |
|------|-------------|
| Code review | ✅ Yes - follows your standards |
| Simple bug fix | ⏸️ Maybe - depends on complexity |
| New component | ✅ Yes - generates architecture-correct code |
| Understand pattern | ✅ Yes - explains with project context |
| Refactor messy code | ✅ Yes - applies conventions |
| Edge case handling | ⏸️ Maybe - verify logic manually |
| Performance optimization | ✅ Yes - knows your patterns |

---

**For detailed setup**, see `.agent/README.md`
**For prompts**, see `.agent/quick-prompts.md`
**For architecture**, see `.agent/architecture.md`
