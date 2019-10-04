using Core.Games;

namespace GameLogic
{
    public class Move : IMove
    {
        public int Position { get; set; }

        public Move(int position)
        {
            Position = position;
        }

        public override bool Equals(object obj)
        {
            return obj is Move move && Position == move.Position;
        }

        public override int GetHashCode()
        {
            return 997 + 107 * Position.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0})", Position);
        }
    }
}