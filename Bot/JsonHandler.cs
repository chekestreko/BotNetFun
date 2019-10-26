using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
    using BotNetFun.Loot.MetaItem;

    public sealed class JsonHandler : IAsyncDisposable, IDisposable
	{
        public JsonHandler(string path = null, string itemPath = null)
        {
            Path = path;
            ItemPath = itemPath;
        }

        public async Task WriteEntry(string property, string val)
		{
            string dataSaved = await File.ReadAllTextAsync(Path);
			Dictionary<string, object> savetext = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataSaved);
            savetext[property] = val;
			await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(savetext, Formatting.Indented));
		}

        public async Task WriteEntry(string property, double val)
        {
            string dataSaved = await File.ReadAllTextAsync(Path);
            Dictionary<string, object> savetext = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataSaved);
            savetext[property] = val;
            await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(savetext, Formatting.Indented));
        }

        public async Task WriteEntry(string property, bool val)
        {
            string dataSaved = await File.ReadAllTextAsync(Path);
            Dictionary<string, object> savetext = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataSaved);
            savetext[property] = val;
            await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(savetext, Formatting.Indented));
        }

        public async Task WriteItemEntry(string property, Item val)
        {
            string dataSaved = await File.ReadAllTextAsync(ItemPath);
            Dictionary<string, Item> savetext = JsonConvert.DeserializeObject<Dictionary<string, Item>>(dataSaved);
            savetext[property] = val;
            await File.WriteAllTextAsync(ItemPath, JsonConvert.SerializeObject(savetext, Formatting.Indented));
        }

        public async Task<T> GetData<T>(string property)
        {
            property = property.RemoveWhitespace();
            string data = await File.ReadAllTextAsync(Path);
            Dictionary<string, object> entrydata = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (typeof(T) == typeof(string))
                return (T)(object)(string)entrydata[property];
            else if (typeof(T) == typeof(double))
                return (T)(object)Convert.ToDouble(entrydata[property]);
            else if (typeof(T) == typeof(int))
                return (T)(object)Convert.ToInt32(entrydata[property]);
            else if (typeof(T) == typeof(long))
                return (T)(object)Convert.ToInt64(entrydata[property]);
            else if (typeof(T) == typeof(bool))
            {
                if (entrydata[property] is string)
                if (entrydata[property].ToString().ToLower() == "true")
                    return (T)(object)true;

                return (T)(object)(bool)entrydata[property];
            }
            else
                throw new ArgumentException("Unknown parsing type", nameof(T));
        }

        public async Task<Item> GetItemData(string slot)
        {
            slot = slot.RemoveWhitespace();
            string data = await File.ReadAllTextAsync(ItemPath);
            Dictionary<string, Item> itemdata = JsonConvert.DeserializeObject<Dictionary<string, Item>>(data);
            foreach (KeyValuePair<string, Item> pair in itemdata)
                if (pair.Key.RemoveWhitespace() == slot)
                    return pair.Value;

            return null;
        }

        private readonly string Path;
        private readonly string ItemPath;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "Visual Studio doesn't recognize IAsyncDisposable#DisposeAsync as a valid object disposal method")]
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            if (File.Exists(Path) && File.Exists(ItemPath))
                return;

            await File.CreateText(Path).DisposeAsync();
            await File.CreateText(ItemPath).DisposeAsync();
            await File.WriteAllBytesAsync(Path, null);
            await File.WriteAllBytesAsync(ItemPath, null);
        }

        /// <summary>
        /// NOTE: this should never be used, as opposed to <see cref="DisposeAsync"/>, since literally every bot runtime operation is async, but it's a good coding
        /// practice to implement <see cref="IDisposable"/> alongside <see cref="IAsyncDisposable"/> for consistency
        /// </summary>
        public void Dispose() =>
            DisposeAsync().GetAwaiter().GetResult();
	}
}
