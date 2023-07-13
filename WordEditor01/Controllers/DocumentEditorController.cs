using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.DocumentEditor;
using WDocument = Syncfusion.DocIO.DLS.WordDocument;
using WFormatType = Syncfusion.DocIO.FormatType;


namespace WordEditor01.Controllers;

[Route("api/[controller]")]
public class DocumentEditorController : Controller
{

    [HttpPost]
    [Route("Import")]
    public string Import(IFormCollection data)
    {
        if (data.Files.Count == 0)
            return null;
        Stream stream = new MemoryStream();
        IFormFile file = data.Files[0];
        int index = file.FileName.LastIndexOf('.');
        string type = index > -1 && index < file.FileName.Length - 1 ?
            file.FileName.Substring(index) : ".docx";
        file.CopyTo(stream);
        stream.Position = 0;

        //Hooks MetafileImageParsed event.
        WordDocument.MetafileImageParsed += OnMetafileImageParsed;
        WordDocument document = WordDocument.Load(stream, GetFormatType(type.ToLower()));
        //Unhooks MetafileImageParsed event.
        WordDocument.MetafileImageParsed -= OnMetafileImageParsed;

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(document);
        document.Dispose();
        return json;
    }

    //Converts Metafile to raster image.
    private static void OnMetafileImageParsed(object sender, MetafileImageParsedEventArgs args)
    {
        //You can write your own method definition for converting metafile to raster image using any third-party image converter.
        args.ImageStream = ConvertMetafileToRasterImage(args.MetafileStream);
    }

    private static Stream ConvertMetafileToRasterImage(Stream ImageStream)
    {
        //Here we are loading a default raster image as fallback.
        Stream imgStream = GetManifestResourceStream("ImageNotFound.jpg");
        return imgStream;
        //To do : Write your own logic for converting metafile to raster image using any third-party image converter(Syncfusion doesn't provide any image converter).
    }

    private static Stream GetManifestResourceStream(string fileName)
    {
        System.Reflection.Assembly execAssembly = typeof(WDocument).Assembly;
        string[] resourceNames = execAssembly.GetManifestResourceNames();
        foreach (string resourceName in resourceNames)
        {
            if (resourceName.EndsWith("." + fileName))
            {
                fileName = resourceName;
                break;
            }
        }
        return execAssembly.GetManifestResourceStream(fileName);
    }

    // GET api/values
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    public class CustomParameter
    {
        public string content { get; set; }
        public string type { get; set; }
    }

    [HttpPost]
    [Route("SystemClipboard")]
    public string SystemClipboard([FromBody] CustomParameter param)
    {
        if (param.content != null && param.content != "")
        {
            try
            {
                //Hooks MetafileImageParsed event.
                WordDocument.MetafileImageParsed += OnMetafileImageParsed;
                WordDocument document = WordDocument.LoadString(param.content, GetFormatType(param.type.ToLower()));
                //Unhooks MetafileImageParsed event.
                WordDocument.MetafileImageParsed -= OnMetafileImageParsed;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(document);
                document.Dispose();
                return json;
            }
            catch (Exception)
            {
                return "";
            }
        }
        return "";
    }

    public class CustomRestrictParameter
    {
        public string passwordBase64 { get; set; }
        public string saltBase64 { get; set; }
        public int spinCount { get; set; }
    }
    public class UploadDocument
    {
        public string DocumentName { get; set; }
    }

    [HttpPost]
    [Route("RestrictEditing")]
    public string[] RestrictEditing([FromBody] CustomRestrictParameter param)
    {
        if (param.passwordBase64 == "" && param.passwordBase64 == null)
            return null;
        return WordDocument.ComputeHash(param.passwordBase64, param.saltBase64, param.spinCount);
    }


    [HttpPost]
    [Route("LoadDefault")]
    public string LoadDefault()
    {
        Stream stream = System.IO.File.OpenRead("App_Data/GettingStarted.docx");
        stream.Position = 0;

        WordDocument document = WordDocument.Load(stream, FormatType.Docx);
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(document);
        document.Dispose();
        return json;
    }
    // [HttpPost]
    // [EnableCors("AllowAllOrigins")]
    // [Route("LoadDocument")]
    // public string LoadDocument([FromForm] UploadDocument uploadDocument)
    // {
    //     string documentPath= Path.Combine(path, uploadDocument.DocumentName);
    //     Stream stream = null;
    //     if (System.IO.File.Exists(documentPath))
    //     {
    //         byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
    //         stream = new MemoryStream(bytes);
    //     }
    //     else
    //     {
    //         bool result = Uri.TryCreate(uploadDocument.DocumentName, UriKind.Absolute, out Uri uriResult)
    //             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    //         if (result)
    //         {
    //             stream = GetDocumentFromURL(uploadDocument.DocumentName).Result;
    //             if (stream != null)
    //                 stream.Position = 0;
    //         }
    //     }
    //     WordDocument document = WordDocument.Load(stream, FormatType.Docx);
    //     string json = Newtonsoft.Json.JsonConvert.SerializeObject(document);
    //     document.Dispose();
    //     return json;
    // }
    async Task<MemoryStream> GetDocumentFromURL(string url)
    {
        var client = new HttpClient(); ;
        var response = await client.GetAsync(url);
        var rawStream = await response.Content.ReadAsStreamAsync();
        if (response.IsSuccessStatusCode)
        {
            MemoryStream docStream = new MemoryStream();
            rawStream.CopyTo(docStream);
            return docStream;
        }
        else { return null; }
    }
    internal static FormatType GetFormatType(string format)
    {
        if (string.IsNullOrEmpty(format))
            throw new NotSupportedException("EJ2 DocumentEditor does not support this file format.");
        switch (format.ToLower())
        {
            case ".dotx":
            case ".docx":
            case ".docm":
            case ".dotm":
                return FormatType.Docx;
            case ".dot":
            case ".doc":
                return FormatType.Doc;
            case ".rtf":
                return FormatType.Rtf;
            case ".txt":
                return FormatType.Txt;
            case ".xml":
                return FormatType.WordML;
            case ".html":
                return FormatType.Html;
            default:
                throw new NotSupportedException("EJ2 DocumentEditor does not support this file format.");
        }
    }
    internal static WFormatType GetWFormatType(string format)
    {
        if (string.IsNullOrEmpty(format))
            throw new NotSupportedException("EJ2 DocumentEditor does not support this file format.");
        switch (format.ToLower())
        {
            case ".dotx":
                return WFormatType.Dotx;
            case ".docx":
                return WFormatType.Docx;
            case ".docm":
                return WFormatType.Docm;
            case ".dotm":
                return WFormatType.Dotm;
            case ".dot":
                return WFormatType.Dot;
            case ".doc":
                return WFormatType.Doc;
            case ".rtf":
                return WFormatType.Rtf;
            case ".txt":
                return WFormatType.Txt;
            case ".xml":
                return WFormatType.WordML;
            case ".odt":
                return WFormatType.Odt;
            default:
                throw new NotSupportedException("EJ2 DocumentEditor does not support this file format.");
        }
    }

    [HttpPost]
    [Route("Export")]
    public FileStreamResult Export(IFormCollection data)
    {
        if (data.Files.Count == 0)
            return null;
        string fileName = this.GetValue(data, "filename");
        string name = fileName;
        int index = name.LastIndexOf('.');
        string format = index > -1 && index < name.Length - 1 ?
            name.Substring(index) : ".doc";
        if (string.IsNullOrEmpty(name))
        {
            name = "Document1";
        }
        Stream stream = new MemoryStream();
        string contentType = "";
        WDocument document = this.GetDocument(data);
        if (format == ".pdf")
        {
            contentType = "application/pdf";
        }
        else
        {
            WFormatType type = GetWFormatType(format);
            switch (type)
            {
                case WFormatType.Rtf:
                    contentType = "application/rtf";
                    break;
                case WFormatType.WordML:
                    contentType = "application/xml";
                    break;
                case WFormatType.Dotx:
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                    break;
                case WFormatType.Doc:
                    contentType = "application/msword";
                    break;
                case WFormatType.Dot:
                    contentType = "application/msword";
                    break;
            }
            document.Save(stream, type);
        }
        document.Close();
        stream.Position = 0;
        return new FileStreamResult(stream, contentType)
        {
            FileDownloadName = fileName
        };
    }
    private string GetValue(IFormCollection data, string key)
    {
        if (data.ContainsKey(key))
        {
            string[] values = data[key];
            if (values.Length > 0)
            {
                return values[0];
            }
        }
        return "";
    }
    private WDocument GetDocument(IFormCollection data)
    {
        Stream stream = new MemoryStream();
        IFormFile file = data.Files[0];
        file.CopyTo(stream);
        stream.Position = 0;

        WDocument document = new WDocument(stream, WFormatType.Docx);
        stream.Dispose();
        return document;
    }
}