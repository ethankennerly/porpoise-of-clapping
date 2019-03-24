using System;
using Unity.Entities;

[Serializable]
public struct RotationTweener2D : IComponentData
{
    public float endRadians;
    public float duration;
}

[UnityEngine.DisallowMultipleComponent]
public class RotationTweener2DComponent : ComponentDataProxy<RotationTweener2D> { }
