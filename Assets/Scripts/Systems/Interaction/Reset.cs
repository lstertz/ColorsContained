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
    /// Defines the Reset System, which destroys all tilesets and resets all tiles.
    /// </summary>
    [AlwaysUpdateSystem]
    public class Reset : ComponentSystem
	{
        private bool resetActivationCount = true;
        private HashSet<Entity> destroyedEntities = new HashSet<Entity>();

        protected override void OnCreateManager()
        {
            Enabled = false;
        }

        public void Update(bool resetActivationCount)
        {
            this.resetActivationCount = resetActivationCount;
            {
                Update();
            }
            this.resetActivationCount = true;
        }

        [BurstCompile]
		protected override void OnUpdate()
		{
            destroyedEntities.Clear();
            
            Entities.ForEach((Entity entity, ref TileProperties properties, 
                ref Position position) =>
            {
                if (properties.Tileset != Entity.Null &&
                    !destroyedEntities.Contains(properties.Tileset))
                {
                    PostUpdateCommands.AddComponent(properties.Tileset, new Destroy());
                    destroyedEntities.Add(properties.Tileset);
                }

                if (resetActivationCount)
                    properties.ActivationCount = 0;
            });
        }
    }
}
