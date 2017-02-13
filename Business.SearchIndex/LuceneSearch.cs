using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.WebPages;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace Business.SearchIndex
{
    internal static class LuceneSearch
    {
        private static readonly string LuceneDir = HostingEnvironment.MapPath("~/LuceneIndex");
        private static FSDirectory directory;
        private static FSDirectory Directory
        {
            get
            {
                if (directory == null) directory = FSDirectory.Open(new DirectoryInfo(LuceneDir));
                if (IndexWriter.IsLocked(directory)) IndexWriter.Unlock(directory);
                try
                {
                    directory.ClearLock("write.lock");
                }
                catch
                {
                    // ignored
                }
                return directory;
            }
        }

        public static void AddUpdateLuceneIndex(IEnumerable<ViewModels.SearchIndex> searchIndexes)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var searchIndex in searchIndexes) AddToLuceneIndex(searchIndex, writer);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(ViewModels.SearchIndex searchIndex)
        {
            AddUpdateLuceneIndex(new List<ViewModels.SearchIndex> { searchIndex });
        }

        public static void ClearLuceneIndexRecord(Guid recordId)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("Id", recordId.ToString()));
                writer.DeleteDocuments(searchQuery);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();

                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        public static IEnumerable<ViewModels.SearchIndex> Search(Sort sort, string[] searchFields, string input = "", int page = 0, int hitsLimit = 10)
        {
            var terms = input.Trim().Replace("-", " ").Replace(",", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim().TrimEnd('s','\'') + "*");
            input = string.Join(" ", terms);

            return _search(sort, input, searchFields, page, hitsLimit);
        }

        public static void UpdateIndex(ViewModels.SearchIndex searchIndex)
        {
            var writer = new IndexWriter(Directory,
                new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30),
                false,
                IndexWriter.MaxFieldLength.LIMITED);
            var document = GetDocument(searchIndex);
            writer.UpdateDocument(new Term("Id", document.Get("Id")), document);
            writer.Dispose();
        }

        public static IEnumerable<ViewModels.SearchIndex> GetIndexRecords(int page = 0, int hitsLimit = 1000)
        {
            if (!System.IO.Directory.EnumerateFiles(LuceneDir).Any()) return new List<ViewModels.SearchIndex>();

            var searcher = new IndexSearcher(Directory, true);
            var reader = IndexReader.Open(Directory, true);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            if (page > 0) term.SkipTo((page * hitsLimit) - 1);
            for (int i = 0; i < hitsLimit; i++)
            {
                if (!term.Next()) break;
                docs.Add(searcher.Doc(term.Doc));
            }
            reader.Dispose();
            searcher.Dispose();
            return MapLuceneToDataList(docs);
        }

        private static IEnumerable<ViewModels.SearchIndex> _search(Sort sort, string searchQuery, string[] searchFields, int page, int hitsLimit)
        {
            if (IndexReader.IndexExists(Directory))
                using (var searcher = new IndexSearcher(Directory, true))
                {
                    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                    IList<Document> luceneDocuments = new List<Document>();
                    int hitsUpperRange = (page + 1) * hitsLimit;
                    var parser = searchFields.Length == 1 
                        ? new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchFields.Single(), analyzer) 
                        : new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, searchFields, analyzer);
                    Query query = searchQuery.IsEmpty() ? new MatchAllDocsQuery() : ParseQuery(searchQuery, parser);
                    TopDocs results = searcher.Search(query, null, hitsUpperRange, sort);
                    ScoreDoc[] scoreDocs = results.ScoreDocs;
                    for (int i = page * hitsLimit; i < results.TotalHits && i < hitsUpperRange; i++)
                    {
                        luceneDocuments.Add(searcher.Doc(scoreDocs[i].Doc));
                    }
                    analyzer.Close();
                    searcher.Dispose();
                    return MapLuceneToDataList(luceneDocuments);
                }
            return null;
        }

        private static void AddToLuceneIndex(ViewModels.SearchIndex searchIndex, IndexWriter writer)
        {
            var document = GetDocument(searchIndex);
            writer.UpdateDocument(new Term("Id", document.Get("Id")), document);
        }

        private static Document GetDocument(ViewModels.SearchIndex searchIndex)
        {
            var doc = new Document();

            doc.Add(new Field("Id", searchIndex.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("PreviewText", searchIndex.PreviewText, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Condition", searchIndex.Condition, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Tags", searchIndex.Tags, Field.Store.YES, Field.Index.ANALYZED));

            return doc;
        }

        private static ViewModels.SearchIndex MapLuceneDocumentToData(Document doc)
        {
            return new ViewModels.SearchIndex
            {
                Id = Guid.Parse(doc.Get("Id")),
                PreviewText = doc.Get("PreviewText"),
                Condition = doc.Get("Condition"),
                Tags = doc.Get("Tags")
            };
        }

        private static IEnumerable<ViewModels.SearchIndex> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }
    }
}
