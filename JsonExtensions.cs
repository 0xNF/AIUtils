using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace AIUtils {
    public static class JsonStuff {

        public static bool getjsonBool(this JsonObject jo, string field, bool Default = true) {
            IJsonValue j;
            jo.TryGetValue(field, out j);
            if (j != null && j.ValueType != JsonValueType.Null) {
                return j.GetBoolean();
            }
            else {
                return Default;
            }
        }
        public static string getjsonString(this JsonObject jo, string field, string Default = "") {
            IJsonValue j;
            jo.TryGetValue(field, out j);
            if (j != null && j.ValueType != JsonValueType.Null) {
                return j.GetString();
            }
            else {
                return Default;
            }
        }
        public static int getJsonInt(this JsonObject jo, string field, int Default = -1) {
            IJsonValue j;
            jo.TryGetValue(field, out j);
            if (j != null && j.ValueType != JsonValueType.Null) {
                return (int)j.GetNumber();
            }
            else {
                return Default;
            }
        }

        public static double getJsonDouble(this JsonObject jo, string field, double Default = 0.0) {
            IJsonValue j;
            jo.TryGetValue(field, out j);
            if (j != null && j.ValueType != JsonValueType.Null) {
                return j.GetNumber();
            }
            else {
                return Default;
            }
        }

        public static JsonObject getJsonObject(this JsonObject jo, string field) {
            IJsonValue j;
            jo.TryGetValue(field, out j);
            if (j != null && j.ValueType != JsonValueType.Null) {
                return j.GetObject();
            }
            else {
                return null;
            }
        }

        public static JsonArray getJsonArray(this JsonObject jo, string field) {
            IJsonValue j;
            jo.TryGetValue(field, out j);
            if (j != null && j.ValueType != JsonValueType.Null) {
                return j.GetArray();
            }
            else {
                return null;
            }
        }

    }
}
