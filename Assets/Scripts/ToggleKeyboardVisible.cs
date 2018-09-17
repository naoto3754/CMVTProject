using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleKeyboardVisible : MonoBehaviour 
{
	[SerializeField]
	private KeyCode _ActiveKey = KeyCode.Space;
	public KeyCode ActiveKey { get { return _ActiveKey; } }

	private Image _Renderer;

	void Awake()
	{
		_Renderer = GetComponent<Image>();
	}

	public void SetVisible(bool visible)
	{
		if(_Renderer)
		{
			_Renderer.enabled = visible;
		}
	}
}
