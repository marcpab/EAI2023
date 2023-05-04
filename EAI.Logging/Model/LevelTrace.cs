namespace EAI.Logging.Model
{
    public class LevelTrace : ILogLevel
    {
        public LogLevel Level { get; } = LogLevel.Trace;
        public LevelTrace() { }

        public override string ToString() => Level.ToString();
    }
}
