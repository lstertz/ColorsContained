using Components.Activity;
using Components.Attributes;
using Components.Supports;
using Entities.Environment;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the Draw System, which interprets user input as commands to 
    /// draw Blocks in the play area.
	/// </summary>
    [AlwaysUpdateSystem]
    public class Draw : ComponentSystem
	{
        public bool FinalizeTileset = false;

        private HashSet<Entity> destroyedEntities = new HashSet<Entity>();

        protected override void OnCreateManager()
        {
            Enabled = false;
        }

        [BurstCompile]
		protected override void OnUpdate()
        {
            ComponentDataFromEntity<Indestructible> indestructibles =
                GetComponentDataFromEntity<Indestructible>(true);
            bool drawnOverIndestructible = false;

            if (!FinalizeTileset)
            {
                Entities.ForEach((Entity entity, ref TileProperties properties,
                    ref Position position) =>
                {
                    if (Input.DrawLocations.Contains(position.Value))
                    {
                        properties.IsBeingDrawn = true;
                        properties.HasDrawnTileToForward =
                            Input.DrawLocations.Contains(ForwardOf(position.Value));
                        properties.HasDrawnTileBehind =
                            Input.DrawLocations.Contains(Behind(position.Value));
                        properties.HasDrawnTileToLeft =
                            Input.DrawLocations.Contains(LeftOf(position.Value));
                        properties.HasDrawnTileToRight =
                            Input.DrawLocations.Contains(RightOf(position.Value));

                        if (indestructibles.Exists(properties.Tileset))
                            drawnOverIndestructible = true;
                    }
                    else
                    {
                        properties.IsBeingDrawn = false;
                    }
                });

                Entities.ForEach((Entity entity, ref TileProperties properties,
                    ref Position position) =>
                {
                    properties.IsInvalidDraw = false;

                    if (Input.DrawLocations.Contains(position.Value))
                    {
                        if (drawnOverIndestructible)
                            properties.IsInvalidDraw = true;
                    }
                });

                return;
            }

            Entities.ForEach((Entity entity, ref TileProperties properties,
                ref Position position) =>
            {
                if (Input.DrawLocations.Contains(position.Value))
                    if (properties.Tileset != Entity.Null &&
                        indestructibles.Exists(properties.Tileset))
                        drawnOverIndestructible = true;
            });

            if (drawnOverIndestructible)
            {
                Entities.ForEach((Entity entity, ref TileProperties properties,
                    ref Position position) =>
                {
                    if (Input.DrawLocations.Contains(position.Value))
                    {
                        properties.IsBeingDrawn = false;
                        properties.IsInvalidDraw = false;
                    }
                });

                return;
            }

            destroyedEntities.Clear();

            Entity tileset = ActiveTileset.Create(Input.DrawLocations.ToArray());
            Entities.ForEach((Entity entity, ref TileProperties properties, 
                ref Position position) =>
            {
                if (Input.DrawLocations.Contains(position.Value))
                {
                    if (properties.Tileset != Entity.Null &&
                        !destroyedEntities.Contains(properties.Tileset))
                    {
                        PostUpdateCommands.AddComponent(properties.Tileset, new Destroy());
                        destroyedEntities.Add(properties.Tileset);
                    }

                    properties.HasTileToForward = 
                        Input.DrawLocations.Contains(ForwardOf(position.Value));
                    properties.HasTileBehind = 
                        Input.DrawLocations.Contains(Behind(position.Value));
                    properties.HasTileToLeft = 
                        Input.DrawLocations.Contains(LeftOf(position.Value));
                    properties.HasTileToRight = 
                        Input.DrawLocations.Contains(RightOf(position.Value));

                    properties.Tileset = tileset;
                }

                properties.IsBeingDrawn = false;
            });
        }

        private float3 ForwardOf(float3 pos)
        {
            return new float3(pos.x, pos.y, pos.z + 1);
        }

        private float3 Behind(float3 pos)
        {
            return new float3(pos.x, pos.y, pos.z - 1);
        }

        private float3 LeftOf(float3 pos)
        {
            return new float3(pos.x - 1, pos.y, pos.z);
        }

        private float3 RightOf(float3 pos)
        {
            return new float3(pos.x + 1, pos.y, pos.z);
        }
    }
}
