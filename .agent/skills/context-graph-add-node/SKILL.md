---
name: context-graph-add-node
description: Add a new node (module/class/system) to the project context graph.
---

# Context Graph / Add Node

## How to Call

```bash
unity-mcp-cli run-tool context-graph-add-node --input '{
  "id": "string_value",
  "label": "string_value",
  "group": "string_value",
  "title": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool context-graph-add-node --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool context-graph-add-node --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `id` | `string` | Yes | ID of the node, usually namespace or system name (e.g. SwordIdle.Game.Combat) |
| `label` | `string` | Yes | Short display label |
| `group` | `string` | Yes | Group layer: core, game, ui, bootstrap, default |
| `title` | `string` | Yes | Description of what this node does |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "id": {
      "type": "string"
    },
    "label": {
      "type": "string"
    },
    "group": {
      "type": "string"
    },
    "title": {
      "type": "string"
    }
  },
  "required": [
    "id",
    "label",
    "group",
    "title"
  ]
}
```

## Output

This tool does not return structured output.

