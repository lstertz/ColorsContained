using Components.Activity;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Wrappers;


namespace Entities.Environment
{
    /// <summary>
    /// Defines the base Tile Entity Setup.
    /// </summary>
    public class Tile<T> : Entity<T> where T : Tile<T>
    {
        protected float emission;
        protected float3 position;

        /// <summary>
        /// Sets the Components of this Entity.
        /// </summary>
        /// <param name="wrapper">The EntityWrapper that allows the Components 
        /// to be set, agnostic to how the setting is performed.</param>
        protected override void SetComponents(EntityWrapper wrapper)
        {
            wrapper.SetComponent(new TileProperties
            {
                ActivationCount = 0,
                ActivatedColor = new Color(0.8745f, 0.5843f, 0.2784f, 1.0f),
                InvalidColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
                UnactivatedColor = new Color(1, 1, 1, 0.3f),
                HasTileToForward = false,
                HasTileBehind = false,
                HasTileToLeft = false,
                HasTileToRight = false,
                HasDrawnTileToForward = false,
                HasDrawnTileBehind = false,
                HasDrawnTileToLeft = false,
                HasDrawnTileToRight = false,
                IsBeingDrawn = false,
                IsInvalidDraw = false,
                Tileset = Entity.Null
            });

            wrapper.SetComponent(new Position
            {
                Value = Setup.position
            });
        }

        /// <summary>
        /// Provides the ComponentTypes of this Entity.
        /// </summary>
        /// <returns>An array of this Entities ComponentTypes.</returns>
        protected override ComponentType[] GetComponentTypes()
        {
            return new ComponentType[]
            {
                typeof(TileProperties),
                typeof(Position)
            };
        }
    }
    /// <summary>
    /// Defines the Tile Entity Setup.
    /// </summary>
    public class Tile : Tile<Tile>
    {
        /// <summary>
        /// Creates, with a Concurrent Command Buffer, a new Entity whose setup 
        /// is defined by this class.
        /// </summary>
        /// <param name="buffer">The Concurrent Command Buffer that will be used 
        /// to create the Entity.</param>
        /// <param name="position">The world position of the created Tile Entity.</param>
        /// <param name="jobIndex">The index of the current concurrent job.</param>
        public static void BufferedCreate(EntityCommandBuffer.Concurrent buffer, int jobIndex,
            float3 position = new float3(), float emission = Resources.BackgroundTileEmission)
        {
            Entity<Tile>.BufferedCreate(buffer, jobIndex);
        }

        /// <summary>
        /// Creates a new Entity whose setup is defined by this class.
        /// </summary>
        /// <param name="gameObject">The GameObject of this Tile Entity, 
        /// for animation and rendering.</param>
        /// <param name="position">The world position of the created Tile Entity.</param>
        /// <returns>The created Entity.</returns>
        public static Entity Create(float3 position = new float3(), 
            float emission = Resources.BackgroundTileEmission)
        {
            Setup.emission = emission;
            Setup.position = position;
            return Entity<Tile>.Create();
        }
    }
}
