namespace Logger;

public abstract record Figure : LogType
{
    public abstract record Pawn : Figure
    {
        public sealed record M2sq : Pawn;
        
        public sealed record M1sq : Pawn;
        
        public sealed record E1sq : Pawn;
    }

    public abstract record Knight : Figure
    {
        public sealed record Move : Knight;
    }

    public abstract record Bishop : Figure
    {
        public sealed record Move : Bishop;
    }


    public abstract record Rook : Figure
    {
        public sealed record Move : Rook;
    }

    public abstract record Queen : Figure
    {
        public sealed record Move : Queen;
    }

    public abstract record King : Figure
    {
        public sealed record Move : King;
        public sealed record Check : King;
    }
}