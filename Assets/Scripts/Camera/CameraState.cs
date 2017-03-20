using UnityEngine;

[ExecuteInEditMode]
public class CameraState : MonoBehaviour {

	public float PosX { get; set; }
	public float PosY { get; set; }
	public float Size { get; set; }

	[SerializeField] private float Damping = 0.2f;
	[SerializeField] private float EdgeBuffer = 5f;
	[SerializeField] private float MinSize = 5f;

	private Camera MainCamera;
	private ParticleSystem MainParticleSystem;
	private Vector3 CameraPositionVelocity = Vector3.zero;
	private Vector3 ParticleSystemPositionVelocity = Vector3.zero;
	private float CameraSizeVelocity = 0f;
	private Vector3 ParticleSystemBoxVelocity = Vector3.zero;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		MainCamera = GetComponentInChildren<Camera>();
		MainParticleSystem = GetComponentInChildren<ParticleSystem>();

		SetParticleSystemBox(MainCamera.orthographicSize, MainCamera.aspect);
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		float targetCameraSize = Mathf.Max(Size + EdgeBuffer, MinSize);

		UpdateCameraPosition();
		UpdateCameraSize(targetCameraSize);

		UpdateParticleSystemPosition();
		UpdateParticleSystemBox(targetCameraSize);
	}

	private void UpdateCameraPosition()
	{
		Vector3 currentCameraPosition = MainCamera.transform.position;
		Vector3 targetCameraPosition = GetTargetPosition(currentCameraPosition);

		MainCamera.transform.position = Vector3.SmoothDamp(
			currentCameraPosition,
			targetCameraPosition,
			ref CameraPositionVelocity,
			Damping
		);
	}

	private void UpdateCameraSize(float targetCameraSize)
	{
		float currentCameraSize = MainCamera.orthographicSize;

		MainCamera.orthographicSize = Mathf.SmoothDamp(
			currentCameraSize,
			targetCameraSize,
			ref CameraSizeVelocity,
			Damping
		);
	}

	private void UpdateParticleSystemPosition()
	{
		Vector3 currentParticleSystemPosition = MainParticleSystem.transform.position;
		Vector3 targetParticleSystemPosition = GetTargetPosition(currentParticleSystemPosition);

		MainParticleSystem.transform.position = Vector3.SmoothDamp(
			currentParticleSystemPosition,
			targetParticleSystemPosition,
			ref ParticleSystemPositionVelocity,
			Damping
		);
	}

	private void UpdateParticleSystemBox(float targetCameraSize)
	{
		float currentCameraAspect = MainCamera.aspect;

		Vector3 currentParticleSystemBox = MainParticleSystem.shape.box;
		Vector3 targetParticleSystemBox = GetTargetBox(targetCameraSize, currentCameraAspect);

		var shape = MainParticleSystem.shape;
		shape.box = Vector3.SmoothDamp(
			currentParticleSystemBox,
			targetParticleSystemBox,
			ref ParticleSystemBoxVelocity,
			Damping
		);
	}

	private Vector3 GetTargetPosition(Vector3 currentPosition)
	{
		return new Vector3(PosX, PosY, currentPosition.z);
	}

	private Vector3 GetTargetBox(float targetSize, float currentAspect)
	{
		return new Vector3(targetSize * currentAspect * 2, targetSize * 2, 0f);
	}

	private void SetParticleSystemBox(float size, float aspect)
	{
		var shape = MainParticleSystem.shape;
		shape.box = new Vector3(size * aspect * 2, size * 2, 0f);
	}

}
