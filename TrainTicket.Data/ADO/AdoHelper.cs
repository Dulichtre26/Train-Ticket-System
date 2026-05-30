using System.Data;
using Microsoft.Data.SqlClient;

namespace TrainTicket.Data.ADO
{
    // Helper tap trung cho thao tác ADO.NET voi stored procedures.
    // Dung cho cac nghiep vu can toi uu/transaction tai SQL Server.
    public class AdoHelper
    {
        private readonly string _connStr;
        private const int DefaultTimeout = 30;

        public AdoHelper(string connectionString) => _connStr = connectionString;

        // Gui SP trc ve DataTable  dng cho bo co, tm chuyen, so do ghe
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

        // Gui SP khong tra ve du lieu  dung cho huy ve, cap nhat trang thai
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

        // Lay giá tri don
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

        // Gui SP tra ve nhieu bang  dung khi can join nhieu ket qua
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

        // Gui SP tra ve DataTable  dung cho bo co, tim chuyen, so do ghe
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

        // ?? Inline SQL async ? DataTable
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

        // Lay giá tri don
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

        // DataAdapter không hỗ trợ async natively, nhưng việc connect và execute command đã được await ở OpenAsync và ExecuteReaderAsync nếu làm thủ công.
        // Để đơn giản với DataSet (nhiều bảng), ta có thể được thực hiện thủ công qua từng kết quả
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

        // Gui SP khong tra ve du lieu  dung cho huy ve, cap nhat trang thai
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

        //  Test kết nối DB ??????????????????????????????????
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
