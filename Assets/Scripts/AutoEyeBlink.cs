using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEyeBlink : MonoBehaviour 
{
	KeyCode _BlinkKeyCode = KeyCode.H;

	[SerializeField]
	KeyboardVisibleController _VisibleController;

	float _BlinkInterval;

	float _BlinkTimer;

	[SerializeField]
	float _MinBlinkInterval = 3.0f;

	[SerializeField]
	float _MaxBlinkInterval = 8.0f;

	[SerializeField]
	float _MinBlinkTime = 0.1f;

	[SerializeField]
	float _MaxBlinkTime = 0.3f;

	void Start () 
	{
		if(_VisibleController == null)
		{
			_VisibleController = GetComponent<KeyboardVisibleController>();
		}
	}
	
	void Update () 
	{
		_BlinkTimer += Time.deltaTime;
		if(_BlinkTimer >_BlinkInterval)
		{
			float rand = Random.Range(0.0f, 1.0f);
			if(rand > 0.5f)
			{
				StartCoroutine(Blink());
				SetNextBlinkInterval();
			}
			else
			{
				StartCoroutine(DoubleBlink());
				SetNextBlinkInterval();
			}
		}
	}

	void SetNextBlinkInterval()
	{
		_BlinkTimer = 0.0f;
		_BlinkInterval = Random.Range(_MinBlinkInterval, _MaxBlinkInterval);
	}

	IEnumerator Blink()
	{
		if(_VisibleController != null)
		{
			_VisibleController.AddPushKey(_BlinkKeyCode);
			float blinkTime = Random.Range(_MinBlinkTime, _MaxBlinkTime);
			yield return new WaitForSeconds(blinkTime);
			_VisibleController.RemovePushKey(_BlinkKeyCode);
		}
		else
		{
			yield return null;
		}
	}

	IEnumerator DoubleBlink()
	{
		if(_VisibleController != null)
		{
			float blinkTime = 0.075f;
			_VisibleController.AddPushKey(_BlinkKeyCode);
			yield return new WaitForSeconds(blinkTime);
			_VisibleController.RemovePushKey(_BlinkKeyCode);
			yield return new WaitForSeconds(blinkTime);
			_VisibleController.AddPushKey(_BlinkKeyCode);
			yield return new WaitForSeconds(blinkTime);
			_VisibleController.RemovePushKey(_BlinkKeyCode);
		}
		else
		{
			yield return null;
		}
	}
}
