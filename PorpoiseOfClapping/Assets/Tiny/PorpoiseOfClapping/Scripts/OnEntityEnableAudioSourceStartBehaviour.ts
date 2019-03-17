namespace game {

    export class OnEntityEnableAudioSourceStartFilter extends ut.EntityFilter {
        entity: ut.Entity;
        start: game.OnEntityEnableAudioSourceStart;
    }

    export class OnEntityEnableAudioSourceStartBehaviour extends ut.ComponentBehaviour {

        data: OnEntityEnableAudioSourceStartFilter;

        // TODO: Callback when disabled and reenabled second time.
        OnEntityEnable(): void {
            console.log("OnEntityEnableAudioSourceStartBehaviour.OnEntityEnable");
            // AudioSourceUtils.start(this.world, this.data.entity);
        }

        OnEntityDisable(): void {
            let didStart:boolean = this.world.hasComponent(this.data.entity,
                ut.Audio.AudioSourceStart);
            console.log("OnEntityEnableAudioSourceStartBehaviour.OnEntityDisable: didStart=" + didStart);
            if (didStart) {
                this.world.removeComponent(this.data.entity,
                    ut.Audio.AudioSourceStart);
            }
        }
    }
}
