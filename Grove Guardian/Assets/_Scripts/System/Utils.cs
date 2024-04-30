using UnityEngine;

public static class Utils {
    public static Vector3 MultiplyVectorWithPoint(Vector3 vector, double d) {
        var multiplier = (float)d;
        var result = new Vector3(
                vector.x * multiplier,
                vector.y * multiplier,
                vector.z * multiplier
            );
        return result;
    }



    public static bool CompareTags(this Collider other, params string[] tags) {
        var result = false;
        foreach(var tag in tags) {
            if(other.CompareTag(tag)) {
                result = true;
            }
        }
        return result;
    }


    public static bool ApproximateCompareVector3(Vector3 vector1, Vector3 vector2) {
        return Mathf.Approximately(vector1.x, vector2.x) &&
               Mathf.Approximately(vector1.y, vector2.y) &&
               Mathf.Approximately(vector1.z, vector2.z);
    }
}
