using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GoogleCalendar.Credentials.Helper.GoogleCalendar.credentials
{
    public static class GoogleCredentials
    {
        public static async Task<dynamic> GenerateUserCredential(string[] scopes, string creds)
        {
            UserCredential credential;

            try
            {
                using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "credentials", creds), FileMode.Open, FileAccess.Read))
                {
                    string credentialsPath = "token.json";

#pragma warning disable CS0618 // Type or member is obsolete
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credentialsPath, true)
                    );
#pragma warning restore CS0618 // Type or member is obsolete
                }

                // define a service:
                if (creds.ToLower().Equals("credentials_calendar.json"))// For calendar
                {
                    var calendarService = new CalendarService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google calendar api"
                    });

                    return calendarService;
                }
                else if (creds.ToLower().Equals("credentials_drive.json"))// For drive
                {
                    var driveService = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google drive api"
                    });

                    return driveService;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
