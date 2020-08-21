using prosr.Parser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHub.Client.Business
{
    public interface IActionTypeNameResolver
    {
        ActionType ResolveType(INode node);
    }

    internal class ActionTypeResolver : IActionTypeNameResolver
    {
        public ActionType ResolveType(INode node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            // hier könnte ihre strategy stehen
            if (node is Sending sending)
            {
                return sending.Returning != null ? ActionType.ClientActionWithReturn : ActionType.ClientActionWithoutReturn;
            }
            if (node is Returning)
            {
                return ActionType.HubAction;
            }
            return ActionType.Invalid;
        }
    }

    public enum ActionType
    {
        [Display(Name = "Client Action", GroupName = "post")]
        ClientActionWithoutReturn,
        [Display(Name = "Client Action", GroupName = "put")]
        ClientActionWithReturn,
        [Display(Name = "Hub Action", GroupName = "get")]
        HubAction,
        [Display(Name = "INVALID ACTOIN", GroupName = "delete")]
        Invalid,
    }
}
