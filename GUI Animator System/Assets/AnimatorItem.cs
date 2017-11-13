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
	BottomRight
}

public enum AnimatorSysyemPositionMode {
	Canvas, Parent
}

[System.Serializable]
public class AnimatorItemSettings {
	public bool state;

	public RectTransform canvasRT;
	public Vector3[] canvasRTBounds;
	public RectTransform parentRT;
	public Vector3[] parentRTBounds;
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
		canvasRT = go.GetComponentInParent<Canvas> ().GetComponent<RectTransform> ();
		CalculateCanvasBounds ();

		parentRT = go.transform.parent.GetComponent<RectTransform> ();
		CalculateParentBounds ();

		rectTransform = go.GetComponent<RectTransform> ();
		CalculateRectTransformBounds ();
	}

	private void CalculateCanvasBounds () {
		Rect canvasRect = canvasRT.rect;
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

	//public float In () {
	//	time.
	//}
}

[System.Serializable]
public class Movement : AnimatorItemSettings {
	public AnimatorSysyemPositionMode moveMode;
	public AnimatorSystemPosition moveFrom;
	public Vector2 startPosition;
	public Vector2 endPosition;

	public void In () {
		//TweenController.Tween (gameObject, "MovementIn", true, 0, 1, time, (tween) => {
		//	//transform.po
		//});
	}

	public Vector2 GetParentEdgePosition (AnimatorSystemPosition position) {
		switch (position) {
			case AnimatorSystemPosition.Custom:
				return startPosition;
			case AnimatorSystemPosition.UpperLeft:
				return parentRTBounds[2] - rectTransformBounds[6];
			case AnimatorSystemPosition.UpperCenter:
				return parentRTBounds[3] - rectTransformBounds[7];
			case AnimatorSystemPosition.UpperRight:
				return parentRTBounds[4] - rectTransformBounds[0];
			case AnimatorSystemPosition.MiddleLeft:
				return parentRTBounds[1] - rectTransformBounds[5];
			case AnimatorSystemPosition.MiddleCenter:
				return parentRT.localPosition;
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
			case AnimatorSystemPosition.UpperLeft:
				edgePosition = canvasRTBounds[2] - rectTransformBounds[6];
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
				edgePosition = new Vector3 (canvasRTBounds[3].x, canvasRTBounds[1].y);
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
	public float size1;
	public float size2;
	public float size3;
	#endregion

	#region Class accesors
	#endregion

	#region MonoBehaviour overrides
	private void OnDrawGizmos () {
		movement.SetUp (gameObject);
		RectTransform trans = GetComponent<RectTransform> ();
		trans.position = movement.GetParentEdgePosition (movement.moveFrom);

		Vector3[] cords = new Vector3[4];
		movement.canvasRT.GetWorldCorners (cords);

		for (int i = 0; i < 4; i++) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube (cords[i], size1 * Vector3.one);
		}

		for (int i = 0; i < 8; i++) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere (movement.canvasRTBounds[i], size1);
		}

		for (int i = 0; i < 8; i++) {
			Gizmos.color = Color.red;
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