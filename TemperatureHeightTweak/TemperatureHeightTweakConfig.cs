namespace TemperatureHeightTweak;

public class TemperatureHeightTweakConfig
{
    public static TemperatureHeightTweakConfig Loaded { get; set; } = new TemperatureHeightTweakConfig();

    public double a = 188.0;
    public double b = 0.03;
    public double c = 155.0;
}