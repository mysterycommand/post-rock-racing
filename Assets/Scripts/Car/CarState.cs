using UnityEngine;

[ExecuteInEditMode]
public class CarState : MonoBehaviour {

	private GameObject TireFrontRightGameObject;
	private GameObject TireFrontLeftGameObject;
	private GameObject TireRearRightGameObject;
	private GameObject TireRearLeftGameObject;
	private GameObject ChassisGameObject;
	private GameObject HoodGameObject;
	private GameObject RoofGameObject;
	private GameObject CannonGameObject;

	/**
	 * Rigidbodies 2D:
	 */
	private Rigidbody2D TireFrontRightBody;
	private Rigidbody2D TireFrontLeftBody;
	private Rigidbody2D ChassisBody;
	private Rigidbody2D CannonBody;

	// private Rigidbody2D TireRearRightBody;
	// private Rigidbody2D TireRearLeftBody;

	/**
	 * Box Colliders 2D:
	 */
	private BoxCollider2D TireFrontRightCollider;
	private BoxCollider2D TireFrontLeftCollider;
	private BoxCollider2D TireRearRightCollider;
	private BoxCollider2D TireRearLeftCollider;
	private BoxCollider2D ChassisCollider;

	/**
	 * Sprite Renderers:
	 */
	private SpriteRenderer HoodRenderer;
	private SpriteRenderer RoofRenderer;
	private SpriteRenderer CannonRenderer;

	/**
	 * Collections:
	 */
	private Rigidbody2D[] TiresSteering { get; set; }
	private Rigidbody2D[] TiresPowered { get; set; }
	private SpriteRenderer[] SpriteRenderers { get; set; }
	private BoxCollider2D[] BoxColliders2D { get; set; }

	/**
	 * Single Values:
	 */
	public float TiresSteeringAngle { get; set; }
	public float TiresPoweredForce { get; set; }
	public bool IsAiming { get; set; }
	public float CannonAngle { get; set; }
	public Color CarColor { get; set; }

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		GetGameObjects(transform);
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		InitializeRigidbodies2D();
		InitializeBoxColliders2D();
		InitializeSpriteRenderers();

		InitializeCollections();
		InitializeSingleValues();

		foreach (SpriteRenderer renderer in SpriteRenderers) {
			renderer.color = CarColor;
		}
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		foreach (Rigidbody2D Tire in TiresSteering) {
			Tire.rotation = Mathf.LerpAngle(Tire.rotation, ChassisBody.rotation + TiresSteeringAngle, 0.1f);
		}

		foreach (Rigidbody2D Tire in TiresPowered) {
			Tire.AddForce(Tire.transform.up * TiresPoweredForce);
		}

		CannonBody.rotation = Mathf.LerpAngle(CannonBody.rotation, (IsAiming) ? CannonAngle : ChassisBody.rotation, 0.1f);
	}

	private void GetGameObjects(Transform parent)
	{
		foreach (Transform child in parent) {
			if (child.childCount > 0) {
				GetGameObjects(child);
			}

			if (child.gameObject.CompareTag("TireFrontRight")) {
				TireFrontRightGameObject = child.gameObject;
			}

			if (child.gameObject.CompareTag("TireFrontLeft")) {
				TireFrontLeftGameObject = child.gameObject;
			}

			if (child.gameObject.CompareTag("TireRearRight")) {
				TireRearRightGameObject = child.gameObject;
			}

			if (child.gameObject.CompareTag("TireRearLeft")) {
				TireRearLeftGameObject = child.gameObject;
			}

			if (child.gameObject.CompareTag("Chassis")) {
				ChassisGameObject = child.gameObject;
			}

			if (child.gameObject.CompareTag("Hood")) {
				HoodGameObject = child.gameObject;
			}

			if (child.gameObject.CompareTag("Roof")) {
				RoofGameObject = child.gameObject;
			}

			if (child.gameObject.CompareTag("Cannon")) {
				CannonGameObject = child.gameObject;
			}
		}
	}

	private void InitializeRigidbodies2D()
	{
		TireFrontRightBody = TireFrontRightGameObject.GetComponent<Rigidbody2D>();
		TireFrontLeftBody = TireFrontLeftGameObject.GetComponent<Rigidbody2D>();
		// TireRearRightBody = TireRearRightGameObject.GetComponent<Rigidbody2D>();
		// TireRearLeftBody = TireRearLeftGameObject.GetComponent<Rigidbody2D>();
		ChassisBody = ChassisGameObject.GetComponent<Rigidbody2D>();
		CannonBody = CannonGameObject.GetComponent<Rigidbody2D>();
	}

	private void InitializeBoxColliders2D()
	{
		TireFrontRightCollider = TireFrontRightGameObject.GetComponent<BoxCollider2D>();
		TireFrontLeftCollider = TireFrontLeftGameObject.GetComponent<BoxCollider2D>();
		TireRearRightCollider = TireRearRightGameObject.GetComponent<BoxCollider2D>();
		TireRearLeftCollider = TireRearLeftGameObject.GetComponent<BoxCollider2D>();
		ChassisCollider = ChassisGameObject.GetComponent<BoxCollider2D>();
	}

	private void InitializeSpriteRenderers()
	{
		HoodRenderer = HoodGameObject.GetComponent<SpriteRenderer>();
		RoofRenderer = RoofGameObject.GetComponent<SpriteRenderer>();
		CannonRenderer = CannonGameObject.GetComponent<SpriteRenderer>();
	}

	private void InitializeCollections()
	{
		TiresSteering = new Rigidbody2D[] {
			TireFrontRightBody,
			TireFrontLeftBody,
		};

		TiresPowered = new Rigidbody2D[] {
			TireFrontRightBody,
			TireFrontLeftBody,
		};

		SpriteRenderers = new SpriteRenderer[] {
			HoodRenderer,
			RoofRenderer,
			CannonRenderer,
		};

		BoxColliders2D = new BoxCollider2D[] {
			TireFrontRightCollider,
			TireFrontLeftCollider,
			TireRearRightCollider,
			TireRearLeftCollider,
			ChassisCollider,
		};
	}

	private void InitializeSingleValues()
	{
		TiresSteeringAngle = 0f;
		TiresPoweredForce = 0f;

		IsAiming = false;
		CannonAngle = 0f;

		CarColor = Color.HSVToRGB(Random.value, 0.7f, 0.7f);
	}

}
