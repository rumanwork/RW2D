using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

[ExecuteInEditMode]
/// <summary>
/// RW animation sprite.
/// <param name="animationConfig">Конфигурационный файл (TextAsset) анимации. ({"animationName":{"duration:1,"frames":["sprite1.png",...]"}})</param>
/// <param name="animationName">Имя анимации для текущего объекта</param>
/// <param name="srartFrame">Номер фрейма, с которого стартует анимация (0 default)</param>
/// <param name="repeat">Зациклить анимацию (true default)</param>
/// <param name="autoPlay">Автостарт анимации (true default)</param>
/// </summary>
public class RWAnimationSprite : RWNode
{
	public TextAsset 	animationConfig;
	public string 		animationName;
	public int 			startFrame = 0;
	public	bool		repeat = true;
	public	bool 		autoPlay = true;

	private Dictionary<string,object> _dict;
	private double _duration;
	private List<object> _frames;
	private int _currentFrame;
	private bool _loop = true;
	private Action callback;

	public override void Awake ()
	{
		_currentFrame = startFrame;

		base.Awake();
		if (animationConfig == null)
		{
			Debug.LogWarning("AnimationConfig has not been assigned.");
			return;
		}
		_dict = Json.Deserialize(animationConfig.text) as Dictionary<string,object>;

		_duration = (double)((Dictionary<string,object>)_dict[animationName])["duration"];
		_frames = (List<object>)((Dictionary<string,object>)_dict[animationName])["frames"];
		_rect = RWAtlasManager.Instance.GetSpriteRect((string)_frames[startFrame], _atlasTexture.name);
		CreateMesh();
		if (autoPlay)
			PlayAnimation(animationName);
	}


	public void PlayAnimation (string animationName, bool repeatAnimation = true, Action action = null)
	{
		repeat = repeatAnimation;
		StopCoroutine("Animate");
		_currentFrame = 0;
		_loop = true;
		_duration = (double)((Dictionary<string,object>)_dict[animationName])["duration"];
		_frames = (List<object>)((Dictionary<string,object>)_dict[animationName])["frames"];
		//StartCoroutine(Animate(_duration));
		StartCoroutine("Animate");
		if (!repeatAnimation)
		{
			callback = action;
		}
	}

	public void StopAnimation ()
	{
		_loop = false;
	}

	IEnumerator Animate() {
		while (_loop)
		{

			if (_currentFrame == _frames.Count)
			{
				_currentFrame = 0;
				if (!repeat)
				{
					_loop = false;
					if (callback != null)
						callback();
					break;
				}
			}

			_rect = RWAtlasManager.Instance.GetSpriteRect((string)_frames[_currentFrame], _atlasTexture.name);
			UpdateMesh();
			
			_currentFrame++;
			yield return new WaitForSeconds((float)_duration);
		}
	}

}