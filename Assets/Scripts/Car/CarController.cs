using UnityEngine;

[RequireComponent(typeof (CarState))]
public class CarController : MonoBehaviour {

	[Range(1,2)]
	[SerializeField] private int PlayerNumber = 1;
	[SerializeField] private float MaxTireAngle = 65f;
	[SerializeField] private float MaxTireForce = 1000f;

	private CarState carState;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		carState = GetComponent<CarState>();

		foreach (BoxCollider2D collider in GetComponentsInChildren<BoxCollider2D>()) {
			collider.gameObject.layer = LayerMask.NameToLayer($"Colliders{PlayerNumber}");
		}
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		float leftStickX = Input.GetAxis($"LeftStickX{PlayerNumber}");
		carState.TiresSteeringAngle = leftStickX * -MaxTireAngle;

		float leftTrigger = Input.GetAxis($"LeftTrigger{PlayerNumber}");
		bool leftBumper = Input.GetButton($"LeftBumper{PlayerNumber}");
		carState.TiresPoweredForce = NormalizeTrigger(leftTrigger) * MaxTireForce;
		if (leftBumper) {
			carState.TiresPoweredForce -= MaxTireForce / 3;
		}

		float rightStickY = Input.GetAxis($"RightStickY{PlayerNumber}");
		float rightStickX = Input.GetAxis($"RightStickX{PlayerNumber}");
		float rightStickAngle = Mathf.Atan2(rightStickY, rightStickX) * Mathf.Rad2Deg;

		carState.IsAiming = !(rightStickX == 0f && rightStickY == 0f);
		carState.CannonAngle = rightStickAngle - 90f;
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("TireFrontRight")
		|| other.gameObject.CompareTag("TireFrontLeft")
		|| other.gameObject.CompareTag("TireRearRight")
		|| other.gameObject.CompareTag("TireRearLeft")
		|| other.gameObject.CompareTag("Chassis")) {
			other.gameObject.GetComponentInParent<CarState>().CarHealth -= 2f;
			carState.CarHealth -= 1f;
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
