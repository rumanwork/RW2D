using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RWNode : MonoBehaviour {

	public List<Material> 	atlasMaterials;
	public Vector2			anchorPoint = new Vector2(0f,0f);
	public string			folderWithConfig = "Texture";
	//public Rect				textureRect;
	//public float			zOrder = 0;

	protected float 		_pxPerUnit;
	protected Mesh			_mesh;
	protected MeshFilter 	_meshFilter;
	protected MeshRenderer	_meshRender;
	protected Texture 		_atlasTexture;
	protected int			_materialIdx = 0;
	protected Rect			_rect;
	private Vector2[] 		uv;



	public virtual void Awake () {

		/*if (zOrder < 0)
		{
			Debug.LogWarning("zOrder can only have a positive valued!");
			zOrder = 0;
		}*/
		
		for (int i = 0;  i < atlasMaterials.Count; i++)
		{
			if (atlasMaterials[i] == null)
				atlasMaterials.RemoveAt(i);
		}
		
		_pxPerUnit = RWManager.Instance.pxPerUnit;

		_meshFilter = gameObject.GetComponent<MeshFilter>();
		if (_meshFilter == null)
			_meshFilter = gameObject.AddComponent<MeshFilter>();
		
		_meshRender = gameObject.GetComponent<MeshRenderer>();
		if (_meshRender == null)
			_meshRender = gameObject.AddComponent<MeshRenderer>();
		
		//_meshRender.castShadows = false;
		//_meshRender.receiveShadows = false;

		if (RWManager.Instance.resourceScale == 2)
		{
			if (atlasMaterials.Count > 1)
				_materialIdx = 1;
		}
		if (RWManager.Instance.resourceScale == 4)
		{
			if (atlasMaterials.Count > 2)
			{
				_materialIdx = 2;
			}
			else if (atlasMaterials.Count == 2)
			{
				_materialIdx = 1;
			}
		}


		_atlasTexture = atlasMaterials[_materialIdx].GetTexture("_MainTex");
		if (_atlasTexture == null)
			Debug.LogWarning("Texture not found in "+atlasMaterials[_materialIdx].name+" material!" );

	}

	public void CreateMesh ()
	{
		_mesh = new Mesh();
		
		Vector3[] verts  = new Vector3[4];
		Vector3[] normals = new Vector3[4];
		uv = new Vector2[4];
		int[] tri = new int[6];

		float width = (_rect.width/_pxPerUnit);
		float height = (_rect.height/_pxPerUnit);
		
		/*verts[0] = new Vector3(0, 0, 0);
		verts[1] = new Vector3(width, 0, 0);
		verts[2] = new Vector3(0, height, 0);
		verts[3] = new Vector3(width, height, 0);*/
		verts[0] = new Vector3(-anchorPoint.x*width , -anchorPoint.y*height, 0);//(float)zOrder/10f);
		verts[1] = new Vector3(width - anchorPoint.x*width, -anchorPoint.y*height, 0);//(float)zOrder/10f);
		verts[2] = new Vector3(-anchorPoint.x*width, height - anchorPoint.y*height, 0);//(float)zOrder/10f);
		verts[3] = new Vector3(width - anchorPoint.x*width, height - anchorPoint.y*height, 0);//(float)zOrder/10f);

		for (int i = 0; i < normals.Length; i++) {
			normals[i] = Vector3.up;
		}

		float minUvX = _rect.x /_atlasTexture.width;
		float minUvY = (_atlasTexture.height - _rect.y - _rect.height) /_atlasTexture.height;
		float maxUvX = (minUvX + _rect.width / _atlasTexture.width);
		float maxUvY = (minUvY + _rect.height / _atlasTexture.height);

		uv[0] = new Vector2(minUvX, minUvY);
		uv[1] = new Vector2(maxUvX, minUvY);
		uv[2] = new Vector2(minUvX, maxUvY);
		uv[3] = new Vector2(maxUvX, maxUvY);
		
		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 3;
		
		tri[3] = 0;
		tri[4] = 3;
		tri[5] = 1;
		
		_mesh.vertices = verts;
		_mesh.triangles = tri;
		_mesh.uv = uv;
		_mesh.normals = normals;


		_meshFilter.mesh = _mesh;
		_meshRender.sharedMaterial = atlasMaterials[_materialIdx];
		
		//Vector3 _pxPosition = Camera.mainCamera.WorldToScreenPoint( transform.position );
		//textureRect = new Rect(_pxPosition.x, _pxPosition.y, _rect.width, _rect.height);
	}

	public void UpdateMesh ()
	{
		float minUvX = _rect.x /_atlasTexture.width;
		float minUvY = (_atlasTexture.height - _rect.y - _rect.height) /_atlasTexture.height;
		float maxUvX = (minUvX + _rect.width / _atlasTexture.width);
		float maxUvY = (minUvY + _rect.height / _atlasTexture.height);
		
		uv[0] = new Vector2(minUvX, minUvY);
		uv[1] = new Vector2(maxUvX, minUvY);
		uv[2] = new Vector2(minUvX, maxUvY);
		uv[3] = new Vector2(maxUvX, maxUvY);

		_mesh.uv = uv;

		//Vector3 _pxPosition = Camera.mainCamera.WorldToScreenPoint( transform.position );
		//textureRect = new Rect(_pxPosition.x, _pxPosition.y, _rect.width, _rect.height);
	}
	

	void Update () 
	{

	}


	void OnDestroy ()
	{
		DestroyImmediate(_mesh, false);
	}
}
