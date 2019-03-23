namespace game {
    // Depends on RotationTweener configuring and adding TweenRotation.
    export class TweenRotationSystem extends ut.ComponentSystem {

        deltaTime:number;
        rotationStep:Quaternion = new Quaternion();
        zAxis:Vector3 = new Vector3(0, 0, 1);

        // Binds `this` to this class.
        // Otherwise, binds `this` to browser `window`.
        // Only allocates memory once.
        // Otherwise, allocates memory per call.
        // https://github.com/Microsoft/TypeScript/wiki/'this'-in-TypeScript#use-instance-functions
        updateRotationInstance = (entity:ut.Entity,
                rotationTweener:game.RotationTweener,
                tweenRotation:game.TweenRotation,
                transformRotation:ut.Core2D.TransformLocalRotation) => {
            this.updateRotation(entity, rotationTweener, tweenRotation, transformRotation);
        };

        OnUpdate():void {
            this.deltaTime = this.scheduler.deltaTime();
            this.world.forEach(
                [ut.Entity, game.RotationTweener, game.TweenRotation, ut.Core2D.TransformLocalRotation],
                this.updateRotationInstance);
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
            this.world.setComponentData(entity, tweenRotation);

            let stepAngle:number = tweenRotation.speed * this.deltaTime;
            this.rotationStep.setFromAxisAngle(this.zAxis, stepAngle);
            let previousRotation:Quaternion = transformRotation.rotation;
            previousRotation.normalize();
            transformRotation.rotation = previousRotation.multiply(this.rotationStep);
        }
    }
}
