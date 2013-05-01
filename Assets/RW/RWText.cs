using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CharParam
{
	public int x;
	public int y;
	public int width;
	public int height;
	public int xoffset;
	public int yoffset;
	public int xadvance;
}
[ExecuteInEditMode]
/// <param name="fontMaterials">Материалы шрифтов, сгенерированые в программе bmGlyph</param>
/// <param name="fontConfig">Конфигурационные файлы для шрифтов, сгенерированых в программе bmGlyph</param>
public class RWText : MonoBehaviour {

	public List<Material> 	fontMaterials; // bmGlyph
	public TextAsset[]		fontConfig; // bmGlyph
	public string			text;
	public float			letterSpace = 0;


	private float 			_pxPerUnit;
	private Texture 		_atlasTexture;
	private int				_materialIdx = 0;
	private Mesh			_mesh, _createdMesh;
	private CombineInstance[] _combine;
	private MeshFilter		_meshFilter;
	private Dictionary<int, CharParam> chars = new Dictionary<int, CharParam>();
	private int			_baseHeight;
	private string			_oldText;
	private float			_vertsYOffset;

	void Start () {

		_pxPerUnit = RWManager.Instance.pxPerUnit;

		if (RWManager.Instance.resourceScale == 2)
		{
			if (fontMaterials.Count > 1)
			{
				_materialIdx = 1;
				letterSpace *=2;
			}
		}
		if (RWManager.Instance.resourceScale == 4)
		{
			if (fontMaterials.Count > 2)
			{
				_materialIdx = 2;
				letterSpace *=4;
			}
			else if (fontMaterials.Count == 2)
			{
				_materialIdx = 1;
				letterSpace *=2;
			}
		}
		_atlasTexture = fontMaterials[_materialIdx].GetTexture("_MainTex");
		_meshFilter = gameObject.GetComponent<MeshFilter>();
		_mesh = new Mesh();
		_meshFilter.mesh = _mesh;
		renderer.material = fontMaterials[_materialIdx];

		ParseConfig ();
		CreateMesh();
		_oldText = text;
	}
	

	void CreateMesh ()
	{
		Vector3[] 	verts  = new Vector3[text.Length * 4];
		Vector3[] 	normals = new Vector3[text.Length * 4];
		Vector2[] 	uv = new Vector2[text.Length * 4];
		int[] 		tri = new int[text.Length * 6];

		int i = 0;
		float charOffsetX = 0;
		foreach (char c in text)
		{
			CharParam cp = chars[(int)c];
			if ((int)c == 32) // Пробел
			{
				cp.width = 20;
			}
			_vertsYOffset = _baseHeight -cp.height;

			_vertsYOffset = _vertsYOffset/_pxPerUnit;

			Rect rect = new Rect(cp.x, cp.y, cp.width, cp.height);
			//cp.yoffset -= 44;
			//cp.xoffset = 0;
			float width = (rect.width/_pxPerUnit);
			float height = (rect.height/_pxPerUnit);
			
			verts[i*4] = new Vector3(charOffsetX/_pxPerUnit + cp.xoffset/_pxPerUnit, _vertsYOffset - cp.yoffset/_pxPerUnit, 0);
			verts[i*4+1] = new Vector3(charOffsetX/_pxPerUnit + width + cp.xoffset/_pxPerUnit, _vertsYOffset -cp.yoffset/_pxPerUnit, 0);
			verts[i*4+2] = new Vector3(charOffsetX/_pxPerUnit + cp.xoffset/_pxPerUnit, height + _vertsYOffset - cp.yoffset/_pxPerUnit, 0);
			verts[i*4+3] = new Vector3(charOffsetX/_pxPerUnit + width + cp.xoffset/_pxPerUnit, height + _vertsYOffset - cp.yoffset/_pxPerUnit, 0);

			float minUvX = rect.x /_atlasTexture.width;
			float minUvY = (_atlasTexture.height - rect.y - rect.height) /_atlasTexture.height;
			float maxUvX = (minUvX + rect.width / _atlasTexture.width);
			float maxUvY = (minUvY + rect.height / _atlasTexture.height);

			uv[i*4+0] = new Vector2(minUvX, minUvY);
			uv[i*4+1] = new Vector2(maxUvX, minUvY);
			uv[i*4+2] = new Vector2(minUvX, maxUvY);
			uv[i*4+3] = new Vector2(maxUvX, maxUvY);

			tri[i*6+0] = i*4+0;
			tri[i*6+1] = i*4+2;
			tri[i*6+2] = i*4+3;
			
			tri[i*6+3] = i*4+0;
			tri[i*6+4] = i*4+3;
			tri[i*6+5] = i*4+1;
			_mesh.Clear();
			_mesh.vertices = verts;
			_mesh.triangles = tri;
			_mesh.uv = uv;
			_mesh.normals = normals;

			i++;
			charOffsetX += cp.xadvance+letterSpace;
		}
	}

	void ParseConfig ()
	{
		string[] lines = fontConfig[_materialIdx].text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
		foreach (string line in lines)
		{
			if (line.IndexOf("base=") != -1)
			{
				string[] charParams = line.Split(new string[] {"base="}, System.StringSplitOptions.None);
				_baseHeight = int.Parse(charParams[1].Split(new string[] {" "}, System.StringSplitOptions.None)[0]);
			}

			if (line.IndexOf("char id=") != -1)
			{
				string[] charParams = line.Split(new string[] {" "}, System.StringSplitOptions.None);
				int id = int.Parse( charParams[1].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				CharParam charParam = new CharParam();
				charParam.x = int.Parse( charParams[2].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				charParam.y = int.Parse( charParams[3].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				charParam.width = int.Parse( charParams[4].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				charParam.height = int.Parse( charParams[5].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				charParam.xoffset = int.Parse( charParams[6].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				charParam.yoffset = int.Parse( charParams[7].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				charParam.xadvance = int.Parse( charParams[8].Split(new string[] {"="}, System.StringSplitOptions.None)[1] );
				chars.Add(id, charParam);
			}
		}

	}

	void Update () {
		if (_oldText != text)
		{
			CreateMesh();
			_oldText = text;
		}
		//text = "Time: " + Time.time.ToString();
	}
}
