using Demo.API.Domain.Data.Base;
using Demo.API.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RauchTech.Common.Model;
using RauchTech.DataExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Demo.API.Domain.Repository
{
    public class CandidateRepository
    {
        private readonly IConfiguration _config;
        private readonly ISqlHelper _sqlHelper;

        public CandidateRepository(IConfiguration configuration, ISqlHelper sqlHelper)
        {
            _config = configuration;
            _sqlHelper = sqlHelper;
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
                command = new SqlCommand(" INSERT INTO Candidate " +
                            " (" +
                                "  Name" +
                                " ,FileID" +
                            " )" +
                            " OUTPUT inserted.ID " +
                            " VALUES " +
                            " (" +
                                "  @Name" +
                                " ,@FileID" +
                            " )");

                command.Parameters.AddWithValue("Name", candidate.Name.AsDbValue());
                command.Parameters.AddWithValue("FileID", candidate.FileID.AsDbValue());

                candidate.ID = (long)_sqlHelper.ExecuteScalar(command);
            }
            catch
            {
                throw;
            }

            return candidate;
        }

        public Candidate Update(Candidate candidate)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand(" UPDATE Candidate SET " +

                            "  Name = @Name" +
                            " ,FileID = @FileID" +

                            " WHERE ID = @ID");

                command.Parameters.AddWithValue("ID", candidate.ID.AsDbValue());
                command.Parameters.AddWithValue("Name", candidate.Name.AsDbValue());
                command.Parameters.AddWithValue("FileID", candidate.FileID.AsDbValue());

                _sqlHelper.ExecuteNonQuery(command);
            }
            catch
            {
                throw;
            }

            return candidate;
        }

        public bool Delete(long id)
        {
            SqlCommand command;

            int result;

            try
            {
                command = new SqlCommand(" DELETE from CandidateJob where CandidateID = @ID " +
                                        " DELETE from Candidate where ID = @ID ");

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

        public Candidate Get(long id)
        {
            SqlCommand command;
            DataSet dataSet;

            Candidate candidate;

            try
            {
                command = new SqlCommand(" SELECT * FROM Candidate WHERE ID = @ID");
                command.Parameters.AddWithValue("ID", id.AsDbValue());

                dataSet = _sqlHelper.ExecuteDataSet(command);

                candidate = Load(dataSet).FirstOrDefault();

            }
            catch
            {
                throw;
            }

            return candidate;
        }

        public PageModel<Candidate> Get(string name = null, string fileID = null, long? jobID = null, PageModel<Candidate> page = null)
        {
            SqlCommand commandCount;
            SqlCommand commandWhere;
            DataSet dataSet;

            List<string> clauses;

            int count;

            try
            {
                page ??= new PageModel<Candidate>();

                commandCount = new SqlCommand(" SELECT DISTINCT COUNT(*) " +
                                " FROM Candidate A LEFT JOIN" +
                                " CandidateJob B ON A.ID = B.CandidateID");

                commandWhere = new SqlCommand(" SELECT DISTINCT A.* " +
                                " FROM Candidate A LEFT JOIN" +
                                " CandidateJob B ON A.ID = B.CandidateID");

                clauses = new List<string>();

                if (!string.IsNullOrEmpty(name))
                {
                    clauses.Add($"A.Name LIKE '%' + @Name + '%'");
                    commandCount.Parameters.AddWithValue($"Name", name.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Name", name.AsDbValue());
                }

                if (!string.IsNullOrEmpty(fileID))
                {
                    clauses.Add($"A.FileID LIKE '%' + @FileID + '%'");
                    commandCount.Parameters.AddWithValue($"FileID", fileID.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"FileID", fileID.AsDbValue());
                }

                if (jobID.HasValue)
                {
                    clauses.Add($"B.JobID = @JobID");
                    commandCount.Parameters.AddWithValue($"JobID", jobID.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"JobID", jobID.AsDbValue());
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
