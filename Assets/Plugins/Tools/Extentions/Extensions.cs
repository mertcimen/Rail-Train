using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ConversionExtensions
{
    public static Vector2 ScreenToCanvasPosition(this Vector2 screenPosition, Canvas canvas)
    {
        if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
        {
            Debug.LogError("RenderMode should be ScreenSpaceCamera. Other render modes not supported yet!");
            return Vector2.zero;
        }

        var screenSize = new Vector2(Screen.width, Screen.height);

        var viewPortPosition = new Vector2(screenPosition.x / screenSize.x, screenPosition.y / screenSize.y);

        var canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        return new Vector2(canvasSize.x * (viewPortPosition.x - .5f), canvasSize.y * (viewPortPosition.y - .5f));
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        var radians = degrees * Mathf.Deg2Rad;
        var sin = Mathf.Sin(radians);
        var cos = Mathf.Cos(radians);

        var tx = v.x;
        var ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    public static Vector3 Rotate(this Vector3 v, float degrees)
    {
        var radians = degrees * Mathf.Deg2Rad;
        var sin = Mathf.Sin(radians);
        var cos = Mathf.Cos(radians);

        var tx = v.x;
        var tz = v.z;

        return new Vector3(cos * tx - sin * tz, v.y, sin * tx + cos * tz);
    }

    public static Vector3 Right(this Vector3 b, Vector3 refAxis)
    {
        var bSubA = b - refAxis;
        var cSubA = -refAxis;

        var cross = Vector3.Cross(bSubA, cSubA);

        return cross;
    }

    public static Vector2 XZ(this Vector3 b)
    {
        return new Vector2(b.x, b.z);
    }

    public static Vector3 X_Z(this Vector2 b, float yValue)
    {
        return new Vector3(b.x, yValue, b.y);
    }

    public static Vector3 Y0(this Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }

    public static Vector3 CenterOfScreen() => new Vector3(Screen.width * .5f, Screen.height * .5f);

    public static bool DistanceCheck(this Vector3 origin, Vector3 pointToCheck, float distanceToCheck)
    {
        // square the distance we compare with
        if ((origin - pointToCheck).sqrMagnitude < distanceToCheck * distanceToCheck)
            return true;
        else
            return false;
    }
}

public static class ListExtensions
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        return list.OrderBy(elem => Guid.NewGuid()).ToList();
    }

    public static List<int> ShuffleInt<T>(this IList<T> list)
    {
        var newLocation = new List<int>();

        int n = list.Count;

        for (int i = 0; i < list.Count; i++)
        {
            newLocation.Add(i);
        }

        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
            (newLocation[n], newLocation[k]) = (newLocation[k], newLocation[n]);
        }

        return newLocation;
    }

    public static void ShuffleWithGivenArray<T>(this IList<T> list, List<int> shuffleList)
    {
        var tempList = new List<T>(list);

        for (int i = 0; i < list.Count; i++)
        {
            list[i] = tempList[shuffleList[i]];
        }
    }

    public static T RandomItem<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T LastItem<T>(this List<T> list)
    {
        return list[^1];
    }
}

public static class FloatExtensions
{
    public static bool EqualTo(this float a, float b)
    {
        return Mathf.Abs(a - b) < Mathf.Epsilon;
    }

    public static bool GreaterThan(this float a, float b)
    {
        return a - b > Mathf.Epsilon;
    }

    public static bool LessThan(this float a, float b)
    {
        return a - b < Mathf.Epsilon;
    }
}

public static class StringExtensions
{
    public static string RemoveDiacritics(this string text)
    {
        return String.Join("", text.Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            .Replace("ı", "i");

        //var normalizedString = text.Normalize(NormalizationForm.FormD);
        //var stringBuilder = new StringBuilder();

        //foreach (var c in normalizedString)
        //{
        //    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        //    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
        //    {
        //        stringBuilder.Append(c);
        //    }
        //}

        //return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string GetOrdinalSuffix(this int num)
    {
        string str = "";
        if (num.ToString().EndsWith("11")) str = "th";
        else if (num.ToString().EndsWith("12")) str = "th";
        else if (num.ToString().EndsWith("13")) str = "th";
        else if (num.ToString().EndsWith("1")) str = "st";
        else if (num.ToString().EndsWith("2")) str = "nd";
        else if (num.ToString().EndsWith("3")) str = "rd";
        else str = "th";
        return num.ToString() + str;
    }
}

public static class IntegerExtensions
{
    public static string LargeIntToString(this int Value)
    {
        var abbreviated = Mathf.Abs(Value).ToString();
        var suffix = "";
        var source = abbreviated;

        switch (abbreviated.Length)
        {
            case > 15:
                abbreviated = abbreviated.Remove(abbreviated.Length - 15) + "." +
                              source.Substring(abbreviated.Length - 15, 1);
                suffix = "Q";
                break;
            case > 12:
                abbreviated = abbreviated.Remove(abbreviated.Length - 12) + "." +
                              source.Substring(abbreviated.Length - 12, 1);
                suffix = "T";
                break;
            case > 9:
                abbreviated = abbreviated.Remove(abbreviated.Length - 9) + "." +
                              source.Substring(abbreviated.Length - 9, 1);
                suffix = "B";
                break;
            case > 6:
                abbreviated = abbreviated.Remove(abbreviated.Length - 6) + "." +
                              source.Substring(abbreviated.Length - 6, 1);
                suffix = "M";
                break;
            case > 3:
                abbreviated = abbreviated.Remove(abbreviated.Length - 3) + "." +
                              source.Substring(abbreviated.Length - 3, 1);
                suffix = "K";
                break;
        }

        return (Value < 0 ? "-" : "") + abbreviated + suffix;
    }
    
    public static string LargeIntToStringRounded(this int value)
    {
        var abbreviated = Mathf.Abs(value).ToString();
        var suffix = "";

        switch (abbreviated.Length)
        {
            case > 15:
                abbreviated = abbreviated.Remove(abbreviated.Length - 15);
                suffix = "Q";
                break;
            case > 12:
                abbreviated = abbreviated.Remove(abbreviated.Length - 12);
                suffix = "T";
                break;
            case > 9:
                abbreviated = abbreviated.Remove(abbreviated.Length - 9);
                suffix = "B";
                break;
            case > 6:
                abbreviated = abbreviated.Remove(abbreviated.Length - 6);
                suffix = "M";
                break;
            case > 3:
                abbreviated = abbreviated.Remove(abbreviated.Length - 3);
                suffix = "K";
                break;
        }

        return (value < 0 ? "-" : "") + abbreviated + suffix;
    }
}

public static class RectExtensions
{
    public static Vector2 GetRandomPointInRect(this Rect rect)
    {
        return new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
    }
}

public static class TransformExtensions
{
    public static bool IsTransformLookingAt(this Transform transform, Vector3 position, float maxAngleForLooking = 80)
    {
        return Vector3.Angle(transform.forward, position - transform.position) < maxAngleForLooking;
    }
}