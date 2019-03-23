namespace game {
    export class TweenRotationSystem extends ut.ComponentSystem {

        deltaTime:number;
        rotationStep:Quaternion = new Quaternion();
        zAxis:Vector3 = new Vector3(0, 0, 1);

        OnUpdate():void {
            this.deltaTime = this.scheduler.deltaTime();
            this.world.forEach(
                [ut.Entity, game.RotationTweener, game.TweenRotation, ut.Core2D.TransformLocalRotation],
                this.updateRotation);
        }

        // Rotating every frame, as in HelloCube sample in Unity.Entities C# sample:
        //
        //      rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), rotSpeed.Value * dT));
        updateRotation(entity:ut.Entity,
                rotationTweener:game.RotationTweener,
                tweenRotation:game.TweenRotation,
                transformRotation:ut.Core2D.TransformLocalRotation):void
        {
            tweenRotation.time += this.deltaTime;
            if (tweenRotation.time >= rotationTweener.duration) {
                let endAngle:number = rotationTweener.endRadians;
                transformRotation.rotation = this.rotationStep.setFromAxisAngle(
                    this.zAxis, endAngle);

                this.world.removeComponent(entity, game.TweenRotation);
                return;
            }

            let stepAngle:number = tweenRotation.speed * this.deltaTime;
            this.rotationStep.setFromAxisAngle(this.zAxis, stepAngle);
            let previousRotation:Quaternion = transformRotation.rotation;
            previousRotation.normalize();
            transformRotation.rotation = previousRotation.multiply(this.rotationStep);
        }
    }
}
