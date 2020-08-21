namespace prosr.Parser.Models
{
    public sealed class Field : INode
    {
        public Field(bool isRepeated, string type, string ident, int number)
        {
            IsRepeated = isRepeated;
            Type = type;
            Ident = ident;
            Number = number;
        }

        public Token Token => Token.Field;

        public bool IsRepeated { get; set; }
        public string Type { get; set; }
        public string Ident { get; set; }
        public int Number { get; set; }
    }
}
