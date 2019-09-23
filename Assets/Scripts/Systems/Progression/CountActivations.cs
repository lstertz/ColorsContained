using Components.Activity;
using Unity.Burst;
using Unity.Entities;

namespace Systems.Progression
{
    /// <summary>
    /// Defines the Count Activations System, which increases the activation counts 
    /// for all activated Tiles.
    [AlwaysUpdateSystem]
    public class CountActivations : ComponentSystem
    {
        protected override void OnCreateManager()
        {
            Enabled = false;
        }

        [BurstCompile]
		protected override void OnUpdate()
        {
            Entities.ForEach((ref TileProperties properties, ref Position position) =>
            {
                if (properties.Tileset != Entity.Null)
                    properties.ActivationCount++;
            });
        }
    }
}
