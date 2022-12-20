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

        if (query.DmlQuery == Query.DMLQuery.SELECT && !string.IsNullOrEmpty(query.TableName))
        {
            SelectQueryParse(query);
        }

        return query;
    }

    private string GetTableName(string? lastWord)
    {
        var pattern = @"(.csv)";
        if (!string.IsNullOrEmpty(lastWord))
        {
            if (Regex.Match(lastWord, pattern).Success)
            {
                return lastWord;
            }

            return string.Empty;
        }
        return string.Empty;
    }

    private string[] SplitQuery(string query)
    {
        char[] delimiterChars = { ' ','\t' };
        return query.Split(delimiterChars);
    }

    private Query.DMLQuery QuerySeparation(string? firstWord)
    {
        Query.DMLQuery dmlQuery;
        
        if (string.IsNullOrEmpty(firstWord)) return Query.DMLQuery.ERROR;
        switch (firstWord)
        {
            case "select" :
                dmlQuery = Query.DMLQuery.SELECT;
                break;
            case "insert":
                dmlQuery = Query.DMLQuery.INSERT;
                break;
            default:
                dmlQuery = Query.DMLQuery.ERROR;
                break;
        }

        return dmlQuery;
    }

    private void InsertQueryParse(string[] query)
    {
        throw new NotImplementedException();
    }

    private void SelectQueryParse(Query query)
    {
        //전체 컬럼을 조회하는 select 문일 때
        var pattern = @"[*]+(from)|[*].(from)";
        if (Regex.Match(query.QueryString, pattern).Success)
        {
            query.IsSelectAllColumns = true;
        }
        
        //where절이 있는 select 문일 때
        pattern = @"(where)";
        if (Regex.Match(query.QueryString, pattern).Success)
        {
            query.IsWhereClause = true;
        }
    }
    
}