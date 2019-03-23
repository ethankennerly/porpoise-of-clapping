using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Game
{
    public sealed class ActivateGameObjectSystem : ComponentSystem
    {
        private ComponentGroup m_MainGroup;
        private readonly List<ActivatableObject> m_UniqueTypes = new List<ActivatableObject>(12);

        protected override void OnCreateManager()
        {
            m_MainGroup = GetComponentGroup(typeof(ActivatableObject));
        }

        protected override void OnUpdate()
        {
            EntityManager.GetAllUniqueSharedComponentData(m_UniqueTypes);

            for (int sharedIndex = 0, numShared = m_UniqueTypes.Count; sharedIndex < numShared; ++sharedIndex)
            {
                ActivatableObject activatableObject = m_UniqueTypes[sharedIndex];
                m_MainGroup.SetFilter(activatableObject);
                if (m_MainGroup.CalculateLength() == 0)
                    continue;

                UpdateGameObjectActive(ref activatableObject);
            }
        }

        private static void UpdateGameObjectActive(ref ActivatableObject activatableObject)
        {
            if (activatableObject.synchronized)
                return;

            GameObject linkedObject = activatableObject.linkedObject;
            if (linkedObject == null)
                return;

            activatableObject.synchronized = true;
            linkedObject.SetActive(activatableObject.linkedObjectActive);
        }
    }
}
