using Unity.Entities;


namespace Utilities.Wrappers
{
	/// <summary>
	/// Defines a wrapper for performing Entity actions without being concerned with 
	/// what performs the actions.
	/// </summary>
	public partial struct EntityWrapper
	{
		/// <summary>
		/// Defines the EntityWrapper ForComponent Interface, for specifying Component-typed 
		/// methods for interacting with the EntityWrapper.
		/// </summary>
		public interface IForComponent
		{
			void AddComponent(IComponentData component);
			void AddReferenceComponent(Entity referencedEntity);
			void RemoveComponent();
			void SetComponent(IComponentData component);
			void SetReferenceComponent(Entity referencedEntity);
		}

		/// <summary>
		/// Defines the EntityWrapper ForComponent, for providing Component-typed 
		/// methods for interacting with the EntityWrapper.
		/// </summary>
		public struct ForComponent<T> : IForComponent where T : struct, IComponentData
		{
			/// <summary>
			/// The EntityWrapper wrapped by this wrapper.
			/// </summary>
			public EntityWrapper Wrapper { private get; set; }


			/// <summary>
			/// Adds the provided Component to the current Entity.
			/// </summary>
			/// <param name="component">The Component being added.</param>
			public void AddComponent(IComponentData component)
			{
				Wrapper.AddComponent((T)component);
			}

			/// <summary>
			/// Adds a Reference Component for the specified Component to the current Entity.
			/// </summary>
			/// <param name="referencedEntity">The Entity whose Component is 
			/// being referenced.</param>
			public void AddReferenceComponent(Entity referencedEntity)
			{
				Wrapper.AddReferenceComponent<T>(referencedEntity);
			}

			/// <summary>
			/// Removes the Component of the specified Type.
			/// Does not remove a Component if the action is for a CommandBuffer when 
			/// the Entity has not been specified.
			/// </summary>
			public void RemoveComponent()
			{
				Wrapper.RemoveComponent<T>();
			}

			/// <summary>
			/// Sets the provided Component for the current Entity.
			/// </summary>
			/// <param name="component">The Component being set.</param>
			public void SetComponent(IComponentData component)
			{
				Wrapper.SetComponent((T)component);
			}

			/// <summary>
			/// Set the Reference Component for the specified Component to the current Entity.
			/// </summary>
			/// <param name="referencedEntity">The Entity whose Component is 
			/// being referenced.</param>
			public void SetReferenceComponent(Entity referencedEntity)
			{
				Wrapper.SetReferenceComponent<T>(referencedEntity);
			}
		}


		/// <summary>
		/// Provides a Component-typed wrapper for the EntityWrapper, enabling the 
		/// generic methods of the EntityWrapper to be called from a source that is 
		/// unaware of the typing.
		/// </summary>
		/// <typeparam name="T">The Type of the Component being bound.</typeparam>
		/// <returns>The Component-typed wrapper.</returns>
		public ForComponent<T> GetForComponent<T>()
			where T : struct, IComponentData
		{
			return new ForComponent<T> { Wrapper = this };
		}
	}
}
