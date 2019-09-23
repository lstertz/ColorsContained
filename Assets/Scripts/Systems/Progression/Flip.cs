using Components.Activity;
using Components.Attributes;
using Entities.Environment;
using System.Collections.Generic;
using Systems.Activity;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems.Progression
{
	/// <summary>
	/// Defines the Flip System, which swaps all previously empty grid spaces for a Block.
	/// </summary>
    [AlwaysUpdateSystem]
    public class Flip : ComponentSystem
    {
        private HashSet<Entity> destroyedEntities = new HashSet<Entity>();
        private List<float3> unactivatedPositions = new List<float3>();
        private HashSet<Entity> unactivatedTiles = new HashSet<Entity>();

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
                        unactivatedTiles.Add(entity);
                        unactivatedPositions.Add(pos);
                    }
                    else
                    {
                        if (!destroyedEntities.Contains(properties.Tileset))
                        {
                            PostUpdateCommands.AddComponent(properties.Tileset, new Destroy());
                            destroyedEntities.Add(properties.Tileset);
                        }
                    }
                }
            });

            Entity tileset = ActiveTileset.Create(unactivatedPositions.ToArray(), true);
            Entities.ForEach((Entity entity, ref TileProperties properties) =>
            {
                if (unactivatedTiles.Contains(entity))
                    properties.Tileset = tileset;
            });

            destroyedEntities.Clear();
            unactivatedPositions.Clear();
            unactivatedTiles.Clear();
        }
    }
}
