using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {

	private float scale_factor= 0.07f;  
	private float MAXSCALE = 6.0f, MIN_SCALE = 0.6f; // zoom-in and zoom-out limits
	private bool isMousePressed;
	private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);
	private Vector2 midPoint = new Vector2(0,0);
	private Vector2 ScreenSize;
	private Vector3 originalPos;
	private GameObject parentObject;

	void Start () {
		
		parentObject = new GameObject("ParentObject");
		parentObject.transform.localScale = new Vector3(1,1,1);
		parentObject.transform.parent = transform.parent;
		parentObject.transform.position = new Vector3(transform.position.x*-1, transform.position.y*-1, transform.position.z);
		transform.parent = parentObject.transform;
		
		ScreenSize = UICamera.mainCamera.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));
		originalPos = Vector3.zero;
		
	}
	
	void Update () {
		
		if(Input.touchCount>0){
			
			Touch touch = Input.GetTouch(0);
			
			if (Input.touchCount == 1){
				
				if(Input.GetTouch(0).phase == TouchPhase.Moved){
					
					Vector3 diff = touch.deltaPosition * 0.1f;
					
					Vector3 pos = transform.position + diff;
					
					if(pos.x > ScreenSize.x * (parentObject.transform.localScale.x-1))
						pos.x = ScreenSize.x * (parentObject.transform.localScale.x-1);
					
					if(pos.x < ScreenSize.x * (parentObject.transform.localScale.x-1)*-1)
						pos.x = ScreenSize.x * (parentObject.transform.localScale.x-1)*-1;
					
					if(pos.y > ScreenSize.y * (parentObject.transform.localScale.y-1))
						pos.y = ScreenSize.y * (parentObject.transform.localScale.y-1);
					
					if(pos.y < ScreenSize.y * (parentObject.transform.localScale.y-1)*-1)
						pos.y = ScreenSize.y * (parentObject.transform.localScale.y-1)*-1;
					
					transform.position = pos;
					
				}
				
			}
			
			if (Input.touchCount == 2){
				
				checkForMultiTouch();
				
			}
			
		}
		
	}
	
	void checkForMultiTouch(){
		
		if(Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved){
			
			midPoint = new Vector2(((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x)/2), ((Input.GetTouch(0).position.y + Input.GetTouch(1).position.y)/2));
			
			midPoint = UICamera.currentCamera.ScreenToWorldPoint(midPoint);
			
			curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
			
			prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
			
			float touchDelta = curDist.magnitude - prevDist.magnitude;
			
			if(touchDelta>0)
			{
				if(parentObject.transform.localScale.x < MAXSCALE && parentObject.transform.localScale.y < MAXSCALE)
				{
					Vector3 scale = new Vector3(parentObject.transform.localScale.x + scale_factor, parentObject.transform.localScale.y + scale_factor, 1);
					scale.x = (scale.x > MAXSCALE) ? MAXSCALE : scale.x; // if true, maxscale, else scale.x
					scale.y = (scale.y > MAXSCALE) ? MAXSCALE : scale.y;
					scaleFromPosition(scale,midPoint);
				}
				
			}
			else if(touchDelta<0)
			{
				if(parentObject.transform.localScale.x > MIN_SCALE && parentObject.transform.localScale.y > MIN_SCALE)
				{
					Vector3 scale = new Vector3(parentObject.transform.localScale.x + scale_factor*-1, parentObject.transform.localScale.y + scale_factor*-1, 1);
					scale.x = (scale.x < MIN_SCALE) ? MIN_SCALE : scale.x;
					scale.y = (scale.y < MIN_SCALE) ? MIN_SCALE : scale.y;
					scaleFromPosition(scale,midPoint);
				}
			}
		}else if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Canceled)) 
		{
			if(parentObject.transform.localScale.x < 1 || parentObject.transform.localScale.y < 1)
			{
				parentObject.transform.localScale = Vector3.one;
				parentObject.transform.position = new Vector3(originalPos.x*-1, originalPos.y*-1, originalPos.z);
				transform.position = originalPos;
			}
			else
			{
				Vector3 pos = transform.position;
				if(pos.x > ScreenSize.x * (parentObject.transform.localScale.x-1))
					pos.x = ScreenSize.x * (parentObject.transform.localScale.x-1);
				if(pos.x < ScreenSize.x * (parentObject.transform.localScale.x-1)*-1)
					pos.x = ScreenSize.x * (parentObject.transform.localScale.x-1)*-1;
				if(pos.y > ScreenSize.y * (parentObject.transform.localScale.y-1))
					pos.y = ScreenSize.y * (parentObject.transform.localScale.y-1);
				if(pos.y < ScreenSize.y * (parentObject.transform.localScale.y-1)*-1)
					pos.y = ScreenSize.y * (parentObject.transform.localScale.y-1)*-1;
				transform.position = pos;
			}
		}
		
	}
	
	static Vector3 prevPos = Vector3.zero;
	private void scaleFromPosition(Vector3 scale, Vector3 fromPos)
	{
		if(!fromPos.Equals(prevPos))
		{
			Vector3 prevParentPos = parentObject.transform.position;
			parentObject.transform.position = fromPos;    
			Vector3 diff = parentObject.transform.position - prevParentPos;
			Vector3 pos = new Vector3(diff.x/parentObject.transform.localScale.x*-1, diff.y/parentObject.transform.localScale.y*-1, transform.position.z);
			transform.localPosition = new Vector3(transform.localPosition.x + pos.x, transform.localPosition.y+pos.y, pos.z);
		}
		parentObject.transform.localScale = scale;
		prevPos = fromPos;
	}
}
