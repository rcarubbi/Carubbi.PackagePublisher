using Carubbi.Utils.Data;
using Lucene.Net.Analysis.BR;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Carubbi.ElasticSearch
{
    public class BrazilianIndexer : IDisposable
    {
        private bool _autoGenerateIds = false;
        public BrazilianIndexer(string indexerFile, bool autoGenerateIds)
        {
            _indexerFile = indexerFile;
            _autoGenerateIds = autoGenerateIds;
            _writer = CreateWriter();
        }
        
        private Lucene.Net.Store.FSDirectory _directory;
        private string _indexerFile;

        private IndexWriter _writer;

        private Document CreateDocument<T>(T content, Guid id)
        {
            Document document = new Document();

            if (_autoGenerateIds)
                document.Add(new Field("#id", id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            foreach (var prop in content.GetType().GetProperties())
            {
                if (!_autoGenerateIds && prop.CustomAttributes.OfType<ElasticSearchKeyPropertyAttribute>().Count() > 0)
                {
                    document.Add(new Field(prop.Name, content.GetProperty<object>(prop.Name).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                else
                {
                    document.Add(new Field(prop.Name, content.GetProperty<object>(prop.Name).ToString(), Field.Store.YES, Field.Index.ANALYZED));
                }
            }
          
            return document;
        }

        private IndexWriter CreateWriter()
        {
            bool newIndex = !Directory.Exists(_indexerFile);
            _directory = Lucene.Net.Store.FSDirectory.Open(_indexerFile);

            return new IndexWriter(
              _directory,
              new BrazilianAnalyzer(Lucene.Net.Util.Version.LUCENE_30, BrazilianAnalyzer.BRAZILIAN_STOP_WORDS),
              newIndex,
              IndexWriter.MaxFieldLength.UNLIMITED);
        }

        public Guid Add<T>(T content)
        {
            Guid newId = Guid.Empty;
            if (_autoGenerateIds)
            {
                newId = Guid.NewGuid();
            }
            
            Document d = CreateDocument(content, newId);

            _writer.AddDocument(d);
            return newId;
        }


        public void Remove<T>(object id)
        {
            if (_autoGenerateIds)
                throw new NotSupportedException("Este método de exclusão é específico para a estratégia de Ids decorados por atributos (autoGenerateIds = false)");


        }

        public void Remove<T>(Guid id)
        {
            if (!_autoGenerateIds)
                throw new NotSupportedException("Este método de exclusão é específico para a estratégia de geração automática de Ids (autoGenerateIds = true)");

            Term idTerm = new Term("#id", id.ToString());
            try
            {
                _writer.DeleteDocuments(idTerm);
            }
            catch (OutOfMemoryException)
            {
                _writer = CreateWriter();
            }
        }

        public void Refresh<T>(T content, Guid id)
        {
            var searcher = new ElasticSearcher(_indexerFile);

            T d = searcher.GetById<T>(id);
            List<Term> terms = new List<Term>();

            foreach (var propertyInfo in d.GetType().GetProperties())
            {
                var guid = Guid.Parse(d.GetProperty<string>("#id"));
                _writer.UpdateDocument(new Term(
                    propertyInfo.Name,
                    content.GetProperty<string>(propertyInfo.Name)), CreateDocument(d, guid));
            }
        }


        public void Commit()
        {
            _writer.Optimize();
            _writer.Flush(true, true, true);
            _writer.Dispose();
            _writer = CreateWriter();
        }

        public void Dispose()
        {
            _writer.Optimize();
            _writer.Flush(true, true, true);
            try
            {
                _writer.Dispose();
            }
            catch (OutOfMemoryException)
            {
                _writer = CreateWriter();
            }
        }
    }
}
