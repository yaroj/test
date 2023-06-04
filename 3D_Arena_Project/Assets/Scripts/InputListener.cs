using UnityEngine;
using UnityEngine.UI;
using Input = InputWrapper.Input;
using Assets.Scripts;
public class InputListener : MonoBehaviour
{
	[SerializeField]
	private Player player;
	[SerializeField]
	private float minXOfRotationTouch = 0.3f;

	[SerializeField]
	private RectTransform movementStick;

	[SerializeField]
	private TMPro.TextMeshProUGUI movementText;

	[SerializeField] private Slider _touchSensitivitySlider;

	private Vector2 _previousTouchPos;


	private float _touchSensitivity = 0.5f;

	public float TouchSensitivity { get => _touchSensitivity; set => _touchSensitivity = value; }

	private void Update()
	{
		player.Move(GetWASDMovement());
		ProcessMouseInput();
	}

	private void Start()
	{
		TouchSensitivity = SaveLoad.TouchSensitivity;
		_touchSensitivitySlider.value = TouchSensitivity;
		_touchSensitivitySlider.onValueChanged.AddListener(UpdateTouchSensitivity);
	}

	private Vector3 GetWASDMovement()
	{
		Vector3 movementVector = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
		{
			movementVector += Vector3.forward;
		}
		if (Input.GetKey(KeyCode.A))
		{
			movementVector += Vector3.left;
		}
		if (Input.GetKey(KeyCode.S))
		{
			movementVector += Vector3.back;
		}
		if (Input.GetKey(KeyCode.D))
		{
			movementVector += Vector3.right;
		}
		return movementVector;
	}

private void UpdateTouchSensitivity(float value)
	{
		TouchSensitivity = value;
		SaveLoad.TouchSensitivity = TouchSensitivity;
	}

	private void ProcessMouseInput()
	{
		void ProcessRotateInput(Touch touch)
		{
			if (touch.phase == TouchPhase.Began)
			{
				_previousTouchPos = touch.position;
				const float degToRad = Mathf.PI / 180;
				const float addedHeight = 0.03f;
				float screenHeightInWorldUnits = 2 * Mathf.Tan(Camera.main.fieldOfView * degToRad * 0.5f);
				float screenWidthInWorldUnits = screenHeightInWorldUnits * Camera.main.aspect;
				Vector3 shootingDirection = Camera.main.transform.forward
				+ (screenWidthInWorldUnits * (_previousTouchPos.x - Screen.width / 2) / Screen.width) * Camera.main.transform.right
				+ (addedHeight + screenHeightInWorldUnits * (_previousTouchPos.y - Screen.height / 2) / Screen.height) * Camera.main.transform.up;
				player.TryShoot(shootingDirection);
			}
			else
			{
				Vector2 newTouchPos = touch.position;
				player.Rotate(TouchSensitivity * (_previousTouchPos - newTouchPos));
				_previousTouchPos = newTouchPos;
			}
		}

		void ProcessMoveInput(Touch touch)
		{
			Vector2 movementVector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(
					movementStick,
					touch.position,
					Camera.main,
					out movementVector
			);
			Vector3 newDiff = movementVector;
			newDiff.z = newDiff.y;
			player.Move(newDiff);
			movementText.text = movementVector.ToString() +
			"\n" + touch.position;
		}


		movementText.text = "0 0";
		foreach (var touch in Input.touches)
		{
			if (touch.position.x > minXOfRotationTouch * Screen.width)
			{
				ProcessRotateInput(touch);
			}
			else
			{
				ProcessMoveInput(touch);
			}
		}
	}
}
