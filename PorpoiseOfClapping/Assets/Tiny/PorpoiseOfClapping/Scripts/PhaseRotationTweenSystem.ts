namespace game {
    export class PhaseRotationTweenSystem extends ut.ComponentSystem {
        minDuration:number = 0.001;

        // Binds `this` to this class.
        // Otherwise, binds `this` to browser `window`.
        // Only allocates memory once.
        // Otherwise, allocates memory per call.
        // https://github.com/Microsoft/TypeScript/wiki/'this'-in-TypeScript#use-instance-functions
        startTweenRotationInstance = (entity:ut.Entity,
                rotationTweener:game.RotationTweener,
                transformRotation:ut.Core2D.TransformLocalRotation) => {
            this.startTweenRotation(entity, rotationTweener, transformRotation);
        };

        OnUpdate():void {
            let phaseConfig = this.world.getConfigData(game.PhaseConfig);
            let phaseIncreased:boolean = phaseConfig.changed && phaseConfig.phase > 0;
            if (!phaseIncreased) {
                return;
            }

            this.world.forEach(
                [ut.Entity, game.RotationTweener, ut.Core2D.TransformLocalRotation],
                this.startTweenRotationInstance
            );
        }

        // Able to rotation beyond half a circle.
        //
        // Sets speed of tween rotation and resets tween time to 0.
        // Depends on TweenRotationSystem to update the rotation.
        startTweenRotation(entity:ut.Entity,
                rotationTweener:game.RotationTweener,
                transformRotation:ut.Core2D.TransformLocalRotation):void {
            if (rotationTweener.endRadians == 0) {
                return;
            }

            let startRotation = new Quaternion().setFromAxisAngle(new Vector3(0, 0, 1), 0);
            transformRotation.rotation = startRotation;
            let duration:number = rotationTweener.duration;
            if (duration <= this.minDuration) {
                duration = this.minDuration;
            }

            if (!this.world.hasComponent(entity, game.TweenRotation)) {
                this.world.addComponent(entity, game.TweenRotation);
            }
            let tweenRotation:game.TweenRotation = this.world.getComponentData(entity, game.TweenRotation);
            tweenRotation.speed = rotationTweener.endRadians / duration;
            tweenRotation.time = 0;
            this.world.setComponentData(entity, tweenRotation);
        }

        // Tween rotation, by shortest distance, never more than half circle.
        // To rotate nearly a full circle, nest one rotator inside another.
        // Copied from TinySamples DinosaurService.ts
        //
        // An alternative is rotating every frame, as invoked by `startTweenRotation`.
        tweenRotationShort(entity:ut.Entity, transformRotation:ut.Core2D.TransformLocalRotation,
                endRadians:number, duration:number):void {
            let startRotation = new Quaternion().setFromAxisAngle(new Vector3(0, 0, 1), 0);
            let endRotation = new Quaternion().setFromAxisAngle(new Vector3(0, 0, 1), endRadians);
            transformRotation.rotation = startRotation;

            let rotateTween = new ut.Tweens.TweenDesc;
            rotateTween.cid = ut.Core2D.TransformLocalRotation.cid;
            rotateTween.offset = 0;
            rotateTween.duration = duration;
            rotateTween.func = ut.Tweens.TweenFunc.Linear;
            rotateTween.loop = ut.Core2D.LoopMode.Once;
            rotateTween.destroyWhenDone = true;
            rotateTween.t = 0.0;

            ut.Tweens.TweenService.addTweenQuaternion(
                this.world,
                entity,
                startRotation,
                endRotation,
                rotateTween);
        }
    }
}
