using UnityEngine;

namespace Funzilla
{
	internal class Character : MonoBehaviour
	{
		[SerializeField] private Ragdoll ragdoll;
		[SerializeField] private Animator animator;
		public Renderer cRenderer;
		internal bool Dead => !animator.enabled;

		internal void Fall()
		{
			animator.enabled = false;
			transform.SetParent(Gameplay.Instance.transform);
			ragdoll.Fall();
			ragdoll.Fly(Gameplay.Instance.Player.Velocity);
		}

		internal void Cheer()
		{
			animator.SetTrigger("Cheer");
		}

		internal void Jump()
		{
			animator.SetTrigger("Jump");
		}
		internal void Balance()
		{
			animator.SetTrigger("Balance");
		}

		internal void Debut()
		{
			animator.SetTrigger("Debut");
		}
	}
}