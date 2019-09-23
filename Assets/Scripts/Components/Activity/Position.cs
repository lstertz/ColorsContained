using Components.Supports;
using Unity.Entities;
using Unity.Mathematics;


namespace Components.Activity
{
	/// <summary>
	/// Defines the Position Component, which details the 3-dimensional position of 
	/// an Entity, with support to set/sync the position of a GameObject.
	/// </summary>
	public struct Position : IComponentData, IGameObjectSyncer<Position>, IGameObjectUpdater
	{
		public float3 Value;

		/// <summary>
		/// Updates the provided GameObject for this Position.
		/// </summary>
		/// <param name="gameObject">The GameObject, with which to be synced.</param>
		public Position GetSynced(UnityEngine.GameObject gameObject)
		{
			return new Position
			{
				Value = gameObject.transform.position
			};
		}

		/// <summary>
		/// Updates the provided GameObject for this Position.
		/// </summary>
		/// <param name="gameObject">The GameObject to be updated.</param>
		public void Update(UnityEngine.GameObject gameObject)
		{
			gameObject.transform.position = Value;
		}
	}
}
