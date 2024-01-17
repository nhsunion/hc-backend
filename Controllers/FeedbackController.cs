using hc_backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace hc_backend.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly AppDbcontext _db;
        public FeedbackController(AppDbcontext context)
        {
            _db = context;
        }
    }
}
