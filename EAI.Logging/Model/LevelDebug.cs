namespace EAI.Logging.Model
{
    public class LevelDebug : ILogLevel
    {
        public LogLevel Level { get; } = LogLevel.Debug;

        public LevelDebug() { }

        public override string ToString() => Level.ToString();
    }
}
