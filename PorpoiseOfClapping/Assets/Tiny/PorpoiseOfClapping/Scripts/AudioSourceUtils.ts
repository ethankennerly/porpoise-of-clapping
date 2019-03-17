namespace game {

    export class AudioSourceUtils {

        static start(world:ut.World, entity:ut.Entity): void {
            let hadStart:boolean = world.hasComponent(entity,
                ut.Audio.AudioSourceStart);
            if (hadStart) {
                world.removeComponent(entity, ut.Audio.AudioSourceStart);
            }
            world.addComponent(entity, ut.Audio.AudioSourceStart);
        }
    }
}
