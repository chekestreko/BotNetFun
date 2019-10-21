namespace BotNetFun.Data
{
    public static class Globals
    {
        public static readonly string NL = System.Environment.NewLine;
        public static readonly string SavePath = System.AppDomain.CurrentDomain.BaseDirectory + "/SaveData";

        public const byte MaxLevel = 100;
        public const double ScaleMultiBase = 3.77046142856;
        public const double NormalEnemyScale = 1.223375934421;
        public const double EliteEnemyScale = 1.64723892138;
        public const double BossEnemyScale = 2.76751868842;
        public const double ClassEfficiencyPercentBonus = 0.2244;

        public static System.Random Rnd { get; } = new System.Random();

        // extension method to easily edit the basic properties of an embedbuilder
        public static void EditEmbed(this Discord.EmbedBuilder builder, string title, string description, Discord.Color color)
        {
            builder.Title = title;
            builder.Description = description;
            builder.Color = color;
        }
    }
}
