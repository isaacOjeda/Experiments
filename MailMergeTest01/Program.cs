using System.Dynamic;
using System.Text.RegularExpressions;
using Humanizer;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.Model;

MailMergeTelerik();
// MailMergeSyncFusion();

static void MailMergeSyncFusion()
{
    //Opens the template Word document.
    Stream docStream = File.OpenRead(Path.GetFullPath(@"LetterTemplate.docx"));
    WordDocument document = new WordDocument(docStream, FormatType.Docx);
    docStream.Dispose();



    //Loads the string arrays with field names and values for mail merge.
    // string[] fieldNames = { "NOMBREPARTEVENDEDORA", "NOMBREPARTECOMPRADORA"};
    // string[] fieldValues = { "Isaac Ojeda Quintana", "Alejandra Eliz Mayorga Madrid"};

    var mergeFields = document.MailMerge.GetMergeFieldNames();

    var fieldNames = new List<string>();
    var fieldValues = new List<string>();
    foreach (var mergeField in mergeFields)
    {
        Console.WriteLine("Ingresa el valor para {0}", mergeField);
        var value = Console.ReadLine();

        fieldNames.Add(mergeField);
        fieldValues.Add(value);
    }

    //Performs the mail merge.
    document.MailMerge.Execute(fieldNames.ToArray(), fieldValues.ToArray());
    //Saves the Word document in DOCX format.
    docStream = File.Create(Path.GetFullPath(@"Result.docx"));
    document.Save(docStream, FormatType.Docx);
    docStream.Dispose();
    //Releases the resources occupied by WordDocument instance.
    document.Dispose();

    Console.WriteLine("Documento generado");
}

static void MailMergeTelerik()
{
    using Stream docStream = File.OpenRead(Path.GetFullPath(@"LetterTemplate.docx"));
    var provider = new DocxFormatProvider();
    var document = provider.Import(docStream);

    var regex = new Regex("MERGEFIELD\\s+(?<Field>[a-zA-Z]+)", RegexOptions.IgnoreCase);
    var fields = document.EnumerateChildrenOfType<Run>()
        .Select(s =>
        {
            var regexResult = regex.Match(s.Text);
            var field = regexResult.Success ? regexResult.Groups["Field"].Value : null;

            return new
            {
                IsMatch = regexResult.Success,
                Field = field,
                Description = field is not null ?
                    field.Humanize(LetterCasing.Title) : null
            };
        })
        .Where(q => q.IsMatch)
        .ToList();


    var source = new ExpandoObject() as IDictionary<string, Object>;

    foreach (var field in fields)
    {
        Console.WriteLine("Ingrese el valor de {0}", field.Description);
        var value = Console.ReadLine();

        source.Add(field.Field, value);
    }

    var result = document.MailMerge(new object[] { source });

    using var output = File.OpenWrite("Result.docx");
    provider.Export(result, output);
}