namespace game {
    export class PhaseEnablerSystem extends ut.ComponentSystem {
        OnUpdate():void {
            let phaseConfig = this.world.getConfigData(game.PhaseConfig);
            if (!phaseConfig.changed) {
                return;
            }

            let phase:number = phaseConfig.phase;
            this.world.forEach(
                [ut.Entity, game.PhaseEnabler],
                (entity, phaseEnabler) => {
                    let enabled:boolean = phase == phaseEnabler.enabledPhase;
                    let changed:boolean = game.GameService.setEntityEnabled(this.world, entity, enabled);
                }
            );

            // Disabled are hidden from queries.
            this.world.forEach(
                [ut.Entity, game.PhaseEnabler, ut.Disabled],
                (entity, phaseEnabler, disabled) => {
                    let enabled:boolean = phase == phaseEnabler.enabledPhase;
                    let changed:boolean = game.GameService.setEntityEnabled(this.world, entity, enabled);
                    let willAudioStart:boolean = this.world.hasComponent(entity, game.OnEntityEnableAudioSourceStart);
                    if (!enabled) {
                        return;
                    }
                    if (!willAudioStart) {
                        return;
                    }
                    AudioSourceUtils.start(this.world, entity);
                }
            );
        }
    }
}

