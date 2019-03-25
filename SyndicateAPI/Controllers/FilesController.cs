using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FilesController : Controller
    {
        private IFileService FileService { get; set; }

        public FilesController([FromServices] IFileService fileService)
        {
            FileService = fileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(long id)
        {
            var file = FileService.Get(id);
            if (file == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "File not found"
                });

            return Ok(new DataResponse<FileViewModel>
            {
                Data = new FileViewModel(file)
            });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest request)
        {
            return Ok(new ResponseModel());
        }
    }
}