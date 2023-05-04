namespace EAI.Logging.Model
{
    public class StagePROD : ILogStage
    {
        public LogStage Stage { get; } = LogStage.PROD;
        public string Description { get; } = "Production" ;
        public StagePROD() { }

        public override string ToString() => Stage.ToString();
    }
}
