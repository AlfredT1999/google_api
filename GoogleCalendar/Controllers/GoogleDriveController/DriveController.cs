using GoogleCalendar.Credentials.Helper.GoogleDrive.services;
using GoogleServices.Credentials.Models.GoogleDrive;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalendar.Controllers.GoogleDriveController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriveController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDriveFiles()
        {
            return Ok(await GoogleDriveGetsHelper.GetAllDriveFiles());
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetFolderContent([FromQuery] string folderId)
        {
            return Ok(await GoogleDriveGetsHelper.GetFolderContent(folderId));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> IsFileInFolder([FromQuery] UploadInFolder data)
        {
            return Ok(await GoogleDriveGetsHelper.IsFileInFolder(data));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetFileById([FromQuery] string fileId)
        {
            return Ok(await GoogleDriveGetsHelper.GetFileById(fileId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateFolderOnDrive([FromQuery] string Folder_Name)
        {
            return Ok(await GoogleDrivePostsHelper.CreateFolderOnDrive(Folder_Name));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> FileUploadInFolder(string folderId, IFormFile file)
        {
            return Ok(await GoogleDrivePostsHelper.FileUploadInFolder(folderId, file));
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DriveMoveFileToFolder(string fileId, string folderId)
        {
            return Ok(await GoogleDriveUpdatesHelper.DriveMoveFileToFolder(fileId, folderId));
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> RemoveFile(string fileId)
        {
            return Ok(await GoogleDriveDeletesHelper.RemoveFile(fileId));
        }
    }
}
