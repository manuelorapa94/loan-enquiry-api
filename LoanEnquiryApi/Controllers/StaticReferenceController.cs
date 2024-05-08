using LoanEnquiryApi.Model.StaticReference;
using LoanEnquiryApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace LoanEnquiryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaticReferenceController : Controller
    {
        private StaticReferenceService _service;

        public StaticReferenceController()
        {
            _service = new StaticReferenceService();
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(StaticReferenceModel), StatusCodes.Status200OK)]
        public IActionResult GetStaticReferences(string name)
        {
            var result = _service.GetStaticReferences(name);

            return Ok(result);
        }
    }
}
