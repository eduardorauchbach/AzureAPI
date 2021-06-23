using ATS.API.Data.Repository;
using ATS.API.Model;
using System;
using System.Collections.Generic;

namespace ATS.API.Service
{
    public class CandidateJobService
    {
        private readonly CandidateJobRepository _candidateJobRepository;

        public CandidateJobService(CandidateJobRepository candidateJobRepository)
        {
            _candidateJobRepository = candidateJobRepository;
        }

        #region Change Data

        public CandidateJob Insert(CandidateJob candidateJob)
        {
            try
            {
                if (candidateJob.ID == 0)
                {
                    if (Get(candidateID: candidateJob.CandidateID, jobID: candidateJob.JobID).Count == 0)
                    {
                        candidateJob = _candidateJobRepository.Insert(candidateJob);
                    }
                    else
                    {
                        throw new Exception("Candidato já aplicado para a vaga");
                    }
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

            return candidateJob;
        }

        public CandidateJob Update(CandidateJob candidateJob)
        {
            try
            {
                if (candidateJob.ID == 0)
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do POST");
                }
                else
                {
                    candidateJob = _candidateJobRepository.Update(candidateJob);
                }
            }
            catch
            {
                throw;
            }

            return candidateJob;
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
                    _candidateJobRepository.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieve Repository

        public CandidateJob Get(long id)
        {
            CandidateJob candidateJob;

            try
            {
                candidateJob = _candidateJobRepository.Get(id);
            }
            catch
            {
                throw;
            }

            return candidateJob;
        }

        public List<CandidateJob> Get(long? candidateID = null, long? jobID = null)
        {
            List<CandidateJob> candidateJobs;

            try
            {
                candidateJobs = _candidateJobRepository.Get(candidateID: candidateID, jobID: jobID);
            }
            catch
            {
                throw;
            }

            return candidateJobs;
        }

        #endregion
    }
}
