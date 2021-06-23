using ATS.API.Data.Repository;
using ATS.API.Model;
using System;
using System.Collections.Generic;

namespace ATS.API.Service
{
    public class JobService
    {
        private readonly JobRepository _jobRepository;

        public JobService(JobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        #region Change Data

        public Job Insert(Job job)
        {
            try
            {
                if (job.ID == 0)
                {
                    job = _jobRepository.Insert(job);
                }
                else
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do PUT");
                }
            }
            catch
            {
                throw;
            }

            return job;
        }

        public Job Update(Job job)
        {
            try
            {
                if (job.ID == 0)
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do POST");
                }
                else
                {
                    job = _jobRepository.Update(job);
                }
            }
            catch
            {
                throw;
            }

            return job;
        }

        public void Delete(long id)
        {
            try
            {
                if (id == 0)
                {
                    throw new Exception("ID inválido");
                }
                else
                {
                    _jobRepository.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieve Repository

        public Job Get(long id)
        {
            Job job;

            try
            {
                job = _jobRepository.Get(id);
            }
            catch
            {
                throw;
            }

            return job;
        }

        public List<Job> Get(long? candidateID = null)
        {
            List<Job> jobs;

            try
            {
                jobs = _jobRepository.Get(candidateID: candidateID);
            }
            catch
            {
                throw;
            }

            return jobs;
        }

        #endregion
    }
}
