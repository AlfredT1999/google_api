using GoogleCalendar.Credentials.Helper.GoogleCalendar.services;
using GoogleCalendar.Credentials.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalendar.Controllers.GoogleCalendarController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ListEvents()
        {
            return Ok(await GoogleCalendarHelper.ListGoogleEvents());
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetEvent([FromQuery] string eventId)
        {
            return Ok(await GoogleCalendarHelper.GetGoogleEvent(eventId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateEvent([FromBody] GoogleCalendarEvent request)
        {
            return Ok(await GoogleCalendarHelper.CreateGoogleEvent(request));
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateEvent([FromBody] GoogleCalendarEvent request)
        {
            return Ok(await GoogleCalendarHelper.UpdateGoogleEvent(request));
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteEvent([FromQuery] string eventId)
        {
            return Ok(await GoogleCalendarHelper.DeleteGoogleEvent(eventId));
        }
    }
}
