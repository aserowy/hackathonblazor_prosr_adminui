namespace prosr.Parser.Models
{
    public sealed class Returning : INode
    {
        public Returning(string ident, string responseType, string target)
        {
            Ident = ident;
            ResponseType = responseType;
            Target = target;
        }

        public Token Token => Token.Returning;

        public string Ident { get; set; }
        public string ResponseType { get; set; }
        public string Target { get; set; }
    }
}
