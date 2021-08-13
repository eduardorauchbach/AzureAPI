using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RauchTech.Common.Extensions;
using RauchTech.Common.Model;
using RauchTech.DataExtensions.Sql;
using Demo.API.Domain.Model;

namespace Demo.API.Domain.Repository
{
    public class CandidateJobRepository
    {
        private readonly IConfiguration _config;
        private readonly ISqlHelper _sqlHelper;

        public CandidateJobRepository(IConfiguration configuration, ISqlHelper sqlHelper)
        {
            _config = configuration;
            _sqlHelper = sqlHelper;
        }

        #region LoadModel

        private List<CandidateJob> Load(DataSet data)
        {
            List<CandidateJob> candidateJobs;
            CandidateJob candidateJob;

            try
            {
                candidateJobs = new List<CandidateJob>();

                foreach (DataRow row in data.Tables[0].Rows)
                {
                    candidateJob = new CandidateJob();

                    candidateJob.CandidateID = row.Field<long>("CandidateID");
                    candidateJob.JobID = row.Field<long>("JobID");

                    candidateJobs.Add(candidateJob);
                }
            }
            catch
            {
                throw;
            }

            return candidateJobs;
        }

        #endregion

        #region Change Data

        public void Insert(List<CandidateJob> candidateJobs)
        {
            SqlCommand command;

            CandidateJob candidateJob;
            List<string> clauses;

            try
            {
                if (candidateJobs.Count > 0)
                {
                    command = new SqlCommand(" INSERT INTO CandidateJob " +
                                " (" +
                                    "  CandidateID" +
                                    " ,JobID" +
                                " )" +
                                " VALUES ");

                    clauses = new List<string>();

                    for (int i = 0; i < candidateJobs.Count; i++)
                    {
                        candidateJob = candidateJobs[i];

                        clauses.Add($"(@CandidateID{i}, @JobID{i})");
                        command.Parameters.AddWithValue($"CandidateID{i}", candidateJob.CandidateID.AsDbValue());
                        command.Parameters.AddWithValue($"JobID{i}", candidateJob.JobID.AsDbValue());
                    }

                    command.CommandText += string.Join(", ", clauses);
                    _sqlHelper.ExecuteScalar(command);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Delete(List<CandidateJob> candidateJobs)
        {
            SqlCommand command;

            CandidateJob candidateJob;
            List<string> clauses;

            try
            {
                if (candidateJobs.Count > 0)
                {
                    command = new SqlCommand(" DELETE from CandidateJob ");

                    clauses = new List<string>();

                    for (int i = 0; i < candidateJobs.Count; i++)
                    {
                        candidateJob = candidateJobs[i];

                        clauses.Add($"(CandidateID = @CandidateID{i} AND JobID = @JobID{i})");
                        command.Parameters.AddWithValue($"CandidateID{i}", candidateJob.CandidateID.AsDbValue());
                        command.Parameters.AddWithValue($"JobID{i}", candidateJob.JobID.AsDbValue());
                    }

                    if (clauses.Count > 0)
                    {
                        command.CommandText += $" WHERE { string.Join(" OR ", clauses)}";
                    }

                    _sqlHelper.ExecuteScalar(command);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Retrieve Data

        public List<CandidateJob> Get(long? candidateID = null, long? jobID = null)
        {
            SqlCommand command;
            DataSet dataSet;

            List<CandidateJob> candidateJobs;
            List<string> clauses;

            try
            {
                command = new SqlCommand(" SELECT * FROM CandidateJob ");

                clauses = new List<string>();

                if (candidateID.HasValue)
                {
                    clauses.Add($"CandidateID = @CandidateID");
                    command.Parameters.AddWithValue($"CandidateID", candidateID.AsDbValue());
                }

                if (jobID.HasValue)
                {
                    clauses.Add($"JobID = @JobID");
                    command.Parameters.AddWithValue($"JobID", jobID.AsDbValue());
                }

                if (clauses.Count > 0)
                {
                    command.CommandText += $" WHERE { string.Join(" AND ", clauses)}";
                }

                dataSet = _sqlHelper.ExecuteDataSet(command);

                candidateJobs = Load(dataSet);

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