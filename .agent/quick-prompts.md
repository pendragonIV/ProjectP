# Quick Prompts for Copilot

## Code Review Prompts

### Analyze This Code
```
Analyze this code using `.agent/conventions.md` and `.agent/architecture.md`. 
Check for:
- Architecture layer compliance
- Naming convention violations
- Memory leak risks
- Performance concerns
```

### Find Architecture Violations
```
Review this code and identify any violations of MVC/MVVM architecture.
Reference `.agent/architecture.md` for proper layering.
Suggest specific refactoring steps.
```

### Suggest Improvements
```
This code works but can be better. Suggest improvements that:
1. Follow Swordidle conventions
2. Maintain testability
3. Reduce coupling
4. Improve performance
```

## Implementation Prompts

### Implement Component
```
Implement a [ComponentName] that:
- Follows the VIEW/ViewModel/Service pattern from `.agent/architecture.md`
- Matches conventions in `.agent/conventions.md`
- Is properly documented with XML comments
- Includes error handling and null checks
```

### Refactor to MVC
```
Refactor this monolithic component into MVC/MVVM pattern:
- View: Display and input only
- ViewModel: Coordination logic
- Service: Business rules

Reference `.agent/architecture.md` for the pattern.
```

### Implement Event Communication
```
Add event-based communication between [Component A] and [Component B].
Follow the pattern in `.agent/architecture.md`.
Ensure proper subscription/unsubscription in OnDestroy().
```

## Documentation First Prompts

### Review & Create Feature Docs
```
Audit the documentation status for the feature [FeatureName].
First, check if `Assets/Docs/PROJECT_STRUCTURE.md` exists and read it.
Then, check if there is an existing `Assets/Docs/Features/[FeatureName]/FEATURE_SPEC.md` or `SETUP.md`.
1. If they exist, verify if they match the current codebase and scene hierarchy. Identify and list any discrepancies.
2. If they do not exist, generate the folder `Assets/Docs/Features/[FeatureName]/` and the specification files (`FEATURE_SPEC.md` and `SETUP.md` if UI/Scene related) using the standard template in rule `10_feature-documentation-first.mdc`.
Do not write any code until the documentation is generated and reviewed.
```

## Documentation Prompts

### Document Architecture
```
The current system needs documentation. 
Create a guide explaining:
- How data flows from Model to View
- Where each layer handles responsibilities
- Example flow for typical operation

Reference `.agent/architecture.md`.
```

### Explain Pattern
```
Explain why we use [Pattern Name] in this codebase.
Include:
- Benefits vs alternatives
- How it's implemented in Swordidle
- Common pitfalls to avoid
- Reference `.agent/conventions.md`
```

## Optimization Prompts

### Performance Audit
```
Audit this code for performance issues:
- LINQ in hot paths
- Unnecessary allocations
- Missing caching
- Object pooling opportunities

Focus on areas called frequently.
```

### Memory Leak Check
```
Check this code for memory leaks:
- Unsubscribed events
- Dangling references
- Missing cleanup in OnDestroy()
- Pooling-related issues

Reference best practices from `.agent/conventions.md`.
```

## Testing Prompts

### Write Unit Tests
```
Write unit tests for [ClassName] assuming it's a Service.
- Mock dependencies
- Test success cases
- Test error cases
- Reference `.agent/architecture.md` for testability notes
```

### Make Testable
```
This code is hard to test. Refactor it for testability:
- Remove tight coupling
- Inject dependencies
- Separate logic from UI
- Reference `.agent/architecture.md`
```

## Integration Prompts

### Integrate Component
```
How do I integrate this new component into the existing system?
- Where should it live in the project structure?
- How should it communicate with other components?
- Reference `.agent/architecture.md` for the pattern
```

### Handle State Updates
```
The state changed in [Service]. How should UI components be notified?
- Use the event pattern from `.agent/architecture.md`
- Show complete implementation with subscription/unsubscription
- Ensure memory safety
```
