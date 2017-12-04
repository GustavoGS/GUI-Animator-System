using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;
using System;

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
	BottomRight
}

public enum AnimatorSystemPositionMode {
	Canvas, Parent
}

public struct AnimatorSystemRect {
	public enum Space { Local, Global }

	public RectTransform rectTransform;

	private Rect LocalRect;
	public Rect localRect {
		get {
			CalculateRect (Space.Local);
			return LocalRect;
		}
		set { }
	}
	public Vector2 localCenter {
		get { return localRect.center; }
	}

	private Vector2[] LocalCorners;
	public Vector2[] localCorners {
		get {
			CalculateAllCorners (Space.Local);
			return LocalCorners;
		}
	}

	private Rect GlobalRect;
	public Rect globalRect {
		get {
			CalculateGlobalRect ();
			return GlobalRect;
		}
		set { }
	}
	public Vector2 globalCenter {
		get { return globalRect.center; }
	}

	private Vector2[] GlobalCorners;
	public Vector2[] globalCorners {
		get {
			CalculateAllCorners (Space.Global);
			return GlobalCorners;
		}
	}

	public AnimatorSystemRect (RectTransform rectTransform) {
		this.rectTransform = rectTransform;
		GlobalRect = new Rect ();
		LocalRect = new Rect ();
		GlobalCorners = new Vector2[8];
		LocalCorners = new Vector2[8];
	}

	public void CalculateRect (Space space) {
		Rect rect = (space == Space.Local) ? LocalRect : GlobalRect;

		Vector2 rectSize = rectTransform.rect.size;
		rectSize.x *= rectTransform.lossyScale.x;
		rectSize.y *= rectTransform.lossyScale.y;

		if (space == Space.Local) {
			rect.size = rectSize;
			rect.center = rectTransform.rect.center;
		}
		else {
			rect.size = rectSize;
			rect.center = rectTransform.rect.center + (Vector2) rectTransform.position;
		}

		CalculateAllCorners (space);
	}

	public void CalculateAllCorners (Space space) {
		if (space == Space.Local) {
			for (int i = 0; i < 8; i++)
				LocalCorners[i] = GetCorner (i, space);
		}
		else {
			for (int i = 0; i < 8; i++)
				GlobalCorners[i] = GetCorner (i, space);
		}
	}

	public Vector2 GetCorner (int cornerIndex, Space space) {
		Rect rect = (space == Space.Local) ? LocalRect : GlobalRect;

		switch (cornerIndex) {
			case 0:
				return new Vector2 (rect.xMin, rect.yMin);
			case 1:
				return new Vector2 (rect.xMin, rect.center.y);
			case 2:
				return new Vector2 (rect.xMin, rect.yMax);
			case 3:
				return new Vector2 (rect.center.x, rect.yMax);
			case 4:
				return new Vector2 (rect.xMax, rect.yMax);
			case 5:
				return new Vector2 (rect.xMax, rect.center.y);
			case 6:
				return new Vector2 (rect.xMax, rect.yMin);
			case 7:
				return new Vector2 (rect.center.x, rect.yMin);
		}

		return Vector2.zero;
	}
}

[System.Serializable]
public class AnimatorItemSettings {
	public bool state;

	public GameObject go;
	public RectTransform canvasRT;
	public AnimatorSystemRect canvasRect;
	public RectTransform parentRT;
	public AnimatorSystemRect parentRect;
	public RectTransform rectTransform;
	public AnimatorSystemRect rectTransformRect;
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

	protected void CallStart () {
		if (OnStart != null)
			OnStart ();
	}

	protected void CallComplete () {
		if (OnComplete != null)
			OnComplete ();
	}

	public void SetUp (GameObject go) {
		this.go = go;
		canvasRT = go.GetComponentInParent<Canvas> ().GetComponent<RectTransform> ();
		canvasRect.
		CalculateCanvasBounds ();

		parentRT = go.transform.parent.GetComponent<RectTransform> ();
		CalculateParentBounds ();

		rectTransform = go.GetComponent<RectTransform> ();
		CalculateRectTransformBounds ();
	}

	private void CalculateCanvasBounds () {
		Rect canvasRect = canvasRT.rect;
		Vector3 rectSize = canvasRect.size;
		rectSize.x *= canvasRT.lossyScale.x;
		rectSize.y *= canvasRT.lossyScale.y;
		canvasRect.size = rectSize;
		canvasRect.center = Vector3.zero;

		canvasRTBounds = new Vector3[8];
		canvasRTBounds[0] = new Vector2 (canvasRect.xMin, canvasRect.yMin);
		canvasRTBounds[1] = new Vector2 (canvasRect.xMin, canvasRect.center.y);
		canvasRTBounds[2] = new Vector2 (canvasRect.xMin, canvasRect.yMax);
		canvasRTBounds[3] = new Vector2 (canvasRect.center.x, canvasRect.yMax);
		canvasRTBounds[4] = new Vector2 (canvasRect.xMax, canvasRect.yMax);
		canvasRTBounds[5] = new Vector2 (canvasRect.xMax, canvasRect.center.y);
		canvasRTBounds[6] = new Vector2 (canvasRect.xMax, canvasRect.yMin);
		canvasRTBounds[7] = new Vector2 (canvasRect.center.x, canvasRect.yMin);
	}

	private void CalculateParentBounds () {
		Rect parentRect = parentRT.rect;
		Vector2 rectSize = parentRect.size;
		rectSize.x *= parentRT.lossyScale.x;
		rectSize.y *= parentRT.lossyScale.y;
		parentRect.size = rectSize;
		parentRect.center = Vector3.zero;

		parentRTBounds = new Vector3[8];
		parentRTBounds[0] = new Vector2 (parentRect.xMin, parentRect.yMin);
		parentRTBounds[1] = new Vector2 (parentRect.xMin, parentRect.center.y);
		parentRTBounds[2] = new Vector2 (parentRect.xMin, parentRect.yMax);
		parentRTBounds[3] = new Vector2 (parentRect.center.x, parentRect.yMax);
		parentRTBounds[4] = new Vector2 (parentRect.xMax, parentRect.yMax);
		parentRTBounds[5] = new Vector2 (parentRect.xMax, parentRect.center.y);
		parentRTBounds[6] = new Vector2 (parentRect.xMax, parentRect.yMin);
		parentRTBounds[7] = new Vector2 (parentRect.center.x, parentRect.yMin);
	}

	private void CalculateRectTransformBounds () {
		Rect rectTransformRect = rectTransform.rect;
		Vector2 rectSize = rectTransformRect.size;
		rectSize.x *= rectTransform.lossyScale.x;
		rectSize.y *= rectTransform.lossyScale.y;
		rectTransformRect.size = rectSize;
		rectTransformRect.center = Vector3.zero;

		rectTransformBounds = new Vector3[8];
		rectTransformBounds[0] = new Vector2 (rectTransformRect.xMin, rectTransformRect.yMin);
		rectTransformBounds[1] = new Vector2 (rectTransformRect.xMin, rectTransformRect.center.y);
		rectTransformBounds[2] = new Vector2 (rectTransformRect.xMin, rectTransformRect.yMax);
		rectTransformBounds[3] = new Vector2 (rectTransformRect.center.x, rectTransformRect.yMax);
		rectTransformBounds[4] = new Vector2 (rectTransformRect.xMax, rectTransformRect.yMax);
		rectTransformBounds[5] = new Vector2 (rectTransformRect.xMax, rectTransformRect.center.y);
		rectTransformBounds[6] = new Vector2 (rectTransformRect.xMax, rectTransformRect.yMin);
		rectTransformBounds[7] = new Vector2 (rectTransformRect.center.x, rectTransformRect.yMin);
	}

	public void GetRectCenter () {

	}

	public void SetRectPosition () {

	}
}

[System.Serializable]
public class Movement : AnimatorItemSettings {
	public AnimatorSystemPositionMode moveMode;
	public AnimatorSystemPosition moveFrom;
	public Vector2 startPosition;
	public Vector2 endPosition;

	public void StartIn () {
		CallStart ();

		if (moveMode == AnimatorSystemPositionMode.Canvas)
			rectTransform.position = GetCanvasEdgePosition (moveFrom);
		else
			rectTransform.localPosition = GetParentEdgePosition (moveFrom);

		//string tweenKey = "AnimatorSystemMovementItem";
		//TweenController.Tween (go, tweenKey, true, 0, 1, time, (tween) => {
		//	rectTransform.position = Vector3.Lerp (startPosition, )
		//}, (tween)=> {
		//	CallComplete ();
		//});
	}

	public void StartOut () {

	}

	public Vector2 GetParentEdgePosition (AnimatorSystemPosition position) {
		switch (position) {
			case AnimatorSystemPosition.Custom:
				return -(Vector3) rectTransform.rect.center - (Vector3) startPosition;
			case AnimatorSystemPosition.UpperLeft:
				return parentRTBounds[2] /*- rectTransformBounds[6]*/;
			case AnimatorSystemPosition.UpperCenter:
				return parentRTBounds[3] - rectTransformBounds[7];
			case AnimatorSystemPosition.UpperRight:
				return parentRTBounds[4] - rectTransformBounds[0];
			case AnimatorSystemPosition.MiddleLeft:
				return parentRTBounds[1] - rectTransformBounds[5];
			case AnimatorSystemPosition.MiddleCenter:
				return -(Vector3) rectTransform.rect.center;
			case AnimatorSystemPosition.MiddleRight:
				return parentRTBounds[5] - rectTransformBounds[1];
			case AnimatorSystemPosition.BottomLeft:
				return parentRTBounds[0] - rectTransformBounds[4];
			case AnimatorSystemPosition.BottomCenter:
				return parentRTBounds[7] - rectTransformBounds[3];
			case AnimatorSystemPosition.BottomRight:
				return parentRTBounds[6] - rectTransformBounds[2];
		}

		return Vector2.zero;
	}

	public Vector2 GetCanvasEdgePosition (AnimatorSystemPosition position) {
		Vector3 edgePosition = Vector3.zero;
		switch (position) {
			case AnimatorSystemPosition.Custom:
				edgePosition = -(Vector3) rectTransform.rect.center + (Vector3) startPosition;
				break;
			case AnimatorSystemPosition.UpperLeft:
				edgePosition = canvasRTBounds[2] /*- rectTransformBounds[6]*/;
				break;
			case AnimatorSystemPosition.UpperCenter:
				edgePosition = canvasRTBounds[3] - rectTransformBounds[7];
				break;
			case AnimatorSystemPosition.UpperRight:
				edgePosition = canvasRTBounds[4] - rectTransformBounds[0];
				break;
			case AnimatorSystemPosition.MiddleLeft:
				edgePosition = canvasRTBounds[1] - rectTransformBounds[5];
				break;
			case AnimatorSystemPosition.MiddleCenter:
				edgePosition = -(Vector3) rectTransform.rect.center;
				break;
			case AnimatorSystemPosition.MiddleRight:
				edgePosition = canvasRTBounds[5] - rectTransformBounds[1];
				break;
			case AnimatorSystemPosition.BottomLeft:
				edgePosition = canvasRTBounds[0] - rectTransformBounds[4];
				break;
			case AnimatorSystemPosition.BottomCenter:
				edgePosition = canvasRTBounds[7] - rectTransformBounds[3];
				break;
			case AnimatorSystemPosition.BottomRight:
				edgePosition = canvasRTBounds[6] - rectTransformBounds[2];
				break;
		}

		return edgePosition + canvasRT.position;
	}
}

public class AnimatorItem : MonoBehaviour {

	public Movement movement;

	//public RectTransform transform;

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
	public float size1;
	public float size2;
	public float size3;
	#endregion

	#region Class accesors
	#endregion

	#region MonoBehaviour overrides
	private void Start () {
		movement.StartIn ();
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			movement.SetUp (gameObject);
			movement.StartIn ();
		}
	}

	private void OnDrawGizmos () {
		movement.SetUp (gameObject);
		movement.StartIn ();


		for (int i = 0; i < 8; i++) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube (movement.GetCanvasEdgePosition (movement.moveFrom), Vector2.one * size1 * 5);
			Gizmos.DrawWireSphere (movement.canvasRTBounds[i], size1);
		}

		for (int i = 0; i < 8; i++) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube (movement.GetParentEdgePosition (movement.moveFrom), Vector2.one * size1 * 5);
			Gizmos.DrawWireSphere (movement.parentRTBounds[i], size2);
		}

		for (int i = 0; i < 8; i++) {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere (movement.rectTransformBounds[i], size3);
		}
	}
	#endregion

	#region Super class overrides
	#endregion

	#region Class implementation
	#endregion

	#region Interface implementation
	#endregion
}