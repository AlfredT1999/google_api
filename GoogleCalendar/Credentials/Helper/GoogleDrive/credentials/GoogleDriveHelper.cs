using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GoogleCalendar.Credentials.Helper.GoogleDrive.credentials
{
    public class GoogleDriveHelper
    {
        private static readonly string[] _scopes =
        {
            "https://www.googleapis.com/auth/drive",
            "https://www.googleapis.com/auth/drive.appdata",
            "https://www.googleapis.com/auth/drive.file",
            "https://www.googleapis.com/auth/drive.metadata"
        };


        // ************************* service section *******************************
        // Create a service to use google drive (v3):
        public async static Task<Google.Apis.Drive.v3.DriveService> GetServiceV3()
        {
            UserCredential credential;
            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "credentials", "credentials_drive.json"), FileMode.Open, FileAccess.Read))
            {
                string credentialsPath = "token_drive.json";

#pragma warning disable CS0618 // Type or member is obsolete
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentialsPath, true)
                );
#pragma warning restore CS0618 // Type or member is obsolete
            }

            Google.Apis.Drive.v3.DriveService service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "google_drive_v3"
            });

            return service;
        }

        // Create a service to use google drive (v2):
        public async static Task<DriveService> GetServiceV2()
        {
            UserCredential credential;
            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "credentials", "credentials_drive.json"), FileMode.Open, FileAccess.Read))
            {
                string credentialsPath = "token_drive.json";

#pragma warning disable CS0618 // Type or member is obsolete
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentialsPath, true)
                );
#pragma warning restore CS0618 // Type or member is obsolete
            }

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "google_drive_v2",
            });

            return service;
        }
    }
}

