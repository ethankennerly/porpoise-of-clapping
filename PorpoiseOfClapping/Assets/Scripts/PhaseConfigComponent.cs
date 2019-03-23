using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PhaseConfig : IComponentData
{
    public int phase;
    public int maxPhase;
    /// <summary>
    /// BlittableBool by Sargon.
    /// Not a bool, bool1, or bool2.
    /// Otherwise, at runtime:
    ///      ArgumentException: PhaseConfig is an IComponentData, and thus must be blittable (No managed object is allowed on the struct).
    /// https://gametorrahod.com/unity-ecs-know-your-blittable-types-360b8f9a7f4a
    /// https://github.com/Unity-Technologies/Unity.Mathematics/blob/master/src/Unity.Mathematics/bool2.gen.cs
    /// https://forum.unity.com/threads/bool1-gone-in-preview-4.532814/
    /// </summary>
    public Bool changed;
    public float minDuration;
    public float timeElapsed;
}

// ComponentDataWrapper in samples was renamed to ComponentDataProxy.
// https://github.com/Unity-Technologies/EntityComponentSystemSamples/issues/46
[UnityEngine.DisallowMultipleComponent]
public class PhaseConfigComponent : ComponentDataProxy<PhaseConfig> { }
