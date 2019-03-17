namespace game {
    // Copied from:
    // https://forum.unity.com/threads/help-how-to-disable-and-enable-the-entity.603145/
    export class GameService {
        static setEntityEnabled(world: ut.World, entity: ut.Entity, enabled: boolean):void {
            let hasDisabledComponent = world.hasComponent(entity, ut.Disabled);
            if (enabled && hasDisabledComponent) {
                world.removeComponent(entity, ut.Disabled);
            }
            else if (!enabled && !hasDisabledComponent) {
                world.addComponent(entity, ut.Disabled);
            }

            // What is a simpler way to hide the sprite and it's children when disabled?
            // The transform child of a disabled entity still appears.
            if (!world.hasComponent(entity, ut.Core2D.Sprite2DRenderer)) {
                return;
            }
            let sprite = world.getComponentData(entity, ut.Core2D.Sprite2DRenderer);
            if (sprite == null) {
                return;
            }
            sprite.color.a = enabled ? 1 : 0;
        }
    }
}
