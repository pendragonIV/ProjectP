---
name: context-graph-add-edge
description: Add a new dependency edge between two nodes in the context graph.
---

# Context Graph / Add Edge

## How to Call

```bash
unity-mcp-cli run-tool context-graph-add-edge --input '{
  "fromId": "string_value",
  "toId": "string_value",
  "label": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool context-graph-add-edge --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool context-graph-add-edge --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `fromId` | `string` | Yes | ID of the source node |
| `toId` | `string` | Yes | ID of the target node |
| `label` | `string` | Yes | Label describing the relationship (e.g. uses, registers, spawns) |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "fromId": {
      "type": "string"
    },
    "toId": {
      "type": "string"
    },
    "label": {
      "type": "string"
    }
  },
  "required": [
    "fromId",
    "toId",
    "label"
  ]
}
```

## Output

This tool does not return structured output.

