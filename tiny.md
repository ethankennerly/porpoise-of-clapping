# Review of Project Tiny

- Much less productive than Unity workflow.

## Documentation

- Manual
    - <https://docs.unity3d.com/Packages/com.unity.tiny@0.14/manual/>
- Runtime documentation
    - <https://docs.unity3d.com/Packages/com.unity.tiny@0.14/rt/tiny_runtime/globals.html>

## Pros

+ 2 MB uncompressed JavaScript build. Compared to 3 MB Unity WebAssembly build.
+ Development builds in 6 seconds. Compared to a minute or more for Unity WebGL build.
+ Runs in mobile web browser.
+ Concise system boilerplate.
+ Lightweight, modular entities and components.
+ Define and attach components in editor.

## Cons

- Code and assets are not compatible with Unity projects.
- Limited components and integration with Unity editor.
    - Just basic sprite, audio, mouse input.
    - For example, does not support text fonts.
    - Otherwise cannot use Tiny in Unity. Cannot use Unity in Tiny.
- Only exports HTML5 or PlayableAd.
- Cannot inspect at runtime.
- No C#. TypeScript only until up to June, 2019.
    - <https://forum.unity.com/threads/c-update-for-project-tiny.643000/>
- Requires .NET 4.0 or higher.
- Hard to find documentation on scripting API.
    - How to callback on entity enable in behaviour after entity is disabled and enabled a second time?
    - How to enable entity <https://forum.unity.com/threads/help-how-to-disable-and-enable-the-entity.603145/>
        - How to show disabled entity?
        - How to hide sprite of child of disabled entity?
    - Undocumented examples, such as: <https://forum.unity.com/threads/what-is-the-meaning-of-requiredcomponents.645160/>
    - What is the API for subtractive foreach and filter?
    - What is the API for a reactive system?
    - What is the API for watching when a tween ends?
- Less stable.
- Demos get entities by name. So if entity name changes, the code breaks.
- Demos use several anonymous functions.
