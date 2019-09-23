using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Utilities.Wrappers;


namespace Entities
{
	/// <summary>
	/// Manages any mappings of Entities to other constructs.
	/// </summary>
	public static class EntityMapping
	{
		private static Dictionary<Entity, GameObject> entityToGameObjects = 
			new Dictionary<Entity, GameObject>();


		/// <summary>
		/// Provides the GameObject associated with the specified Entity.
		/// </summary>
		/// <param name="entity">The Entity whose associated GameObject is being retrieved.</param>
		/// <returns>The specified Entity's associated GameObject, or null if it is not 
		/// associated with a GameObject.</returns>
		public static GameObject GetGameObject<T>(this T gameObjectComponent, Entity entity)
			where T : Components.Supports.IGameObject
		{
			if (entityToGameObjects.ContainsKey(entity))
				return entityToGameObjects[entity];
			return null;
		}

		/// <summary>
		/// Sets the GameObject to be associated with the specified Entity.
		/// </summary>
		/// <typeparam name="T">The type of the Entity setup whose Entity's 
		/// associated GameObject is being set.</typeparam>
		/// <param name="entitySetup">This Entity setup, whose Entity's associated GameObject 
		/// is being set.</param>
		/// <param name="entity">The Entity being associated with the GameObject.</param>
		/// <param name="gameObject">The GameObject being associated 
		/// with the specified Entity.</param>
		public static void SetGameObject<T>(this T entitySetup, Entity entity, 
			GameObject gameObject)
			where T : Entity<T>
		{
			if (entityToGameObjects.ContainsKey(entity))
				entityToGameObjects[entity] = gameObject;
			else
				entityToGameObjects.Add(entity, gameObject);
		}
	}


	/// <summary>
	/// Defines the shared attributes of all Entities.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// The EntityArchetype that represents an Entity.
		/// </summary>
		EntityArchetype Archetype { get; }
	}


	/// <summary>
	/// Defines an Entity setup, with specific Components and settings to be 
	/// specified in sub-classes implemented as MonoBehaviours on GameObjects.
	/// </summary>
	/// <typeparam name="T">The Type of Entity setup (the sub-class).</typeparam>
	public abstract class Entity<T> : MonoBehaviour, IEntity where T : Entity<T>
	{
		public static EntityArchetype Archetype;
		protected static T Setup = null;


		/// <summary>
		/// Creates, with a Concurrent Command Buffer, a new Entity whose setup 
		/// is defined by this class.
		/// </summary>
		/// <param name="buffer">The Concurrent Command Buffer that will be used 
		/// to create the Entity.</param>
		/// <param name="jobIndex">The index of the current concurrent job.</param>
		public static void BufferedCreate(EntityCommandBuffer.Concurrent buffer, int jobIndex)
		{
			buffer.CreateEntity(jobIndex, Archetype);
			Setup.SetComponents(new EntityWrapper(buffer, jobIndex));
		}

		/// <summary>
		/// Creates a new Entity whose setup is defined by this class.
		/// </summary>
		/// <returns>The created Entity.</returns>
		public static Entity Create(GameObject setup = null)
		{
			EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
			Entity e = manager.CreateEntity(Archetype);

			if (setup == null)
				Setup.SetComponents(new EntityWrapper(e));
			else
			{
				T t = setup.GetComponent<T>();
				if (t == null)
				{
					setup.AddComponent<T>();
					t = setup.GetComponent<T>();
				}

				Setup.CopyTo(t);
				t.SetComponents(new EntityWrapper(e));
				t.SetGameObject(e, setup);
			}

			return e;
		}


		EntityArchetype IEntity.Archetype { get { return Archetype; } }

		/// <summary>
		/// Initializes this Entity.
		/// </summary>
		private void Awake()
		{
			if (Setup != null)
				return;

			EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
			Archetype = manager.CreateArchetype(GetComponentTypes());

			RegisterArchetypes();
			Setup = this as T;
		}

		/// <summary>
		/// Sets the Components of this Entity.
		/// </summary>
		/// <param name="wrapper">The EntityWrapper that allows the Components 
		/// to be set, agnostic to how the setting is performed.</param>
		protected abstract void SetComponents(EntityWrapper wrapper);

		/// <summary>
		/// Provides the ComponentTypes of this Entity.
		/// </summary>
		/// <returns>An array of this Entities ComponentTypes.</returns>
		protected abstract ComponentType[] GetComponentTypes();

		/// <summary>
		/// Copies this Entity's class-specific settings to the provided Entity.
		/// </summary>
		/// <param name="copiedTo">The Entity being copied to.</param>
		protected virtual void CopyTo(T copiedTo) { }

		/// <summary>
		/// Registers this Entity for EntityArchetype-based creation.
		/// </summary>
		private void RegisterArchetypes()
		{
			EntityArchetypeExtensions.RegisterArchetypeBufferedCreate(Archetype, BufferedCreate);
			EntityArchetypeExtensions.RegisterArchetypeCreate(Archetype, 
				() => { return Create(); });
		}
	}
}
