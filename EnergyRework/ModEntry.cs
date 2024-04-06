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
		{
			var api = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

			if (api is null)
			{
				return;
			}

			ConfigHelper configHelper = new(api, ModManifest, Helper.Translation, Config);

			api.Register(
				mod: ModManifest, 
				reset: () => Config = new(), 
				save: () => Helper.WriteConfig(Config)
			);

			api.AddSectionTitle(
				mod: ModManifest, 
				text: () => Helper.Translation.Get("title")
			);

			configHelper.AddSetting("base_energy_loss", () => Config.BaseEnergyLoss);
			configHelper.AddSetting("energy_floor", () => Config.EnergyFloor, min: 5f, max: 40f);
			configHelper.AddSetting("moving_energy_offset", () => Config.MovingEnergyOffset);
			configHelper.AddSetting("sitting_energy_offset", () => Config.SittingEnergyOffset, min: 0f, max: 15f);
			configHelper.AddSetting("sitting_energy_ceiling", () => Config.SittingEnergyCeiling, min: 45f, max: 200f, interval: 5f);
		}

		private void TimeChanged(object? sender, TimeChangedEventArgs e)
		{
			Utilities.UpdateEnergy();
		}
	}
}