namespace game {
    export class PhaseRotationTweenSystem extends ut.ComponentSystem {
        OnUpdate():void {
            let phaseConfig = this.world.getConfigData(game.PhaseConfig);
            let phaseIncreased:boolean = phaseConfig.changed && phaseConfig.phase > 0;
            if (!phaseIncreased) {
                return;
            }

            let duration:number = phaseConfig.minDuration;
            let endRadians:number = Math.PI * -0.999;
            this.world.forEach(
                [ut.Entity, game.RotationTweener, ut.Core2D.TransformLocalRotation],
                (entity, rotationTweener, transformRotation) => {
                    this.tweenRotationShort(entity, transformRotation, endRadians, duration);
                }
            );
        }

        // Tween rotation, by shortest distance, never more than half circle.
        // To rotate nearly a full circle, nest one rotator inside another.
        // Copied from TinySamples DinosaurService.ts
        tweenRotationShort(entity:ut.Entity, transformRotation:ut.Core2D.TransformLocalRotation,
                endRadians:number, duration:number):void
        {
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
