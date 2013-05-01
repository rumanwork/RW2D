using System;
using UnityEngine;

[ExecuteInEditMode]
/// <summary>
/// RW repeat sprite.
/// Для корректной работы использовать материал с одним спрайтом (1 картинка на 1 RepeatSprite!)
/// <param name="repeat">Количество повторений спрайта по X и Y</param>
/// </summary>
public class RWRepeatSprite : RWNode
{

	public Vector2	repeat = new Vector2(1,1);	

	public override void Awake ()
	{
		if (atlasMaterials.Count == 0)
		{
			Debug.LogError("Materials are not assigned an object \""+gameObject.name+"\"");
			return;
		}
		base.Awake ();

		_rect = new Rect(0,0, _atlasTexture.width * repeat.x, _atlasTexture.height * repeat.y);
		CreateMesh ();

	}
}