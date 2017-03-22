//using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Accounting.Sqlserver
{
    public static class XmlMapper
    {
        //public static BsonDocument MapToBson(this XmlDocument xDom)
        //{
        //    var eventNode = xDom["Event"];
        //    var doc = new BsonDocument();


        //    foreach (XmlElement c in eventNode.ChildNodes)
        //    {
        //        var dataType = (DataType)int.Parse(c.GetAttribute("data_type"));

        //        doc.Add(GetBsonElement(dataType, c.Name, c.InnerText));
        //    }
        //    return doc;
        //}

        //private static BsonElement GetBsonElement(DataType datatype, string name, object value)
        //{
        //    if (!String.IsNullOrEmpty(name) && name.Contains("-"))
        //    {
        //        name = name.Replace("-", "_");
        //    }
        //    if (Enum.IsDefined(typeof(DataType), datatype))
        //    {
        //        switch (datatype)
        //        {
        //            case DataType.NonNegativeIntegers:
        //                if (value != null)
        //                {
        //                    return new BsonElement(name, ulong.Parse(value.ToString()));
        //                }
        //                break;
        //            case DataType.Strings:
        //                if (name == "User_Name" || name == "SAM_Account_Name")
        //                {
        //                    return new BsonElement(name, value?.ToString().ToLower());
        //                }
        //                return new BsonElement(name, value?.ToString());
        //            case DataType.HexadecimalNumbers:
        //                return new BsonElement(name, value?.ToString());
        //            case DataType.IPv4Addresses:
        //                return new BsonElement(name, value?.ToString());
        //            case DataType.DateAndTime:
        //                if (value != null)
        //                {
        //                    return new BsonElement(name, DateTime.Parse(value.ToString()));
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return new BsonElement(name + "_unknown_" + datatype.ToString(), value?.ToString());
        //}

        public static string MapToJsonString(this XmlDocument xDom)
        {
            var eventNode = xDom["Event"];
            //var doc = new BsonDocument();
            var doc = new StringBuilder();

            var count = eventNode.ChildNodes.Count;
            var i = 1;

            doc.Append('{');
            foreach (XmlElement c in eventNode.ChildNodes)
            {
                var dataType = (DataType)int.Parse(c.GetAttribute("data_type"));


                if (i++ == count)
                {
                    doc.AppendLine(GetJsonKeyValue(dataType, c.Name, c.InnerText));
                }
                else
                {
                    doc.AppendLine(GetJsonKeyValue(dataType, c.Name, c.InnerText) + ",");
                }
            }

            doc.Append('}');
            return doc.ToString();
        }

        private static string GetJsonKeyValue(DataType datatype, string name, object value)
        {
            if (!String.IsNullOrEmpty(name) && name.Contains("-"))
            {
                name = name.Replace("-", "_");
            }
            if (Enum.IsDefined(typeof(DataType), datatype))
            {
                switch (datatype)
                {
                    case DataType.NonNegativeIntegers:
                        if (value != null)
                        {
                            return $@"""{name}"": {value.ToString()}";
                            //return new BsonElement(name, ulong.Parse(value.ToString()));
                        }
                        break;
                    case DataType.Strings:
                        if (name == "User_Name" || name == "SAM_Account_Name")
                        {
                            return $@"""{name}"": ""{value?.ToString().ToLower()}""";
                            //return new BsonElement(name, value?.ToString().ToLower());
                        }
                        return $@"""{name}"": ""{value?.ToString()}""";
                    //return new BsonElement(name, value?.ToString());
                    case DataType.HexadecimalNumbers:
                        return $@"""{name}"": ""{value?.ToString()}""";
                    //return new BsonElement(name, value?.ToString());
                    case DataType.IPv4Addresses:
                        return $@"""{name}"": ""{value?.ToString()}""";
                    //return new BsonElement(name, value?.ToString());
                    case DataType.DateAndTime:
                        if (value != null)
                        {
                            return $@"""{name}"": ""{DateTime.Parse(value.ToString())}""";
                            //return new BsonElement(name, DateTime.Parse(value.ToString()));
                        }
                        break;
                    default:
                        break;
                }
            }
            return $@"""{name}_unknown_{datatype.ToString()}"": ""{value?.ToString()}""";
            //return new BsonElement(name + "_unknown_" + datatype.ToString(), value?.ToString());
        }

        public enum DataType
        {
            NonNegativeIntegers = 0,
            Strings = 1,
            HexadecimalNumbers = 2,
            IPv4Addresses = 3,
            DateAndTime = 4,
        }
        /*
        Non-negative integers (data_type=0)
        Strings (data_type=1)
        Hexadecimal numbers (data_type=2)
        IPv4 addresses (data_type=3)
        Date and time (data_type=4)
        */
    }

}
