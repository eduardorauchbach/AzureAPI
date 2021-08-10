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
    public class CandidatesJobsController : ControllerBase
    {
        private readonly ICustomLog _logger;
        private readonly CandidateJobService _candidateJobService;

        public CandidatesJobsController(ICustomLogFactory logger, CandidateJobService candidateJobService)
        {
            _logger = logger.CreateLogger<CandidatesJobsController>();
            _candidateJobService = candidateJobService;
        }


        [HttpGet]
        public IActionResult Get(long? candidateID = null, long? jobID = null)
        {
            List<CandidateJob> candidateJobs;
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                candidateJobs = _candidateJobService.Get(candidateID: candidateID, jobID: jobID);

                response = Ok(candidateJobs);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex);;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }

        // GET: api/CandidateJob/5
        [HttpGet("{id}", Name = "GetCandidateJob")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(long id)
        {
            CandidateJob candidateJob;
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                candidateJob = _candidateJobService.Get(id);

                if (candidateJob != null)
                {
                    response = Ok(candidateJob);
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
        public IActionResult Post([FromBody] CandidateJob candidateJob)
        {
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                candidateJob = _candidateJobService.Insert(candidateJob);

                response = Ok(candidateJob);

                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Finish);
            }
            catch (Exception ex)
            {
                _logger.LogCustom(LogLevel.Error, exception: ex);;
                response = StatusCode(500, ex.Message);
            }

            return response;
        }

        // PUT: api/CandidateJob/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Put(long id, [FromBody] CandidateJob candidateJob)
        {
            ObjectResult response;

            try
            {
                _logger.LogCustom(LogLevel.Information, message: ICustomLog.Begin);

                candidateJob.ID = id;
                candidateJob = _candidateJobService.Update(candidateJob);

                response = Ok(candidateJob);

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

                _candidateJobService.Delete(id);

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
