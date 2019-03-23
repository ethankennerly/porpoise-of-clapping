namespace game {
    // Depends on RotationTweener2D configuring and adding TweenRotation2D.
    export class TweenRotation2DSystem extends ut.ComponentSystem {

        deltaTime:number;
        rotationStep:Quaternion = new Quaternion();
        zAxis:Vector3 = new Vector3(0, 0, 1);

        // Binds `this` to this class.
        // Otherwise, binds `this` to browser `window`.
        // Only allocates memory once.
        // Otherwise, allocates memory per call.
        // https://github.com/Microsoft/TypeScript/wiki/'this'-in-TypeScript#use-instance-functions
        updateRotationInstance = (entity:ut.Entity,
                rotationTweener2D:game.RotationTweener2D,
                tweenRotation2D:game.TweenRotation2D,
                transformRotation:ut.Core2D.TransformLocalRotation) => {
            this.updateRotation(entity, rotationTweener2D, tweenRotation2D, transformRotation);
        };

        OnUpdate():void {
            this.deltaTime = this.scheduler.deltaTime();
            this.world.forEach(
                [ut.Entity, game.RotationTweener2D, game.TweenRotation2D, ut.Core2D.TransformLocalRotation],
                this.updateRotationInstance);
        }

        // Rotating every frame, as in HelloCube sample in Unity.Entities C# sample:
        //
        //      rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), rotSpeed.Value * dT));
        updateRotation(entity:ut.Entity,
                rotationTweener2D:game.RotationTweener2D,
                tweenRotation2D:game.TweenRotation2D,
                transformRotation:ut.Core2D.TransformLocalRotation):void
        {
            tweenRotation2D.time += this.deltaTime;
            if (tweenRotation2D.time >= rotationTweener2D.duration) {
                let endAngle:number = rotationTweener2D.endRadians;
                transformRotation.rotation = this.rotationStep.setFromAxisAngle(
                    this.zAxis, endAngle);

                this.world.removeComponent(entity, game.TweenRotation2D);
                return;
            }
            this.world.setComponentData(entity, tweenRotation2D);

            let stepAngle:number = tweenRotation2D.speed * this.deltaTime;
            this.rotationStep.setFromAxisAngle(this.zAxis, stepAngle);
            let previousRotation:Quaternion = transformRotation.rotation;
            previousRotation.normalize();
            transformRotation.rotation = previousRotation.multiply(this.rotationStep);
        }
    }
}
