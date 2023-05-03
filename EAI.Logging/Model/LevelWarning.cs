namespace EAI.Logging.Model
{
    public class LevelWarning : ILogLevel
    {
        public LogLevel Level { get; } = LogLevel.Warning;

        public LevelWarning() { }

        public override string ToString() => Level.ToString();
    }
}
