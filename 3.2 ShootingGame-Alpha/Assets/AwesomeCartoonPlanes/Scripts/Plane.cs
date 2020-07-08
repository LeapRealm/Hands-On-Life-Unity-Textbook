using UnityEngine;

namespace AwesomeCartoonPlanes.Scripts
{
	public class Plane : MonoBehaviour 
	{
		public GameObject prop;
		public GameObject propBlured;

		public bool engenOn;
		
		private void Update() 
		{
			if (engenOn) 
			{
				prop.SetActive(false);
				propBlured.SetActive(true);
				propBlured.transform.Rotate(1000 * Time.deltaTime, 0, 0);
			} 
			else 
			{
				prop.SetActive(true);
				propBlured.SetActive(false);
			}
		}
	}
}