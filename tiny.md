# Review of Project Tiny

## Pros

+ 2 MB uncompressed JavaScript build. Compared to 3 MB Unity WebAssembly build.
+ Development builds in 6 seconds. Compared to a minute or more for Unity WebGL build.
+ Runs in mobile web browser.

## Cons

- Not compatible with Unity projects.
- Only exports HTML5 or PlayableAd.
- Cannot inspect at runtime.
- No C#. TypeScript.
. Requires .NET 4.0 or higher.
- Limited components and integration with Unity editor.
- Hard to find documentation on scripting API.
    - Undocumented examples, such as: <https://forum.unity.com/threads/what-is-the-meaning-of-requiredcomponents.645160/>
    - What is the API for subtractive foreach and filter?
    - What is the API for a reactive system?
- Demos get entities by name. So if entity name changes, the code breaks.
- Demos use several anonymous functions.
