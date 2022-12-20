namespace CSVQueryLanguage.Model;

public class Query
{
    public enum DMLQuery
    {
        SELECT,
        INSERT,
        ERROR
    }

    public DMLQuery DmlQuery { get; set; }
    public string QueryString { get; set; }
    public string[] Words { get; set; }
    public string TableName { get; set; }
    public bool IsWhereClause { get; set; }
    public bool IsSelectAllColumns { get; set; }
}