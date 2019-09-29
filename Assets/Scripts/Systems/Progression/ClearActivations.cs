using Components.Activity;
using Systems.Activity;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems.Progression
{
	/// <summary>
	/// Defines the Clear Activiations System, which clears the activation count 
    /// of all clear tiles within the user's containing tile space.
	/// </summary>
    [AlwaysUpdateSystem]
    public class ClearActivations : ComponentSystem
    {
        protected override void OnCreateManager()
        {
            Enabled = false;
        }

        [BurstCompile]
		protected override void OnUpdate()
        {
            float minX = DefineGridDimensions.NegativeX;
            float minY = DefineGridDimensions.NegativeY;
            float minZ = DefineGridDimensions.NegativeZ;

            float maxX = DefineGridDimensions.PositiveX;
            float maxY = DefineGridDimensions.PositiveY;
            float maxZ = DefineGridDimensions.PositiveZ;

            Entities.ForEach((Entity entity, ref TileProperties properties,
                ref Position position) =>
            {
                float3 pos = position.Value;

                if (minX <= pos.x && pos.x < maxX && minY <= pos.y && pos.y < maxY && 
                    minZ <= pos.z && pos.z < maxZ)
                {
                    if (properties.Tileset == Entity.Null)
                    {
                        properties.ActivationCount = 0;
                        // TODO :: Initiate an animation to show monsters being destroyed.
                        // TODO :: Account for different colors of activation.
                    }
                }
            });
        }
    }
}
