using System;
using System.Threading.Tasks;
using BLOBi.Core;
using Microsoft.AspNetCore.Mvc;

namespace BLOBi.WebClient.Tests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IBlobContainerService _blobContainerService;

        public TestController(IBlobContainerService blobContainerService)
        {
            _blobContainerService = blobContainerService;
        }

        [HttpGet]
        public async Task TestAsync()
        {
            string containerName = "container" + Guid.NewGuid();

            Console.WriteLine("Create Container");
            Console.WriteLine(await _blobContainerService.CreateContainerAsync(containerName));

            Console.WriteLine("Get container details");
            Azure.Storage.Blobs.Models.BlobContainerProperties containerDetails = await _blobContainerService.GetContainerProperties(containerName);
            Console.WriteLine(containerDetails);

            Console.WriteLine("List container content");
            Azure.Storage.Blobs.Models.BlobItem[] containerContent = await _blobContainerService.ListContainerContentAsync(containerName);
            Console.WriteLine(containerContent);

            Console.WriteLine("Delete container");
            Console.WriteLine(await _blobContainerService.DeleteContainerAsync(containerName));
        }
    }
}
