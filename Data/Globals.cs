using System;
using Discord;

namespace BotNetFun.Data
{
    public static class Globals
    {
        // Newline
        public static string NL { get; } = Environment.NewLine;

        // Savepath directory
        public static string SavePath { get; } = AppDomain.CurrentDomain.BaseDirectory + "/SaveData";

        public const byte MaxLevel = 100;
        public const double ScaleMultiBase = 3.77046142856;
        public const double NormalEnemyScale = 1.223375934421;
        public const double EliteEnemyScale = 1.64723892138;
        public const double BossEnemyScale = 2.76751868842;
        public const double ClassEfficiencyPercentBonus = 0.25144;

        // Thread-safe random property
        public static Random Rnd {
            get {
                lock (_rnd)
                {
                    return _rnd;
                }
            }
        }
        
        // backing-field of Rnd property
        private static readonly Random _rnd = new Random();

        // extension method to easily edit the basic properties of an embedbuilder
        public static void EditEmbed(this EmbedBuilder builder, string title, string description, Color color)
        {
            builder.Title = title;
            builder.Description = description;
            builder.Color = color;
        }

        public static double RoundSlightly(double toRound)
            => Math.Round(toRound, 4);
        
        public static T GetRandomEnum<T>() where T : Enum
            => (T)Enum.GetValues(typeof(T)).GetValue(Rnd.Next(Enum.GetValues(typeof(T)).Length));
    }
}
