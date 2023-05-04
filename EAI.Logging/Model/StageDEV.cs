namespace EAI.Logging.Model
{
    public class StageDEV : ILogStage
    {
        public LogStage Stage { get; } = LogStage.DEV;
        public string Description { get; } = "Development" ;
        public StageDEV() { }

        public override string ToString() => Stage.ToString();
    }
}
