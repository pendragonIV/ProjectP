# Context Graph Schema & Integration

This Graph Viewer is designed to be cross-platform. You can use it to visualize architecture for Unity, Web (NodeJS/React), Mobile (Flutter/React Native), or any other project.

## How to use in other projects
1. Copy this `graph-viewer` folder to your project.
2. Write a script (Python, NodeJS, etc.) that parses your codebase and generates a `context_graph.js` file in the parent folder.
3. Open `index.html` in your browser.

## The `context_graph.js` Schema

The file MUST be named `context_graph.js` and contain a single global assignment:

```javascript
window.CONTEXT_DATA = {
  "nodes": [
    {
      "id": "Unique.Node.ID",
      "label": "Display Name",
      "group": "group_name_for_color",
      "title": "Hover tooltip text",
      "filePath": "C:/absolute/or/relative/path/to/file.cs",
      "methods": ["+ public void Awake()", "- private void Update()"],
      "fields": ["+ public int score", "- private float speed"]
    }
  ],
  "edges": [
    {
      "from": "Source.Node.ID",
      "to": "Target.Node.ID",
      "arrows": "to"
    }
  ]
};
```

### Properties
- `id` (string): Unique identifier for the node (e.g. namespace + class name).
- `label` (string): Short text displayed on the node.
- `group` (string): Used for coloring. Default styles support: `core`, `game`, `ui`, `bootstrap`, `default`.
- `title` (string): HTML or plain text shown when hovering over the node.
- `filePath` (string, optional): Absolute path to the source code file. Powers the "Open in IDE" feature.
- `methods` (array of strings, optional): List of methods displayed in the sidebar.
- `fields` (array of strings, optional): List of properties/fields displayed in the sidebar.

## Supported Layouts
- **Force-directed (Network):** Best for showing complex webs of dependencies.
- **Hierarchical (Tree):** Best for showing layered architecture (e.g. UI -> Domain -> Database). You can switch to this via the toolbar.
