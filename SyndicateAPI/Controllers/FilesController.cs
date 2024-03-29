﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : Controller
    {
        private IFileService FileService { get; set; }
        private IUserService UserService { get; set; }

        public FilesController([FromServices]
            IFileService fileService,
            IUserService userService)
        {
            FileService = fileService;
            UserService = userService;
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

        [HttpPost("list")]
        public async Task<IActionResult> UploadFileList([FromBody] UploadFileListRequest request)
        {
            var now = DateTime.UtcNow;
            string dir = System.IO.Path.Combine("/var", "www", "html", "files", now.Month.ToString(), now.Day.ToString());
            string filename = string.Empty;
            string path = string.Empty;

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            var toCreate = new List<File>();
            var response = new List<long>();

            foreach (var f in request.FileList)
            {
                var now2 = DateTime.UtcNow;
                filename = $"{(now2.Hour.ToString() + now2.Minute.ToString() + now2.Second.ToString() + now2.Millisecond.ToString()).Replace(" ", "").Replace(".", "").Replace(":", "")}";
                if (f.Type.Equals(FileType.JPEG))
                    filename += ".jpg";
                else if (f.Type.Equals(FileType.PNG))
                    filename += ".png";
                else if (f.Type.Equals(FileType.AVI))
                    filename += ".avi";
                else if (f.Type.Equals(FileType.MP4))
                    filename += ".mp4";
                else if (f.Type.Equals(FileType.MOV))
                    filename += ".mov";
                else
                    return Json(new ResponseModel
                    {
                        Success = false,
                        Message = "Тип одного или нескольких файлов не поддерживается"
                    });

                path = System.IO.Path.Combine(dir, filename);
                System.IO.Stream stream = System.IO.File.Create(path);

                foreach (var byte1 in f.Base64)
                {
                    stream.WriteByte(byte1);
                }

                stream.Close();

                var file = new File
                {
                    Name = filename,
                    Type = f.Type,
                    LocalPath = path,
                    Url = $"http://185.159.82.174/files/{now.Month.ToString()}/{now.Day.ToString()}/{filename}"
                };

                toCreate.Add(file);
            }

            foreach (var toCreateFile in toCreate)
            {
                FileService.Create(toCreateFile);
                response.Add(toCreateFile.ID);
            }

            return Ok(new UploadFileListResponse
            {
                FileIDList = response
            });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest request)
        {
            var now = DateTime.Now;
            string dir = System.IO.Path.Combine("/var", "www", "html", "files", now.Month.ToString(), now.Day.ToString());
            string filename = string.Empty;
            string path = string.Empty;

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            filename = $"{(now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString()).Replace(" ", "").Replace(".", "").Replace(":", "")}";
            if (request.Type.Equals(FileType.JPEG))
                filename += ".jpg";
            else if (request.Type.Equals(FileType.PNG))
                filename += ".png";
            else if (request.Type.Equals(FileType.AVI))
                filename += ".avi";
            else if (request.Type.Equals(FileType.MP4))
                filename += ".mp4";
            else if (request.Type.Equals(FileType.MOV))
                filename += ".mov";
            else
                return Json(new ResponseModel
                {
                    Success = false,
                    Message = "Тип файла не поддерживается"
                });

            path = System.IO.Path.Combine(dir, filename);
            System.IO.Stream stream = System.IO.File.Create(path);

            foreach (var byte1 in request.Base64)
            {
                stream.WriteByte(byte1);
            }

            stream.Close();

            var file = new File
            {
                Name = filename,
                Type = request.Type,
                LocalPath = path,
                Url = $"http://185.159.82.174/files/{now.Month.ToString()}/{now.Day.ToString()}/{filename}"
            };

            FileService.Create(file);

            return Ok(new UploadFileResponse
            {
                FileID = file.ID
            });
        }

        [HttpPost("form-data")]
        public async Task<IActionResult> UploadFileFormData([FromForm] UploadFileFormDataRequest request)
        {
            var file = await SaveFile(request.File);
            if (file == null)
                return Forbid();

            return Ok(new UploadFileResponse
            {
                FileID = file.ID
            });
        }

        [HttpPost("form-data/list")]
        public async Task<IActionResult> UploadFileListFormData([FromBody] UploadFileListFormDataRequest request)
        {
            var result = new List<long>();

            foreach (var formFile in request.Files)
            {
                var file = await SaveFile(formFile);
                if (file != null)
                    result.Add(file.ID);
            }

            return Ok(result);
        }

        private async Task<File> SaveFile(IFormFile formFile)
        {
            var now = DateTime.Now;
            string dir = System.IO.Path.Combine("/var", "www", "html", "files", now.Month.ToString());
            string filename = string.Empty;
            string path = string.Empty;

            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            filename = $"{(now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString()).Replace(" ", "").Replace(".", "").Replace(":", "")}.{GetFileType(GetFileType(formFile.ContentType))}";
            path = System.IO.Path.Combine(dir, filename);

            if (formFile.Length <= 0)
                return null;

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            var file = new File
            {
                Name = filename,
                Type = GetFileType(formFile.ContentType),
                LocalPath = path,
                Url = $"http://185.159.82.174/files/{now.Month.ToString()}/{filename}"
            };

            FileService.Create(file);

            return file;
        }

        private FileType GetFileType(string strType)
        {
            switch (strType)
            {
                case "image/jpeg":
                    return FileType.JPEG;
                case "image/png":
                    return FileType.PNG;
                case "image/gif":
                    return FileType.GIF;
                case "video/mp4":
                    return FileType.MP4;
                case "video/quicktime":
                    return FileType.MOV;
                default:
                    return FileType.TXT;
            }
        }

        private string GetFileType(FileType type)
        {
            switch (type)
            {
                case FileType.JPEG:
                    return "jpg";
                case FileType.PNG:
                    return "png";
                case FileType.GIF:
                    return "gif";
                case FileType.MP4:
                    return "mp4";
                case FileType.MOV:
                    return "mov";
                default:
                    return "txt";
            }
        }
    }
}