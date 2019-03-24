using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Game
{
    public class PhaseEnablerSystem : ComponentSystem
    {
        private ComponentGroup m_PhaseConfigGroup;
        private ComponentGroup m_PhaseEnablerGroup;

        protected override void OnCreateManager()
        {
            m_PhaseConfigGroup = GetComponentGroup(typeof(PhaseConfig));
            m_PhaseEnablerGroup = GetComponentGroup(typeof(PhaseEnabler), typeof(ActivatableObject));
        }

        private bool TryGetFirstPhaseConfig(ref PhaseConfig phaseConfig)
        {
            NativeArray<PhaseConfig> phaseConfigs = m_PhaseConfigGroup.ToComponentDataArray<PhaseConfig>(
                Allocator.TempJob);
            if (phaseConfigs.Length < 1)
            {
                phaseConfigs.Dispose();
                return false;
            }

            phaseConfig = phaseConfigs[0];

            phaseConfigs.Dispose();
            return true;
        }

        [BurstCompile]
        struct PhaseEnablerSharedJob
        {
            [ReadOnly] public PhaseConfig phaseConfig;

            public void ExecuteEach(EntityManager entityManager, ComponentGroup phaseEnablerGroup)
            {
                NativeArray<Entity> entities = phaseEnablerGroup.ToEntityArray(Allocator.TempJob);

                for (int entityIndex = 0, numEntities = entities.Length; entityIndex < numEntities; ++entityIndex)
                {
                    Entity entity = entities[entityIndex];
                    PhaseEnabler phaseEnabler = entityManager.GetComponentData<PhaseEnabler>(entity);
                    ActivatableObject activatableObject = entityManager.GetSharedComponentData<ActivatableObject>(entity);
                    if (!TryActivateObjectByPhase(ref phaseEnabler, ref activatableObject))
                        continue;
                    
                    entityManager.SetSharedComponentData<ActivatableObject>(entity, activatableObject);
                }
                entities.Dispose();
            }

            public bool TryActivateObjectByPhase(ref PhaseEnabler phaseEnabler, ref ActivatableObject activatableObject)
            {
                int phase = phaseConfig.phase;
                bool enabled = phase == phaseEnabler.enabledPhase;
                if (activatableObject.linkedObjectActive == enabled)
                    return false;

                activatableObject.linkedObjectActive = enabled;
                activatableObject.synchronized = false;
                return true;
            }
        }

        protected override void OnUpdate()
        {
            PhaseConfig phaseConfig = default(PhaseConfig);
            if (!TryGetFirstPhaseConfig(ref phaseConfig))
                return;

            var sharedJob = new PhaseEnablerSharedJob()
            {
                phaseConfig = phaseConfig
            };

            sharedJob.ExecuteEach(EntityManager, m_PhaseEnablerGroup);
        }
    }
}
