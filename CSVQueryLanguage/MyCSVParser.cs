using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace CSVQueryLanguage;

public class MyCSVParser
{
    public List<List<string>> GetCsvDataList(string file)
    {
        var inputStream = new AntlrInputStream(file);
        var lexer = new CSVLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new CSVParser(commonTokenStream);

        var listener = new MyCSVListener();
        var parseTreeWalker = new ParseTreeWalker();
        parseTreeWalker.Walk(listener,parser.csvFile());

        var data = listener.Result;

        return data;
    }
}