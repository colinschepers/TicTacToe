using Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class State : IState
    {
        static State()
        {
            FillMovesCache();
        }

        private static void FillMovesCache()
        {
            for (int i = 0; i < 0b1000000000; i++)
            {
                _moves[i] = Enumerable.Range(0, 9)
                    .Where(x => (i & (1 << x)) == 0)
                    .Select(x => new Move(x))
                    .ToArray();
            }
        }

        private static readonly int[] _bitMove = new[]
            {
            0b000001000000001000000001,
            0b000000000001000000000010,
            0b001000001000000000000100,
            0b000000000000010000001000,
            0b010010000010000000010000,
            0b000000010000000000100000,
            0b100000000000100001000000,
            0b000000000100000010000000,
            0b000100100000000100000000
        };

        private static readonly int _lineFilter =
            0b001001001001001001001001;

        private static readonly Move[][] _moves = new Move[512][];

        public int PlayerToMove => Round & 1;
        public int Round { get; private set; }
        public bool GameOver { get; private set; }

        private int[] _boards = new int[2];
        private double _score;

        public void Play(IMove move)
        {
            if (!IsValid(move))
            {
                throw new ArgumentException("Invalid move " + move);
            }

            var m = (Move)move;
            var player = Round++ & 1;
            var board = _boards[player] |= _bitMove[m.Position];

            if (IsLine(board))
            {
                GameOver = true;
                _score = 1 - player;
            }
            else if (Round == 9)
            {
                GameOver = true;
                _score = 0.5;
            }
        }

        private bool IsLine(int bitBoard)
        {
            return (bitBoard & (bitBoard >> 1) & (bitBoard >> 2) & _lineFilter) != 0;
        }

        private int GetMergedBoard()
        {
            return (_boards[0] | _boards[1]) & 0b111111111;
        }

        public bool IsValid(IMove move)
        {
            return move is Move m && (GetMergedBoard() & _bitMove[m.Position]) == 0;
        }
        
        public IEnumerable<IMove> GetValidMoves()
        {
            return _moves[GetMergedBoard()];
        }

        public double GetScore(int playerNr)
        {
            return playerNr == 0 ? _score : 1 - _score;
        }

        public void Set(IState state)
        {
            var s = (State)state;
            _boards[0] = s._boards[0];
            _boards[1] = s._boards[1];
            _score = s._score;
            Round = s.Round;
            GameOver = s.GameOver;
        }

        public IState Clone()
        {
            var state = new State();
            state.Set(this);
            return state;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 9; i++)
            {
                var p0 = (_boards[0] & (1 << i)) != 0;
                var p1 = (_boards[1] & (1 << i)) != 0;
                
                builder.Append(p0 ? 'X' : p1 ? 'O' : '.');

                if ((i + 1) % 3 == 0)
                {
                    builder.AppendLine();
                } 
            }
            return builder.ToString();
        }
    }
}