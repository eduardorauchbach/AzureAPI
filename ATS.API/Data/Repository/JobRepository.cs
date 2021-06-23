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
    public class JobRepository
    {
        private readonly SqlHelper _dataConnection;

        public JobRepository(SqlHelper sqlHelper)
        {
            _dataConnection = sqlHelper;
        }

        #region LoadModel

        private List<Job> Load(DataSet data)
        {
            List<Job> jobs;
            Job job;

            try
            {
                jobs = new List<Job>();

                foreach (DataRow row in data.Tables[0].Rows)
                {
                    job = new Job();

                    job.ID = row.Field<long>("ID");
                    job.Title = row.Field<string>("Title");
                    job.Description = row.Field<string>("Description");

                    jobs.Add(job);
                }
            }
            catch
            {
                throw;
            }

            return jobs;
        }

        #endregion

        #region Change Data

        public Job Insert(Job job)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand($@" INSERT INTO Job
											    (
												     Title
												    ,Description
											    )
										     OUTPUT inserted.ID 
										     VALUES
											    (
												     @Title
												    ,@Description
											    )");

                command.Parameters.AddWithValue("Title", job.Title.AsDbValue());
                command.Parameters.AddWithValue("Description", job.Description.AsDbValue());

                job.ID = (long)_dataConnection.ExecuteScalar(command);
            }
            catch
            {
                throw;
            }

            return job;
        }

        public Job Update(Job job)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand($" UPDATE Job SET " +
                                         $" Title = @Title," +
                                         $" Description = @Description" +
                                         $" WHERE ID = @ID");

                command.Parameters.AddWithValue("ID", job.ID.AsDbValue());
                command.Parameters.AddWithValue("Title", job.Title.AsDbValue());
                command.Parameters.AddWithValue("Description", job.Description.AsDbValue());

                _dataConnection.ExecuteNonQuery(command);
            }
            catch
            {
                throw;
            }

            return job;
        }

        public bool Delete(long id)
        {
            SqlCommand command;
            int result;

            try
            {
                command = new SqlCommand($@" DELETE from Job where ID=@ID ");

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

        public Job Get(long id)
        {
            SqlCommand command;
            DataSet dataSet;

            Job job;

            try
            {
                command = new SqlCommand($" SELECT * FROM Job WHERE ID = @ID ");
                command.Parameters.AddWithValue("ID", id.AsDbValue());

                dataSet = _dataConnection.ExecuteDataSet(command);

                job = Load(dataSet).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return job;
        }

        public List<Job> Get(long? candidateID = null)
        {
            SqlCommand command;
            DataSet dataSet;

            List<Job> jobs;
            List<string> clauses;

            try
            {
                command = new SqlCommand($" SELECT * " +
                                         $" FROM " +
                                         $" Job J LEFT JOIN " +
                                         $" CandidateJob CJ ON J.ID = CJ.JobID ");

                clauses = new List<string>();
                if (candidateID.HasValue)
                {
                    clauses.Add($"CJ.CandidateID = @CandidateID");
                    command.Parameters.AddWithValue("CandidateID", candidateID.AsDbValue());
                }

                if (clauses.Count > 0)
                {
                    command.CommandText += $" WHERE {string.Join(" and ", clauses)}";
                }

                dataSet = _dataConnection.ExecuteDataSet(command);

                jobs = Load(dataSet);
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
