using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bookstore.Services.External
{

    public partial class GoogleBookSearchResultDto
    {
        [JsonProperty("kind")]
        public string? Kind { get; set; }

        [JsonProperty("totalItems")]
        public long TotalItems { get; set; }

        [JsonProperty("items")]
        public List<Item>? Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("kind")]
        public string? Kind { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("etag")]
        public string? Etag { get; set; }

        [JsonProperty("selfLink")]
        public Uri SelfLink { get; set; } = default!;

        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; } = default!;

        [JsonProperty("saleInfo")]
        public SaleInfo SaleInfo { get; set; } = default!;

        [JsonProperty("accessInfo")]
        public AccessInfo AccessInfo { get; set; } = default!;

        [JsonProperty("searchInfo")]
        public SearchInfo SearchInfo { get; set; } = default!;
    }

    public partial class AccessInfo
    {
        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("viewability")]
        public string? Viewability { get; set; }

        [JsonProperty("embeddable")]
        public bool Embeddable { get; set; }

        [JsonProperty("publicDomain")]
        public bool PublicDomain { get; set; }

        [JsonProperty("textToSpeechPermission")]
        public string? TextToSpeechPermission { get; set; }

        [JsonProperty("epub")]
        public Epub Epub { get; set; } = default!;

        [JsonProperty("pdf")]
        public Epub Pdf { get; set; } = default!;

        [JsonProperty("webReaderLink")]
        public Uri WebReaderLink { get; set; } = default!;

        [JsonProperty("accessViewStatus")]
        public string? AccessViewStatus { get; set; }

        [JsonProperty("quoteSharingAllowed")]
        public bool QuoteSharingAllowed { get; set; }
    }

    public partial class Epub
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }
    }

    public partial class SaleInfo
    {
        [JsonProperty("country")]
        public string? Country { get; set; } 

        [JsonProperty("saleability")]
        public string? Saleability { get; set; }

        [JsonProperty("isEbook")]
        public bool IsEbook { get; set; }
    }

    public partial class SearchInfo
    {
        [JsonProperty("textSnippet")]
        public string? TextSnippet { get; set; }
    }

    public partial class VolumeInfo
    {
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("subtitle")]
        public string? Subtitle { get; set; }

        [JsonProperty("authors")]
        public List<string>? Authors { get; set; }

        [JsonProperty("publisher")]
        public string? Publisher { get; set; }

        //[JsonProperty("publishedDate")]
        //public string? PublishedDate { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("industryIdentifiers")]
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; } = default!;

        [JsonProperty("readingModes")]
        public ReadingModes ReadingModes { get; set; } = default!;

        [JsonProperty("pageCount")]
        public long PageCount { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; } = default!;

        [JsonProperty("categories")]
        public List<string>? Categories { get; set; }

        [JsonProperty("maturityRating")]
        public string? MaturityRating { get; set; }

        [JsonProperty("allowAnonLogging")]
        public bool AllowAnonLogging { get; set; }

        [JsonProperty("contentVersion")]
        public string? ContentVersion { get; set; }

        [JsonProperty("panelizationSummary")]
        public PanelizationSummary PanelizationSummary { get; set; } = default!;

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; } = default!;

        [JsonProperty("language")]
        public string? Language { get; set; }

        [JsonProperty("previewLink")]
        public Uri PreviewLink { get; set; } = default!;

        [JsonProperty("infoLink")]
        public Uri InfoLink { get; set; } = default!;

        [JsonProperty("canonicalVolumeLink")]
        public Uri CanonicalVolumeLink { get; set; } = default!;
    }

    public partial class ImageLinks
    {
        [JsonProperty("smallThumbnail")]
        public Uri SmallThumbnail { get; set; } = default!;

        [JsonProperty("thumbnail")]
        public Uri Thumbnail { get; set; } = default!;
    }

    public partial class IndustryIdentifier
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("identifier")]
        public string? Identifier { get; set; }
    }

    public partial class PanelizationSummary
    {
        [JsonProperty("containsEpubBubbles")]
        public bool ContainsEpubBubbles { get; set; }

        [JsonProperty("containsImageBubbles")]
        public bool ContainsImageBubbles { get; set; }
    }

    public partial class ReadingModes
    {
        [JsonProperty("text")]
        public bool Text { get; set; }

        [JsonProperty("image")]
        public bool Image { get; set; }
    }

    public partial class GoogleBookSearchResultDto
    {
        public static GoogleBookSearchResultDto FromJson(string json) => JsonConvert.DeserializeObject<GoogleBookSearchResultDto>(json, Bookstore.Services.External.Converter.Settings)!;
    }

    public static class GoogleSerialize
    {
        public static string ToJson(this GoogleBookSearchResultDto self) => JsonConvert.SerializeObject(self, Bookstore.Services.External.Converter.Settings);
    }

    //internal static class GoogleConverter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters =
    //        {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}

    internal class GoogleParseStringConverter : JsonConverter
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
            throw new Exception("Cannot unmarshal type long");
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

