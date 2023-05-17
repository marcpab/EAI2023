namespace EAI.Logging.Model
{
    public class StageDEV : ILogStage
    {
        public LogStage Stage { get; } = LogStage.DEV;
        public string Description { get; } = "Development";
        public int Id { get; } = 10;
        public StageDEV() { }

        public override string ToString() => Stage.ToString();
    }
}
