using System;

namespace Grid
{
    public readonly struct CellPosition
    {
        public static CellPosition Zero => new(0, 0);

        // ReSharper disable once InconsistentNaming
        public readonly int x;

        // ReSharper disable once InconsistentNaming
        public readonly int y;

        public CellPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static CellPosition FromSerializableData(SerializableData data) => new(data.X, data.Y);

        public override string ToString() => $"{x};{y}";

        public CellPosition GetOnAbove() => new(x, y + 1);

        public CellPosition GetOnRight() => new(x + 1, y);

        public CellPosition GetOnBelow() => new(x, y - 1);

        public CellPosition GetOnLeft() => new(x - 1, y);

        public static bool operator ==(CellPosition a, CellPosition b) => a.x == b.x && a.y == b.y;

        public static bool operator !=(CellPosition a, CellPosition b) => !(a == b);

        public override bool Equals(object obj) => obj is CellPosition other && base.Equals(other);

        public override int GetHashCode() => HashCode.Combine(x, y);

        public static CellPosition operator +(CellPosition a, CellPosition b) => new(a.x + b.x, a.y + b.y);

        public static CellPosition operator -(CellPosition a, CellPosition b) => new(a.x - b.x, a.y - b.y);

        [Serializable]
        public struct SerializableData
        {
            public int X;
            public int Y;
        }
    }
}