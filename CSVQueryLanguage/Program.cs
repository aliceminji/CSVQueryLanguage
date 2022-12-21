using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CSVQueryLanguage;
using CSVQueryLanguage.Model;


while (true)
{
    QueryEditor();
    if (Console.ReadKey().Key == ConsoleKey.Escape) break;
}

void QueryEditor()
{
    Console.WriteLine("⭐ Please enter your query ⭐ ");
    Console.WriteLine("If you want to exit, press ESC key");
    Console.WriteLine("===========================================");

    var query = Console.ReadLine();
    var path = "/Users/minjipark/workspace/CSVQueryLanguage/CSVQueryLanguage/";

    var queryParser = new QueryParser();

    if (query == null) return;
    var result = queryParser.Parse(query);

    if (result.DmlQuery != Query.DMLQuery.ERROR)
    {
        var isExists = File.Exists(path + result.TableName);

        if (isExists)
        {
            var file = File.ReadAllText(path + result.TableName);
            var myCSVparser = new MyCSVParser();
            var dataList = myCSVparser.GetCsvDataList(file);
            
            PrintData(result,dataList);
        }
        else PrintError("file does not exist");
    }
    else PrintError("Invalided Query");
}


void PrintData(Query query,List<List<string>> list)
{
    if (query.IsSelectAllColumns)
    {
        foreach (var data in list)
        {
            foreach (var row in list)
            {
                Console.Write(row + " ");
            }

            Console.WriteLine();
        }
    }
    else
    {
        //TODO: 명시한 컬럼에 대한 값만 출력
    }
    
}

void PrintError(string errorMsg)
{
    Console.WriteLine("⛔️ Error: " + errorMsg);
    Console.WriteLine("");
}