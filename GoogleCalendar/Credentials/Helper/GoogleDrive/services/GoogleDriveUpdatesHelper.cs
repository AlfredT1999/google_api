using GoogleCalendar.Credentials.Helper.GoogleDrive.credentials;

namespace GoogleCalendar.Credentials.Helper.GoogleDrive.services
{
    public class GoogleDriveUpdatesHelper : GoogleDriveHelper
    {
        public async static Task<IList<string>> DriveMoveFileToFolder(string fileId, string folderId)
        {
            try
            {
                Google.Apis.Drive.v3.DriveService service = await GetServiceV3();

                // Retrieve the existing parents to remove
                var getRequest = service.Files.Get(fileId);
                getRequest.Fields = "parents";
                var file = getRequest.Execute();
                var previousParents = String.Join(",", file.Parents);

                // Move the file to the new folder
                var updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId);

                updateRequest.Fields = "id, parents";
                updateRequest.AddParents = folderId;
                updateRequest.RemoveParents = previousParents;
                file = updateRequest.Execute();

                return file.Parents;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
