using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChanmomoController : MonoBehaviour 
{
	[SerializeField]
	private float _JumpPower = 25.0f;

	private RectTransform _Transform;
	private Vector3 _DefaultPosition;
	private Quaternion _DefaultRotation;
	private Tween _CurrentTween;

	void Start () 
	{
		_Transform = GetComponent<RectTransform>();
		_DefaultPosition = _Transform.position;
		_DefaultRotation = _Transform.rotation;
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			Jump();
		}
		else if(Input.GetKeyDown(KeyCode.Q))
		{
			Reset();
		}
		else if(Input.GetKeyDown(KeyCode.E))
		{
			Bow();
		}
		else if(Input.GetKeyDown(KeyCode.R))
		{
			Shuffle();
		}
	}

	void Jump()
	{
		ResetTween();
		_CurrentTween = transform.DOJump(transform.position, _JumpPower, 2, 1.0f);
	}

	void Shuffle()
	{
		ResetTween();
		_CurrentTween = transform.DOShakePosition(1.0f, 20.0f, 25, 90.0f, false, false);
	}

	void Bow()
	{
		ResetTween();
		Sequence seq = DOTween.Sequence();
		seq.Append(transform.DOLocalRotate(new Vector3(-45.0f, 0.0f, -0.0f), 0.5f, RotateMode.Fast).SetEase(Ease.InOutQuad));
		seq.Append(transform.DOLocalRotate(Vector3.zero, 0.5f, RotateMode.Fast).SetEase(Ease.InOutQuad).SetDelay(0.5f));
		_CurrentTween = seq;
	}

	void Reset()
	{
		ResetTween();
		_Transform.position = _DefaultPosition;
		_Transform.rotation = _DefaultRotation;
	}

	void ResetTween()
	{
		_CurrentTween.Complete();
	}
}
