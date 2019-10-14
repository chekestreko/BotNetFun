using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
    using BotNetFun.Loot.MetaItem;

    public static class JsonHandler
	{
		public static async Task WriteEntry(string property, string val)
		{
            string dataSaved = await File.ReadAllTextAsync(Path);
			Dictionary<string, object> savetext = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataSaved);
            savetext[property] = val;
			await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(savetext, Formatting.Indented));
		}

        public static async Task WriteItemEntry(string property, Item val)
        {
            string dataSaved = await File.ReadAllTextAsync(ItemPath);
            Dictionary<string, Item> savetext = JsonConvert.DeserializeObject<Dictionary<string, Item>>(dataSaved);
            savetext[property] = val;
            await File.WriteAllTextAsync(ItemPath, JsonConvert.SerializeObject(savetext, Formatting.Indented));
        }

        public static async Task<T> GetData<T>(string property)
        {
            property = property.RemoveWhitespace();
            string data = await File.ReadAllTextAsync(Path);
            Dictionary<string, string> entrydata = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            if (typeof(T) == typeof(string))
                return (T)(object)entrydata[property];
            else if (typeof(T) == typeof(double))
                return (T)(object)Convert.ToDouble(entrydata[property]);
            else if (typeof(T) == typeof(int))
                return (T)(object)Convert.ToInt32(entrydata[property]);
            else if (typeof(T) == typeof(long))
                return (T)(object)Convert.ToInt64(entrydata[property]);
            else if (typeof(T) == typeof(bool))
            {
                if (entrydata[property].ToLower() == "false")
                    return (T)(object)false;
                else return (T)(object)true;
            }
            else
                throw new ArgumentException("Unknown parsing type", nameof(T));
        }

        public static async Task<Item> GetItemData(string slot)
        {
            slot = slot.RemoveWhitespace();
            string data = await File.ReadAllTextAsync(ItemPath);
            Dictionary<string, Item> itemdata = JsonConvert.DeserializeObject<Dictionary<string, Item>>(data);
            foreach (KeyValuePair<string, Item> pair in itemdata)
                if (pair.Key.RemoveWhitespace() == slot)
                    return pair.Value;

            return null;
        }

        public static async Task WriteEntry(string property, double val)
			=> await WriteEntry(property, val.ToString());

        public static string Path { get; set; }
        public static string ItemPath { get; set; }
	}
}
