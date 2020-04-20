using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteDois.Utils
{
    public class DateTimeUtcConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value) => (DateTime)value;

        public object FromEntry(DynamoDBEntry entry)
        {
            var dateTime = entry.AsDateTime();
            return dateTime.ToUniversalTime();
        }
    }
}