namespace game {
    // Copied from:
    // https://forum.unity.com/threads/help-how-to-disable-and-enable-the-entity.603145/
    // returns if changed.
    export class GameService {
        static setEntityEnabled(world: ut.World, entity: ut.Entity, enabled: boolean):boolean {
            let hasDisabledComponent = world.hasComponent(entity, ut.Disabled);
            let changed = enabled != hasDisabledComponent;
            if (enabled && hasDisabledComponent) {
                world.removeComponent(entity, ut.Disabled);
            }
            else if (!enabled && !hasDisabledComponent) {
                world.addComponent(entity, ut.Disabled);
            }

            // What is a simpler way to hide the sprite and it's children when disabled?
            // The transform child of a disabled entity still appears.
            if (!world.hasComponent(entity, ut.Core2D.Sprite2DRenderer)) {
                return changed;
            }
            let sprite = world.getComponentData(entity, ut.Core2D.Sprite2DRenderer);
            if (sprite == null) {
                return changed;
            }
            sprite.color.a = enabled ? 1 : 0;
            return changed;
        }
    }
}
