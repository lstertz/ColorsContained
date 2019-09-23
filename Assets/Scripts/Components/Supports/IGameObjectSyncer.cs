using Unity.Entities;


namespace Components.Supports
{
	/// <summary>
	/// Defines the interface of a GameObject syncer.
	/// </summary>
	public interface IGameObjectSyncer<T> where T : IComponentData, IGameObjectSyncer<T>
	{
		/// <summary>
		/// Returns a new copy of the ComponentData, synced with the provided GameObject.
		/// </summary>
		/// <param name="gameObject">The GameObject, with which to be synced..</param>
		T GetSynced(UnityEngine.GameObject gameObject);
	}
}
