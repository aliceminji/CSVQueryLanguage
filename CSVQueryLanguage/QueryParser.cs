using System.Text.RegularExpressions;
using Antlr4.Runtime.Tree.Pattern;
using CSVQueryLanguage.Model;

namespace CSVQueryLanguage;

public class QueryParser
{
    public Query Parse(string queryString)
    {
        var words = SplitQuery(queryString);
        var dml = QuerySeparation(words.FirstOrDefault());
        var tableName = GetTableName(words.LastOrDefault());

        var query = new Query
        {
            DmlQuery = dml,
            Words = words,
            QueryString = queryString,
            TableName = tableName
        };

        CheckValidation(query);

        if (query.DmlQuery == Query.DMLQuery.SELECT && !string.IsNullOrEmpty(query.TableName))
        {
            SelectQueryParse(query);
        }

        return query;
    }

    void CheckValidation(Query query)
    {
        if (string.IsNullOrEmpty(query.TableName))
        {
            query.DmlQuery = Query.DMLQuery.ERROR;
        }

        if (query.Words.Length <= 0)
        {
            query.DmlQuery = Query.DMLQuery.ERROR;
        }
    }

    private string GetTableName(string? lastWord)
    {
        if (string.IsNullOrEmpty(lastWord)) return string.Empty;
        return Regex.Match(lastWord, "(.csv)|(.CSV)").Success ? lastWord : string.Empty;
    }

    private string[] SplitQuery(string query)
    {
        char[] delimiterChars = { ' ', '\t' };
        return query.Split(delimiterChars);
    }

    private Query.DMLQuery QuerySeparation(string? firstWord)
    {
        if (string.IsNullOrEmpty(firstWord)) return Query.DMLQuery.ERROR;

        if (Regex.Match(firstWord, "(SELECT)|(select)").Success)
        {
            return Query.DMLQuery.SELECT;
        }

        if (Regex.Match(firstWord, "(INSERT)|(insert)").Success)
        {
            return Query.DMLQuery.INSERT;
        }

        return Query.DMLQuery.ERROR;
    }

    private void InsertQueryParse(string[] query)
    {
        throw new NotImplementedException();
    }

    private void SelectQueryParse(Query query)
    {
        //전체 컬럼을 조회하는 select문
        var pattern = @"[*]+(from)|[*].(from)";
        if (Regex.Match(query.QueryString, pattern).Success)
        {
            query.IsSelectAllColumns = true;
        }
        //컬럼을 명시한 select문
        else
        {
            query.IsSelectAllColumns = false;
            GetColumns(query);
        }

        //where절이 있는 select 문일 때
        if (Regex.Match(query.QueryString, "(WHERE)|(where)").Success)
        {
            query.IsWhereClause = true;
        }
    }

    private void GetColumns(Query query)
    {
        foreach (var word in query.Words)
        {
            if (!Regex.Match(word, "(FROM)|(from)").Success) continue;
            var index = Array.IndexOf(query.Words, word);

            //select 와 from 사이에 있는 것 (컬럼)구하기
            var colList = new List<string>();
            var range = query.Words.ToList().GetRange(1, index - 1);
            char[] delimiterChars = { ' ', '\t', ',' };
            foreach (var strings in range.Select(col => col.Split(delimiterChars)))
            {
                colList.AddRange(strings.Where(str => !string.IsNullOrEmpty(str)));
            }

            query.Columns = colList.ToArray();
        }

        if (query.Columns.Length <= 0) query.DmlQuery = Query.DMLQuery.ERROR;
    }
}