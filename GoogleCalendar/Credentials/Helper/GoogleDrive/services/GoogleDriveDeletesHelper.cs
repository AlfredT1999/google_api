using Google.Apis.Drive.v2;
using GoogleCalendar.Credentials.Helper.GoogleDrive.credentials;

namespace GoogleCalendar.Credentials.Helper.GoogleDrive.services
{
    public class GoogleDriveDeletesHelper : GoogleDriveHelper
    {
        public async static Task<string> RemoveFile(string fileId)
        {
            try
            {
                DriveService service = await GetServiceV2();
                service.Files.Delete(fileId).Execute();

                return "Folder deleted successfully";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
