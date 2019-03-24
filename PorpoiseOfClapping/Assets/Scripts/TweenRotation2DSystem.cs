using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// To rotate the game object, attach all these proxies to the object:
/// - Rotation Tweener 2D Component
/// - Rotation Proxy
/// - Position Proxy
/// - Copy Initial Transform From Game Object Proxy
/// - Copy Transform To Game Object Proxy
/// </summary>
public class TweenRotation2DSystem : JobComponentSystem
{
    [BurstCompile]
    struct TweenRotation2DJob : IJobProcessComponentData<Rotation, RotationTweener2D>
    {
        [ReadOnly] public float deltaTime;
        [ReadOnly] public float3 zAxis;

        public void Execute(ref Rotation rotation, ref RotationTweener2D rotationTweener2D)
        {
            if (!rotationTweener2D.enabled)
                return;

            rotationTweener2D.time += deltaTime;
            if (rotationTweener2D.time >= rotationTweener2D.duration)
            {
                rotationTweener2D.enabled = false;
                return;
            }

            rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(zAxis,
                rotationTweener2D.speed * deltaTime));
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
