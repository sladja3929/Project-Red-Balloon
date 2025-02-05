using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalloonController : MonoBehaviour
{
	public enum BalloonState
	{
		Aim, Charge, Fly, Fall, Freeze, Cinematic, DeveloperMode
	}
	
	private BalloonState _balloonState;

	private ShowArrow _showArrow;
	private DragRotation _dragRotation;
	private BalloonShoot _balloonShoot;
	private Rigidbody _rigidbody;

	private float _time;
	private bool isOnPlatform = false;

	[SerializeField] private bool isDebug = false;
	[SerializeField] private float stopCriterionVelocity;
	[SerializeField] private float chargeGauge;
	[SerializeField] private KeyCode chargeKey;
	[SerializeField] private float chargeSpeed;
	[SerializeField] private ChargeUI ui;

	[SerializeField] private AudioClip balloonShootSound;
	[SerializeField] private AudioClip balloonChargeSound;
	[SerializeField] private AudioClip balloonBoundSound;

	[SerializeField] private float rayToBottomLength;

	[SerializeField] private bool isGamePaused;

	private void Awake()
	{
		_showArrow = GetComponent<ShowArrow>();
		_dragRotation = GetComponent<DragRotation>();
		_balloonShoot = GetComponent<BalloonShoot>();
		_rigidbody = GetComponent<Rigidbody>();
		isOnPlatform = false;
		
		if (SaveManager.instance.IsNewSave() is false &&
		    SaveManager.instance.BuildIndex == SceneManager.GetActiveScene().buildIndex)
		{
			// 릴리즈면 무조건 실행, 디버그면 isDebug에 따라
			//if(!Debug.isDebugBuild || !isDebug) transform.position = SaveManager.instance.Position;
			if(!isDebug) transform.position = SaveManager.instance.Position;
		}
	}

	private void Start()
	{
		ChangeState(BalloonState.Fall);
		GameManager.instance.IsCinematic = false;
		GameManager.instance.CanSuicide = true;
		GameManager.instance.onBalloonDead.AddListener(ResetCollision);
	}

	private void ResetCollision()
	{
		isOnPlatform = false;
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

	public BalloonState GetBalloonState()
	{
		return _balloonState;
	}
	
	public void SetBasicState()
	{
		ChangeState(BalloonState.Fall);
	}
	
	public void SetFreezeState()
	{
		GameManager.instance.CanSuicide = false;
		ChangeState(BalloonState.Freeze);
	}
	
	public void SetCinematicState()
	{
		ChangeState(BalloonState.Cinematic);
	}
	
	/// <summary>
	/// 풍선 아래에 뭔가 있는지 확인하는 함수
	/// </summary>
	/// <returns></returns>
	private bool SomethingUnderBalloon()
	{
		LayerMask wallLayer = LayerMask.GetMask("Platform");
		return Physics.Raycast(transform.position, Vector3.down, rayToBottomLength, wallLayer);
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer.Equals(3))
		{
			isOnPlatform = true;
			float magnitude = _rigidbody.velocity.magnitude;
			
			if(_balloonState == BalloonState.Fall && magnitude > stopCriterionVelocity + 0.5f)
			{
				magnitude = Mathf.Clamp(magnitude, 1, 10);
				magnitude = Mathf.InverseLerp(1, 10, magnitude);
				SoundManager.instance.SfxPlay("BalloonBound", balloonBoundSound, transform.position, magnitude);
			}
			
		}
	}
	
	private void OnCollisionExit(Collision collision)
	{
		isOnPlatform = false;
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.layer.Equals(3) && !isOnPlatform)
		{
			isOnPlatform = true;
		}
	}

	private IEnumerator Aim()
	{
		Debug.Log("Aim State");
		ui.SetChargeUI(0);

		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
		
		// ReSharper disable once Unity.NoNullPropagation
		_dragRotation.ResetDirection();
		_showArrow?.Show();
		//카메라 컨트롤 타입 드래그로 변경
		CameraController.instance.onControll = CameraController.ControllType.Drag;
		_dragRotation.onControll = true;
	
		while (true)
		{
			if (Input.GetKey(chargeKey)) break;
			
			if (!isOnPlatform)
			{
				ChangeState(BalloonState.Fall);
			}
			
			yield return null;
		}

		CameraController.instance.onControll = CameraController.ControllType.Stop;
		_dragRotation.onControll = false;

		ChangeState(BalloonState.Charge);
	}

	
	private IEnumerator Charge()
	{
		Debug.Log("Charge State");
		//SoundManager.instance.SfxPlay("BalloonCharge", balloonChargeSound);
		
		chargeGauge = 0f;

		while (true)
		{
			if (Input.GetKey(chargeKey))
			{
				chargeGauge += chargeSpeed * Time.deltaTime;
				if (chargeGauge > 1f) chargeGauge = 1f;
				
				ui.SetChargeUI(chargeGauge);
			}
			else break;

			yield return null;
		}

		
		ChangeState(BalloonState.Fly);
	}


	private IEnumerator Fly()
	{
		Debug.Log("Fly State");
		_showArrow?.Hide();
		_rigidbody.constraints = RigidbodyConstraints.None;
		
		//SoundManager.instance.SfxPlay("BalloonShoot", balloonShootSound);

		var dirVec = new Vector3(0, 0, 1);
		var rotatedDirVec = _dragRotation.GetDirection() * dirVec;
		
		_balloonShoot.SetMoveDirection(rotatedDirVec);
		if (_balloonShoot.StartMove(chargeGauge))
		{
			CameraController.instance.onControll = 
				CameraController.ControllType.Drag;
			chargeGauge = 0f;
		}

		yield return new WaitForSeconds(0.5f);
		
		ChangeState(BalloonState.Fall);
	}

	private IEnumerator Fall()
	{
		Debug.Log("Fall state");
		_showArrow?.Hide();
		
		_rigidbody.constraints = RigidbodyConstraints.None;
		_time = 0;
		
		while (true)
		{
			_time += Time.deltaTime;
			if (_rigidbody.velocity.magnitude <= stopCriterionVelocity &&
			    isOnPlatform && SomethingUnderBalloon() && !isGamePaused) 
			{
				break;
			}

			yield return null;
		}
		
		ChangeState(BalloonState.Aim);
	}
	
	private IEnumerator Freeze()  //스크립트로만 진입, 탈출
	{
		Debug.Log("Freeze state");
		
		_showArrow?.Hide();
		_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		CameraController.instance.onControll = CameraController.ControllType.Drag;
		_dragRotation.onControll = false;
		
		yield return null;
	}
	
	private IEnumerator Cinematic()  //스크립트로만 진입, 탈출
	{
		Debug.Log("Cinematic state");
		
		_showArrow?.Hide();
		_rigidbody.constraints = RigidbodyConstraints.None;
		CameraController.instance.onControll = CameraController.ControllType.Stop;
		_dragRotation.onControll = false;
		
		yield return null;
	}

	public void SetOnPlatform(bool value)
	{
		isOnPlatform = value;
	}
	
	public float GetChargeGauge()
	{
		return chargeGauge;
	}

	private void OnDestroy()
	{
		GameManager.instance.onBalloonDead.RemoveListener(ResetCollision);
	}
	
#if UNITY_EDITOR || DEVELOPMENT_BUILD
	public KeyCode flyKey;
	
	private IEnumerator DeveloperMode()
	{
		yield return new WaitForSeconds(0.5f);
		
		Debug.Log("Developer_mode");

		DeveloperMode developerModeComponent = gameObject.AddComponent<DeveloperMode>();
		while (!Input.GetKeyDown(flyKey))
		{
			yield return null;
		}
		Destroy(developerModeComponent);

		ChangeState(BalloonState.Fall);
	}

	private float t = 0;
	
	private void Update()
	{
		if (Input.GetKeyDown(flyKey) && _balloonState != BalloonState.DeveloperMode)
		{
			ChangeState(BalloonState.DeveloperMode);
		}
		Debug.DrawRay(transform.position, Vector3.down * rayToBottomLength, Color.blue);
		t += Time.deltaTime;
		if (t >= 2f)
		{
			// 실행할 코드 (예: 로그 출력)
			Debug.Log(isOnPlatform);

			// 타이머 초기화 또는 timer -= 2f;로 남은 시간을 반영할 수 있음
			t = 0f;
		}
	}
#endif
}