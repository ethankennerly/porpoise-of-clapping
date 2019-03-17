namespace game {
    export class TapEnablerSystem extends ut.ComponentSystem {
        OnUpdate():void {
            let mouseDown:boolean = ut.Runtime.Input.getMouseButtonDown(0);
            if (!mouseDown) {
                return;
            }

            this.world.forEach(
                [ut.Entity, game.TapEnabler],
                (entity, tapEnabler) => {
                    game.GameService.setEntityEnabled(this.world, entity, tapEnabler.setEnabledTo);
                }
            );
            // Disabled are hidden from queries.
            this.world.forEach(
                [ut.Entity, game.TapEnabler, ut.Disabled],
                (entity, tapEnabler, disabled) => {
                    game.GameService.setEntityEnabled(this.world, entity, tapEnabler.setEnabledTo);
                }
            );
        }
    }
}

