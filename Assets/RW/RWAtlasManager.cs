using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class RWAtlasManager  {

	private static RWAtlasManager instance = null;
	public static RWAtlasManager Instance { 
		get {
			if (instance == null)
			{
				instance = new RWAtlasManager();
			}
			return instance; 
		} 
	}



	public Dictionary<string, Rect> 			spritesRect = new Dictionary<string, Rect>();
	private Dictionary<string, TextAsset>		_spriteConfigs = new Dictionary<string, TextAsset>();
	private TextAsset 							_configFile;

	public RWAtlasManager ()
	{

		Debug.Log("RWAtlasManager init...");
	


	}



	public Rect GetSpriteRect (string spriteName, string configFileName, string folderName = "Texture") // TexturePacker format
	{

		if (!spritesRect.ContainsKey(spriteName+"_"+configFileName))
		{
			if (!_spriteConfigs.ContainsKey(folderName+"/"+configFileName))
			{
				Debug.Log("Load config '"+folderName+"/"+configFileName+"'");
				_configFile = Resources.Load(folderName+"/"+configFileName, typeof(TextAsset)) as TextAsset; //TODO: Придумать, как правильно выставить путь до конфиг файла атласа.
				_spriteConfigs.Add(folderName+"/"+configFileName, _configFile);
			}
			else
			{
				_configFile = _spriteConfigs[folderName+"/"+configFileName];
			}
			if (_configFile != null)
			{
				var dict = Json.Deserialize(_configFile.text) as Dictionary<string,object>;
				var dict2 = dict["frames"] as Dictionary<string,object>;
				var sprite = dict2[spriteName] as Dictionary<string,object>;
				if (sprite == null)
					Debug.LogWarning("Sprite not found!");
				var frame = sprite["frame"] as Dictionary<string,object>;
	
				long x = (long)frame["x"];
				long y = (long)frame["y"];
				long width = (long)frame["w"];
				long height = (long)frame["h"];
				Rect res = new Rect(x, y, width, height);
				spritesRect.Add(spriteName+"_"+configFileName, res);
				return res;
			}
			else
			{
				Debug.LogError("Sprite config file is not found!");
				return new Rect(0,0,0,0);
			}
		}
		else {
			return spritesRect[spriteName+"_"+configFileName];
		}
	}
}
