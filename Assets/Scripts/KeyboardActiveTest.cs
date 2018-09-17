using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardActiveTest : MonoBehaviour 
{
	[SerializeField]
	KeyCode _ActiveKey = KeyCode.Space;

	[SerializeField]
	Renderer _Renderer;

	void Start () 
	{
		if(_Renderer)
		{
			_Renderer.enabled = false;
		}
	}
	
	void Update () 
	{
		if(_Renderer)
		{
			_Renderer.enabled = Input.GetKey(_ActiveKey);
		}
	}
}
