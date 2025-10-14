# Visual Studio 2026 Developer Productivity Enhancements

## 30 September 2025

> 1. VS 2026 Installation is much faster than VS 2022
> 1. VS 2026 IDE loads faster than VS 2022
> 1. Upgrades popup instead of showing the notification icon at the bottom.
> 1. Executing a .net aspire solution is much (at least 50%) faster than VS 2022.
> 1. Create a sample application with .NET 10, and Aspire 9.5
> 1. Creating Commit Messages
> 1. Code Clean Up

## 14 October 2025

### Adaptive Pasting

> Let Copilot intelligently adjust your pasted code to seamlessly fit the context of your existing code – no more manual tweaking required!

```text
You can trigger Adaptive Paste by:

- Pressing **Shift + Alt + V** after copying your code to see the suggestion immediately.
- Selecting **Edit > Paste Special > Paste with Copilot** from the menu.
```

### Mermaid Chart Rendering

Visual Studio 2026 introduces native Mermaid diagram support within Markdown files. This enhancement allows developers to create visual representations of system architectures, workflows, and data flows directly in documentation.

Key capabilities include:

- Real-time rendering in the Markdown preview pane
- Integration with GitHub Copilot for diagram generation
- Support for multiple diagram types (flowcharts, sequence diagrams, class diagrams, etc.)

Example workflow diagram for a CI/CD pipeline:

```mermaid
---
config:
  layout: elk
---
flowchart TD
    A["Code Commit"] --> B["Build & Test"]
    B --> C{"Tests Pass?"}
    C -- Yes --> D["Deploy to Staging"]
    C -- No --> E["Notify Developer"]
    D --> F["Run Integration Tests"]
    F --> G{"Ready for Prod?"}
    G -- Yes --> H["Deploy to Production"]
    G -- No --> I["Rollback"]
    E --> A
     A:::code
     B:::build
     C:::decision
     D:::deploy
     E:::notify
     F:::deploy
     G:::decision
     H:::deploy
     I:::rollback
    classDef code fill:#bfdbfe,stroke:#60a5fa,color:#1e3a8a           %% Soft Blue
    classDef build fill:#a7f3d0,stroke:#34d399,color:#065f46          %% Soft Green
    classDef decision fill:#fde68a,stroke:#facc15,color:#78350f       %% Soft Yellow
    classDef deploy fill:#c7d2fe,stroke:#818cf8,color:#312e81         %% Soft Indigo
    classDef notify fill:#fecaca,stroke:#f87171,color:#7f1d1d         %% Soft Red
    classDef rollback fill:#fed7aa,stroke:#fb923c,color:#7c2d12       %% Soft Orange
```

To use this feature, simply write Mermaid syntax in code blocks and view the rendered output through the preview panel. Copilot can assist by generating diagram syntax based on your descriptions.

### Code Coverage

Code coverage is now available in Visual Studio Community and Professional editions for the first time – ensuring your code is well-tested has never been easier! (Yet to explore)
