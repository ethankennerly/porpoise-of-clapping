using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// This system updates all entities in the scene with both a RotationTweener2D and Rotation component.
public class PhaseRotationTween2DSystem : JobComponentSystem
{
    // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
    [BurstCompile]
    struct PhaseRotationTween2DJob : IJobProcessComponentData<Rotation, RotationTweener2D>
    {
        [ReadOnly] public float deltaTime;
        [ReadOnly] public float3 zAxis;

        public void Execute(ref Rotation rotation, ref RotationTweener2D tweenRotation2D)
        {
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new PhaseRotationTween2DJob()
        {
            deltaTime = Time.deltaTime,
            zAxis = new float3(0f, 0f, 1f)
        };

        return job.Schedule(this, inputDependencies);
    }
}
