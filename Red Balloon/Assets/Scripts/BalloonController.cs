using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    private enum BalloonState { Aim, Charge, Fly }
	
	private BalloonState _balloonState;

	private ShowArrow _showArrow;
	private DragRotation _dragRotation;
	private BalloonShoot _balloonShoot;
	private Rigidbody _rigidbody;

	private float _time;

	[SerializeField] private float stopCriterionVelocity;
	[SerializeField] private float chargeGauge;
	[SerializeField] private KeyCode chargeKey;
	[SerializeField] private float chargeSpeed;

	private void Awake()
	{
		_showArrow = GetComponent<ShowArrow>();
		_dragRotation = GetComponent<DragRotation>();
		_balloonShoot = GetComponent<BalloonShoot>();
		_rigidbody = GetComponent<Rigidbody>();
		//_slider = GameObject.FindWithTag("ChargeSlider").GetComponent<Slider>();
	}

	private void Start()
	{
		ChangeState(BalloonState.Aim);
	}

	/// <summary>
	/// 풍선의 행동을 newState로 변경한다.
	/// </summary>
	private void ChangeState(BalloonState newState)
	{
		// 이전 상태의 코루틴 종료
		StopCoroutine(_balloonState.ToString());
		// 새로운 상태로 변경
		_balloonState = newState;
		// 현재 상태의 코루틴 실행
		StartCoroutine(_balloonState.ToString());
	}

	
	private IEnumerator Aim()
	{
		Debug.Log("Aim State");

		_showArrow?.Show();
		//카메라 컨트롤 타입 드래그로 변경
		CameraController.instance.onControll = CameraController.ControllType.Drag;
		_dragRotation.onControll = true;

		while (true)
		{
			if (Input.GetKey(chargeKey)) break;
			yield return null;
		}

		CameraController.instance.onControll = CameraController.ControllType.Stop;
		_dragRotation.onControll = false;

		ChangeState(BalloonState.Charge);
	}

	
	private IEnumerator Charge()
	{
		Debug.Log("Charge State");
		
		chargeGauge = 0f;

		while (true)
		{
			if (Input.GetKey(chargeKey))
			{
				chargeGauge += chargeSpeed * Time.deltaTime;
				if (chargeGauge > 1f) chargeGauge = 1f;
			}
			else break;

			yield return null;
		}

		_showArrow?.Hide();
		ChangeState(BalloonState.Fly);
	}


	private IEnumerator Fly()
	{
		Debug.Log("Fly State");
		
		_time = 0;
		
		var dirVec = new Vector3(0, 0, 1);
		var rotatedDirVec = _dragRotation.GetDirection() * dirVec;
		
		_balloonShoot.SetMoveDirection(rotatedDirVec);
		if (_balloonShoot.StartMove(chargeGauge))
		{
			CameraController.instance.onControll = 
				CameraController.ControllType.LookAround;
			chargeGauge = 0f;
		}
		

		while (true)
		{
			_time += Time.deltaTime;
			if (_rigidbody.velocity.magnitude <= stopCriterionVelocity && _time > 0.5f)
			{
				break;
			}

			yield return null;
		}
		
		ChangeState(BalloonState.Aim);
	}

	public float GetChargeGauge()
	{
		return chargeGauge;
	}
}