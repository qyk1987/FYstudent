﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Common
{
    public class JsonDateTimeConverter : IsoDateTimeConverter
    {
        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    DateTime dataTime;
        //    if (DateTime.TryParse(reader.Value.ToString(), out dataTime))
        //    {
        //        return dataTime;
        //    }
        //    else
        //    {
        //        return existingValue;
        //    }
        //}

        //public JsonDateTimeConverter()
        //{
        //    DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        //}
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long jsTimeStamp = long.Parse(reader.Value.ToString());
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime dt = startTime.AddMilliseconds(jsTimeStamp);
            return dt;
        }

        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{
        //    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        //    long timeStamp = (long)(((DateTime)value) - startTime).TotalMilliseconds;
        //    writer.WriteValue(timeStamp);
        //}
    }
}