using ATS.API.Data.Base;
using ATS.API.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ATS.API.Data.Repository
{
    public class CandidateJobRepository
    {
        private readonly SqlHelper _dataConnection;

        public CandidateJobRepository(SqlHelper sqlHelper)
        {
            _dataConnection = sqlHelper;
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

                    candidateJob.ID = row.Field<long>("ID");
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

        public CandidateJob Insert(CandidateJob candidateJob)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand($@" INSERT INTO CandidateJob
											    (
												     CandidateID
												    ,JobID
											    )
										     OUTPUT inserted.ID 
										     VALUES
											    (
												     @CandidateID
												    ,@JobID
											    )");

                command.Parameters.AddWithValue("CandidateID", candidateJob.CandidateID.AsDbValue());
                command.Parameters.AddWithValue("JobID", candidateJob.JobID.AsDbValue());

                candidateJob.ID = (long)_dataConnection.ExecuteScalar(command);
            }
            catch
            {
                throw;
            }

            return candidateJob;
        }

        public CandidateJob Update(CandidateJob candidatejob)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand($" UPDATE CandidateJob SET " +
                                         $" CandidateID = @CandidateID," +
                                         $" JobID = @JobID" +
                                         $" WHERE ID = @ID");

                command.Parameters.AddWithValue("ID", candidatejob.ID.AsDbValue());
                command.Parameters.AddWithValue("CandidateID", candidatejob.CandidateID.AsDbValue());
                command.Parameters.AddWithValue("JobID", candidatejob.JobID.AsDbValue());

                _dataConnection.ExecuteNonQuery(command);
            }
            catch
            {
                throw;
            }

            return candidatejob;
        }

        public bool Delete(long id)
        {
            SqlCommand command;
            int result;

            try
            {
                command = new SqlCommand($@" DELETE from CandidateJob where ID=@ID ");

                command.Parameters.AddWithValue("ID", id.AsDbValue());

                result = _dataConnection.ExecuteNonQuery(command);
            }
            catch
            {
                throw;
            }

            return result > 0;
        }

        #endregion

        #region Retrieve Data

        public CandidateJob Get(long id)
        {
            SqlCommand command;
            DataSet dataSet;

            CandidateJob candidateJob;

            try
            {
                command = new SqlCommand($" SELECT * FROM CandidateJob WHERE ID = @ID ");
                command.Parameters.AddWithValue("ID", id.AsDbValue());

                dataSet = _dataConnection.ExecuteDataSet(command);

                candidateJob = Load(dataSet).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return candidateJob;
        }

        public List<CandidateJob> Get(long? candidateID = null, long? jobID = null)
        {
            SqlCommand command;
            DataSet dataSet;

            List<CandidateJob> candidateJobs;
            List<string> clauses;

            try
            {
                command = new SqlCommand($" SELECT * FROM CandidateJob ");

                clauses = new List<string>();
                if (candidateID.HasValue)
                {
                    clauses.Add($"CandidateID = @CandidateID");
                    command.Parameters.AddWithValue("CandidateID", candidateID.AsDbValue());
                }
                if (jobID.HasValue)
                {
                    clauses.Add($"JobID = @JobID");
                    command.Parameters.AddWithValue("JobID", candidateID.AsDbValue());
                }

                if (clauses.Count > 0)
                {
                    command.CommandText += $" WHERE {string.Join(" and ", clauses)}";
                }

                dataSet = _dataConnection.ExecuteDataSet(command);

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
