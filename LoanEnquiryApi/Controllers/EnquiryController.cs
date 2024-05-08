using LoanEnquiryApi.Model.Enquiry;
using LoanEnquiryApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace LoanEnquiryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnquiryController : Controller
    {
        private EnquiryService _service;
        private EmailService _emailService;
        private IConfiguration _configuration;

        public EnquiryController(DataContext dataContext, IConfiguration configuration)
        {
            _service = new EnquiryService(dataContext, configuration);
            _emailService = new EmailService(configuration);
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<ListRecommendedBankModel>), StatusCodes.Status200OK)]
        public IActionResult CreateEnquiry(CreateEnquiryModel param)
        {
            var models = _service.CreateEnquiry(param);

            var url = _configuration["mortgage-url"]?.ToString();

            foreach (var model in models)
            {
                var content = "<p>Dear {name},</p><br><p>Your have a new inquiry.</p><p>To access the inquirer detail, kindly click the link below:</p><br><p><a href=\"{url}/inquirystatus/{enquiryId}\">{enquiryNo}</a></p><br><br><p>Best regards</p><p>Atlasadv</p><p><em>This email is sent from an automated system. Please do not reply.</em></p>";

                content = content.Replace("{name}", model.ContactPersonName);
                content = content.Replace("{url}", url);
                content = content.Replace("{enquiryId}", model.EnquiryId.ToString());
                content = content.Replace("{enquiryNo}", "ENQ" + model.EnquiryCode.ToString("D10"));

                _emailService.Send(model.ContactEmail, "New Inquiry", content);
            }

            return Ok(models);
        }

        [HttpPut]
        public IActionResult UpdateEnquiry(UpdateEnquiryModel param)
        {
            _service.UpdateEnquiry(param);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteEnquiry(Guid id)
        {
            var isSuccess = _service.DeleteEnquiry(id);

            if (isSuccess)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ListEnquiryModel>), StatusCodes.Status200OK)]
        public IActionResult GetEnquiries([FromQuery] ListEnquiryParam param)
        {
            var result = _service.GetEnquiries(param);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewEnquiryModel), StatusCodes.Status200OK)]
        public IActionResult GetEnquiry(Guid id)
        {
            var result = _service.GetEnquiry(id);

            return Ok(result);
        }
    }
}
