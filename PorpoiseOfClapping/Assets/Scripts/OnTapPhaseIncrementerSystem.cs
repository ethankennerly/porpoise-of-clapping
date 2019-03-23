using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class OnTapPhaseIncrementerSystem : JobComponentSystem
    {
        [BurstCompile]
        struct OnTapPhaseIncrementerJob : IJobProcessComponentData<PhaseConfig>
        {
            [ReadOnly] public float deltaTime;
            [ReadOnly] public bool mouseDown;

            public void Execute(ref PhaseConfig phaseConfig)
            {
                if (phaseConfig.changed)
                {
                    phaseConfig.changed = false;
                }
                if (phaseConfig.timeElapsed == 0 && phaseConfig.phase == 0)
                {
                    phaseConfig.changed = true;
                }
                phaseConfig.timeElapsed += deltaTime;

                if (!mouseDown)
                {
                    return;
                }

                if (phaseConfig.phase > 0 && phaseConfig.timeElapsed < phaseConfig.minDuration)
                {
                    return;
                }

                phaseConfig.phase++;
                if (phaseConfig.phase > phaseConfig.maxPhase)
                {
                    phaseConfig.phase = 0;
                }
                phaseConfig.timeElapsed = 0;
                phaseConfig.changed = true;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new OnTapPhaseIncrementerJob()
            {
                deltaTime = Time.deltaTime,
                mouseDown = Input.GetMouseButtonDown(0)
            };

            return job.Schedule(this, inputDependencies);
        }

    }
}

