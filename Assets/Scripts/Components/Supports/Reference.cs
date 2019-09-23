using Unity.Entities;


namespace Components.Supports
{
	/// <summary>
	/// Defines a Reference Component, which is a Component that represents a reference 
	/// to a Component (of type T) on the ReferencedEntity.
	/// </summary>
	/// <typeparam name="T">The Type of Component being referenced.</typeparam>
	public struct Reference<T> : IComponentData where T : struct, IComponentData
	{
        /// <summary>
        /// Specifies whether the reference (referenced Entity) exists.
        /// This property is not safe for use with parallel jobs.
        /// </summary>
        public bool Exists
        {
            get
            {
                return World.Active.GetOrCreateManager<EntityManager>().Exists(ReferencedEntity);
            }
        }

		/// <summary>
		/// The referenced Entity.
		/// </summary>
		public Entity ReferencedEntity { get; set; }

        
        /// <summary>
        /// Provides the referenced Component.
        /// This method is not safe for use with parallel jobs.
        /// </summary>
        /// <returns>The referenced Component.</returns>
        public T GetReferenced()
		{
			EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();

			if (manager.HasComponent<T>(ReferencedEntity))
				return manager.GetComponentData<T>(ReferencedEntity);
			return default;
		}

		/// <summary>
		/// Provides the referenced Component.
		/// This method is safe for use with parallel jobs.
		/// </summary>
		/// <param name="fromEntity">The ComponentDataFromEntity required to access the 
		/// referenced Component during parallel job execution.</param>
		/// <returns>The referenced Component.</returns>
		public T GetReferenced(ComponentDataFromEntity<T> fromEntity)
		{
			if (fromEntity.Exists(ReferencedEntity))
				return fromEntity[ReferencedEntity];
			return default;
		}
	}
}
