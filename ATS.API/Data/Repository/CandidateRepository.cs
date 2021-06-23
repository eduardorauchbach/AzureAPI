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
    public class CandidateRepository
    {
        private readonly IConfiguration _config;
        private readonly SqlHelper _dataConnection;

        public CandidateRepository(IConfiguration configuration, SqlHelper sqlHelper)
        {
            _config = configuration;
            _dataConnection = sqlHelper;
        }

        #region LoadModel

        private List<Candidate> Load(DataSet data)
        {
            List<Candidate> candidates;
            Candidate candidate;

            try
            {
                candidates = new List<Candidate>();

                foreach (DataRow row in data.Tables[0].Rows)
                {
                    candidate = new Candidate();

                    candidate.ID = row.Field<long>("ID");
                    candidate.Name = row.Field<string>("Name");
                    candidate.FileID = row.Field<string>("FileID");

                    candidate.LoadUrls(_config);

                    candidates.Add(candidate);
                }
            }
            catch
            {
                throw;
            }

            return candidates;
        }

        #endregion

        #region Change Data

        public Candidate Insert(Candidate candidate)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand($@" INSERT INTO Candidate
											    (
												     Name
												    ,FileID
											    )
										     OUTPUT inserted.ID 
										     VALUES
											    (
												     @Name
												    ,@FileID
											    )");

                command.Parameters.AddWithValue("Name", candidate.Name.AsDbValue());
                command.Parameters.AddWithValue("FileID", candidate.FileID.AsDbValue());

                candidate.ID = (long)_dataConnection.ExecuteScalar(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return candidate;
        }

        public Candidate Update(Candidate candidate)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand($" UPDATE Candidate SET " +
                                         $" Name = @Name," +
                                         $" FileID = @FileID" +
                                         $" WHERE ID = @ID");

                command.Parameters.AddWithValue("ID", candidate.ID.AsDbValue());
                command.Parameters.AddWithValue("Name", candidate.Name.AsDbValue());
                command.Parameters.AddWithValue("FileID", candidate.FileID.AsDbValue());

                _dataConnection.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            candidate.LoadUrls(_config);
            return candidate;
        }

        public bool Delete(long id)
        {
            SqlCommand command;
            int result;

            try
            {
                command = new SqlCommand($@" DELETE from Candidate WHERE ID = @ID ");

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

        public Candidate Get(long id)
        {
            SqlCommand command;
            DataSet dataSet;

            Candidate candidate;

            try
            {
                command = new SqlCommand($" SELECT * FROM Candidate WHERE ID = @ID ");
                command.Parameters.AddWithValue("ID", id.AsDbValue());

                dataSet = _dataConnection.ExecuteDataSet(command);

                candidate = Load(dataSet).FirstOrDefault();
            }
            catch 
            {
                throw;
            }

            return candidate;
        }

        public List<Candidate> Get(long? jobID = null, string name = null)
        {
            SqlCommand command;
            DataSet dataSet;

            List<Candidate> candidates;
            List<string> clauses;

            try
            {
                command = new SqlCommand($" SELECT DISTINCT C.*" +
                                         $" FROM Candidate C LEFT JOIN" +
                                         $" CandidateJob CJ ON C.ID = CJ.CandidateID LEFT JOIN " +
                                         $" Job J ON CJ.JobID = J.ID");

                clauses = new List<string>();
                if (jobID.HasValue)
                {
                    clauses.Add($"J.ID = @JobID");
                    command.Parameters.AddWithValue("JobID", jobID.AsDbValue());
                }
                if (!string.IsNullOrEmpty(name))
                {
                    clauses.Add($"C.Name like '%' + @Name + '%'");
                    command.Parameters.AddWithValue("Name", name.AsDbValue());
                }

                if (clauses.Count > 0)
                {
                    command.CommandText += $" WHERE {string.Join(" and ", clauses)}";
                }

                dataSet = _dataConnection.ExecuteDataSet(command);

                candidates = Load(dataSet);
            }
            catch 
            {
                throw;
            }

            return candidates;
        }

        #endregion
    }
}
