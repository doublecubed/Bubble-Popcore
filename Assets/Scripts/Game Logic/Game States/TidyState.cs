// Onur Ereren - June 2024
// Popcore case

namespace PopsBubble
{
    public class TidyState : GameState
    {

        public override void OnEnter()
        {
            OnStateComplete?.Invoke();
        }
    }
}
