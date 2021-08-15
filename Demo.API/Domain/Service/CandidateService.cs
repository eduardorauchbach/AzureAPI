using Demo.API.Domain.Repository;
using Demo.API.Domain.Model;
using RauchTech.Common.Model;
using System;
using System.Collections.Generic;
using RauchTech.DataExtensions.AzureBlob;

namespace Demo.API.Domain.Service
{
    public class CandidateService
    {
        private readonly CandidateRepository _candidateRepository;
        private readonly CandidateJobService _candidateJobService;
        private readonly IAzureBlobService _blobFileService;

        public CandidateService(CandidateRepository candidateRepository, CandidateJobService candidateJobService, IAzureBlobService blobFileService)
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
                    _candidateJobService.Save(candidate.ID, candidate.CandidateJobs);
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
                    _candidateJobService.Save(candidate.ID, candidate.CandidateJobs);
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
                candidate.CandidateJobs = _candidateJobService.Get(candidateID: id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return candidate;
        }

        public PageModel<Candidate> Get(string name = null, string fileID = null, long? jobID = null, PageModel<Candidate> page = null)
        {
            try
            {
                page = _candidateRepository.Get(name: name, fileID: fileID, jobID: jobID, page: page);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return page;
        }

        #endregion
    }
}
