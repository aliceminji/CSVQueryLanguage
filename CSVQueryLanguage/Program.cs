using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CSVQueryLanguage;

Console.WriteLine("Please enter your query ...");
var query = Console.ReadLine();

var queryParser = new QueryParser();
if (query != null) queryParser.Parse(query);

var path ="/Users/minjipark/workspace/CSVQueryLanguage/CSVQueryLanguage/";
var file = File.ReadAllText(path +"test.csv");

var inputStream = new AntlrInputStream(file);
var lexer = new CSVLexer(inputStream);
var commonTokenStream = new CommonTokenStream(lexer);
var parser = new CSVParser(commonTokenStream);

var listener = new MyCSVListener();
var parseTreeWalker = new ParseTreeWalker();
parseTreeWalker.Walk(listener,parser.csvFile());

var data = listener.Result;

foreach (var list in data)
{
    foreach (var row in list)
    {
        Console.Write(row + " ");
    }
    Console.WriteLine();
}

Console.WriteLine("Press any key to exit");
Console.ReadLine();
