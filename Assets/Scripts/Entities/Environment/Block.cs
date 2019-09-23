using Components.Activity;
using Components.Attributes;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Wrappers;


namespace Entities.Environment
{
    /// <summary>
    /// Defines the Block Entity Setup.
    /// </summary>
    public class Block : Entity<Block>
    {
        /// <summary>
        /// Creates, with a Concurrent Command Buffer, a new Entity whose setup 
        /// is defined by this class.
        /// </summary>
        /// <param name="buffer">The Concurrent Command Buffer that will be used 
        /// to create the Entity.</param>
        /// <param name="position">The world position of the created Block Entity.</param>
        /// <param name="jobIndex">The index of the current concurrent job.</param>
        public static void BufferedCreate(EntityCommandBuffer.Concurrent buffer, int jobIndex, 
            float3 position = new float3())
        {
            Entity<Block>.BufferedCreate(buffer, jobIndex);
        }

        /// <summary>
        /// Creates a new Entity whose setup is defined by this class.
        /// </summary>
        /// <param name="position">The world position of the created Block Entity.</param>
        /// <param name="color">The color of the created Block Entity.</param>
        /// <returns>The created Entity.</returns>
        public static Entity Create(float3 position, Color color)
        {
            Setup.color = color;
            Setup.position = position;

            return Create();
        }


        private Color color;
        private float3 position;


        /// <summary>
        /// Sets the Components of this Entity.
        /// </summary>
        /// <param name="wrapper">The EntityWrapper that allows the Components 
        /// to be set, agnostic to how the setting is performed.</param>
        protected override void SetComponents(EntityWrapper wrapper)
        {
            wrapper.SetComponent(new BlockProperties
            {
                Color = color
            });

            wrapper.SetComponent(new Position
            {
                Value = Setup.position
            });
        }

        /// <summary>
        /// Provides the ComponentTypes of thie Entity.
        /// </summary>
        /// <returns>An array of this Entities ComponentTypes.</returns>
        protected override ComponentType[] GetComponentTypes()
        {
            return new ComponentType[]
            {
                typeof(BlockProperties),
                typeof(Position)
            };
        }
    }
}
