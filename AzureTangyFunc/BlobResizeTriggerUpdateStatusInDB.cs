using System;
using System.IO;
using System.Linq;
using AzureTangyFunc.Data;
using AzureTangyFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureTangyFunc
{
    public class BlobResizeTriggerUpdateStatusInDB
    {
        private readonly AzureTangyDbContext _db;

        public BlobResizeTriggerUpdateStatusInDB(AzureTangyDbContext db)
        {
           _db = db;
        }
        [FunctionName("BlobResizeTriggerUpdateStatusInDB")]
        public void Run([BlobTrigger("functionsalesreq-sm/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            var filename= Path.GetFileNameWithoutExtension(name);
            SalesRequest salesRequest = _db.SalesRequests.FirstOrDefault(x => x.Id == filename);
            if (salesRequest != null)
            {
                salesRequest.Status = "Image Processed";
                _db.SalesRequests.Update(salesRequest);
                _db.SaveChanges();
            }
        }
    }
}
