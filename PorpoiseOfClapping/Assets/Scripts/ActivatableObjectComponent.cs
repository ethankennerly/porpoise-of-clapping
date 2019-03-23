using System;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Is there a simpler way to get the game object that is linked to this entity?
///
/// Shared component data can refer to a managed game object.
/// </summary>
[Serializable]
public struct ActivatableObject : ISharedComponentData
{
    public Bool linkedObjectActive;
    public GameObject linkedObject;
}

public class ActivatableObjectComponent : SharedComponentDataProxy<ActivatableObject> { }
