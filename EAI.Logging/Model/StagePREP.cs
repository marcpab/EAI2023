namespace EAI.Logging.Model
{
    public class StagePREP : ILogStage
    {
        public LogStage Stage { get; } = LogStage.PREP;
        public string Description { get; } = "Prepare";

        public StagePREP() { }

        public override string ToString() => Stage.ToString();
    }
}
