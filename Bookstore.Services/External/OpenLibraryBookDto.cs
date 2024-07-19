using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Bookstore.Services.External
{
    public partial class OpenLibraryBookDto
    {
        [JsonProperty("url")]
        public Uri Url { get; set; } = default!;

        [JsonProperty("key")]
        public string? Key { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("subtitle")]
        public string? Subtitle { get; set; }

        [JsonProperty("authors")]
        public List<Author>? Authors { get; set; }

        [JsonProperty("number_of_pages")]
        public long NumberOfPages { get; set; }

        [JsonProperty("pagination")]
        public string? Pagination { get; set; }

        [JsonProperty("by_statement")]
        public string? ByStatement { get; set; }

        [JsonProperty("identifiers")]
        public Dictionary<string, List<string>>? Identifiers { get; set; }

        [JsonProperty("classifications")]
        public Classifications Classifications { get; set; } = default!;

        [JsonProperty("publishers")]
        public List<Publish>? Publishers { get; set; }

        [JsonProperty("publish_places")]
        public List<Publish>? PublishPlaces { get; set; }

        //[JsonProperty("publish_date")]
        //public string? PublishDate { get; set; }

        [JsonProperty("subjects")]
        public List<Author>? Subjects { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; }

        [JsonProperty("ebooks")]
        public List<Ebook>? Ebooks { get; set; }

        [JsonProperty("cover")]
        public Cover Cover { get; set; } = default!;
    }

    public partial class Author
    {
        [JsonProperty("url")]
        public Uri Url { get; set; } = default!;

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public partial class Classifications
    {
        [JsonProperty("lc_classifications")]
        public List<string>? LcClassifications { get; set; }

        [JsonProperty("dewey_decimal_class")]
        public List<string>? DeweyDecimalClass { get; set; }
    }

    public partial class Cover
    {
        [JsonProperty("small")]
        public Uri Small { get; set; } = default!;

        [JsonProperty("medium")]
        public Uri Medium { get; set; } = default!; 

        [JsonProperty("large")]
        public Uri Large { get; set; } = default!;
    }

    public partial class Ebook
    {
        [JsonProperty("preview_url")]
        public Uri PreviewUrl { get; set; } = default!;

        [JsonProperty("availability")]
        public string? Availability { get; set; }

        [JsonProperty("formats")]
        public Formats Formats { get; set; } = default!;

        [JsonProperty("borrow_url")]
        public Uri BorrowUrl { get; set; } = default!;

        [JsonProperty("checkedout")]
        public bool Checkedout { get; set; }
    }

    public partial class Formats
    {
    }

    public partial class Publish
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public partial class OpenLibraryBookDto
    {
        public static OpenLibraryBookDto FromJson(string json) => JsonConvert.DeserializeObject<OpenLibraryBookDto>(json, Bookstore.Services.External.Converter.Settings)!;
    }

    public static class Serialize
    {
        public static string ToJson(this OpenLibraryBookDto self) => JsonConvert.SerializeObject(self, Bookstore.Services.External.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null!;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long"); //дава грешка и не иска да продължи да отваря друга книга!
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
