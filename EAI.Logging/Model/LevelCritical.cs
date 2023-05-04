namespace EAI.Logging.Model
{
    public class LevelCritical : ILogLevel
    {
        public LogLevel Level { get; } = LogLevel.Critical;
        public LevelCritical() { }

        public override string ToString() => Level.ToString();
    }
}
