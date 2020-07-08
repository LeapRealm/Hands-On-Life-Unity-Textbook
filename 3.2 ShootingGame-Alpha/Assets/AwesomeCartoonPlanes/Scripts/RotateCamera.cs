using UnityEngine;

namespace AwesomeCartoonPlanes.Scripts
{
	public class RotateCamera : MonoBehaviour
	{
		private void Update()
		{
			transform.Rotate(0, -50 * Time.deltaTime, 0);
		}
	}
}