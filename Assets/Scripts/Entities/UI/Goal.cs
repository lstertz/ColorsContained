using Components.Activity;
using Components.Goal;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Wrappers;


namespace Entities.Environment
{
    /// <summary>
    /// Defines the Goal Entity Setup.
    /// </summary>
    public class Goal : Entity<Goal>
    {
        /// <summary>
        /// Creates, with a Concurrent Command Buffer, a new Entity whose setup 
        /// is defined by this class.
        /// </summary>
        /// <param name="buffer">The Concurrent Command Buffer that will be used 
        /// to create the Entity.</param>
        /// <param name="position">The world position of the created GridCube Entity.</param>
        /// <param name="jobIndex">The index of the current concurrent job.</param>
        public static void BufferedCreate(EntityCommandBuffer.Concurrent buffer, int jobIndex, 
            float3 position = new float3())
        {
            Entity<Goal>.BufferedCreate(buffer, jobIndex);
        }

        /// <summary>
        /// Creates a new Entity whose setup is defined by this class.
        /// </summary>
        /// <param name="gameObject">The GameObject of this GridCube Entity, 
        /// for animation and rendering.</param>
        /// <param name="position">The world position of the created GridCube Entity.</param>
        /// <returns>The created Entity.</returns>
        public static Entity Create(int requirement, Color type, Color background, 
            bool isBlockGoal = false)
        {
            Setup.background = background;
            Setup.requirement = requirement;
            Setup.type = type;

            GameObject go;
            if (isBlockGoal)
                go = GameObject.Instantiate(Setup.BlockGoalUI);
            else
                go = GameObject.Instantiate(Setup.TileGoalUI);

            go.transform.SetParent(Setup.Parent, false);
            return Create(go);
        }

        public Transform Parent;

        public GameObject TileGoalUI;
        public GameObject BlockGoalUI;

        private Color background;
        private int requirement;
        private Color type;


        /// <summary>
        /// Copies this Entity's class-specific settings to the provided Entity.
        /// </summary>
        /// <param name="copiedTo">The Entity being copied to.</param>
        protected override void CopyTo(Goal copiedTo)
        {
            copiedTo.background = background;
            copiedTo.requirement = requirement;
            copiedTo.type = type;
        }

        /// <summary>
        /// Sets the Components of this Entity.
        /// </summary>
        /// <param name="wrapper">The EntityWrapper that allows the Components 
        /// to be set, agnostic to how the setting is performed.</param>
        protected override void SetComponents(EntityWrapper wrapper)
        {
            wrapper.SetComponent(new Components.Supports.GameObject
            {
                Entity = wrapper.Entity
            });

            wrapper.SetComponent(new Fulfillment
            {
                Value = 2
            });

            wrapper.SetComponent(new Requirement
            {
                Background = background,
                Type = type,
                Value = requirement
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
                typeof(Components.Supports.GameObject),
                typeof(Fulfillment),
                typeof(Requirement)
            };
        }
    }
}
