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

			api.Register(
				mod: ModManifest, 
				reset: () => Config = new(), 
				save: () => Helper.WriteConfig(Config)
			);

			api.AddSectionTitle(
				mod: ModManifest, 
				text: () => Helper.Translation.Get("settings_page_title")
			);

			api.AddNumberOption(
				mod: ModManifest,
				name: () => Helper.Translation.Get("base_energy_loss_setting"),
				getValue: () => Config.BaseEnergyLoss,
				setValue: (value) => Config.BaseEnergyLoss = value,
				min: 0f,
				interval: 1f
			);

			api.AddNumberOption(
				mod: ModManifest,
				name: () => Helper.Translation.Get("energy_floor_setting"),
				getValue: () => Config.EnergyFloor,
				setValue: (value) => Config.EnergyFloor = value,
				min: 0f,
				max: 30f,
				interval: 2f
			);

			api.AddNumberOption(
				mod: ModManifest,
				name: () => Helper.Translation.Get("moving_energy_offset_setting"),
				getValue: () => Config.MovingEnergyOffset,
				setValue: (value) => Config.MovingEnergyOffset = value,
				min: -10f,
				max: 0f,
				interval: 1f
			);

			api.AddNumberOption(
				mod: ModManifest,
				name: () => Helper.Translation.Get("sitting_energy_offset_setting"),
				getValue: () => Config.SittingEnergyOffset,
				setValue: (value) => Config.SittingEnergyOffset = value,
				min: 0f,
				max: 30f,
				interval: 2f
			);

			api.AddNumberOption(
				mod: ModManifest,
				name: () => Helper.Translation.Get("sitting_energy_ceiling_setting"),
				getValue: () => Config.SittingEnergyCeiling,
				setValue: (value) => Config.SittingEnergyCeiling = value,
				min: 50f,
				max: 140f,
				interval: 5f
			);
		}

		private void TimeChanged(object? sender, TimeChangedEventArgs e)
		{
			Utilities.UpdateEnergy();
		}
	}
}