using Microsoft.AspNetCore.Mvc;
using TrainingB.Core.Services;

namespace TrainingB.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DaoSoController : ControllerBase
    {
        [HttpPost("dao2-2")]
        public IActionResult Dao2Trong2([FromBody] DaoSoRequest request)
        {
            if (!DaoSoService.ValidateInput(request.Input, 2))
            {
                return BadRequest(new { error = "Input must be exactly 2 digits" });
            }

            var results = DaoSoService.Dao2Trong2(request.Input);
            return Ok(new
            {
                input = request.Input,
                mode = "Đảo 2/2",
                count = results.Count,
                results = results,
                resultsString = string.Join(",", results)
            });
        }

        [HttpPost("dao2-3")]
        public IActionResult Dao2Trong3([FromBody] DaoSoRequest request)
        {
            if (!DaoSoService.ValidateInput(request.Input, 3))
            {
                return BadRequest(new { error = "Input must be exactly 3 digits" });
            }

            var results = DaoSoService.Dao2Trong3(request.Input);
            return Ok(new
            {
                input = request.Input,
                mode = "Đảo 2/3",
                count = results.Count,
                results = results,
                resultsString = string.Join(",", results)
            });
        }

        [HttpPost("dao2-4")]
        public IActionResult Dao2Trong4([FromBody] DaoSoRequest request)
        {
            if (!DaoSoService.ValidateInput(request.Input, 4))
            {
                return BadRequest(new { error = "Input must be exactly 4 digits" });
            }

            var results = DaoSoService.Dao2Trong4(request.Input);
            return Ok(new
            {
                input = request.Input,
                mode = "Đảo 2/4",
                count = results.Count,
                results = results,
                resultsString = string.Join(",", results)
            });
        }

        [HttpPost("dao3-3")]
        public IActionResult Dao3Trong3([FromBody] DaoSoRequest request)
        {
            if (!DaoSoService.ValidateInput(request.Input, 3))
            {
                return BadRequest(new { error = "Input must be exactly 3 digits" });
            }

            var results = DaoSoService.Dao3Trong3(request.Input);
            return Ok(new
            {
                input = request.Input,
                mode = "Đảo 3/3",
                count = results.Count,
                results = results,
                resultsString = string.Join(",", results)
            });
        }

        [HttpPost("dao3-4")]
        public IActionResult Dao3Trong4([FromBody] DaoSoRequest request)
        {
            if (!DaoSoService.ValidateInput(request.Input, 4))
            {
                return BadRequest(new { error = "Input must be exactly 4 digits" });
            }

            var results = DaoSoService.Dao3Trong4(request.Input);
            return Ok(new
            {
                input = request.Input,
                mode = "Đảo 3/4",
                count = results.Count,
                results = results,
                resultsString = string.Join(",", results)
            });
        }

        [HttpPost("dao4-4")]
        public IActionResult Dao4Trong4([FromBody] DaoSoRequest request)
        {
            if (!DaoSoService.ValidateInput(request.Input, 4))
            {
                return BadRequest(new { error = "Input must be exactly 4 digits" });
            }

            var results = DaoSoService.Dao4Trong4(request.Input);
            return Ok(new
            {
                input = request.Input,
                mode = "Đảo 4/4",
                count = results.Count,
                results = results,
                resultsString = string.Join(",", results)
            });
        }
    }

    public class DaoSoRequest
    {
        public string Input { get; set; } = string.Empty;
    }
}
