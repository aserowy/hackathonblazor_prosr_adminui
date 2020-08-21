using System.Collections.Generic;
using prosr.Parser.Models;

namespace BlazorHub.Client.Tmp
{
    public interface ITmpModelFactory
    {
        Ast Create();
    }

    internal class TmpModelFactory : ITmpModelFactory
    {
        public Ast Create()
        {
            return new Ast()
            {
                Nodes = new List<INode> {
                    new Hub("MyFirstHub")
                    {
                        Nodes = new List<INode>()
                        {
                            new Sending("MyFirstSending", "InputType"),
                            new Returning("MyFirstReturning", "ResponseType", "all")
                        }
                    },

                    new Hub("MySecondHub")
                    {
                        Nodes = new List<INode>()
                        {
                            new Sending("MySecondSending", "InputType2"){
                                Returning = new Returning("MySecondReturning", "ResponseType2", "caller")
                            },
                        }
                    },
                    new Message("ResponseType")
                    {
                        Nodes = new List<Field>
                        {
                            new Field(false, "string", "Property1", 0),
                            new Field(false, "Int32", "Property2", 1)
                        }
                    },
                    new Message("InputType2")
                    {
                        Nodes = new List<Field>
                        {
                            new Field(false, "string", "Property1", 0),
                            new Field(false, "Int32", "Property2", 1)
                        }
                    },
                    new Message("InputType")
                    {
                        Nodes = new List<Field>
                        {
                            new Field(false, "string", "Property1", 0),
                            new Field(false, "Int32", "Property2", 1)
                        }
                    },
                }
            };
        }
    }
}
