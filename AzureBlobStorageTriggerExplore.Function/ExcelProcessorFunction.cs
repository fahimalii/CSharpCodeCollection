using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ExcelMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureBlobStorageTriggerExplore.Function
{
    // By Default Configuration is read from file local.settings.json
    public class ExcelProcessorFunction
    {
        [FunctionName("ProcessExcel")]
        public void Run([BlobTrigger("dev/sample/{name}.xlsx", Connection = "AzureBlobConnection")]Stream myBlob, string name, ILogger logger)
        {
            // TODO: Do some work with the stream
            try
            {
                /* 
                 * Source: https://github.com/ExcelDataReader/ExcelDataReader
                By default, ExcelDataReader throws a NotSupportedException "No data is available for encoding 1252." on .NET Core and .NET 5.0 or later.
                To fix, add a dependency to the package System.Text.Encoding.CodePages and then add code to register the code page provider during application initialization (f.ex in Startup.cs):
                This is required to parse strings in binary BIFF2-5 Excel documents encoded with DOS-era code pages. These encodings are registered by default in the full .NET Framework, but not on .NET Core and .NET 5.0 or later.
                 */

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using var importer = new ExcelImporter(myBlob);
                ExcelSheet sheet = importer.ReadSheet();
                IReadOnlyCollection<ExcelDataModel> data = sheet.ReadRows<ExcelDataModel>().ToList();

                foreach (ExcelDataModel dataItem in data)
                {
                    var json = JsonSerializer.Serialize(dataItem, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    Console.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            logger.LogInformation("Blob trigger function Processed blob\n Name:{BlobName} \n Size: {BlobSize} Bytes", name, myBlob.Length);
        }
    }
}
