using GoogleCalendar.Credentials.Helper;
using GoogleCalendar.Credentials.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDrive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriveController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDriveFiles()
        {
            return Ok(await GoogleDriveHelper.GetAllDriveFiles());
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetFolderContent([FromQuery] String folderId)
        {
            return Ok(await GoogleDriveHelper.GetFolderContent(folderId));
        }

        //TODO:
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> IsFileInFolder([FromQuery] UploadInFolder data)
        {
            return Ok(await GoogleDriveHelper.IsFileInFolder(data));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateFolderOnDrive([FromQuery] string Folder_Name)
        {
            return Ok(await GoogleDriveHelper.CreateFolderOnDrive(Folder_Name));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> FileUploadInFolder(string folderId, IFormFile file)
        {
            return Ok(await GoogleDriveHelper.FileUploadInFolder(folderId, file));
        }

        //[HttpPut]
        //[Route("[action]")]
        //public async Task<IActionResult> UpdateDrive([FromBody] GoogleCalendarEvent request)
        //{
        //    return Ok(await GoogleCalendarHelper.UpdateGoogleEvent(request));
        //}

        //[HttpDelete]
        //[Route("[action]")]
        //public async Task<IActionResult> DeleteDrive([FromQuery] string eventId)
        //{
        //    return Ok(await GoogleCalendarHelper.DeleteGoogleEvent(eventId));
        //}
    }
}
