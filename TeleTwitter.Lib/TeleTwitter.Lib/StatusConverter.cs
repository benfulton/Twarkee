using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace TeleTwitter.Lib.Json
{
    /// <summary>
    /// Converts a JSON string to a Status object
    /// </summary>
    public class StatusConverter : JsonConverter
    {
        #region Writing

        public override void WriteJson(JsonWriter writer, object value)
        {
            Status status = value as Status;

            if (status == null)
                throw new ArgumentException("Value must be a Status", "value");

            writer.WriteStartObject();

            //"created_at"
            writer.WritePropertyName("created_at");
            writer.WriteValue(status.CreatedAt);
            //"text"
            writer.WritePropertyName("text");
            writer.WriteValue(status.Text);
            //"id"
            writer.WritePropertyName("id");
            writer.WriteValue(status.Id);
            
            //"user"
            writer.WritePropertyName("user");
            new UserConverter().WriteJson(writer, status.User);


            writer.WriteEndObject();
        }

        #endregion

        #region Reading
        public override object ReadJson(JsonReader reader, Type objectType)
        {
            // maybe have CanReader and a CanWrite methods so this sort of test wouldn't be necessary
            if (objectType != typeof(Status))
                throw new JsonSerializationException("StatusConverter only supports deserializing Status objects");

            Status status = new Status();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        string memberName = reader.Value.ToString();
                        if (!reader.Read())
                            throw new JsonSerializationException(string.Format("Unexpected end when setting {0}'s value.", memberName));

                        switch (memberName)
                        {
                            //"created_at"
                            case "created_at":
                                {
                                    status.CreatedAt = DateTime.ParseExact(GetReaderValue(reader.Value), "ddd MMM dd HH:mm:ss zzzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                                    break;
                                }
                            //"description":"",
                            case "text":
                                {
                                    status.Text = GetReaderValue(reader.Value);
                                    break;
                                }
                            //"id"
                            case "id":
                                {
                                    status.Id = GetReaderValue(reader.Value);
                                    break;
                                }
                            //"user"
                            case "user":
                                {
                                    UserConverter converter = new UserConverter();
                                    status.User = converter.ReadJson(reader, typeof(User)) as User;
                                    break;
                                }
                        }
                        break;
                    case JsonToken.EndObject:
                        return status;
                    default:
                        throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
                }
            }

            throw new JsonSerializationException("Unexpected end when deserializing object.");
            return null;
        }

        #endregion

        public override bool CanConvert(Type valueType)
        {
            return typeof(Status).IsAssignableFrom(valueType);
        }

        private string GetReaderValue(object value)
        {
            string rtn = string.Empty;
            if (value != null)
            {
                rtn = value.ToString();
            }
            return rtn;
        }
    }
}
