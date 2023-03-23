using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalendar.Credentials.Helper
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

                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credentialsPath, true)
                    );
                }

                // define a service:
                var service = EmptyResult;

                if (creds.ToLower().Equals("credentials_calendar.json"))// For calendar
                {
                    service = new CalendarService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google calendar api"
                    });
                }
                else if(creds.ToLower().Equals("credentials_drive.json"))// For drive
                {
                    service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google drive api"
                    });
                }

                return service;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
