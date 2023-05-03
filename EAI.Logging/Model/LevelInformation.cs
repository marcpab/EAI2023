namespace EAI.Logging.Model
{
    public class LevelInformation : ILogLevel
    {
        public LogLevel Level { get; } = LogLevel.Information;

        public LevelInformation() { }

        public override string ToString() => Level.ToString();
    }
}
