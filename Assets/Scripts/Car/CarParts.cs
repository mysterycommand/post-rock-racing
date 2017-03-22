using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CarParts : MonoBehaviour {

	/**
	 * Collections:
	 */
	public Rigidbody2D[] TiresSteering { get; private set; }
	public Rigidbody2D[] TiresPowered { get; private set; }
	public Rigidbody2D ChassisBody { get; private set; }
	public Rigidbody2D CannonBody { get; private set; }
	public Slider HealthSlider { get; private set; }

	public BoxCollider2D[] BoxColliders2D { get; private set; }
	public SpriteRenderer[] SpriteRenderers { get; private set; }

	[SerializeField] private GameObject TireFrontRightGameObject;
	[SerializeField] private GameObject TireFrontLeftGameObject;
	[SerializeField] private GameObject TireRearRightGameObject;
	[SerializeField] private GameObject TireRearLeftGameObject;
	[SerializeField] private GameObject ChassisGameObject;
	[SerializeField] private GameObject HoodGameObject;
	[SerializeField] private GameObject RoofGameObject;
	[SerializeField] private GameObject CannonGameObject;

	[SpaceAttribute(8f)]
	[HeaderAttribute("Bullets:")]
	[SpaceAttribute(2f)]
	[SerializeField] public GameObject BulletGameObject;
	[SerializeField] public Transform BulletSpawnTransform;

	[SpaceAttribute(8f)]
	[HeaderAttribute("Audio:")]
	[SpaceAttribute(2f)]
	[SerializeField] public AudioSource EngineSource;
	[SerializeField] public AudioSource FireSource;
	[SerializeField] public AudioSource CollideSource;

	/**
	 * Rigidbodies 2D:
	 */
	private Rigidbody2D TireFrontRightBody;
	private Rigidbody2D TireFrontLeftBody;

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

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		InitializeRigidbodies2D();
		InitializeBoxColliders2D();
		InitializeSpriteRenderers();

		HealthSlider = GetComponentInChildren<Slider>();

		InitializeCollections();
	}

	private void InitializeRigidbodies2D()
	{
		TireFrontRightBody = TireFrontRightGameObject.GetComponent<Rigidbody2D>();
		TireFrontLeftBody = TireFrontLeftGameObject.GetComponent<Rigidbody2D>();

		// TireRearRightBody = TireRearRightGameObject.GetComponent<Rigidbody2D>();
		// TireRearLeftBody = TireRearLeftGameObject.GetComponent<Rigidbody2D>();

		ChassisBody = gameObject.GetComponent<Rigidbody2D>();
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
}
