using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class PicksDetails
    {
        
            [JsonProperty("UserId")]
            public int UserId
            {
                get;
                set;
            }

            [JsonProperty("UserName")]
            public string UserName
            {
                get;
                set;
            }

            [JsonProperty("UserImage")]
            public string UserImage
            {
                get;
                set;
            }

            [JsonProperty("PicksedDate")]
            public string PickedDate
            {
                get;
                set;
            }
            [JsonIgnore]
            public string DisplayPickedDate
            {
                get
                {
                    if (!string.IsNullOrEmpty(PickedDate))
                    {
                        DateTime _pickedDateTime = Convert.ToDateTime(PickedDate);
                        TimeSpan _timeSpan = DateTime.Now.Subtract(_pickedDateTime);
                        if (_timeSpan.Hours < 24)
                        {
                            return _timeSpan.Hours + " hours ago";
                        }
                        else if (_timeSpan.Hours < 48)
                        {
                            return "1 day ago";
                        }
                        else
                        {
                            return PickedDate;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
            }
            [JsonProperty("Status")]
            public string Status
            {
                get;
                set;
            }
            [JsonProperty("MoneyLine")]
            public string MoneyLine
            {
                get;
                set;
            }
            
            [JsonProperty("PuchaseCount")]
            public int PuchaseCount
            {
                get;
                set;
            }
            [JsonProperty("Last15Good")]
            public string Last15Good
            {
                get;
                set;
            }
            [JsonProperty("Last15Avg")]
            public string Last15Avg
            {
                get;
                set;
            }
            [JsonProperty("Last15Bad")]
            public string Last15Bad
            {
                get;
                set;
            }
            [JsonProperty("BestPick")]
            public string BestPick
            {
                get;
                set;
            }
            [JsonProperty("PickAccuracy")]
            public string PickAccuracy
            {
                get;
                set;
            }
            [JsonProperty("GameList")]
            public List<GameDetails> GameList
            {
                get;
                set;
            }
            [JsonProperty("LikeCount")]
            public int LikeCount
            {
                get;
                set;
            }
            [JsonProperty("CommentCount")]
            public int CommentCount
            {
                get;
                set;
            }
            [JsonProperty("DislikeCount")]
            public int DislikeCount
            {
                get;
                set;
            }
        }
}
