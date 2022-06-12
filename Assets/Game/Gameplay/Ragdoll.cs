using UnityEngine;

namespace Funzilla
{
	internal class Ragdoll : MonoBehaviour
	{
		[SerializeField] private CharacterJoint[] joints;
		[SerializeField] private Rigidbody[] bodies;
		private Quaternion[] _rotations;
		private Vector3[] _positions;

		private void Awake()
		{
			_rotations = new Quaternion[joints.Length];
			_positions = new Vector3[joints.Length];
			for (var i = 0; i < joints.Length; i++)
			{
				_rotations[i] = joints[i].transform.localRotation;
				_positions[i] = joints[i].transform.localPosition;
			}
		}

		private void OnEnable()
		{
			for (var i = 0; i < joints.Length; i++)
			{
				joints[i].gameObject.SetActive(false);
				joints[i].transform.localRotation = _rotations[i];
				joints[i].transform.localPosition = _positions[i];
				joints[i].gameObject.SetActive(true);
			}
		}

		internal void Fall()
		{
			foreach (var body in bodies)
			{
				body.isKinematic = false;
			}
		}

		internal void Fly(Vector3 velocity)
		{
			foreach (var body in bodies)
			{
				body.velocity = velocity;
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Setup")]
		private void Setup()
		{
			joints = GetComponentsInChildren<CharacterJoint>();
			bodies =  GetComponentsInChildren<Rigidbody>();
			foreach (var joint in joints)
			{
				joint.enableProjection = true;
				joint.projectionDistance = 0.01f;
			}

			foreach (var body in bodies)
			{
				body.isKinematic = true;
			}
		}
#endif
	}
}