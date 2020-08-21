using Antlr4.Runtime.Misc;
using prosr.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prosr.Parser.Compiler
{
    internal sealed class Prosr1Listener : Prosr1BaseListener
    {
        private readonly Ast _ast;
        private readonly Stack<INode> _stack;

        public Prosr1Listener()
        {
            _ast = new Ast();
            _stack = new Stack<INode>();
        }

        public override void ExitPkg([NotNull] Prosr1Parser.PkgContext context)
        {
            _ast.Nodes.Add(new Package(context.fullIdent().GetText()));
        }

        public override void ExitHub([NotNull] Prosr1Parser.HubContext context)
        {
            var hub = new Hub(context.hubIdent().GetText());
            while (true)
            {
                if (!_stack.Any())
                {
                    break;
                }

                hub.Nodes.Add(_stack.Pop());
            }

            _ast.Nodes.Add(hub);
        }

        public override void ExitSending([NotNull] Prosr1Parser.SendingContext context)
        {
            var sending = new Sending(
                context.sendingIdent().GetText(),
                context.sendingMessageIdent().GetText());

            if (!(context.sendingTarget() is null))
            {
                sending.Returning = new Returning(
                    context.returningIdent().GetText(),
                    context.returningMessageIdent().GetText(),
                    context.sendingTarget().GetText());
            }

            _stack.Push(sending);
        }

        public override void ExitReturning([NotNull] Prosr1Parser.ReturningContext context)
        {
            var returning = new Returning(
                context.returningIdent().GetText(),
                context.returningMessageIdent().GetText(),
                context.returningTarget().GetText());

            _stack.Push(returning);
        }

        public override void ExitMessage([NotNull] Prosr1Parser.MessageContext context)
        {
            var message = new Message(context.messageIdent().GetText());
            while (true)
            {
                if (!_stack.Any())
                {
                    break;
                }

                if (!(_stack.Pop() is Field field))
                {
                    throw new InvalidOperationException($"Invalid node type on stack.");
                }

                message.Nodes.Add(field);
            }

            _ast.Nodes.Add(message);
        }

        public override void ExitField([NotNull] Prosr1Parser.FieldContext context)
        {
            var field = new Field(
                !(context.REPEATED() is null),
                context.typeIdent().GetText(),
                context.fieldIdent().GetText(),
                int.Parse(context.NUMBER().GetText()));

            _stack.Push(field);
        }

        internal Ast GetAst()
        {
            _ast.Pack();

            return _ast;
        }
    }
}
