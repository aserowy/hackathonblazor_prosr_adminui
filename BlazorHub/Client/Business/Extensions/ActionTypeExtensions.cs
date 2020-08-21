using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorHub.Client.Business.Extensions
{
    public static class ActionTypeExtensions
    {
        public static string GetActionName(this ActionType actionType) 
        {
            return actionType.GetDisplayAttribute()?.GetName() ?? actionType.ToString();
        }

        public static string GetGroupName(this ActionType actionType)
        {
            return actionType.GetDisplayAttribute()?.GetGroupName();
        }

        public static DisplayAttribute GetDisplayAttribute(this ActionType actionType)
        {
            var fieldInfo = actionType.GetType().GetField(actionType.ToString());
            return fieldInfo.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
