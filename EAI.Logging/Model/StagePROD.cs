namespace EAI.Logging.Model
{
    public class StagePROD : ILogStage
    {
        public LogStage Stage { get; } = LogStage.PROD;
        public string Description { get; } = "Production";
        public int Id { get; } = 50;
        public StagePROD() { }

        public override string ToString() => Stage.ToString();
    }
}
