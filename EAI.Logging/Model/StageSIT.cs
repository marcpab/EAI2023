namespace EAI.Logging.Model
{
    public class StageSIT : ILogStage
    {
        public LogStage Stage { get; } = LogStage.SIT;
        public string Description { get; } = "System Integration Testing";

        public StageSIT() { }

        public override string ToString() => Stage.ToString();
    }
}
