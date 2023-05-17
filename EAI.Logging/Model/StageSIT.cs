namespace EAI.Logging.Model
{
    public class StageSIT : ILogStage
    {
        public LogStage Stage { get; } = LogStage.SIT;
        public string Description { get; } = "System Integration Testing";
        public int Id { get; } = 30;
        public StageSIT() { }

        public override string ToString() => Stage.ToString();
    }
}
