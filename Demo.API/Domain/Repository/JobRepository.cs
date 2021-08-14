using Demo.API.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RauchTech.Common.Extensions;
using RauchTech.Common.Model;
using RauchTech.DataExtensions.Sql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Demo.API.Domain.Repository
{
    public class JobRepository
    {
        private readonly IConfiguration _config;
        private readonly ISqlHelper _sqlHelper;

        public JobRepository(IConfiguration configuration, ISqlHelper sqlHelper)
        {
            _config = configuration;
            _sqlHelper = sqlHelper;
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
                command = new SqlCommand(" INSERT INTO Job " +
                                        " (" +
                                            "  Title" +
                                            " ,Description" +
                                        " )" +
                                        " OUTPUT inserted.ID " +
                                        " VALUES " +
                                        " (" +
                                            "  @Title" +
                                            " ,@Description" +
                                        " )");

                command.Parameters.AddWithValue("Title", job.Title.AsDbValue());
                command.Parameters.AddWithValue("Description", job.Description.AsDbValue());

                job.ID = (long)_sqlHelper.ExecuteScalar(command);
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
                command = new SqlCommand(" UPDATE Job SET " +

                                        "  Title = @Title" +
                                        " ,Description = @Description" +

                                        " WHERE ID = @ID");

                command.Parameters.AddWithValue("ID", job.ID.AsDbValue());
                command.Parameters.AddWithValue("Title", job.Title.AsDbValue());
                command.Parameters.AddWithValue("Description", job.Description.AsDbValue());

                _sqlHelper.ExecuteNonQuery(command);
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
                command = new SqlCommand(" DELETE from CandidateJob where JobID = @ID " +
                                         " DELETE from Job where ID = @ID ");

                command.Parameters.AddWithValue("ID", id.AsDbValue());
                result = _sqlHelper.ExecuteNonQuery(command);
            }
            catch
            {
                throw;
            }

            return (result > 0);
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
                command = new SqlCommand(" SELECT * FROM Job WHERE ID = @ID");
                command.Parameters.AddWithValue("ID", id.AsDbValue());

                dataSet = _sqlHelper.ExecuteDataSet(command);

                job = Load(dataSet).FirstOrDefault();

            }
            catch
            {
                throw;
            }

            return job;
        }

        public PageModel<Job> Get(string title = null, string description = null, long? candidateID = null, PageModel<Job> page = null)
        {
            SqlCommand commandCount;
            SqlCommand commandWhere;
            DataSet dataSet;

            List<string> clauses;

            int count;

            try
            {
                page ??= new PageModel<Job>();

                commandCount = new SqlCommand(" SELECT COUNT(DISTINCT A.ID) " +
                                " FROM Job A LEFT JOIN" +
                                " CandidateJob B ON A.ID = B.JobID");

                commandWhere = new SqlCommand(" SELECT DISTINCT A.* " +
                                " FROM Job A LEFT JOIN" +
                                " CandidateJob B ON A.ID = B.JobID");

                clauses = new List<string>();

                if (!string.IsNullOrEmpty(title))
                {
                    clauses.Add($"A.Title LIKE '%' + @Title + '%'");
                    commandCount.Parameters.AddWithValue($"Title", title.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Title", title.AsDbValue());
                }

                if (!string.IsNullOrEmpty(description))
                {
                    clauses.Add($"A.Description LIKE '%' + @Description + '%'");
                    commandCount.Parameters.AddWithValue($"Description", description.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Description", description.AsDbValue());
                }

                if (candidateID.HasValue)
                {
                    clauses.Add($"B.CandidateID = @CandidateID");
                    commandCount.Parameters.AddWithValue($"CandidateID", candidateID.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"CandidateID", candidateID.AsDbValue());
                }

                if (clauses.Count > 0)
                {
                    commandCount.CommandText += $" WHERE { string.Join(" AND ", clauses)}";
                    commandWhere.CommandText += $" WHERE { string.Join(" AND ", clauses)}";
                }

                if (page.OrderBy?.Count == 0)
                {
                    page.OrderBy.Add(("ID", true));
                }

                commandWhere.CommandText += page.ToOrderByScript("A");
                commandWhere.CommandText += page.ToFetchScript();

                count = (int)_sqlHelper.ExecuteScalar(commandCount);

                dataSet = _sqlHelper.ExecuteDataSet(commandWhere);

                page.ItemsCount = count;
                page.Items = Load(dataSet);
            }
            catch
            {
                throw;
            }

            return page;
        }

        #endregion

    }
}
