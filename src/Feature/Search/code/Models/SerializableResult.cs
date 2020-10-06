using System;
using Sitecore.XA.Feature.Search.Models;

namespace XA.Extensions.Feature.Search.Models
{
    public class SerializableResult
    {
        public Guid Id { get; set; }

        public string Language { get; set; }

        public string Path { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Html { get; set; }

        public SerializableResult() { }

        public SerializableResult(Result result)
        {
            Id = result.Id;
            Language = result.Language;
            Path = result.Path;
            Url = result.Url;
            Name = result.Name;
            Html = result.Html;
        }
    }
}
