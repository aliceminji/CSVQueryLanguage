using Antlr4.Runtime.Misc;

namespace CSVQueryLanguage;

public class MyCSVListener : CSVBaseListener
{
    private List<string> _header;
    private List<string> _currentRow;
    private List<List<string>> result;

    public List<List<string>> Result { get => result; set => result = value; }

    public MyCSVListener()
    {        Result=new List<List<string>>();
    }

    public override void EnterRow([NotNull] CSVParser.RowContext context)
    {
        _currentRow = new List<string>();
    }
    public override void ExitRow([NotNull] CSVParser.RowContext context)
    {
        if (context.Parent.RuleIndex == CSVParser.RULE_hdr)
        {
            return;
        }
       
        var m = new List<string>();
        int i = 0;
        foreach (var row in _currentRow)
        {
            m.Add(row);
            i++;
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