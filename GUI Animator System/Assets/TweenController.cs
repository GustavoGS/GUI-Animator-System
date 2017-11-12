using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MyNamespace {
	public enum EaseType { Linear, Clerp, Spring, Punch, Quad, Cubic, Quart, Quint, Sine, Expo, Circ, Bounce, Back, Elastic }
	public enum EaseMode { In, Out, InOut }
	public enum TweenState { Running, Paused, Stopped }
	public enum TweenStopState { NotModify, Complete }

	public class TweenController : MonoBehaviour {

		#region Class members
		private static GameObject root;
		private static readonly List<Tween> tweens = new List<Tween> ();
		#endregion

		#region Class accesors
		#endregion

		#region MonoBehaviour overrides
		private void Awake () {
			tweens.Clear ();
		}

		private void Update () {
			for (int i = 0; i < tweens.Count; i++) {
				Tween tween = tweens[i];

				if (tween.reference == null || !tween.reference.activeSelf) {
					tweens.RemoveAt (i);
					continue;
				}

				if (tween.Update (tween.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime))
					tweens.RemoveAt (i);
			}
		}
		#endregion

		#region Super class overrides
		#endregion

		#region Class implementation
		public static Tween Tween (GameObject reference, string identifier, bool useUnscaledTime, float start, float end, float duration, Action<Tween> progress) {
			return StartTween (reference, identifier, useUnscaledTime, start, end, duration, EasingCurves.linear, progress, null);
		}

		public static Tween Tween (GameObject reference, string identifier, bool useUnscaledTime, float start, float end, float duration, Func<float, float> easing, Action<Tween> progress) {
			return StartTween (reference, identifier, useUnscaledTime, start, end, duration, easing, progress, null);
		}

		public static Tween Tween (GameObject reference, string identifier, bool useUnscaledTime, float start, float end, float duration, Action<Tween> progress, Action<Tween> finish) {
			return StartTween (reference, identifier, useUnscaledTime, start, end, duration, EasingCurves.linear, progress, finish);
		}

		public static Tween Tween (GameObject reference, string identifier, bool useUnscaledTime, float start, float end, float duration, Func<float, float> easing, Action<Tween> progress, Action<Tween> finish) {
			return StartTween (reference, identifier, useUnscaledTime, start, end, duration, EasingCurves.linear, progress, finish);
		}

		protected static Tween StartTween (GameObject reference, string identifier, bool useUnscaledTime, float start, float end, float duration, Func<float, float> easing, Action<Tween> progress, Action<Tween> finish) {
			Tween tween = new Tween (reference, reference.GetInstanceID () + identifier, useUnscaledTime, start, end, duration, easing, progress, finish);
			AddTween (tween);
			return tween;
		}

		protected static void AddTween (Tween tween) {
			if (root == null)
				root = new GameObject ("TweenController", typeof (TweenController));

			if (tween.identifier != null)
				RemoveTween (tween.identifier, TweenStopState.NotModify);

			tweens.Add (tween);
		}

		protected static bool RemoveTween (Tween tween, TweenStopState stopState) {
			tween.Stop (stopState);
			return tweens.Remove (tween);
		}

		protected static bool RemoveTween (string identifier, TweenStopState stopState) {
			bool founded = false;
			for (int i = 0; i < tweens.Count - 1; i++) {
				Tween tween = tweens[i];

				if (identifier == tween.identifier) {
					tween.Stop (stopState);
					tweens.RemoveAt (i);
					founded = true;
				}
			}

			return founded;
		}
		#endregion
	}

	public class Tween {
		#region Class members
		public TweenState state;

		private float startValue;
		private float endValue;
		private float currentTime;
		private Func<float, float> easing;

		private Action<Tween> progressCallback;
		private Action<Tween> finishCallback;
		#endregion

		#region Class accesors
		public GameObject reference { get; private set; }
		public string identifier { get; private set; }
		public bool useUnscaledTime { get; private set; }
		public float duration { get; set; }
		public float value { get; private set; }
		public float progress { get; private set; }
		#endregion

		#region Class implementation
		public Tween (GameObject reference, string identifier, bool useUnscaledTime, float start, float end, float duration, Func<float, float> easing, Action<Tween> progress, Action<Tween> finish) {
			this.reference = reference;
			this.identifier = identifier;
			this.useUnscaledTime = useUnscaledTime;
			this.startValue = start;
			this.endValue = end;
			this.duration = duration;
			this.easing = easing;
			progressCallback = progress;
			finishCallback = finish;

			currentTime = 0;
			UpdateValue ();
		}

		public void Pause () {
			if (state == TweenState.Running)
				state = TweenState.Paused;
		}

		public void Resume () {
			if (state == TweenState.Paused)
				state = TweenState.Running;
		}

		public void Stop (TweenStopState stopState) {
			if (state != TweenState.Stopped) {
				state = TweenState.Stopped;

				if (stopState == TweenStopState.Complete) {
					currentTime = duration;
					UpdateValue ();

					if (finishCallback != null) {
						finishCallback.Invoke (this);
						finishCallback = null;
					}
				}
			}
		}

		public bool Update (float elapsedTime) {
			if (reference == null) {
				Stop (TweenStopState.NotModify);
				return false;
			}

			if (state == TweenState.Running) {
				currentTime += elapsedTime;

				if (currentTime >= duration) {
					Stop (TweenStopState.Complete);
					return true;
				}
				else {
					UpdateValue ();
					return false;
				}
			}
			return (state == TweenState.Stopped);
		}

		private void UpdateValue () {
			if (reference == null) {
				Stop (TweenStopState.NotModify);
				return;
			}

			value = easing (currentTime / duration);
			progress = Mathf.Lerp (startValue, endValue, progress);

			if (progressCallback != null)
				progressCallback.Invoke (this);
		}
		#endregion
	}

	public class EasingCurves {
		public static readonly Func<float, float> linear = Linear;
		public static readonly Func<float, float> clerp = Clerp;
		public static readonly Func<float, float> spring = Spring;
		public static readonly Func<float, float, float> punch = Punch;
		public static readonly Func<float, float> easeInQuad = EaseInQuad;
		public static readonly Func<float, float> easeOutQuad = EaseOutQuad;
		public static readonly Func<float, float> easeInOutQuad = EaseInOutQuad;
		public static readonly Func<float, float> easeInCubic = EaseInCubic;
		public static readonly Func<float, float> easeOutCubic = EaseOutCubic;
		public static readonly Func<float, float> easeInOutCubic = EaseInOutCubic;
		public static readonly Func<float, float> easeInQuart = EaseInQuart;
		public static readonly Func<float, float> easeOutQuart = EaseOutQuart;
		public static readonly Func<float, float> easeInOutQuart = EaseInOutQuart;
		public static readonly Func<float, float> easeInQuint = EaseInQuint;
		public static readonly Func<float, float> easeOutQuint = EaseOutQuint;
		public static readonly Func<float, float> easeInOutQuint = EaseInOutQuint;
		public static readonly Func<float, float> easeInSine = EaseInSine;
		public static readonly Func<float, float> easeOutSine = EaseOutSine;
		public static readonly Func<float, float> easeInOutSine = EaseInOutSine;
		public static readonly Func<float, float> easeInExpo = EaseInExpo;
		public static readonly Func<float, float> easeOutExpo = EaseOutExpo;
		public static readonly Func<float, float> easeInOutExpo = EaseInOutExpo;
		public static readonly Func<float, float> easeInCirc = EaseInCirc;
		public static readonly Func<float, float> easeOutCirc = EaseOutCirc;
		public static readonly Func<float, float> easeInOutCirc = EaseInOutCirc;
		public static readonly Func<float, float> easeInBounce = EaseInBounce;
		public static readonly Func<float, float> easeOutBounce = EaseOutBounce;
		public static readonly Func<float, float> easeInOutBounce = EaseInOutBounce;
		public static readonly Func<float, float> easeInBack = EaseInBack;
		public static readonly Func<float, float> easeOutBack = EaseOutBack;
		public static readonly Func<float, float> easeInOutBack = EaseInOutBack;
		public static readonly Func<float, float> easeInElastic = EaseInElastic;
		public static readonly Func<float, float> easeOutElastic = EaseOutElastic;
		public static readonly Func<float, float> easeInOutElastic = EaseInOutElastic;

		private static float Linear (float value) {
			float start = 0;
			float end = 1;
			return Mathf.Lerp (start, end, value);
		}

		public static float Clerp (float value) {
			float start = 0;
			float end = 1;
			float min = 0.0f;
			float max = 360.0f;
			float half = Mathf.Abs ((max - min) * 0.5f);
			float retval = 0.0f;
			float diff = 0.0f;

			if ((end - start) < -half) {
				diff = ((max - start) + end) * value;
				retval = start + diff;
			}
			else if ((end - start) > half) {
				diff = -((max - end) + start) * value;
				retval = start + diff;
			}
			else retval = start + (end - start) * value;
			return retval;
		}

		public static float Spring (float value) {
			float start = 0;
			float end = 1;
			value = Mathf.Clamp01 (value);
			value = (Mathf.Sin (value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow (1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
			return start + (end - start) * value;
		}

		public static float Punch (float amplitude, float value) {
			float s = 9;
			if (value == 0) {
				return 0;
			}
			else if (value == 1) {
				return 0;
			}
			float period = 1 * 0.3f;
			s = period / (2 * Mathf.PI) * Mathf.Asin (0);
			return (amplitude * Mathf.Pow (2, -10 * value) * Mathf.Sin ((value * 1 - s) * (2 * Mathf.PI) / period));
		}

		public static float EaseInQuad (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return end * value * value + start;
		}

		public static float EaseOutQuad (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return -end * value * (value - 2) + start;
		}

		public static float EaseInOutQuad (float value) {
			float start = 0;
			float end = 1;
			value /= 0.5f;
			end -= start;

			if (value < 1)
				return end * 0.5f * value * value + start;

			value--;
			return -end * 0.5f * (value * (value - 2) - 1) + start;
		}

		public static float EaseInCubic (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return end * value * value * value + start;
		}

		public static float EaseOutCubic (float value) {
			float start = 0;
			float end = 1;
			value--;
			end -= start;
			return end * (value * value * value + 1) + start;
		}

		public static float EaseInOutCubic (float value) {
			float start = 0;
			float end = 1;
			value /= 0.5f;
			end -= start;

			if (value < 1)
				return end * 0.5f * value * value * value + start;

			value -= 2;
			return end * 0.5f * (value * value * value + 2) + start;
		}

		public static float EaseInQuart (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return end * value * value * value * value + start;
		}

		public static float EaseOutQuart (float value) {
			float start = 0;
			float end = 1;
			value--;
			end -= start;
			return -end * (value * value * value * value - 1) + start;
		}

		public static float EaseInOutQuart (float value) {
			float start = 0;
			float end = 1;
			value /= 0.5f;
			end -= start;

			if (value < 1)
				return end * 0.5f * value * value * value * value + start;

			value -= 2;
			return -end * 0.5f * (value * value * value * value - 2) + start;
		}

		public static float EaseInQuint (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return end * value * value * value * value * value + start;
		}

		public static float EaseOutQuint (float value) {
			float start = 0;
			float end = 1;
			value--;
			end -= start;
			return end * (value * value * value * value * value + 1) + start;
		}

		public static float EaseInOutQuint (float value) {
			float start = 0;
			float end = 1;
			value /= 0.5f;
			end -= start;

			if (value < 1)
				return end * 0.5f * value * value * value * value * value + start;

			value -= 2;
			return end * 0.5f * (value * value * value * value * value + 2) + start;
		}

		public static float EaseInSine (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return -end * Mathf.Cos (value * (Mathf.PI * 0.5f)) + end + start;
		}

		public static float EaseOutSine (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return end * Mathf.Sin (value * (Mathf.PI * 0.5f)) + start;
		}

		public static float EaseInOutSine (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return -end * 0.5f * (Mathf.Cos (Mathf.PI * value) - 1) + start;
		}

		public static float EaseInExpo (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return end * Mathf.Pow (2, 10 * (value - 1)) + start;
		}

		public static float EaseOutExpo (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return end * (-Mathf.Pow (2, -10 * value) + 1) + start;
		}

		public static float EaseInOutExpo (float value) {
			float start = 0;
			float end = 1;
			value /= 0.5f;
			end -= start;

			if (value < 1)
				return end * 0.5f * Mathf.Pow (2, 10 * (value - 1)) + start;

			value--;
			return end * 0.5f * (-Mathf.Pow (2, -10 * value) + 2) + start;
		}

		public static float EaseInCirc (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			return -end * (Mathf.Sqrt (1 - value * value) - 1) + start;
		}

		public static float EaseOutCirc (float value) {
			float start = 0;
			float end = 1;
			value--;
			end -= start;
			return end * Mathf.Sqrt (1 - value * value) + start;
		}

		public static float EaseInOutCirc (float value) {
			float start = 0;
			float end = 1;
			value /= 0.5f;
			end -= start;
			if (value < 1) return -end * 0.5f * (Mathf.Sqrt (1 - value * value) - 1) + start;
			value -= 2;
			return end * 0.5f * (Mathf.Sqrt (1 - value * value) + 1) + start;
		}

		public static float EaseInBounce (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			float d = 1f;
			return end - EaseOutBounce (d - value) + start;
		}

		public static float EaseOutBounce (float value) {
			float start = 0;
			float end = 1;
			value /= 1f;
			end -= start;
			if (value < (1 / 2.75f)) {
				return end * (7.5625f * value * value) + start;
			}
			else if (value < (2 / 2.75f)) {
				value -= (1.5f / 2.75f);
				return end * (7.5625f * (value) * value + .75f) + start;
			}
			else if (value < (2.5 / 2.75)) {
				value -= (2.25f / 2.75f);
				return end * (7.5625f * (value) * value + .9375f) + start;
			}
			else {
				value -= (2.625f / 2.75f);
				return end * (7.5625f * (value) * value + .984375f) + start;
			}
		}

		public static float EaseInOutBounce (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			float d = 1f;
			if (value < d * 0.5f) return EaseInBounce (value * 2) * 0.5f + start;
			else return EaseOutBounce (value * 2 - d) * 0.5f + end * 0.5f + start;
		}

		public static float EaseInBack (float value) {
			float start = 0;
			float end = 1;
			end -= start;
			value /= 1;
			float s = 1.70158f;
			return end * (value) * value * ((s + 1) * value - s) + start;
		}

		public static float EaseOutBack (float value) {
			float start = 0;
			float end = 1;
			float s = 1.70158f;
			end -= start;
			value = (value) - 1;
			return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
		}

		public static float EaseInOutBack (float value) {
			float start = 0;
			float end = 1;
			float s = 1.70158f;
			end -= start;
			value /= 0.5f;
			if ((value) < 1) {
				s *= (1.525f);
				return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
			}
			value -= 2;
			s *= (1.525f);
			return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
		}

		public static float EaseInElastic (float value) {
			float start = 0;
			float end = 1;
			end -= start;

			float d = 1f;
			float p = d * 0.3f;
			float s = 0;
			float a = 0;

			if (value == 0) return start;

			if ((value /= d) == 1) return start + end;

			if (a == 0f || a < Mathf.Abs (end)) {
				a = end;
				s = p / 4;
			}
			else {
				s = p / (2 * Mathf.PI) * Mathf.Asin (end / a);
			}

			return -(a * Mathf.Pow (2, 10 * (value -= 1)) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p)) + start;
		}

		public static float EaseOutElastic (float value) {
			float start = 0;
			float end = 1;
			end -= start;

			float d = 1f;
			float p = d * 0.3f;
			float s = 0;
			float a = 0;

			if (value == 0) return start;

			if ((value /= d) == 1) return start + end;

			if (a == 0f || a < Mathf.Abs (end)) {
				a = end;
				s = p * 0.25f;
			}
			else {
				s = p / (2 * Mathf.PI) * Mathf.Asin (end / a);
			}

			return (a * Mathf.Pow (2, -10 * value) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p) + end + start);
		}

		public static float EaseInOutElastic (float value) {
			float start = 0;
			float end = 1;
			end -= start;

			float d = 1f;
			float p = d * 0.3f;
			float s = 0;
			float a = 0;

			if (value == 0) return start;

			if ((value /= d * 0.5f) == 2) return start + end;

			if (a == 0f || a < Mathf.Abs (end)) {
				a = end;
				s = p / 4;
			}
			else {
				s = p / (2 * Mathf.PI) * Mathf.Asin (end / a);
			}

			if (value < 1) return -0.5f * (a * Mathf.Pow (2, 10 * (value -= 1)) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p)) + start;
			return a * Mathf.Pow (2, -10 * (value -= 1)) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
		}
	}
}