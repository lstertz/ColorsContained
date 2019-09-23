using System;
using System.Collections.Generic;

using ECB = Unity.Entities.EntityCommandBuffer;


namespace Unity.Entities
{
	/// <summary>
	/// Defines extensions of the EntityArchetype struct.
	/// </summary>
	public static class EntityArchetypeExtensions
	{
		private static Dictionary<EntityArchetype, Func<Entity>> creates =
			new Dictionary<EntityArchetype, Func<Entity>>();
		private static Dictionary<EntityArchetype, Action<ECB.Concurrent, int>> bufferedCreates =
			new Dictionary<EntityArchetype, Action<ECB.Concurrent, int>>();


		/// <summary>
		/// Registers an EntityArchetype with an Action to create an Entity of that archetype, 
		/// with a Concurrent Command Buffer.
		/// </summary>
		/// <param name="archetype">The EntityArchetype whose Entity can be 
		/// created by the Func.</param>
		/// <param name="bufferedCreate">The Action that would create the Entity.</param>
		public static void RegisterArchetypeBufferedCreate(EntityArchetype archetype,
			Action<ECB.Concurrent, int> bufferedCreate)
		{
			if (!bufferedCreates.ContainsKey(archetype))
				bufferedCreates.Add(archetype, bufferedCreate);
		}

		/// <summary>
		/// Registers an EntityArchetype with a Func to create an Entity of that archetype.
		/// </summary>
		/// <param name="archetype">The EntityArchetype whose Entity can be 
		/// created by the Func.</param>
		/// <param name="create">The Func that would create the Entity.</param>
		public static void RegisterArchetypeCreate(EntityArchetype archetype, Func<Entity> create)
		{
			if (!creates.ContainsKey(archetype))
				creates.Add(archetype, create);
		}


		/// <summary>
		/// Creates, with a Concurrent Command Buffer, an Entity registered as being defined by 
		/// this EntityArchetype.
		/// </summary>
		/// <param name="archetype">This EntityArchetype, whose Entity is to be created.</param>
		/// <param name="buffer">The Concurrent Command Buffer that will be used 
		/// to create the Entity.</param>
		/// <param name="jobIndex">The index of the current concurrent job.</param>
		public static void BufferedCreate(this EntityArchetype archetype, ECB.Concurrent buffer, 
			int jobIndex)
		{
			if (bufferedCreates.ContainsKey(archetype))
				bufferedCreates[archetype](buffer, jobIndex);
		}

		/// <summary>
		/// Creates an Entity registered as being defined by this EntityArchetype.
		/// </summary>
		/// <param name="archetype">This EntityArchetype, whose Entity is to be created.</param>
		/// <returns>The created Entity, or Entity.Null if 
		/// no such Entity could be created.</returns>
		public static Entity Create(this EntityArchetype archetype)
		{
			if (creates.ContainsKey(archetype))
				return creates[archetype]();
			return Entity.Null;
		}
	}
}
