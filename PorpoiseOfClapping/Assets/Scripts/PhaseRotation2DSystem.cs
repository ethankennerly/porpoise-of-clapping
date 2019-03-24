using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class PhaseRotationTween2DSystem : JobComponentSystem
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
        struct PhaseRotationTween2DJob : IJobProcessComponentData<Rotation, RotationTweener2D>
        {
            [ReadOnly] public PhaseConfig phaseConfig;
            [ReadOnly] public float3 zAxis;

            public void Execute(ref Rotation rotation, ref RotationTweener2D tweenRotation2D)
            {
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            PhaseConfig phaseConfig = default(PhaseConfig);
            if (!TryGetFirstPhaseConfig(ref phaseConfig))
                return default(JobHandle);

            var job = new PhaseRotationTween2DJob()
            {
                phaseConfig = phaseConfig,
                zAxis = new float3(0f, 0f, 1f)
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}
