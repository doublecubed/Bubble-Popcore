// Onur Ereren - June 2024
// Popcore case

namespace PopsBubble
{
    public interface IShootIndicator
    {
        public void Set(int value, int nextValue);

        public Bubble CurrentBubble();
        public Bubble NextBubble();
    }
}
