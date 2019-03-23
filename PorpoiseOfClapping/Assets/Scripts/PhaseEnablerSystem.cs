using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public class PhaseEnablerSystem : JobComponentSystem
    {
        private ComponentGroup m_PhaseConfigGroup;

        protected override void OnCreateManager()
        {
            m_PhaseConfigGroup = GetComponentGroup(typeof(PhaseConfig));
        }

        private bool TryGetFirstPhaseConfig(ref PhaseConfig phaseConfig)
        {
            NativeArray<PhaseConfig> phaseConfigs = m_PhaseConfigGroup.ToComponentDataArray<PhaseConfig>(Allocator.TempJob);
            if (phaseConfigs.Length < 1)
            {
                phaseConfigs.Dispose();
                return false;
            }

            phaseConfig = phaseConfigs[0];

            phaseConfigs.Dispose();
            return true;
        }

        [BurstCompile]
        struct PhaseEnablerJob : IJobProcessComponentData<PhaseEnabler>
        {
            [ReadOnly] public PhaseConfig phaseConfig;

            public void Execute(ref PhaseEnabler phaseEnabler)
            {
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            PhaseConfig phaseConfig = default(PhaseConfig);
            if (!TryGetFirstPhaseConfig(ref phaseConfig))
                return default(JobHandle);

            var job = new PhaseEnablerJob()
            {
                phaseConfig = phaseConfig
            };

            return job.Schedule(this, inputDependencies);
        }

    }
}
/*
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
 */
