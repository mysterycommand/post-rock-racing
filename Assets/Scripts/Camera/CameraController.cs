using UnityEngine;

[RequireComponent(typeof (CameraState))]
public class CameraController : MonoBehaviour {

	[SerializeField] private Transform[] TargetTransforms = new Transform[2];

	private Camera MainCamera;
	private CameraState cameraState;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		MainCamera = GetComponentInChildren<Camera>();
		cameraState = GetComponent<CameraState>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		// float size = 0f;
		Vector3 targetCameraPosition = GetTargetCameraPosition();
		float targetCameraSize = GetTargetCameraSize(targetCameraPosition);

		cameraState.PosX = targetCameraPosition.x;
		cameraState.PosY = targetCameraPosition.y;
		cameraState.Size = targetCameraSize;
	}

	private Vector3 GetTargetCameraPosition()
	{
		Vector3 pos = Vector3.zero;
		int i = 0;

		foreach (Transform targetTransform in TargetTransforms) {
			if (!targetTransform.gameObject.activeSelf) {
				continue;
			}

			pos += targetTransform.position;
			i++;
		}

		if (i > 0) {
			pos /= i;
		}

		return new Vector3(pos.x, pos.y, 0f);
	}

	private float GetTargetCameraSize(Vector3 targetCameraPosition)
	{
		Vector3 localTargetCameraPosition = transform.InverseTransformPoint(targetCameraPosition);
		float size = 0f;

		foreach (Transform targetTransform in TargetTransforms) {
			if (!targetTransform.gameObject.activeSelf) {
				continue;
			}

			Vector3 localTargetTransformPosition = transform.InverseTransformPoint(targetTransform.position);
			Vector3 localDistance = localTargetTransformPosition - localTargetCameraPosition;

			size = Mathf.Max(
				Mathf.Max(size, Mathf.Abs(localDistance.y)),
				Mathf.Abs(localDistance.x / MainCamera.aspect)
			);
		}

		return size;
	}

}
