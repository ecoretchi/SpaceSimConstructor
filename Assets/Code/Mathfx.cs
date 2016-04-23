using UnityEngine;


static class Mathfx{

	/// <summary>
	///		Линейная интерполяция между from и to
	/// </summary>
	public static float lerp(float from, float to, float value) {
		if(value < 0.0f)
			return from;
		else if(value > 1.0f)
			return to;
		return (to - from) * value + from;
	}

	/// <summary>
	///		Линейная интерполяция между from и to без отсечения value
	/// </summary>
	public static float lerpUnclamped(float from, float to, float value) {
		return (1.0f - value) * from + value * to;
	}

    /// <summary>
    /// Линейный старт, плавная остановка
    /// </summary>
    /// <returns></returns>
    public static float sinerp(float start, float end, float value) {
		return lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
	}
	
	// Плавный старт, линейная остановка
	public static float Coserp(float start, float end, float value) {
		return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
	}

	// линейный старт, финиш с тройным "промахом"
	public static float berp(float start, float end, float value) {
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}

    /// <summary>
    /// Плавный старт, плавный финиш
    /// </summary>
    public static float smoothStep(float from, float to, float value) {
		if(value < 0.0f)
			return from;
		else if(value > 1.0f)
			return to;
		value = value * value * (3.0f - 2.0f * value);
		return (1.0f - value) * from + value * to;
	}

	public static Color colorLerp(Color c1, Color c2, float value) {
		if(value > 1.0f)
			return c2;
		else if(value < 0.0f)
			return c1;
		return new Color(c1.r + (c2.r - c1.r) * value,
							c1.g + (c2.g - c1.g) * value,
							c1.b + (c2.b - c1.b) * value,
							c1.a + (c2.a - c1.a) * value);
	}

	public static Vector2 vector2Lerp(Vector2 v1, Vector2 v2, float value) {
		if(value > 1.0f)
			return v2;
		else if(value < 0.0f)
			return v1;
		return new Vector2(v1.x + (v2.x - v1.x) * value,
							v1.y + (v2.y - v1.y) * value);
	}

	public static Vector3 vector3Lerp(Vector3 v1, Vector3 v2, float value) {
		if(value > 1.0f)
			return v2;
		else if(value < 0.0f)
			return v1;
		return new Vector3(v1.x + (v2.x - v1.x) * value,
							v1.y + (v2.y - v1.y) * value,
							v1.z + (v2.z - v1.z) * value);
	}

	public static Vector4 vector4Lerp(Vector4 v1, Vector4 v2, float value) {
		if(value > 1.0f)
			return v2;
		else if(value < 0.0f)
			return v1;
		return new Vector4(v1.x + (v2.x - v1.x) * value,
							v1.y + (v2.y - v1.y) * value,
							v1.z + (v2.z - v1.z) * value,
							v1.w + (v2.w - v1.w) * value);
	}

	public static Vector3 nearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
		Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
		float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
		return lineStart + (closestPoint * lineDirection);
	}

	public static Vector3 nearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
		Vector3 fullDirection = lineEnd - lineStart;
		Vector3 lineDirection = Vector3.Normalize(fullDirection);
		float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
		return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
	}

	public static float bounce(float x) {
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	/// <summary>
	/// test for value that is near specified float (due to floating point inprecision)
	/// all thanks to Opless for this!
	/// </summary>
	public static bool approx(float val, float about, float range) {
		return ((Mathf.Abs(val - about) < range));
	}

	/// <summary>
	/// test if a Vector3 is close to another Vector3 (due to floating point inprecision)
	/// compares the square of the distance to the square of the range as this 
	/// avoids calculating a square root which is much slower than squaring the range
	/// </summary>
	public static bool approx(Vector3 val, Vector3 about, float range) {
		return ((val - about).sqrMagnitude < range * range);
	}

	 
	/// <summary>
	///  CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
	///  This is useful when interpolating eulerAngles and the object
	///  crosses the 0/360 boundary.  The standard Lerp function causes the object
	///  to rotate in the wrong direction and looks stupid. Clerp fixes that.
	/// </summary>
	/// <returns>Interpolated value</returns>
	public static float clerp(float start, float end, float value) {
		//const float minAngle = 0.0f;
		const float maxAngle = 360.0f;
		//float half = Mathf.Abs((maxAngle - minAngle) / 2.0f); //half the distance between min and max
		const float half = 180.0f;

		float retval = 0.0f;
		float diff = 0.0f;

		if((end - start) < -half) {
			diff = ((maxAngle - start) + end) * value;
			retval = start + diff;
		} else if((end - start) > half) {
			diff = -((maxAngle - end) + start) * value;
			retval = start + diff;
		} else
			retval = start + (end - start) * value;

		return retval;
	}
}
