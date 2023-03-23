using Google.Apis.Drive.v3;

namespace GoogleCalendar.Credentials.Helper
{
    public class GoogleDriveHelper
    {
        private static string[] _scopes =
        {
            "https://www.googleapis.com/auth/drive",
            "https://www.googleapis.com/auth/drive.appdata",
            "https://www.googleapis.com/auth/drive.file",
            "https://www.googleapis.com/auth/drive.metadata"
        };
        private static string _creds = "credentials_drive.json";

        public GoogleDriveHelper()
        {
        }

        //create Drive API service.
        public static async Task<List<DriveService>> GetService()
        {
            var service = await GoogleCredentials.GenerateUserCredential(_scopes, _creds);
            Google.Apis.Drive.v3.FilesResource.ListRequest FileListRequest = service.Files.List();

            return service;
        }
    }
}
