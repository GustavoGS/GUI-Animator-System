using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;

public class Test : MonoBehaviour {

	#region Class members
	public RectTransform canvas;
	public RectTransform parentRect;
	public RectTransform rectTrans;
	public Vector2[] rectBounds = new Vector2[8];

	public AnimatorSystemPosition pos;

	public Vector3[] parentCorners = new Vector3[4];
	public Vector3[] worldCorners;
	public Vector3[] localCorners;

	public float radius;
	#endregion

	#region Class accesors
	#endregion

	#region MonoBehaviour overrides
	private void Start () {

	}

	private void Update () {
		//transform.position = new Vector3 (0, EasingCurves.EaseInCubic (0, 10, value), 0);

		//if (Input.GetKeyDown (KeyCode.Space)) {
		//	TweenController.Tween (gameObject, "Movement", false, 0, 1, time, EasingCurves.easeInOutElastic, (tween) => {
		//		transform.position = Vector3.LerpUnclamped (startPos, endPos, tween.value);
		//		//print (tween.progress.ToString ("F5"));
		//	});
		//}
	}

	private void CalculateRectCorners () {
		Rect rect = rectTrans.rect;
		rectBounds[0] = new Vector2 (rect.xMin, rect.yMin);
		rectBounds[1] = new Vector2 (rect.xMin, rect.y);
		rectBounds[2] = new Vector2 (rect.xMin, rect.yMax);
		rectBounds[3] = new Vector2 (rect.x, rect.yMax);
		rectBounds[4] = new Vector2 (rect.xMax, rect.yMax);
		rectBounds[5] = new Vector2 (rect.xMax, rect.y);
		rectBounds[6] = new Vector2 (rect.xMax, rect.yMin);
		rectBounds[7] = new Vector2 (rect.x, rect.yMin);
	}

	private void OnDrawGizmos () {
		//parentRect = transform.parent.GetComponent<RectTransform> ();
		//rectTrans = GetComponent<RectTransform> ();
		////canvas.GetLocalCorners (localCorners);
		////canvas.GetWorldCorners (worldCorners);

		//for (int i = 0; i < 4; i++) {
		//	Gizmos.color = Color.red;
		//	Gizmos.DrawWireSphere (worldCorners[i], radius);
		//	Gizmos.color = Color.blue;
		//	Gizmos.DrawWireSphere (localCorners[i], radius);
		//}

		//parentRect.GetLocalCorners (parentCorners);
		//CalculateRectCorners ();
		//switch (pos) {
		//	case AnimatorSystemPosition.Custom:
		//		break;
		//	case AnimatorSystemPosition.UpperLeft:
		//		rectTrans.localPosition = parentCorners[1] - (Vector3) rectBounds[6];
		//		break;
		//	case AnimatorSystemPosition.UpperCenter:
		//		break;
		//	case AnimatorSystemPosition.UpperRight:
		//		break;
		//	case AnimatorSystemPosition.MiddleLeft:
		//		break;
		//	case AnimatorSystemPosition.MiddleCenter:
		//		break;
		//	case AnimatorSystemPosition.MiddleRight:
		//		break;
		//	case AnimatorSystemPosition.BottomLeft:
		//		break;
		//	case AnimatorSystemPosition.BottomCenter:
		//		break;
		//	case AnimatorSystemPosition.BottomRight:
		//		break;
		//	case AnimatorSystemPosition.UpperLeftScreen:
		//		break;
		//	case AnimatorSystemPosition.UpperCenterScreen:
		//		break;
		//	case AnimatorSystemPosition.UpperRightScreen:
		//		break;
		//	case AnimatorSystemPosition.MiddleLeftScreen:
		//		break;
		//	case AnimatorSystemPosition.MiddleCenterScreen:
		//		break;
		//	case AnimatorSystemPosition.MiddleRightScreen:
		//		break;
		//	case AnimatorSystemPosition.BottomLeftScreen:
		//		break;
		//	case AnimatorSystemPosition.BottomCenterScreen:
		//		break;
		//	case AnimatorSystemPosition.BottomRightScreen:
		//		break;
		//	default:
		//		break;
		//}
	}
	#endregion

	#region Super class overrides
	#endregion

	#region Class implementation
	#endregion

	#region Interface implementation
	#endregion
}