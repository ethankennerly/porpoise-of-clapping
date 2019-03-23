using System;
using System.Text;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Is there a simpler way to get the game object that is linked to this entity?
/// Would get component object work?
/// <a href="https://docs.unity3d.com/Packages/com.unity.entities@0.0/api/Unity.Entities.EntityManager.html#Unity_Entities_EntityManager_GetComponentObject__1_Unity_Entities_Entity_">GetComponentObject</a>
///
/// Shared component data can refer to a managed game object.
/// </summary>
[Serializable]
public struct ActivatableObject : ISharedComponentData
{
    public Bool linkedObjectActive;
    public Bool synchronized;
    public GameObject linkedObject;

    public override string ToString()
    {
        var builder = new StringBuilder("ActivatableObject(").
            Append("active=").Append(linkedObjectActive).
            Append(", linkedObject=").Append(linkedObject == null ? "null" : linkedObject.ToString()).
            Append(", synchronized=").Append(synchronized).
            Append(')');

        return builder.ToString();
    }
}

public class ActivatableObjectComponent : SharedComponentDataProxy<ActivatableObject> { }
