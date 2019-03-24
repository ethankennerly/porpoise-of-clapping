using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// This system updates all entities in the scene with both a TweenRotation2D and Rotation component.
public class TweenRotation2DSystem : JobComponentSystem
{
    // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
    [BurstCompile]
    struct TweenRotation2DJob : IJobProcessComponentData<Rotation, TweenRotation2D>
    {
        [ReadOnly] public float deltaTime;
        [ReadOnly] public float3 zAxis;

        public void Execute(ref Rotation rotation, ref TweenRotation2D tweenRotation2D)
        {
            tweenRotation2D.time += deltaTime;
            if (tweenRotation2D.time >= tweenRotation2D.duration)
                return;

            rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(zAxis,
                tweenRotation2D.speed * deltaTime));
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new TweenRotation2DJob()
        {
            deltaTime = Time.deltaTime,
            zAxis = new float3(0f, 0f, 1f)
        };

        return job.Schedule(this, inputDependencies);
    }
}
