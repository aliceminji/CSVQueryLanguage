using Antlr4.Runtime.Misc;

namespace CSVQueryLanguage;

public class MyCSVListener : CSVBaseListener
{
    private List<string> _header;
    private List<string> _currentRow;

    public List<List<string>> Result { get; }

    public MyCSVListener()
    {
        Result = new List<List<string>>();
    }

    public override void EnterRow([NotNull] CSVParser.RowContext context)
    {
        _currentRow = new List<string>();
    }

    public override void ExitRow([NotNull] CSVParser.RowContext context)
    {
        // if (context.Parent.RuleIndex == CSVParser.RULE_hdr)
        // {
        //     
        // }

        var m = new List<string>();
        foreach (var row in _currentRow)
        {
            m.Add(row);
        }

        Result.Add(m);
    }

    public override void ExitHdr([NotNull] CSVParser.HdrContext context)
    {
        _header = new List<string>();
        _header.AddRange(_currentRow);
    }

    public override void ExitField([NotNull] CSVParser.FieldContext context)
    {
        _currentRow.Add(context.TEXT().GetText());
    }
}