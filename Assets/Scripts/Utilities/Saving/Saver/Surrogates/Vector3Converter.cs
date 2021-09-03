using System;
using Assets.Scripts.Extensions;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Utilities.Saving.Saver.Surrogates
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            var vector3String = $"{value.x} {value.y} {value.z}";

            writer.WriteValue(vector3String);
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var str = (string)reader.Value;

            var (strX, strY, strZ) = str.Split();

            if (float.TryParse(strX, out var x) && float.TryParse(strY, out var y) && float.TryParse(strZ, out var z))
                return new Vector3(x, y, z);

            throw new InvalidCastException($"Unable to cast string value to Vector3. Source string: {str}");
        }
    }
}
