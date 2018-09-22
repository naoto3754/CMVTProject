using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardActiveTest : MonoBehaviour 
{
	[SerializeField]
	KeyCode _ActiveKey = KeyCode.Space;

	Image _Renderer;

	void Start () 
	{
		_Renderer = GetComponent<Image>();

		if(_Renderer)
		{
			_Renderer.enabled = false;
		}
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(_ActiveKey))
		{
			if(_Renderer)
			{
				_Renderer.enabled = !_Renderer.enabled;
			}
		}
	}
}
