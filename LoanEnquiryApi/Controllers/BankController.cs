using LoanEnquiryApi.Model.Bank;
using LoanEnquiryApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace LoanEnquiryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController(DataContext dataContext) : Controller
    {
        private readonly BankService _services = new BankService(dataContext);

        [HttpPost]
        public IActionResult CreateBank(CreateBankModel param)
        {
            var isSuccess = _services.CreateBank(param);

            if (isSuccess)
                return Ok();

            return BadRequest();
        }


        [HttpPut]
        public IActionResult UpdateBank(UpdateBankModel param)
        {
            var isSuccess = _services.UpdateBank(param);

            if (isSuccess)
                return Ok();

            return BadRequest();
        }

        [HttpPut("logo")]
        public IActionResult UpdateBankLogo(UpdateBankLogoModel param)
        {
            var isSuccess = _services.UpdateBankLogo(param);

            if (isSuccess)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBank(Guid id)
        {
            var isSuccess = _services.DeleteBank(id);

            if (isSuccess)
                return Ok();

            return BadRequest();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewBankModel), StatusCodes.Status200OK)]
        public IActionResult GetBank(Guid id)
        {
            var result = _services.GetBank(id);

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ListBankModel>), StatusCodes.Status200OK)]
        public IActionResult GetBanks()
        {
            var result = _services.GetBanks();

            return Ok(result);
        }

        [HttpGet("dropdown")]
        [ProducesResponseType(typeof(IEnumerable<BankDropdownModel>), StatusCodes.Status200OK)]
        public IActionResult GetBankDropdown()
        {
            var result = _services.GetBankDropdown();

            return Ok(result);
        }

        [HttpPost("rate/import")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ImportBankRate(ImportBankRateModel param)
        {
            var result = _services.ImportBankRate(param);

            if (!string.IsNullOrEmpty(result)) return BadRequest(result);

            return Ok();
        }
    }
}
