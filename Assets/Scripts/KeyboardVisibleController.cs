using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardVisibleController : MonoBehaviour 
{
	[SerializeField]
	private List<Image> _Defaults;

	private Dictionary<KeyCode, List<ToggleKeyboardVisible>> _Map;

	private List<KeyCode> _PushKeys;

	void Start () 
	{
		_PushKeys = new List<KeyCode>();
		_Map = new Dictionary<KeyCode, List<ToggleKeyboardVisible>>();
		var visibles = GetComponentsInChildren<ToggleKeyboardVisible>();
		foreach(var visible in visibles)
		{
			var key = visible.ActiveKey;
			if(!_Map.ContainsKey(key))
			{
				_Map[key] = new List<ToggleKeyboardVisible>();
			}
			_Map[key].Add(visible);
			visible.SetVisible(false);
		}
	}
	
	void Update () 
	{
		CheckInput();
	}

	void CheckInput()
	{
		foreach(var key in _Map.Keys)
		{
			if(Input.GetKeyDown(key))
			{
				AddPushKey(key);
			}
			if(Input.GetKeyUp(key))
			{
				RemovePushKey(key);
			}
		}
	}

	void CheckKey()
	{
		if(_PushKeys.Count <= 0)
		{
			SetDefaultVisible(true);
			foreach(var key in _Map.Keys)
			{
				foreach(var visible in _Map[key])
				{
					visible.SetVisible(false);
				}
			}
		}
		else
		{
			SetDefaultVisible(false);
			var pushKey = _PushKeys[_PushKeys.Count - 1];
			foreach(var key in _Map.Keys)
			{
				foreach(var visible in _Map[key])
				{
					visible.SetVisible(key == pushKey);
				}
			}
		}
	}

	void SetDefaultVisible(bool visible)
	{
		foreach(var image in _Defaults)
		{
			image.enabled = visible;
		}
	}

	public void AddPushKey(KeyCode key)
	{
		if(key == KeyCode.None)
		{
			return;
		}

		if(!_PushKeys.Contains(key))
		{
			_PushKeys.Add(key);
			CheckKey();
		}
	}

	public void RemovePushKey(KeyCode key)
	{
		if(key == KeyCode.None)
		{
			return;
		}

		if(Input.GetKey(key))
		{
			return;
		}

		_PushKeys.Remove(key);
		CheckKey();
	}
}
