using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AgentPortal.ViewModels.Home;
using Microsoft.Extensions.Configuration;

namespace AgentPortal.Models
{
    public class AgentsRepository
    {
        private readonly IConfiguration _configuration;

        public AgentsRepository(IConfiguration congifuration)
        {
            _configuration = congifuration;
        }

        public List<Agent> GetAllAgents()
        {
            var agentsList = new List<Agent>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM AGENTS"
                };

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var agentCode = reader["AgentCode"].ToString();
                    var agentName = reader["AgentName"].ToString();
                    var workingArea = reader["WorkingArea"].ToString();
                    var commission = Decimal.Parse(reader["Commission"].ToString());
                    var phoneNo = reader["PhoneNo"].ToString();

                    agentsList.Add(new Agent
                    {
                        AgentCode = agentCode,
                        AgentName = agentName,
                        WorkingArea = workingArea,
                        Commission = commission,
                        PhoneNo = phoneNo
                    });
                }
            }

            return agentsList;
        }

        public Agent GetAgent(string code)
        {
            Agent agent = new Agent();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@AgentCode", code);
                cmd.CommandText = $"SELECT * FROM AGENTS WHERE AgentCode = @AgentCode";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var agentCode = reader["AgentCode"].ToString();
                    var agentName = reader["AgentName"].ToString();
                    var workingArea = reader["WorkingArea"].ToString();
                    var commission = Decimal.Parse(reader["Commission"].ToString());
                    var phoneNo = reader["PhoneNo"].ToString();

                    agent = new Agent
                    {
                        AgentCode = agentCode,
                        AgentName = agentName,
                        WorkingArea = workingArea,
                        Commission = commission,
                        PhoneNo = phoneNo
                    };
                }
            }
            return agent;
        }

        public void CreateAgent(Agent agent)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                var agentCodeParam = new SqlParameter()
                { 
                    ParameterName = "@AgentCode",
                    SqlDbType = SqlDbType.Char,
                    Value = agent.AgentCode
                };
                cmd.Parameters.Add(agentCodeParam);

                cmd.Parameters.AddWithValue("@AgentName", agent.AgentName);
                cmd.Parameters.AddWithValue("@WorkingArea", agent.WorkingArea);
                cmd.Parameters.AddWithValue("@Commission", agent.Commission);
                cmd.Parameters.AddWithValue("@PhoneNo", agent.PhoneNo);
                cmd.CommandText = 
                    $"INSERT INTO AGENTS " +
                    $"(AgentCode, AgentName, WorkingArea, Commission, PhoneNo) " +
                    $"Values (@AgentCode, @AgentName, @WorkingArea, @Commission, @PhoneNo)";
                var reader = cmd.ExecuteNonQuery();
            }
        }
    }
}
