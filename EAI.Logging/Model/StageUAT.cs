namespace EAI.Logging.Model
{
    public class StageUAT : ILogStage
    {
        public LogStage Stage { get; } = LogStage.UAT;
        public string Description { get; } = "User Acceptance Testing";
        public int Id { get; } = 20;
        public StageUAT() { }

        public override string ToString() => Stage.ToString();
    }
}
