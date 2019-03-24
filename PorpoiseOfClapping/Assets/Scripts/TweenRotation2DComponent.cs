using System;
using Unity.Entities;

[Serializable]
public struct TweenRotation2D : IComponentData
{
    public float speed;
    public float time;
    public float duration;
}

[UnityEngine.DisallowMultipleComponent]
public class TweenRotation2DComponent : ComponentDataProxy<TweenRotation2D> { }
