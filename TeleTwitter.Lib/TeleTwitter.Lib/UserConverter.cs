using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TeleTwitter.Lib.Json
{
    /// <summary>
    /// Converts a json string to a User object
    /// </summary>
    public class UserConverter : JsonConverter
    {
        #region Writing

        public override void WriteJson(JsonWriter writer, object value)
        {
            User user = value as User;

            if (user == null)
                throw new ArgumentException("Value must be a User", "value");

            writer.WriteStartObject();

            //name"
            writer.WritePropertyName("name");
            writer.WriteValue(user.Name);
            //"description"
            writer.WritePropertyName("description");
            writer.WriteValue(user.Description);
            //"location"
            writer.WritePropertyName("location");
            writer.WriteValue(user.Location);
            //"url"
            writer.WritePropertyName("url");
            writer.WriteValue(user.Url);
            //"screen_name"
            writer.WritePropertyName("screen_name");
            writer.WriteValue(user.ScreenName);
            //"id"
            writer.WritePropertyName("id");
            writer.WriteValue(user.Id);
            //"protected"
            //writer.WritePropertyName("protected");
            //writer.WriteValue(user.isprotected);
            //"profile_image_url"
            writer.WritePropertyName("profile_image_url");
            writer.WriteValue(user.ProfileImageUrl);

            writer.WriteEndObject();
        }

        #endregion

        #region Reading
        public override object ReadJson(JsonReader reader, Type objectType)
        {
            // maybe have CanReader and a CanWrite methods so this sort of test wouldn't be necessary
            if (objectType != typeof(User))
                throw new JsonSerializationException("UserConverter only supports deserializing User objects");

            User user = new User();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        string memberName = reader.Value.ToString();
                        if (!reader.Read())
                            throw new JsonSerializationException(string.Format("Unexpected end when setting {0}'s value.", memberName));

                        if (reader.Value != null)
                        {
                            switch (memberName)
                            {
                                //"name"
                                case "name":
                                    {
                                        user.Name = reader.Value.ToString();
                                        break;
                                    }
                                //"description"
                                case "description":
                                    {
                                        user.Description = reader.Value.ToString();
                                        break;
                                    }
                                //"location"
                                case "location":
                                    {
                                        user.Location = reader.Value.ToString();
                                        break;
                                    }
                                //"url"
                                case "url":
                                    {
                                        user.Url = reader.Value.ToString();
                                        break;
                                    }
                                //"screen_name",
                                case "screen_name":
                                    {
                                        user.ScreenName = reader.Value.ToString();
                                        break;
                                    }
                                //"id"
                                case "id":
                                    {
                                        user.Id = reader.Value.ToString();
                                        break;
                                    }
                                //"protected"
                                case "protected":
                                    {
                                        //user.isprotected = Convert.ToBoolean(reader.Value);
                                        break;
                                    }
                                //"profile_image_url"
                                case "profile_image_url":
                                    {
                                        user.ProfileImageUrl = reader.Value.ToString();
                                        break;
                                    }
                            }
                        }
                        break;
                    case JsonToken.EndObject:
                        return user;
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
            return typeof(User).IsAssignableFrom(valueType);
        }
    }
}
