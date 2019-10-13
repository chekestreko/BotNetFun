using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
	public static class JsonHandler
	{
		public static async Task WriteEntry(string property, string val)
		{
            string dataSaved = await File.ReadAllTextAsync(Path);
			Dictionary<string, string> savetext = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataSaved);
            savetext[property] = val;
			await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(savetext, Formatting.Indented));
		}

        public static async Task<T> GetData<T>(string property)
        {
            property = property.RemoveWhitespace();
            string data = await File.ReadAllTextAsync(Path);
            Dictionary<string, string> savetext = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            if (typeof(T) == typeof(string))
                return (T)(object)savetext[property];
            else if (typeof(T) == typeof(double))
                return (T)(object)Convert.ToDouble(savetext[property]);
            else if (typeof(T) == typeof(int))
                return (T)(object)Convert.ToInt32(savetext[property]);
            else if (typeof(T) == typeof(long))
                return (T)(object)Convert.ToInt64(savetext[property]);
            else if (typeof(T) == typeof(bool))
            {
                if (savetext[property].ToLower() == "false")
                    return (T)(object)false;
                else return (T)(object)true;
            }
            else
                throw new ArgumentException("Unknown parsing type", nameof(T));
        }

        public static async Task WriteEntry(string property, double val)
			=> await WriteEntry(property, val.ToString());

        public static string Path { get; set; }
	}
}
