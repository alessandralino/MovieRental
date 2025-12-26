using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Rental.DTO;
using MovieRental.Rental.Features;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }


        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> Post(
            [FromBody] RentalSaveInput input,
            [FromServices] IValidator<RentalSaveInput> validator)
        {
            var validationResult = await validator.ValidateAsync(input);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                var savedRental = await _features.Save(input);
                return Created("", savedRental);
            }
            catch (Exception ex)
            { 
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("getByCustomerName")]
        public async Task<IActionResult> GetRentalsByCustomerName(
            string customerName,
            [FromServices] IValidator<string> validator)
        {
            var validationResult = await validator.ValidateAsync(customerName);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            { 
                return Ok(await _features.GetRentalsByCustomerName(customerName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
