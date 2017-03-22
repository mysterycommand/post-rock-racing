using UnityEngine;

[RequireComponent(typeof (CarState))]
public class CarController : MonoBehaviour {

	[HideInInspector] public int PlayerNumber;

	private CarState carState;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		carState = GetComponent<CarState>();

		int layerIndex = LayerMask.NameToLayer($"Colliders{PlayerNumber}");
		foreach (BoxCollider2D collider in carState.carParts.BoxColliders2D) {
			collider.gameObject.layer = layerIndex;
		}
		gameObject.layer = layerIndex;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		float leftStickX = Input.GetAxis($"LeftStickX{PlayerNumber}");

		float leftTrigger = Input.GetAxis($"LeftTrigger{PlayerNumber}");
		bool leftBumper = Input.GetButton($"LeftBumper{PlayerNumber}");

		float rightStickY = Input.GetAxis($"RightStickY{PlayerNumber}");
		float rightStickX = Input.GetAxis($"RightStickX{PlayerNumber}");
		float rightStickAngle = Mathf.Atan2(rightStickY, rightStickX) * Mathf.Rad2Deg;

		float rightTrigger = Input.GetAxis($"RightTrigger{PlayerNumber}");

		carState.TiresSteeringAngle = leftStickX * -carState.MaxTireAngle;

		carState.TiresPoweredForce = NormalizeTrigger(leftTrigger) * carState.MaxTireForce;
		if (leftBumper) {
			carState.TiresPoweredForce -= carState.MaxTireForce / 3;
		}

		carState.IsAiming = !(rightStickX == 0f && rightStickY == 0f);
		carState.CannonAngle = rightStickAngle - 90f;

		carState.RateOfFire = NormalizeTrigger(rightTrigger) > 0f ? 8f : 0f;
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (
			other.gameObject.CompareTag("Bullet") &&
			other.gameObject.layer != LayerMask.NameToLayer($"Colliders{PlayerNumber}")
		) {
			carState.CarHealth -= other.relativeVelocity.magnitude / 8f;
			carState.carParts.CollideBulletSource.Play();
			Destroy(other.gameObject);
		} else if (
			other.gameObject.CompareTag("TireFrontRight") ||
			other.gameObject.CompareTag("TireFrontLeft") ||
			other.gameObject.CompareTag("TireRearRight") ||
			other.gameObject.CompareTag("TireRearLeft") ||
			other.gameObject.CompareTag("Chassis")
		) {
			other.gameObject.GetComponentInParent<CarState>().CarHealth -= other.relativeVelocity.magnitude;
			carState.carParts.CollideCarSource.Play();
			carState.CarHealth -= other.relativeVelocity.magnitude;
		}

		if (carState.CarHealth <= 0f) {
			gameObject.SetActive(false);
		}
	}

	private float NormalizeTrigger(float value)
	{
		if (value == 0f) {
			return value;
		}

		return (value + 1f) / 2f;
	}

}
