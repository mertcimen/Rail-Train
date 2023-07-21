using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace FunGames.Sdk.Tune.Helpers
{
    public static class SimpleJsonHelpers
    {
        public static object GetObjectFromJsonNode(JSONNode node)
        {
            if (node is null)
                return null;
            TypeCode type;
            
            switch (node.Tag)
            {
                case JSONNodeType.Number:
                    type = TypeCode.Double;
                    break;
                case JSONNodeType.Boolean:
                    type = TypeCode.Boolean;
                    break;
                default:
                    type = TypeCode.String;
                    break;
            }
            
            return Convert.ChangeType(node.Value, type);
        }
        
        public static void AddObjectToJsonArray(ref JSONArray jsonArray, object obj)
        {
            if (jsonArray is null)
                return;
            switch (obj)
            {
                case bool b:
                    jsonArray.Add(b);
                    break;
                case string s:
                    jsonArray.Add(s);
                    break;
                case int i:
                    jsonArray.Add(i);
                    break;
                case double d:
                    jsonArray.Add(d);
                    break;
                case RangeInt r:
                    jsonArray.Add(CreateJsonRange(r));
                    break;
            }
        }
        
        public static void AddObjectToJsonObject(ref JSONObject jsonObject, string key, object obj)
        {
            if (jsonObject is null)
                return;
            switch (obj)
            {
                case bool b:
                    jsonObject.Add(key, b);
                    break;
                case string s:
                    jsonObject.Add(key, s);
                    break;
                case int i:
                    jsonObject.Add(key, i);
                    break;
                case double d:
                    jsonObject.Add(key, d);
                    break;
                case RangeInt r:
                    jsonObject.Add(key, CreateJsonRange(r));
                    break;
            }
        }

        private static JSONObject CreateJsonRange(RangeInt r)
        {
            var rangeObjectJson = new JSONObject();
            var rangeJson = new JSONArray();

            rangeJson.Add(r.start);
            rangeJson.Add(r.end);
            rangeObjectJson.Add("range", rangeJson);

            return rangeObjectJson;
        }
    }
}