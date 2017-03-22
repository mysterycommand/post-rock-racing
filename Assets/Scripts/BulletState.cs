using UnityEngine;

public class BulletState : MonoBehaviour {

	private Camera MainCamera;
	private Rigidbody2D BulletBody;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		MainCamera = Camera.main;
		BulletBody = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		Vector3 screenPosition = MainCamera.WorldToScreenPoint(BulletBody.position);

		if (
			screenPosition.x < 0f ||
			screenPosition.y < 0f ||
			screenPosition.x > Screen.width ||
			screenPosition.y > Screen.height
		) {
			Destroy(gameObject);
		}
	}

}
