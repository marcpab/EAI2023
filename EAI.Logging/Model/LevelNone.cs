namespace EAI.Logging.Model
{
    public class LevelNone : ILogLevel
    {
        public LogLevel Level { get; } = LogLevel.None;
        public LevelNone() { }

        public override string ToString() => Level.ToString();
    }
}
