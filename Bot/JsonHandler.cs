using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
	internal static class JsonHandler
	{
		public static async Task WriteEntry(string property, string val, string savePath)
		{
            string dataSaved = await File.ReadAllTextAsync(savePath);
			Dictionary<string, string> savetext = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataSaved);
            savetext[property] = val;
			await File.WriteAllTextAsync(savePath, JsonConvert.SerializeObject(savetext, Formatting.Indented));
		}

        public static async Task<string> GetData(string property, string savePath)
        {
            property = property.RemoveWhitespace();
            string data = await File.ReadAllTextAsync(savePath);
            Dictionary<string, string> savetext = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            return savetext[property];
        }

        #region WriteEntryNumberOverload
        public static async Task WriteEntry(string property, double val, string savePath)
			=> await WriteEntry(property, val.ToString(), savePath);
		#endregion
	}
}
