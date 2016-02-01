using Carubbi.Utils.Data;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.BR;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace Carubbi.ElasticSearch
{
    public class ElasticSearcher
    {

        private IndexSearcher _searcher;

        public ElasticSearcher(string path)
        {
            Directory directory = FSDirectory.Open(path);
            _searcher = new IndexSearcher(directory);
        }

        public T GetById<T>(Guid id)
        {
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "#id", CreateAnalyzer());
            Query qry = parser.Parse(id.ToString());

            TopScoreDocCollector collector = TopScoreDocCollector.Create(1, true);
            _searcher.Search(qry, collector);
            ScoreDoc[] hits = collector.TopDocs().ScoreDocs;
            int docId = hits[0].Doc;
            Lucene.Net.Documents.Document doc = _searcher.Doc(docId);


            
            T item = Activator.CreateInstance<T>();
            foreach (var field in doc.GetFields())
            {
                item.SetProperty(field.Name, field.StringValue);
            }

            return item;
        }

        public IEnumerable<T> Search<T>(string terms, params string[] fields)
        {
            //QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, CreateAnalyzer());
            //Query query = parser.Parse(terms);
            //var query = parser.Parse(string.Format("\"{0}\"", terms));
            var query = MultiFieldQueryParser.Parse(Lucene.Net.Util.Version.LUCENE_CURRENT, new string[] { terms }, fields, CreateAnalyzer());
            var results = _searcher.Search(query, int.MaxValue);

            foreach (var result in results.ScoreDocs.OrderByDescending(d => d.Score))
            {
                yield return CastDocument<T>(_searcher.Doc(result.Doc));
            }
        }

        private Analyzer CreateAnalyzer()
        {
            return new BrazilianAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }

        private T CastDocument<T>(Lucene.Net.Documents.Document doc)
        {
            T item = Activator.CreateInstance<T>();
            foreach (var field in doc.GetFields())
            {
                if (field.Name == "#id")
                    continue;

                var converter = TypeDescriptor.GetConverter(item.GetType().GetProperty(field.Name).PropertyType);
                item.SetProperty(field.Name, converter.ConvertFromString(field.StringValue));
            }
            return item;
        }
    }
}
