namespace prosr.Parser.Models
{
    public sealed class Sending : INode
    {
        public Sending(string ident, string inputType)
        {
            Ident = ident;
            InputType = inputType;
        }

        public Token Token => Token.Sending;

        public string Ident { get; set; }
        public string InputType { get; set; }
        public Returning? Returning { get; set; }
    }
}
