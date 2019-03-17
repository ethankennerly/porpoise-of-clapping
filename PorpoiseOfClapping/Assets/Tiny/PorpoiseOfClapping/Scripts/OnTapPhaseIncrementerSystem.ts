namespace game {
    export class OnTapPhaseIncrementerSystem extends ut.ComponentSystem {
        OnUpdate():void {
            let phaseConfig = this.world.getConfigData(game.PhaseConfig);
            let deltaTime:number = this.scheduler.deltaTime();
            if (phaseConfig.changed) {
                phaseConfig.changed = false;
            }
            if (phaseConfig.timeElapsed == 0 && phaseConfig.phase == 0) {
                phaseConfig.changed = true;
            }
            phaseConfig.timeElapsed += deltaTime;

            let mouseDown:boolean = ut.Runtime.Input.getMouseButtonDown(0);
            if (!mouseDown) {
                this.world.setConfigData(phaseConfig);
                return;
            }

            if (phaseConfig.phase > 0 && phaseConfig.timeElapsed < phaseConfig.minDuration) {
                this.world.setConfigData(phaseConfig);
                return;
            }

            phaseConfig.phase++;
            if (phaseConfig.phase > phaseConfig.maxPhase)
            {
                phaseConfig.phase = 0;
            }
            phaseConfig.timeElapsed = 0;
            phaseConfig.changed = true;
            this.world.setConfigData(phaseConfig);
        }
    }
}

