using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
/// <summary>
/// RW Sprite.
/// <param name="spriteName">Имя спрайта из спрайтшит</param>
/// </summary>
public class RWSprite : RWNode {

	public string			spriteName;

	public override void Awake () 
	{
		if (atlasMaterials.Count == 0)
		{
			Debug.LogError("Materials are not assigned an object \""+gameObject.name+"\"");
			return;
		}
		base.Awake ();

		_rect = RWAtlasManager.Instance.GetSpriteRect(spriteName, _atlasTexture.name, folderWithConfig);
		CreateMesh ();
	}

	void Update () 
	{

	}
}
