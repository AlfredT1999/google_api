using Google.Apis.Calendar.v3.Data;
using GoogleCalendar.Credentials.Models;

namespace GoogleCalendar.Credentials.Helper
{
    public class GoogleCalendarHelper
    {
        private static readonly string[] _scopes =
        {
            "https://www.googleapis.com/auth/calendar",
            "https://www.googleapis.com/auth/calendar.events",
            "https://www.googleapis.com/auth/calendar.events.owned"
        };
        private static readonly string _creds = "credentials_calendar.json";

        protected GoogleCalendarHelper()
        {
        }

        public static async Task<List<GoogleCalendarEvent>> ListGoogleEvents()
        {
            try
            {
                var service = await GoogleCredentials.GenerateUserCredential(_scopes, _creds);
                Events listOfCalendarInfo = await service.Events.List("primary").ExecuteAsync();
                GoogleCalendarEvent googleEvent = new();
                List<GoogleCalendarEvent> googleEvents = new();

                foreach (var item in listOfCalendarInfo.Items)
                {
                    googleEvent.Id = item.Id;
                    googleEvent.Summary = item.Summary;
                    googleEvent.Description = item.Description;
                    googleEvent.Location = item.Location;
                    googleEvent.Start = item.Start.DateTime;
                    googleEvent.End = item.End.DateTime;

                    googleEvents.Add(googleEvent);
                    googleEvent = new();
                }

                return googleEvents;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<GoogleCalendarEvent> GetGoogleEvent(string eventId)
        {
            try
            {
                var service = await GoogleCredentials.GenerateUserCredential(_scopes, _creds);
                Event googleEvent = await service.Events.Get("primary", eventId).ExecuteAsync();
                GoogleCalendarEvent googleEventResponse = new()
                {
                    Id = googleEvent.Id,
                    Summary = googleEvent.Summary,
                    Description = googleEvent.Description,
                    Location = googleEvent.Location,
                    Start = googleEvent.Start.DateTime,
                    End = googleEvent.End.DateTime
                };

                return googleEventResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<Event> CreateGoogleEvent(GoogleCalendarEvent request)
        {
            try
            {
                var service = await GoogleCredentials.GenerateUserCredential(_scopes, _creds); 
                Event eventCalendar = new()
                {
                    Summary = request.Summary,
                    Location = request.Location,
                    Start = new EventDateTime
                    {
                        DateTime = request.Start,
                        TimeZone = "America/Mexico_City"
                    },
                    End = new EventDateTime
                    {
                        DateTime = request.End,
                        TimeZone = "America/Mexico_City"
                    },
                    Description = request.Description
                };
                var eventRequest = service.Events.Insert(eventCalendar, "primary");
                var requestCreate = await eventRequest.ExecuteAsync();

                return requestCreate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<Event> UpdateGoogleEvent(GoogleCalendarEvent request)
        {
            try
            {
                var service = await GoogleCredentials.GenerateUserCredential(_scopes, _creds);

                // Retrieve the event from the API
                Event googleEvent = await service.Events.Get("primary", request.Id).ExecuteAsync();
                
                googleEvent.Summary = request.Summary;
                googleEvent.Location = request.Location;
                googleEvent.Description = request.Description;
                googleEvent.Start.DateTime = request.Start;
                googleEvent.End.DateTime = request.End;

                // Update the event
                Event updatedEvent = await service.Events.Update(googleEvent, "primary", googleEvent.Id).ExecuteAsync();

                return updatedEvent;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<object> DeleteGoogleEvent(string eventId)
        {
            try
            {
                var service = await GoogleCredentials.GenerateUserCredential(_scopes, _creds);
                var res = await service.Events.Delete("primary", eventId).ExecuteAsync();

                return res == "" ? "Deleted succesfully." : "Error while trying to delete.";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
