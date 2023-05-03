namespace EAI.Logging.Model
{
    public class LevelError : ILogLevel
    {
        public LogLevel Level { get; } = LogLevel.Error;

        public LevelError() { }

        public override string ToString() => Level.ToString();
    }
}
