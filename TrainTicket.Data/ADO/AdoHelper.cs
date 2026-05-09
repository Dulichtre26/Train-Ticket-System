using System.Data;
using Microsoft.Data.SqlClient;

namespace TrainTicket.Data.ADO
{
    // Helper t?p trung cho thao tác ADO.NET v?i stored procedures.
    // Dùng ? các nghi?p v? c?n t?i ?u/transaction t?i SQL Server.
    public class AdoHelper
    {
        private readonly string _connStr;

        public AdoHelper(string connectionString)
        {
            _connStr = connectionString;
        }

        // G?i SP tr? v? DataTable  dng cho bo co, tm chuy?n, s? ?? gh?
        public async Task<DataTable> ExecuteStoredProcedureAsync(
            string procedureName,
            Dictionary<string, object?>? parameters = null)
        {
            await using var conn = new SqlConnection(_connStr);
            await using var cmd = new SqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
            };

            AddParameters(cmd, parameters);
            await conn.OpenAsync();

            var dt = new DataTable();
            await using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }

        // G?i SP khng tr? d? li?u  dng cho h?y v, c?p nh?t tr?ng thi
        public async Task ExecuteNonQueryAsync(
            string procedureName,
            Dictionary<string, object?>? parameters = null)
        {
            await using var conn = new SqlConnection(_connStr);
            await using var cmd = new SqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            AddParameters(cmd, parameters);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // G?i SP tr? v? nhi?u b?ng  dng khi c?n join nhi?u k?t qu?
        public async Task<DataSet> ExecuteStoredProcedureDataSetAsync(
            string procedureName,
            Dictionary<string, object?>? parameters = null)
        {
            await using var conn = new SqlConnection(_connStr);
            await using var cmd = new SqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            AddParameters(cmd, parameters);
            await conn.OpenAsync();

            var ds = new DataSet();
            // DataAdapter không h? tr? async natively, nh?ng vi?c connect và execute command ?ã ???c await ? OpenAsync và ExecuteReaderAsync n?u làm th? công.
            // ?? ??n gi?n v?i DataSet (nhi?u b?ng), ta có th? ??c th? công qua t?ng k?t qu?
            await using var reader = await cmd.ExecuteReaderAsync();
            do
            {
                var dt = new DataTable();
                dt.Load(reader);
                ds.Tables.Add(dt);
            } while (!reader.IsClosed); // reader.NextResult() is called inside Load if there are more results sometimes depending on how Load is implemented. Actually dt.Load consumes the current result set. 

            return ds;
        }

        // Helper thm parameters vo SqlCommand
        private static void AddParameters(
            SqlCommand cmd,
            Dictionary<string, object?>? parameters)
        {
            if (parameters == null) return;
            foreach (var p in parameters)
                cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
        }
    }
}
