using Demo.API.Domain.Repository;
using Demo.API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.API.Domain.Service
{
    public class CandidateJobService
    {
        private readonly CandidateJobRepository _candidateJobRepository;

        public CandidateJobService(CandidateJobRepository candidateJobRepository)
        {
            _candidateJobRepository = candidateJobRepository;
        }

        #region Change Data

        public void Save(long id, List<CandidateJob> candidateJobs)
        {
            List<CandidateJob> oldLinks;
            List<CandidateJob> toDelete;
            List<CandidateJob> toSave;

            try
            {
                toDelete = new List<CandidateJob>();
                toSave = new List<CandidateJob>();

                if (candidateJobs != null && candidateJobs.Count > 0)
                {
                    candidateJobs.ForEach(x => x.CandidateID = id);

                    oldLinks = Get(id);
                    if (oldLinks.Count > 0)
                    {
                        toDelete = oldLinks.Where(o => !candidateJobs.Any(n => n.CandidateID == o.CandidateID && n.JobID == o.JobID)).ToList();

                        if (toDelete.Count > 0)
                        {
                            _candidateJobRepository.Delete(toDelete);
                        }
                    }

                    toSave = candidateJobs.Where(o => !oldLinks.Any(n => n.CandidateID == o.CandidateID && n.JobID == o.JobID)).ToList();

                    if (toSave.Count > 0)
                    {
                        _candidateJobRepository.Insert(toSave);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieve Repository

        public List<CandidateJob> Get(long? candidateID = null, long? jobID = null)
        {
            List<CandidateJob> candidateJobs;

            try
            {
                candidateJobs = _candidateJobRepository.Get(candidateID: candidateID, jobID: jobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return candidateJobs;
        }

        #endregion
    }
}
