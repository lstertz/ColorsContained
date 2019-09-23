using Components.Supports;
using Unity.Entities;
using UnityEngine;

namespace Components.Goal
{
	/// <summary>
	/// Defines the Requirement Component, which details the requirements of a goal with the 
    /// ability to sync with a Goal UI GameObject.
	/// </summary>
	public struct Requirement : IComponentData, IGameObjectUpdater
	{
        public Color Background;
        public Color Type;
		public int Value;
        
		/// <summary>
		/// Updates the provided GameObject for this Requirement.
		/// </summary>
		/// <param name="gameObject">The GameObject to be updated.</param>
		public void Update(UnityEngine.GameObject gameObject)
		{
            GoalUI goalUI = gameObject.GetComponent<GoalUI>();

            goalUI.SetTotalRequired(Value);
            goalUI.SetBackgroundColor(Background);
            goalUI.SetType(Type);
        }
	}
}
