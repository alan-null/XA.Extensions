using System.Collections.Generic;
using System.Linq;
using Sitecore.XA.Feature.Search.Models;

namespace XA.Extensions.Feature.Search.Models
{
    public class SerializableResultSet
    {
        public long TotalTime { get; set; }

        public long QueryTime { get; set; }

        public string Signature { get; set; }

        public string Index { get; set; }

        public int Count { get; set; }

        public List<SerializableResult> Results { get; set; }

        public SerializableResultSet() { }

        public SerializableResultSet(ResultSet resultSet)
        {
            TotalTime = resultSet.TotalTime;
            QueryTime = resultSet.QueryTime;
            Signature = resultSet.Signature;
            Index = resultSet.Index;
            Count = resultSet.Count;
            Results = resultSet.Results.Select(r => new SerializableResult(r)).ToList();
        }
    }
}
