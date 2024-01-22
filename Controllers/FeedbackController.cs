using hc_backend.Data;
using hc_backend.DTO;
using hc_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hc_backend.Controllers
{
    [ApiController]
    [Route("/api")]
    public class FeedbackController : Controller
    {
        private readonly AppDbcontext _db;
        public FeedbackController(AppDbcontext context)
        {
            _db = context;
        }

        [HttpPost("feedback")]
        public async Task<ActionResult> CreateFeedback([FromBody] FeedbackDTO feedbackDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Patient? patient = null;

            if (feedbackDTO.PatientId is not null)
            {
                patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == feedbackDTO.PatientId);
            }

            var feedback = new Feedback()
            {
                Message = feedbackDTO.Message,
                PatientId = feedbackDTO.PatientId,
                Patient = patient
            };

            await _db.Feedback.AddAsync(feedback);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
