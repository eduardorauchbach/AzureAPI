using Demo.API.Domain.Model;
using Demo.API.Domain.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RauchTech.Common.Logging;
using System;
using System.Collections.Generic;

namespace Demo.API.Domain.Controllers
{
    [EnableCors("Policy1")]
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ICustomLog _logger;
        private readonly CandidateService _candidateService;

        public CandidatesController(ICustomLogFactory logger, CandidateService candidateService)
        {
            _logger = logger.CreateLogger<CandidatesController>();
            _candidateService = candidateService;
        }

        [HttpGet]
        public IActionResult Get(string name = null, long? jobID = null)
        {
            List<Candidate> candidates;
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);
                _logger.AddID("Teste", "AAAAA");

                candidates = _candidateService.Get(name: name, jobID: jobID);

                response = Ok(candidates);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex);;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }

        // GET: api/Candidate/5
        [HttpGet("{id}", Name = "GetCandidate")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(long id)
        {
            Candidate candidate;
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                candidate = _candidateService.Get(id);

                if (candidate != null)
                {
                    response = Ok(candidate);
                }
                else
                {
                    response = NotFound(string.Empty);
                }

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex);;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] Candidate candidate)
        {
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                candidate = _candidateService.Insert(candidate);

                response = Ok(candidate);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex);;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }

        // PUT: api/Candidate/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Put(long id, [FromBody] Candidate candidate)
        {
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                candidate.ID = id;
                candidate = _candidateService.Update(candidate);

                response = Ok(candidate);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex);;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(long id)
        {
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                _candidateService.Delete(id);

                response = Ok(string.Empty);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex);;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }
    }
}
