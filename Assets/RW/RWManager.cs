using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public sealed class RWManager
{

	/*private static readonly RWManager instance = null;
	public static RWManager Instance { 
		get {
			if (instance == null)
			{
				Debug.Log("RWMAnager Instantiate");
				instance = new RWManager();
			}
			return instance; 
		} 
	}*/
	private static volatile RWManager instance;
    private static object syncRoot = new System.Object();

	public static RWManager Instance
   	{
    	get {
			if (instance == null) 
			{
				lock (syncRoot) 
				{
				   if (instance == null) 
				      instance = new RWManager();
				}
			}
         	return instance;
		}
	}

	public float pxPerUnit;
	public Camera camera = null;
	public int				resourceScale = 1;
	public Resolution		wnd;

	public RWManager ()
	{
		//pxPerUnit = Camera.mainCamera.GetScreenHeight() / (Camera.mainCamera.orthographicSize*2f);
		pxPerUnit = 0;
		wnd.width = Display.main.renderingWidth;
		wnd.height = Display.main.renderingHeight;
		/*
#if UNITY_EDITOR || UNITY_ANDROID
		wnd.width = Display.main.renderingWidth;
		wnd.height = Display.main.renderingHeight;
#endif
#if UNITY_IOS && !UNITY_EDITOR
		wnd.width = Display.main.renderingHeight;
		wnd.height = Display.main.renderingWidth;
#endif
		*/
		Debug.Log("Window size:" +wnd.width+ " " + wnd.height);
		GetResourceScale ();
	}

	void GetResourceScale ()
	{
		if (wnd.height <= 480)
		{
			resourceScale = 1;
			pxPerUnit = 480 / (Camera.mainCamera.orthographicSize*2);
		}
		else if (wnd.height <= 960)
		{
			Debug.Log("iphone4");
			resourceScale = 2;
			pxPerUnit = 960 / (Camera.mainCamera.orthographicSize*2);
		}
		else if (wnd.height <= 1136)
		{
			Debug.Log("iphone5");
			resourceScale = 2;
			pxPerUnit = 1136 / (Camera.mainCamera.orthographicSize*2);
		}
		
		/*if (wnd.width <= 480) // TODO: Возможно нужно исправить на большее значение
		{
			//(352); (384 - 320)/2 + 320
			resourceScale = 1;
			pxPerUnit = 352 / (Camera.mainCamera.orthographicSize*2f);
		}
		else if (wnd.width <= 1280)
		{
			//(704) // (768 - 640)/2 + 640
			resourceScale = 2;
			pxPerUnit = 704 / (Camera.mainCamera.orthographicSize*2f);
		}
		else if (wnd.width <= 2048)
		{
			//(1664) // (1536 - 1280)/2 + 1280
			resourceScale = 4;
			pxPerUnit =  1408 / (Camera.mainCamera.orthographicSize*2f);
		}*/
		
	}
}