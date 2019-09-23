using Components.Supports;
using Unity.Entities;
using UnityEngine;

namespace Components.Goal
{
	/// <summary>
	/// Defines the Requirement Component, which details how much of a goal has been 
    /// fulfilled, with the ability to sync with a Goal UI GameObject.
	/// </summary>
	public struct Fulfillment : IComponentData, IGameObjectUpdater
	{
		public int Value;
        
		/// <summary>
		/// Updates the provided GameObject for the current fulfillment.
		/// </summary>
		/// <param name="gameObject">The GameObject to be updated.</param>
		public void Update(UnityEngine.GameObject gameObject)
		{
            gameObject.GetComponent<GoalUI>().SetRequirementMet(Value);
        }
	}
}
