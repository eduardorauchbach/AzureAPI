using ATS.API.Data.Repository;
using ATS.API.Model;
using System;
using System.Collections.Generic;

namespace ATS.API.Service
{
    public class CandidateService
    {
        private readonly CandidateRepository _candidateRepository;
        private readonly CandidateJobService _candidateJobService;
        private readonly BlobFileService _blobFileService;

        public CandidateService(CandidateRepository candidateRepository, CandidateJobService candidateJobService, BlobFileService blobFileService)
        {
            _candidateRepository = candidateRepository;
            _candidateJobService = candidateJobService;
            _blobFileService = blobFileService;
        }

        #region Change Data

        public Candidate Insert(Candidate candidate)
        {
            try
            {
                if (candidate.ID == 0)
                {
                    if (!string.IsNullOrEmpty(candidate.BlobFile?.Data))
                    {
                        candidate.BlobFile = _blobFileService.Insert(candidate.BlobFile);
                        candidate.FileID = candidate.BlobFile.ID;
                    }

                    candidate = _candidateRepository.Insert(candidate);
                }
                else
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do PUT");
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(candidate.FileID) && candidate.ID == 0)
                {
                    _blobFileService.Delete(candidate.FileID);
                }

                throw ex;
            }

            return candidate;
        }

        public Candidate Update(Candidate candidate)
        {
            try
            {
                if (candidate.ID == 0)
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do POST");
                }
                else
                {
                    if (!string.IsNullOrEmpty(candidate.BlobFile?.Data))
                    {
                        if (!string.IsNullOrEmpty(candidate.BlobFile?.ID))
                        {
                            candidate.BlobFile = _blobFileService.Update(candidate.BlobFile);
                        }
                        else
                        {
                            candidate.BlobFile = _blobFileService.Insert(candidate.BlobFile);
                        }
                        candidate.FileID = candidate.BlobFile.ID;
                    }

                    candidate = _candidateRepository.Update(candidate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return candidate;
        }

        public void Delete(long id)
        {
            Candidate candidate;

            try
            {
                if (id == 0)
                {
                    throw new Exception("ID inválido");
                }
                else
                {
                    candidate = Get(id);
                    if (candidate != null)
                    {
                        if (!string.IsNullOrEmpty(candidate.FileID))
                        {
                            _blobFileService.Delete(candidate.FileID);
                        }
                        _candidateRepository.Delete(id);
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

        public Candidate Get(long id)
        {
            Candidate candidate;

            try
            {
                candidate = _candidateRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return candidate;
        }

        public List<Candidate> Get(string name = null, long? jobID = null)
        {
            List<Candidate> candidates;

            try
            {
                candidates = _candidateRepository.Get(name: name, jobID: jobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return candidates;
        }

        #endregion
    }
}
