using System.Data;
using Microsoft.Data.SqlClient;

namespace TrainTicket.Data.ADO
{
    // Helper t?p trung cho thao tác ADO.NET v?i stored procedures.
    // Dùng ? các nghi?p v? c?n t?i ?u/transaction t?i SQL Server.
    public class AdoHelper
    {
        private readonly string _connStr;
        private const int DefaultTimeout = 30;

        public AdoHelper(string connectionString) => _connStr = connectionString;

        // G?i SP tr? v? DataTable  dng cho bo co, tm chuy?n, s? ?? gh?
        public DataTable ExecuteStoredProcedure(
            string procedureName,
            Dictionary<string, object?>? parameters = null,
            int timeout = DefaultTimeout)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd  = CreateSpCommand(conn, procedureName, timeout, parameters);
            conn.Open();
            var dt = new DataTable();
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        // ?? Inline SQL ? DataTable (cho Views, Functions) ???
        public DataTable ExecuteQuery(
            string sql,
            Dictionary<string, object?>? parameters = null)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd  = new SqlCommand(sql, conn) { CommandTimeout = DefaultTimeout };
            AddParameters(cmd, parameters);
            conn.Open();
            var dt = new DataTable();
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        // G?i SP khng tr? d? li?u  dng cho h?y v, c?p nh?t tr?ng thi
        public void ExecuteNonQuery(
            string procedureNameOrSql,
            Dictionary<string, object?>? parameters = null,
            bool isStoredProcedure = false)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd  = new SqlCommand(procedureNameOrSql, conn)
            {
                CommandType    = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text,
                CommandTimeout = DefaultTimeout
            };
            AddParameters(cmd, parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // L?y giá tr? ??n
        public T? ExecuteScalar<T>(
            string sql,
            Dictionary<string, object?>? parameters = null)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd  = new SqlCommand(sql, conn) { CommandTimeout = DefaultTimeout };
            AddParameters(cmd, parameters);
            conn.Open();
            var result = cmd.ExecuteScalar();
            if (result == null || result == DBNull.Value) return default;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        // G?i SP tr? v? nhi?u b?ng  dng khi c?n join nhi?u k?t qu?
        public DataSet ExecuteStoredProcedureDataSet(
            string procedureName,
            Dictionary<string, object?>? parameters = null)
        {
            using var conn = new SqlConnection(_connStr);
            using var cmd  = CreateSpCommand(conn, procedureName, DefaultTimeout, parameters);
            conn.Open();
            var ds = new DataSet();
            using var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        // G?i SP tr? v? DataTable  dng cho bo co, tm chuy?n, s? ?? gh?
        public async Task<DataTable> ExecuteStoredProcedureAsync(
            string procedureName,
            Dictionary<string, object?>? parameters = null,
            int timeout = DefaultTimeout)
        {
            await using var conn = new SqlConnection(_connStr);
            using var cmd = CreateSpCommand(conn, procedureName, timeout, parameters);
            await conn.OpenAsync();
            var dt = new DataTable();
            using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }

        // ?? Inline SQL async ? DataTable ????????????????????
        public async Task<DataTable> ExecuteQueryAsync(
            string sql,
            Dictionary<string, object?>? parameters = null)
        {
            await using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand(sql, conn) { CommandTimeout = DefaultTimeout };
            AddParameters(cmd, parameters);
            await conn.OpenAsync();
            var dt = new DataTable();
            using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }

        // L?y giá tr? ??n
        public async Task<T?> ExecuteScalarAsync<T>(
            string sql,
            Dictionary<string, object?>? parameters = null)
        {
            await using var conn = new SqlConnection(_connStr);
            using var cmd  = new SqlCommand(sql, conn) { CommandTimeout = DefaultTimeout };
            AddParameters(cmd, parameters);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value) return default;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        // DataAdapter không h? tr? async natively, nh?ng vi?c connect và execute command ?ã ???c await ? OpenAsync và ExecuteReaderAsync n?u làm th? công.
        // ?? ??n gi?n v?i DataSet (nhi?u b?ng), ta có th? ??c th? công qua t?ng k?t qu?
        public async Task<DataSet> ExecuteStoredProcedureDataSetAsync(
            string procedureName,
            Dictionary<string, object?>? parameters = null)
        {
            await using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            AddParameters(cmd, parameters);
            await conn.OpenAsync();

            var ds = new DataSet();
            await using var reader = await cmd.ExecuteReaderAsync();
            do
            {
                var dt = new DataTable();
                dt.Load(reader);
                ds.Tables.Add(dt);
            } while (!reader.IsClosed); // reader.NextResult() is called inside Load if there are more results sometimes depending on how Load is implemented. Actually dt.Load consumes the current result set. 

            return ds;
        }

        // G?i SP khng tr? d? li?u  dng cho h?y v, c?p nh?t tr?ng thi
        public async Task ExecuteNonQueryAsync(
            string procedureNameOrSql,
            Dictionary<string, object?>? parameters = null,
            bool isStoredProcedure = false)
        {
            await using var conn = new SqlConnection(_connStr);
            using var cmd = new SqlCommand(procedureNameOrSql, conn)
            {
                CommandType    = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text,
                CommandTimeout = DefaultTimeout
            };
            AddParameters(cmd, parameters);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ?? Test k?t n?i DB ??????????????????????????????????
        public bool TestConnection()
        {
            try
            {
                using var conn = new SqlConnection(_connStr);
                conn.Open();
                return true;
            }
            catch { return false; }
        }

        // Helper thm parameters vo SqlCommand
        private static SqlCommand CreateSpCommand(
            SqlConnection conn, string name, int timeout,
            Dictionary<string, object?>? parameters)
        {
            var cmd = new SqlCommand(name, conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = timeout
            };
            AddParameters(cmd, parameters);
            return cmd;
        }

        private static void AddParameters(SqlCommand cmd, Dictionary<string, object?>? parameters)
        {
            if (parameters == null) return;
            foreach (var (key, value) in parameters)
                cmd.Parameters.AddWithValue(key, value ?? DBNull.Value);
        }
    }
}
