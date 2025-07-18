public class BattleCanvas : SourceCanvas
{
    public override void InvokeCanvas()
    {
        base.InvokeCanvas();

        OpenPanel<BattlePanel>();
    }
}
