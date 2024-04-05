namespace EnergyRework
{
	public sealed class ModConfig
	{
		public float BaseEnergyLoss { get; set; } = -2f;
		public float EnergyFloor { get; set; } = 15f;
		public float MovingEnergyOffset { get; set; } = -3f;
		public float SittingEnergyOffset { get; set; } = 3f;
		public float SittingEnergyCeiling { get; set; } = 140f;
    }
}