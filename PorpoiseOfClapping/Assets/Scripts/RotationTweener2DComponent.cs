using System;
using Unity.Entities;

[Serializable]
public struct RotationTweener2D : IComponentData
{
    public Bool enabled;
    public float endRadians;
    public float speed;
    public float duration;
    public float time;
}

[UnityEngine.DisallowMultipleComponent]
public class RotationTweener2DComponent : ComponentDataProxy<RotationTweener2D> { }
