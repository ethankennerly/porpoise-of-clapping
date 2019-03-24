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
        private const float kMinDuration = 0.0001f;

        private ComponentGroup m_PhaseConfigGroup;

        protected override void OnCreateManager()
        {
            m_PhaseConfigGroup = GetComponentGroup(typeof(PhaseConfig));
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

            public void Execute(ref Rotation rotation, ref RotationTweener2D rotationTweener2D)
            {
                if (!phaseConfig.changed)
                    return;

                if (phaseConfig.phase < 1)
                    return;

                if (rotationTweener2D.endRadians == 0f)
                    return;

                quaternion startRotation = quaternion.AxisAngle(zAxis, 0f);
                rotation.Value = startRotation;
                float duration = rotationTweener2D.duration;
                if (duration <= kMinDuration)
                {
                    duration = kMinDuration;
                }

                rotationTweener2D.speed = rotationTweener2D.endRadians / duration;
                rotationTweener2D.time = 0f;
                rotationTweener2D.enabled = true;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            PhaseConfig phaseConfig = default(PhaseConfig);
            TryGetFirstPhaseConfig(ref phaseConfig);

            var job = new PhaseRotationTween2DJob()
            {
                phaseConfig = phaseConfig,
                zAxis = new float3(0f, 0f, 1f)
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}
