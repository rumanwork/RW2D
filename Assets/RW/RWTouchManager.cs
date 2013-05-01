using UnityEngine;
using System.Collections;


public struct RWTouch 
{
	public Vector2 position;
	public Vector3 worldPosition;
	public Vector2 deltaPosition;
	public TouchPhase phase;
}

public interface RWSingleTouchableInterface
{
	void HandleSingleTouchBegan(RWTouch touch);
	
	void HandleSingleTouchMoved(RWTouch touch);
	
	void HandleSingleTouchEnded(RWTouch touch);
}

public class RWTouchManager : MonoBehaviour {

	public RWTouch 		rwTouch;

	private Vector2 	_previousPosition = new Vector2(0,0);
	private bool		_isMouseEmulateTouch = true;

	void Start () 
	{

/*#if UNITY_ANDROID
		_isMouseEmulateTouch = false;
#endif
#if UNITY_IPHONE
		_isMouseEmulateTouch = false;
#endif
#if UNITY_EDITOR
		_isMouseEmulateTouch = true;
#endif*/
		// Эмулируем тачи через мышь
		_isMouseEmulateTouch = true;
	}

	void Update () 
	{
		rwTouch = new RWTouch();

		if (_isMouseEmulateTouch)
		{
			rwTouch.position = Input.mousePosition;

			if (Input.GetMouseButtonDown(0))
			{
				rwTouch.deltaPosition = Vector2.zero;

				_previousPosition = rwTouch.position;
				rwTouch.worldPosition = Camera.mainCamera.ScreenToWorldPoint(new Vector3(rwTouch.position.x, rwTouch.position.y, 0));
				rwTouch.phase = TouchPhase.Began;
				gameObject.SendMessage("HandleSingleTouchBegan", rwTouch, SendMessageOptions.DontRequireReceiver);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				rwTouch.deltaPosition = new Vector2(rwTouch.position.x - _previousPosition.x, rwTouch.position.y - _previousPosition.y);
				_previousPosition = rwTouch.position;
				rwTouch.worldPosition = Camera.mainCamera.ScreenToWorldPoint(new Vector3(rwTouch.position.x, rwTouch.position.y, 0));
				rwTouch.phase = TouchPhase.Ended;
				gameObject.SendMessage("HandleSingleTouchEnded", rwTouch, SendMessageOptions.DontRequireReceiver);
			}
			else if (Input.GetMouseButton(0))
			{
				rwTouch.deltaPosition = new Vector2(rwTouch.position.x - _previousPosition.x, rwTouch.position.y - _previousPosition.y);
				_previousPosition = rwTouch.position;
				if (rwTouch.deltaPosition != Vector2.zero)
				{
					rwTouch.phase = TouchPhase.Moved;
					rwTouch.worldPosition = Camera.mainCamera.ScreenToWorldPoint(new Vector3(rwTouch.position.x, rwTouch.position.y, 0));
					gameObject.SendMessage("HandleSingleTouchMoved", rwTouch, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		else
		{
			foreach (Touch touch in Input.touches)
			{
				rwTouch.position = touch.position;
				if (touch.phase == TouchPhase.Began)
				{
					rwTouch.deltaPosition = Vector2.zero;
					_previousPosition = rwTouch.position;
					rwTouch.worldPosition = Camera.mainCamera.ScreenToWorldPoint(new Vector3(rwTouch.position.x, rwTouch.position.y, 0));
					rwTouch.phase = TouchPhase.Began;
					gameObject.SendMessage("HandleSingleTouchBegan", rwTouch, SendMessageOptions.DontRequireReceiver);
				}
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					rwTouch.deltaPosition = new Vector2(rwTouch.position.x - _previousPosition.x, rwTouch.position.y - _previousPosition.y);
					
					_previousPosition = rwTouch.position;
					rwTouch.worldPosition = Camera.mainCamera.ScreenToWorldPoint(new Vector3(rwTouch.position.x, rwTouch.position.y, 0));
					rwTouch.phase = TouchPhase.Ended;
					gameObject.SendMessage("HandleSingleTouchEnded", rwTouch, SendMessageOptions.DontRequireReceiver);
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					rwTouch.deltaPosition = new Vector2(rwTouch.position.x - _previousPosition.x, rwTouch.position.y - _previousPosition.y);
					_previousPosition = rwTouch.position;
					if (rwTouch.deltaPosition != Vector2.zero)
					{
						rwTouch.phase = TouchPhase.Moved;
						rwTouch.worldPosition = Camera.mainCamera.ScreenToWorldPoint(new Vector3(rwTouch.position.x, rwTouch.position.y, 0));
						gameObject.SendMessage("HandleSingleTouchMoved", rwTouch, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}

	}
}
