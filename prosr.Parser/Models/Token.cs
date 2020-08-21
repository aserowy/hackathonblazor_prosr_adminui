namespace prosr.Parser.Models
{
    public class Token : Enumeration
    {
        public static readonly Token Package = new Token(1, "package");
        public static readonly Token Hub = new Token(1, "hub");
        public static readonly Token Sending = new Token(1, "sending");
        public static readonly Token Returning = new Token(1, "returning");
        public static readonly Token Message = new Token(1, "message");
        public static readonly Token Field = new Token(1, "field");

        public Token(int id, string name) : base(id, name)
        {
        }
    }
}
