using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
    using BotNetFun.MainGame;
    using BotNetFun.MainGame.Enums;

    using BotNetFun.Loot.MetaItem;

    public sealed class JsonHandler : IAsyncDisposable, IDisposable
	{
        public JsonHandler(string path = null)
            => Path = path;
        

        public async Task WriteEntry<T>(PlayerData datatype, T rawData)
        {
            string dataSaved = await File.ReadAllTextAsync(Path);
            Player save = JsonConvert.DeserializeObject<Player>(dataSaved);

            try
            {
                object data = rawData;

                switch (datatype)
                {
                    case PlayerData.MaxHealth:
                        save.MaxHealth = (double)data;
                        break;
                    case PlayerData.Health:
                        save.Health = (double)data;
                        break;
                    case PlayerData.DodgeChance:
                        save.DodgeChance = (double)data;
                        break;
                    case PlayerData.Defense:
                        save.Defense = (double)data;
                        break;
                    case PlayerData.Damage:
                        save.Damage = (double)data;
                        break;
                    case PlayerData.CritChance:
                        save.CritChance = (double)data;
                        break;
                    case PlayerData.CritDamage:
                        save.CritDamage = (double)data;
                        break;
                    case PlayerData.Level:
                        save.Level = (byte)data;
                        break;
                    case PlayerData.XP:
                        save.XP = (double)data;
                        break;
                    case PlayerData.Gold:
                        save.Gold = (double)data;
                        break;
                    case PlayerData.InBattle:
                        save.InBattle = (bool)data;
                        break;
                    case PlayerData.CurrentLocation:
                        save.CurrentLocation = (Location)data;
                        break;
                    case PlayerData.Helmet:
                        save.Helmet = (Item)data;
                        break;
                    case PlayerData.Chestplate:
                        save.Chestplate = (Item)data;
                        break;
                    case PlayerData.Gauntlets:
                        save.Gauntlets = (Item)data;
                        break;
                    case PlayerData.Pants:
                        save.Pants = (Item)data;
                        break;
                    case PlayerData.Boots:
                        save.Boots = (Item)data;
                        break;
                    case PlayerData.PrimaryItem:
                        save.PrimaryItem = (Item)data;
                        break;
                    case PlayerData.SecondaryItem:
                        save.SecondaryItem = (Item)data;
                        break;
                    case PlayerData.Charm:
                        save.Charm = (Item)data;
                        break;
                    case PlayerData.I1:
                        save.I1 = (Item)data;
                        break;
                    case PlayerData.I2:
                        save.I2 = (Item)data;
                        break;
                    case PlayerData.I3:
                        save.I3 = (Item)data;
                        break;
                    case PlayerData.I4:
                        save.I4 = (Item)data;
                        break;
                    case PlayerData.I5:
                        save.I5 = (Item)data;
                        break;
                    case PlayerData.I6:
                        save.I6 = (Item)data;
                        break;
                    case PlayerData.I7:
                        save.I7 = (Item)data;
                        break;
                    case PlayerData.I8:
                        save.I8 = (Item)data;
                        break;
                    case PlayerData.I9:
                        save.I9 = (Item)data;
                        break;
                    case PlayerData.I10:
                        save.I10 = (Item)data;
                        break;
                    case PlayerData.I11:
                        save.I11 = (Item)data;
                        break;
                    case PlayerData.I12:
                        save.I12 = (Item)data;
                        break;
                    case PlayerData.I13:
                        save.I13 = (Item)data;
                        break;
                    case PlayerData.I14:
                        save.I14 = (Item)data;
                        break;
                    case PlayerData.I15:
                        save.I15 = (Item)data;
                        break;
                    case PlayerData.I16:
                        save.I16 = (Item)data;
                        break;
                    case PlayerData.I17:
                        save.I17 = (Item)data;
                        break;
                    case PlayerData.I18:
                        save.I18 = (Item)data;
                        break;
                    case PlayerData.I19:
                        save.I19 = (Item)data;
                        break;
                    case PlayerData.I20:
                        save.I20 = (Item)data;
                        break;
                    case PlayerData.Class:
                        save.Class = (PlayerClass)data;
                        break;
                }
            } catch (InvalidCastException)
            {
                throw new InvalidCastException("Invalid type parameter for WriteEntry<T>(), does not match with type of to-write data");
            }

            await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(save, Formatting.Indented));
        }

        public async Task<T> GetData<T>(PlayerData datatype)
        {
            string dataSaved = await File.ReadAllTextAsync(Path);
            Player save = JsonConvert.DeserializeObject<Player>(dataSaved);

            try
            {
                return (T)(object)(datatype switch
                {
                    PlayerData.MaxHealth => save.MaxHealth,
                    PlayerData.Health => save.Health,
                    PlayerData.DodgeChance => save.DodgeChance,
                    PlayerData.Defense => save.Defense,
                    PlayerData.Damage => save.Damage,
                    PlayerData.CritChance => save.CritChance,
                    PlayerData.CritDamage => save.CritDamage,
                    PlayerData.Level => save.Level,
                    PlayerData.XP => save.XP,
                    PlayerData.Gold => save.Gold,
                    PlayerData.InBattle => save.InBattle,
                    PlayerData.CurrentLocation => save.CurrentLocation,
                    PlayerData.Helmet => save.Helmet,
                    PlayerData.Chestplate => save.Chestplate,
                    PlayerData.Gauntlets => save.Gauntlets,
                    PlayerData.Pants => save.Pants,
                    PlayerData.Boots => save.Boots,
                    PlayerData.PrimaryItem => save.PrimaryItem,
                    PlayerData.SecondaryItem => save.SecondaryItem,
                    PlayerData.Charm => save.Charm,
                    PlayerData.I1 => save.I1,
                    PlayerData.I2 => save.I2,
                    PlayerData.I3 => save.I3,
                    PlayerData.I4 => save.I4,
                    PlayerData.I5 => save.I5,
                    PlayerData.I6 => save.I6,
                    PlayerData.I7 => save.I7,
                    PlayerData.I8 => save.I8,
                    PlayerData.I9 => save.I9,
                    PlayerData.I10 => save.I10,
                    PlayerData.I11 => save.I11,
                    PlayerData.I12 => save.I12,
                    PlayerData.I13 => save.I13,
                    PlayerData.I14 => save.I14,
                    PlayerData.I15 => save.I15,
                    PlayerData.I16 => save.I16,
                    PlayerData.I17 => save.I17,
                    PlayerData.I18 => save.I18,
                    PlayerData.I19 => save.I19,
                    PlayerData.I20 => save.I20,
                    PlayerData.Class => save.Class,
                    _ => throw new InvalidOperationException("Unknown PlayerData value (?)")
                });
            } catch (InvalidCastException)
            {
                throw new InvalidCastException("Invalid type parameter for GetData<T>(), does not match with type of retrived data");
            } 
        }

        private string Path { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "Visual Studio doesn't recognize IAsyncDisposable/DisposeAsync as a valid object disposal interface/method")]
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            if (File.Exists(Path))
                return;

            await File.CreateText(Path).DisposeAsync();
            await File.WriteAllBytesAsync(Path, null);
        }

        /// <summary>
        /// NOTE: this should never be used, as opposed to <see cref="DisposeAsync"/>, since literally every bot runtime operation is async, but it's a good coding
        /// practice to implement <see cref="IDisposable"/> alongside <see cref="IAsyncDisposable"/> for consistency
        /// </summary>
        public void Dispose() =>
            DisposeAsync().GetAwaiter().GetResult();
    }
}
