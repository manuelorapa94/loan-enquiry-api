using LoanEnquiryApi.Model.Dashboard;
using LoanEnquiryApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace LoanEnquiryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        private DashboardService _service;

        public DashboardController(DataContext dataContext)
        {
            _service = new DashboardService(dataContext);
        }

        [HttpGet("numberOfEnquiry")]
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfEnquiry([FromQuery] DashboardParam param)
        {
            var result = _service.GetNumberOfEnquiry(param);

            return Ok(result);
        }

        [HttpGet("numberOfEnquiry/detail")]
        [ProducesResponseType(typeof(IEnumerable<DashboardDetailModel>), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfEnquiryDetail([FromQuery] DashboardDetailParam param)
        {
            var result = _service.GetNumberOfEnquiryDetail(param);

            return Ok(result);
        }

        [HttpGet("numberOfNewPurchase")]
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfNewPurchase([FromQuery] DashboardParam param)
        {
            var result = _service.GetNumberOfEnquiry(param, loanType: Constant.LoanType.NewPurchase);

            return Ok(result);
        }

        [HttpGet("numberOfNewPurchase/detail")]
        [ProducesResponseType(typeof(IEnumerable<DashboardDetailModel>), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfNewPurchaseDetail([FromQuery] DashboardDetailParam param)
        {
            var result = _service.GetNumberOfEnquiryDetail(param, loanType: Constant.LoanType.NewPurchase);

            return Ok(result);
        }

        [HttpGet("numberOfRefinance")]
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfRefinance([FromQuery] DashboardParam param)
        {
            var result = _service.GetNumberOfEnquiry(param, loanType: Constant.LoanType.Refinance);

            return Ok(result);
        }

        [HttpGet("numberOfRefinance/detail")]
        [ProducesResponseType(typeof(IEnumerable<DashboardDetailModel>), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfRefinanceDetail([FromQuery] DashboardDetailParam param)
        {
            var result = _service.GetNumberOfEnquiryDetail(param, loanType: Constant.LoanType.Refinance);

            return Ok(result);
        }

        [HttpGet("numberOfPrivateResidential")]
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfPrivateResidential([FromQuery] DashboardParam param)
        {
            var result = _service.GetNumberOfEnquiry(param, propertyType: Constant.PropertyType.PrivateResidential);

            return Ok(result);
        }

        [HttpGet("numberOfPrivateResidential/detail")]
        [ProducesResponseType(typeof(IEnumerable<DashboardDetailModel>), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfPrivateResidentialDetail([FromQuery] DashboardDetailParam param)
        {
            var result = _service.GetNumberOfEnquiryDetail(param, propertyType: Constant.PropertyType.PrivateResidential);

            return Ok(result);
        }

        [HttpGet("numberOfHDB")]
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfHDB([FromQuery] DashboardParam param)
        {
            var result = _service.GetNumberOfEnquiry(param, propertyType: Constant.PropertyType.HDB);

            return Ok(result);
        }

        [HttpGet("numberOfHDB/detail")]
        [ProducesResponseType(typeof(IEnumerable<DashboardDetailModel>), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfHDBDetail([FromQuery] DashboardDetailParam param)
        {
            var result = _service.GetNumberOfEnquiryDetail(param, propertyType: Constant.PropertyType.HDB);

            return Ok(result);
        }

        [HttpGet("numberOfCommercial")]
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfCommercial([FromQuery] DashboardParam param)
        {
            var result = _service.GetNumberOfEnquiry(param, propertyType: Constant.PropertyType.Commercial);

            return Ok(result);
        }

        [HttpGet("numberOfCommercial/detail")]
        [ProducesResponseType(typeof(IEnumerable<DashboardDetailModel>), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfCommercialDetail([FromQuery] DashboardDetailParam param)
        {
            var result = _service.GetNumberOfEnquiryDetail(param, propertyType: Constant.PropertyType.Commercial);

            return Ok(result);
        }

        [HttpGet("numberOfBUC")]
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfBUC([FromQuery] DashboardParam param)
        {
            var result = _service.GetNumberOfEnquiry(param, propertyType: Constant.PropertyType.BUC);

            return Ok(result);
        }

        [HttpGet("numberOfBUC/detail")]
        [ProducesResponseType(typeof(IEnumerable<DashboardDetailModel>), StatusCodes.Status200OK)]
        public IActionResult GetNumberOfBUCDetail([FromQuery] DashboardDetailParam param)
        {
            var result = _service.GetNumberOfEnquiryDetail(param, propertyType: Constant.PropertyType.BUC);

            return Ok(result);
        }
    }
}
