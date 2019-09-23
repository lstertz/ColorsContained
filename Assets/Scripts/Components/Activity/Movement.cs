using Components.Supports;
using Unity.Entities;
using UnityEngine;

namespace Components.Activity
{
	/// <summary>
	/// Defines the Movement Component, which details a change in position by x, y, and z.
	/// </summary>
	public struct Movement : IComponentData, IGameObjectUpdater
	{
		public const string SpeedParameter = "Speed";

		public float X;
		public float Y;
		public float Z;


		public void Update(UnityEngine.GameObject gameObject)
		{
			Animator animator = gameObject.GetComponent<Animator>();
			if (animator == null)
				return;

			if (X != 0 || Y != 0 || Z != 0)
				animator.SetFloat(SpeedParameter, 1.0f);
			else
				animator.SetFloat(SpeedParameter, 0.0f);
		}
	}
}
