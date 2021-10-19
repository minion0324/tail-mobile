using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Tail.Common
{
    public static class Enumerations
    {
        public static string GetDescription(Enum value)
        {
            return value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description
                ?? value.ToString();
        }
    }

    public enum DeviceModel
    {
        iPhone5 = 0,
        iPhone8 = 1,
        iPhone8Plus = 2,
        iPhoneXR = 3,
        iPhoneX = 4,
        iPhoneXSMax = 5,
        Unknown = 6
    }
    public enum TagTypes
    {
        None = 0,
        Free = 1,
        Paid = 2,
        MoneyLine = 3,
        Spread = 4,
        OverUnder = 5,
        Purchased = 6,

    }
    public enum PostType
    {
        Free = 0,
        Pick = 1,
        SharedFree=2,
        SharedPick=3
    }
    public enum PickPurchaseType
    {
        None = 0,
        Free = 1,
        Paid = 2,
    }
    public enum PickType
    {
        None = 0,
        MoneyLine = 1,
        Spread = 2,
        OverUnder = 3,
    }
    public enum GradientDirection
    {
        Right = 0,
        Left = 1,
        Bottom = 2,
        Top = 3,
        TopLeftToBottomRight = 4,
        TopRightToBottomLeft = 5,
        BottomLeftToTopRight = 6,
    }
    public enum LikeStatus
    {
        None = 0,
        Like = 1,
        Dislike = 2,
    }
    public enum SportType
    {
        None=0,
        Baseball = 1,
        Basketball = 2,
        Football = 3,
        Hocky = 4,
        MMA = 5,
    }
    public enum ResultType
    {
        None = 0,
        Won = 1,
        Lost = 2,
        Push = 3,
    }
    public enum TeamType
    {
        None = 0,
        Team1 = 1,
        Team2 = 2,
    }
    public enum TrendingType
    {
        None = 0,
        Pick = 1,
        Post = 2,
        People = 3,
        PickResult = 4,
        PostResult = 5,
        PeopleResult = 6,

    }
    public enum DeviceType
    {
        Web = 0,
        IOS = 1,
        Android = 2,
    }
    public enum Events
    {
        SmsRecieved,
}
    public enum LoginType
    {
        Email = 0,
        FB = 1,
        Apple = 2,
    }
    public enum TrendingResultType
    {
        All = 0,
        Post = 1,
        Pick = 2,
        User=3
    }
    public enum PushNotificationType
    {
        Unknown = 0,
        Post,
        User,
        SessionOut
    }
}
