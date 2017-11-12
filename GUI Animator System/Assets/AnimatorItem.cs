using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;

public enum AnimatorSystemPosition {
	Custom,
	UpperLeft,
	UpperCenter,
	UpperRight,
	MiddleLeft,
	MiddleCenter,
	MiddleRight,
	BottomLeft,
	BottomCenter,
	BottomRight,
	UpperLeftScreen,
	UpperCenterScreen,
	UpperRightScreen,
	MiddleLeftScreen,
	MiddleCenterScreen,
	MiddleRightScreen,
	BottomLeftScreen,
	BottomCenterScreen,
	BottomRightScreen,
}

[System.Serializable]
public class AnimatorItemSettings {
	public bool state;

	public RectTransform canvasRect;
	public Vector3[] canvasRectCorners;
	public RectTransform parentRect;
	public Vector3[] parentRectCorners;
	public RectTransform rectTransform;
	public Vector3[] rectTransformBounds;
	public bool loop;
	public float time;
	public float delay;

	public bool animating;
	public EaseType easeType;
	public EaseMode easeMode;

	public delegate void StartDelegate ();
	public event StartDelegate OnStart;
	public delegate void CompleteDelegate ();
	public event CompleteDelegate OnComplete;

	public AnimatorItemSettings () {

	}

	public void SetUp (GameObject go) {
		canvasRect = go.GetComponentInParent<Canvas> ().GetComponent<RectTransform> ();
		canvasRectCorners = new Vector3[4];
		canvasRect.GetLocalCorners (canvasRectCorners);

		parentRect = go.transform.parent.GetComponent<RectTransform>();
		parentRectCorners = new Vector3[4];
		parentRect.GetLocalCorners (parentRectCorners);

		rectTransform = go.GetComponent<RectTransform> ();
		CalculateRectTransformBounds ();
	}

	private void CalculateRectTransformBounds () {
		Rect rect = rectTransform.rect;
		rectTransformBounds[0] = new Vector2 (rect.xMin, rect.yMin);
		rectTransformBounds[1] = new Vector2 (rect.xMin, rect.y);
		rectTransformBounds[2] = new Vector2 (rect.xMin, rect.yMax);
		rectTransformBounds[3] = new Vector2 (rect.x, rect.yMax);
		rectTransformBounds[4] = new Vector2 (rect.xMax, rect.yMax);
		rectTransformBounds[5] = new Vector2 (rect.xMax, rect.y);
		rectTransformBounds[6] = new Vector2 (rect.xMax, rect.yMin);
		rectTransformBounds[7] = new Vector2 (rect.x, rect.yMin);
	}

	public void GetRectCenter () {

	}

	public void SetRectPosition () {

	}

	//public float In () {
	//	time.
	//}
}

[System.Serializable]
public class Movement : AnimatorItemSettings {
	public AnimatorSystemPosition moveFrom;
	public Vector2 startPosition;
	public Vector2 endPosition;

	public void In () {
		//TweenController.Tween (gameObject, "MovementIn", true, 0, 1, time, (tween) => {
		//	//transform.po
		//});
	}

	public Vector2 GetPosition (AnimatorSystemPosition position) {
		switch (position) {
			case AnimatorSystemPosition.Custom:
				return startPosition;
			case AnimatorSystemPosition.UpperLeft:
			//rect.rect.
			case AnimatorSystemPosition.UpperCenter:
				break;
			case AnimatorSystemPosition.UpperRight:
				break;
			case AnimatorSystemPosition.MiddleLeft:
				break;
			case AnimatorSystemPosition.MiddleCenter:
				break;
			case AnimatorSystemPosition.MiddleRight:
				break;
			case AnimatorSystemPosition.BottomLeft:
				break;
			case AnimatorSystemPosition.BottomCenter:
				break;
			case AnimatorSystemPosition.BottomRight:
				break;
			case AnimatorSystemPosition.UpperLeftScreen:
				break;
			case AnimatorSystemPosition.UpperCenterScreen:
				break;
			case AnimatorSystemPosition.UpperRightScreen:
				break;
			case AnimatorSystemPosition.MiddleLeftScreen:
				break;
			case AnimatorSystemPosition.MiddleCenterScreen:
				break;
			case AnimatorSystemPosition.MiddleRightScreen:
				break;
			case AnimatorSystemPosition.BottomLeftScreen:
				break;
			case AnimatorSystemPosition.BottomCenterScreen:
				break;
			case AnimatorSystemPosition.BottomRightScreen:
				break;
		}
		return Vector2.zero;
	}
}

public class AnimatorItem : MonoBehaviour {

	public Movement movement;

	public RectTransform transform;

	//public class Rotation : AnimatorItemSettings {
	//	public Vector3 startEuler;
	//	public Vector3 endEuler;
	//}

	//public class Scale : AnimatorItemSettings {
	//	public Vector3 startScale;
	//	public Vector3 endScale;
	//}

	//public class Fade : AnimatorItemSettings {
	//	public float startAlpha;
	//	public float endAlpha;
	//}

	#region Class members
	#endregion

	#region Class accesors
	#endregion

	#region MonoBehaviour overrides
	private void OnDrawGizmos () {
		RectTransform trans = GetComponent<RectTransform> ();
		trans.position = movement.GetPosition (AnimatorSystemPosition.UpperLeft);
	}
	#endregion

	#region Super class overrides
	#endregion

	#region Class implementation
	#endregion

	#region Interface implementation
	#endregion
}