using StardewModdingAPI;
using StardewModdingAPI.Events;
using EnergyRework.Common;
using EnergyRework.Interfaces;

namespace EnergyRework
{
	internal sealed class ModEntry : Mod
	{
		internal static ModConfig Config { get; set; } = null!;
        public override void Entry(IModHelper helper)
		{
			Config = Helper.ReadConfig<ModConfig>();
			helper.Events.GameLoop.TimeChanged += TimeChanged;
            helper.Events.GameLoop.GameLaunched += GameLaunched;
        }

		private void GameLaunched(object? sender, GameLaunchedEventArgs e)
		{
			SetupConfig();
		}

		private void SetupConfig()
		{	// TODO: need to register number settings
            var GMCMAPI = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (GMCMAPI is null)
			{
                return;
            }

			GMCMAPI.Register(ModManifest, () => Config = new(), () => Helper.WriteConfig(Config));
        }

		private void TimeChanged(object? sender, TimeChangedEventArgs e)
		{
			Utilities.UpdateEnergy();
		}
    }
}
