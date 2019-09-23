using Components.Supports;
using Unity.Entities;


namespace Utilities.Wrappers
{
	/// <summary>
	/// Defines a wrapper for performing Entity actions without being concerned with 
	/// what performs the actions.
	/// </summary>
	public partial struct EntityWrapper
	{
		public Entity Entity { get; private set; }

		private EntityCommandBuffer.Concurrent buffer { get; set; }
		private int jobIndex { get; set; }
		private EntityManager manager;

		private bool hasEntity
		{
			get { return Entity != Entity.Null; }
		}


		/// <summary>
		/// Sets up this EntityWrapper for a CommandBuffer without an Entity.
		/// </summary>
		/// <param name="buffer">The Concurrent CommandBuffer of this EntityWrapper.</param>
		/// <param name="jobIndex">The index of the current concurrent job.</param>
		public EntityWrapper(EntityCommandBuffer.Concurrent buffer, int jobIndex)
		{
			this.buffer = buffer;
			this.jobIndex = jobIndex;

			manager = null;
			Entity = Entity.Null;
		}

		/// <summary>
		/// Sets up this EntityWrapper for a CommandBuffer with an Entity.
		/// </summary>
		/// <param name="buffer">The Concurrent CommandBuffer of this EntityWrapper.</param>
		/// <param name="entity">The Entity of this EntityWrapper.</param>
		/// <param name="jobIndex">The index of the current concurrent job.</param>
		public EntityWrapper(EntityCommandBuffer.Concurrent buffer, Entity entity, int jobIndex)
		{
			manager = null;
			this.jobIndex = jobIndex;

			this.Entity = entity;
			this.buffer = buffer;
		}

		/// <summary>
		/// Sets up this EntityWrapper for an Entity, with actions to be performed 
		/// through the EntityManager.
		/// </summary>
		/// <param name="entity">The Entity of this EntityWrapper.</param>
		public EntityWrapper(Entity entity)
		{
			manager = World.Active.GetOrCreateManager<EntityManager>();
			buffer = new EntityCommandBuffer.Concurrent();
			this.jobIndex = -1;

			this.Entity = entity;
		}


		/// <summary>
		/// Adds the provided Component to the current Entity.
		/// </summary>
		/// <typeparam name="T">The Type of Component being added.</typeparam>
		/// <param name="component">The Component being added.</param>
		public void AddComponent<T>(T component)
			where T : struct, IComponentData
		{
			if (manager != null)
				manager.AddComponentData(Entity, component);
			else if (hasEntity)
				buffer.AddComponent(jobIndex, Entity, component);
			//else
			//	buffer.AddComponent(jobIndex, component);
		}

		/// <summary>
		/// Adds a Reference Component for the specified Component to the current Entity.
		/// </summary>
		/// <typeparam name="T">The Type of Component being referenced.</typeparam>
		/// <param name="referencedEntity">The Entity whose Component is being referenced.</param>
		public void AddReferenceComponent<T>(Entity referencedEntity)
			where T : struct, IComponentData
		{
			Reference<T> component = new Reference<T>
			{
				ReferencedEntity = referencedEntity
			};

			AddComponent(component);
		}

		/// <summary>
		/// Adds the provided shared Component to the current Entity.
		/// </summary>
		/// <typeparam name="T">The Type of shared Component being added.</typeparam>
		/// <param name="component">The shared Component being added.</param>
		public void AddSharedComponent<T>(T component)
			where T : struct, ISharedComponentData
		{
			if (manager != null)
				manager.AddSharedComponentData(Entity, component);
			else if (hasEntity)
				buffer.AddSharedComponent(jobIndex, Entity, component);
			//else
			//	buffer.AddSharedComponent(jobIndex, component);
		}


		/// <summary>
		/// Removes the Component of the specified Type.
		/// Does not remove a Component if the action is for a CommandBuffer when 
		/// the Entity has not been specified.
		/// </summary>
		/// <typeparam name="T">The Type of the Component to be removed.</typeparam>
		public void RemoveComponent<T>()
		{
			if (manager != null)
				manager.RemoveComponent<T>(Entity);
			else if (hasEntity)
				buffer.RemoveComponent<T>(jobIndex, Entity);
		}

		/// <summary>
		/// Removes the Component of the specified Type.
		/// Does not remove a Component if the action is for a CommandBuffer.
		/// </summary>
		/// <param name="type">The Type of the Component to be removed.</param>
		public void RemoveComponent(ComponentType type)
		{
			if (manager != null)
				manager.RemoveComponent(Entity, type);
		}


		/// <summary>
		/// Sets the provided Component for the current Entity.
		/// </summary>
		/// <typeparam name="T">The Type of Component being set.</typeparam>
		/// <param name="component">The Component being set.</param>
		public void SetComponent<T>(T component)
			where T : struct, IComponentData
		{
			if (manager != null)
				manager.SetComponentData(Entity, component);
			else if (hasEntity)
				buffer.SetComponent(jobIndex, Entity, component);
			//else
			//	buffer.SetComponent(jobIndex, component);
		}

		/// <summary>
		/// Set the Reference Component for the specified Component to the current Entity.
		/// </summary>
		/// <typeparam name="T">The Type of Component being referenced.</typeparam>
		/// <param name="referencedEntity">The Entity whose Component is being referenced.</param>
		public void SetReferenceComponent<T>(Entity referencedEntity)
			where T : struct, IComponentData
		{
			Reference<T> component = new Reference<T>
			{
				ReferencedEntity = referencedEntity
			};

			SetComponent(component);
		}

		/// <summary>
		/// Sets the provided shared Component for the current Entity.
		/// </summary>
		/// <typeparam name="T">The Type of shared Component being set.</typeparam>
		/// <param name="component">The shared Component being set.</param>
		public void SetSharedComponent<T>(T component)
			where T : struct, ISharedComponentData
		{
			if (manager != null)
				manager.SetSharedComponentData(Entity, component);
			else if (hasEntity)
				buffer.SetSharedComponent(jobIndex, Entity, component);
			//else
			//	buffer.SetSharedComponent(jobIndex, component);
		}
	}
}
