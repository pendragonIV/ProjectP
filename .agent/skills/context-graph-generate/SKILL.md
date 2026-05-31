---
name: context-graph-generate
description: Run the C# Reflection Generator to scan the entire project and output the full context graph to .agent/context_graph.js. MUST be called after any code modification!
---

# Context Graph / Generate Full Graph

## How to Call

```bash
unity-mcp-cli run-tool context-graph-generate --input '{}'
```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

This tool takes no input parameters.

### Input JSON Schema

```json
{
  "type": "object",
  "additionalProperties": false
}
```

## Output

This tool does not return structured output.

