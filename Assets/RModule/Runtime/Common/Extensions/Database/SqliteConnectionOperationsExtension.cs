#if USE_SQLITE
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System;

public enum SQLOperation {
	BEGIN, TRANSACTION, COMMIT, DELETE, INSERT, INTO, FROM, MAX, SELECT, SET, UPDATE, VALUES, WHERE, CREATE_TABLE_IF_NOT_EXISTS, SUM, ORDER_BY_RANDOM_LIMIT
}

public enum LogicOperation {
	EQUALS, AND, NOT_EQUALS, OR, MORE_THAN, LESS_THAN
}

public enum SQLFieldParametr {
	TEXT, NOT_NULL, UNIQUE, INTEGER
}

public static class SqliteConnectionOperationsExtension {
	

	public static void OperationBeginTransaction(this SqliteConnection _sqliteConnection) {
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.BEGIN.ToString(), SQLOperation.TRANSACTION.ToString(), ";");
			using (IDbCommand dbcmd = _sqliteConnection.CreateCommand()) {
				dbcmd.CommandText = sql;
				dbcmd.ExecuteNonQuery();
			}
		}
	}

	public static void OperationCommit(this SqliteConnection _sqliteConnection) {
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.COMMIT.ToString(), ";");
			using (IDbCommand dbcmd = _sqliteConnection.CreateCommand()) {
				dbcmd.CommandText = sql;
				dbcmd.ExecuteNonQuery();
			}
		}
	}

	public static void OperationCustom(this SqliteConnection _sqliteConnection, string _sql) {
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = "";
			sql = CombineSQLWithDebug(_sql);
			IDbCommand dbcmd = _sqliteConnection.CreateCommand();
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}
	}

	public static void OperationCustom(this SqliteConnection _sqliteConnection, string _sql, out List<int> result) {
		result = new List<int>();
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = "";
			sql = CombineSQLWithDebug(_sql);
			IDbCommand dbcmd = _sqliteConnection.CreateCommand();
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
			result = _sqliteConnection.intResultFromCommand(sql);
		}
    }

    public static void OperationCustom(this SqliteConnection _sqliteConnection, string _sql, out List<string> result)
    {
        result = new List<string>();
        if (_sqliteConnection.State == ConnectionState.Open)
        {
            string sql = "";
            sql = CombineSQLWithDebug(_sql);
            IDbCommand dbcmd = _sqliteConnection.CreateCommand();
            dbcmd.CommandText = sql;
            dbcmd.ExecuteNonQuery();
            result = _sqliteConnection.stringResultFromCommand(sql);
        }
	}

	public static void OperationSelectAllTable(this SqliteConnection _sqliteConnection, string fromTableName, out List<List<string>> result) {
		result = new List<List<string>>();

		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), "*", SQLOperation.FROM.ToString(), fromTableName, ";");
			result = _sqliteConnection.allStringResultFromCommand(sql);
		}
	}

	public static void OperationSelectAllTable(this SqliteConnection _sqliteConnection, string fromTableName, out List<List<int>> result) {
		result = new List<List<int>>();

		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), "*", SQLOperation.FROM.ToString(), fromTableName, ";");
			result = _sqliteConnection.allIntResultFromCommand(sql);
		}
	}

	public static void OperationSelect(this SqliteConnection _sqliteConnection, string field, string fromTableName, out List<string> result) {
		result = new List<string>(); 

        if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, ";");
			result = _sqliteConnection.stringResultFromCommand(sql);
		}
	}

	public static void OperationSelect(this SqliteConnection _sqliteConnection, string field, string fromTableName, out List<int> result) {
		result = new List<int>();
        if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, ";");
			result = _sqliteConnection.intResultFromCommand(sql);
		}
    }

    public static void OperationSelect(this SqliteConnection _sqliteConnection, string field, string fromTableName, string fieldWhere, LogicOperation logicOper, string valueWhere, out List<string> result) {
		result = new List<string>();
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, SQLOperation.WHERE.ToString(), fieldWhere, Convert(logicOper), $"'{valueWhere}'", ";");
			result = _sqliteConnection.stringResultFromCommand(sql);
		}
	}

	public static void OperationSelect(this SqliteConnection _sqliteConnection, string field, string fromTableName, string fieldWhere, LogicOperation logicOper, string valueWhere, out List<int> result) {
		result = new List<int>();
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, SQLOperation.WHERE.ToString(), fieldWhere, Convert(logicOper), $"'{valueWhere}'", ";");
			result = _sqliteConnection.intResultFromCommand(sql);
		}
	}

	public static void OperationSelect(this SqliteConnection _sqliteConnection, string field, string fromTableName, List<string> fieldWhere, List<LogicOperation> operations, List<string> valueWhere, List<LogicOperation> operations1, out List<int> result) {
		result = new List<int>();
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithoutDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, SQLOperation.WHERE.ToString());
			for (int i = 0; i < operations1.Count; i++)
				sql += CombineSQLWithoutDebug(fieldWhere[i], Convert(operations[i]), $"'{valueWhere[i]}'", Convert(operations1[i]));
			sql = CombineSQLWithDebug(sql, fieldWhere[operations1.Count], Convert(operations[operations1.Count]), $"'{valueWhere[operations1.Count]}'", ";");
			result = _sqliteConnection.intResultFromCommand(sql);
		}
	}

	public static void OperationSelect(this SqliteConnection _sqliteConnection, string field, string fromTableName, List<string> fieldWhere, List<LogicOperation> operations, List<string> valueWhere, List<LogicOperation> operations1, out List<string> result) {
		result = new List<string>();
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithoutDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, SQLOperation.WHERE.ToString());
			for (int i = 0; i < operations1.Count; i++)
				sql += CombineSQLWithoutDebug(fieldWhere[i], Convert(operations[i]), $"'{valueWhere[i]}'", Convert(operations1[i]));
			sql = CombineSQLWithDebug(sql, fieldWhere[operations1.Count], Convert(operations[operations1.Count]), $"'{valueWhere[operations1.Count]}'", ";");
			result = _sqliteConnection.stringResultFromCommand(sql);
		}
    }

    public static void OperationSelectNotNull(this SqliteConnection _sqliteConnection, string field, string fromTableName, out List<string> result)
    {
        result = new List<string>();

        if (_sqliteConnection.State == ConnectionState.Open)
        {
            string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), "*", SQLOperation.FROM.ToString(), fromTableName, SQLOperation.WHERE.ToString(), field,"IS NOT NULL", ";");
            result = _sqliteConnection.stringResultFromCommandWithoutNULLValues(sql, field);
        }
    }

    public static int OperationSelectMax(this SqliteConnection _sqliteConnection, string field, string tableName) {
		int result = 0;
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), SQLOperation.MAX.ToString(), $"({field})", SQLOperation.FROM.ToString(), tableName, ";");
			result = _sqliteConnection.intResultFromCommand(sql)[0];
		}
		return result;
	}

	public static int OperationSelectSum(this SqliteConnection _sqliteConnection, string field, string tableName) {
		int result = 0;
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), SQLOperation.SUM.ToString(), $"({field})", SQLOperation.FROM.ToString(), tableName, ";");
			result = _sqliteConnection.intResultFromCommand(sql)[0];
		}
		return result;
	}

	public static bool OperationSelectByRandomLimit(this SqliteConnection _sqliteConnection, string field, string fromTableName, int limit, out List<string> result) {
		result = default;
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, Convert(SQLOperation.ORDER_BY_RANDOM_LIMIT), limit.ToString(), ";");
			result = _sqliteConnection.stringResultFromCommand(sql);

			return true;
		}

		return false;
	}

	public static void OperationSelectByRandomLimit(this SqliteConnection _sqliteConnection, string field, string fromTableName, int limit, List<string> fieldWhere, List<LogicOperation> operations, List<string> valueWhere, List<LogicOperation> operations1, out List<string> result) {
		result = new List<string>();
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithoutDebug(SQLOperation.SELECT.ToString(), field, SQLOperation.FROM.ToString(), fromTableName, SQLOperation.WHERE.ToString());
			for (int i = 0; i < operations1.Count; i++)
				sql += CombineSQLWithoutDebug(fieldWhere[i], Convert(operations[i]), $"'{valueWhere[i]}'", Convert(operations1[i]));
			sql = CombineSQLWithDebug(sql, fieldWhere[operations1.Count], Convert(operations[operations1.Count]), $"'{valueWhere[operations1.Count]}'", Convert(SQLOperation.ORDER_BY_RANDOM_LIMIT), limit.ToString(), ";");
			result = _sqliteConnection.stringResultFromCommand(sql);
		}
	}

	// Записать данные
	public static void OperationInsert(this SqliteConnection _sqliteConnection, string tableName, string fields, string values) {
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = "";
			sql = CombineSQLWithDebug(SQLOperation.INSERT.ToString(), SQLOperation.INTO.ToString(), tableName, $"({fields})", SQLOperation.VALUES.ToString(), $"({values})", ";");
			IDbCommand dbcmd = _sqliteConnection.CreateCommand();
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}
	}

	public static void OperationInsert(this SqliteConnection _sqliteConnection, string tableName, string fields, List<string> values) {
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = "";
			sql += CombineSQLWithoutDebug(SQLOperation.INSERT.ToString(), SQLOperation.INTO.ToString(), tableName, $"({fields})", SQLOperation.VALUES.ToString());
			for (int i = 0; i < values.Count; i++) {
				sql += CombineSQLWithoutDebug($"({values[i]})");
				if (i < (values.Count - 1))
					sql += " , ";
			}
			sql = CombineSQLWithDebug(sql, " ; ");
			IDbCommand dbcmd = _sqliteConnection.CreateCommand();
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}
	}

	public static void OperationUpdate(this SqliteConnection _sqliteConnection, string tableName, string fieldForUpdate, string value, string fieldWhere, LogicOperation logicOper, string valueWhere) {
		if (_sqliteConnection.State == ConnectionState.Open) {
			string sql = CombineSQLWithDebug(SQLOperation.UPDATE.ToString(), tableName, SQLOperation.SET.ToString(), fieldForUpdate, Convert(LogicOperation.EQUALS), value, SQLOperation.WHERE.ToString(), fieldWhere, Convert(logicOper), $"'{valueWhere}'", ";");
			IDbCommand dbcmd = _sqliteConnection.CreateCommand();
			dbcmd.CommandText = sql;
			dbcmd.ExecuteNonQuery();
		}
	}

	static string CombineSQLWithDebug(params string[] pieces) {
		string sql = "";
		for (int i = 0; i < pieces.Length; i++)
			sql += pieces[i] + " ";
		Debug.Log("SQL:");
		Debug.Log(sql);
		return sql;
	}

	static string CombineSQLWithoutDebug(params string[] pieces) {
		string sql = "";
		for (int i = 0; i < pieces.Length; i++)
			sql += pieces[i] + " ";
		return sql;
	}

	static string Convert(LogicOperation logicOperation) {
		if (logicOperation == LogicOperation.EQUALS)
			return " = ";
		else if (logicOperation == LogicOperation.NOT_EQUALS)
			return " != ";
		else if (logicOperation == LogicOperation.AND)
			return " AND ";
		else if (logicOperation == LogicOperation.OR)
			return " OR ";
		else if (logicOperation == LogicOperation.MORE_THAN)
			return " > ";
		else if (logicOperation == LogicOperation.LESS_THAN)
			return " < "; 
		else
			return "STRING FOR LOGIC OPERATION NOT SETTED";
	}

	static string Convert(SQLOperation sqlOperation) {
		if (sqlOperation == SQLOperation.CREATE_TABLE_IF_NOT_EXISTS)
			return " CREATE TABLE IF NOT EXISTS ";
		else if (sqlOperation == SQLOperation.ORDER_BY_RANDOM_LIMIT) {
			return " ORDER BY RANDOM() LIMIT ";
		}
			return "STRING FOR SQL OPERATION NOT SETTED";
	}

	static string Convert(SQLFieldParametr sqlFieldParametr) {
		if (sqlFieldParametr == SQLFieldParametr.NOT_NULL)
			return " NOT_NULL ";
		else
			return "STRING FOR SQL FIELD PARAMETR NOT SETTED";
	}

	static List<string> stringResultFromCommand(this SqliteConnection _sqliteConnection, string sql) {
		List<string> result = new List<string>();
		using (IDbCommand dbcmd = _sqliteConnection.CreateCommand()) {
            dbcmd.CommandText = sql;
			try {
				using (IDataReader reader = dbcmd.ExecuteReader())
					while (reader.Read())
						result.Add(System.Convert.ToString(reader.GetString(0)));
			} catch (SqliteException ex) {
				Debug.LogError(ex.Message);
			}
		}
		return result;
	}

	static List<List<string>> allStringResultFromCommand(this SqliteConnection _sqliteConnection, string sql) {
		List<List<string>> result = new List<List<string>>();
		using (IDbCommand dbcmd = _sqliteConnection.CreateCommand()) {
			dbcmd.CommandText = sql;
			try {
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					for (int i = 0; i < reader.FieldCount; i++)
						result.Add(new List<string>());
					while (reader.Read()) {
						for (int i = 0; i < reader.FieldCount; i++)
							if (reader.IsDBNull(i)) {
								result[i].Add("");
							} else {
								result[i].Add(System.Convert.ToString(reader.GetValue(i)));
							}
					}
				}
			} catch (SqliteException ex) {
				Debug.LogError(ex.Message);
			}

		}
		return result;
	}

	static List<string> stringResultFromCommandWithoutNULLValues(this SqliteConnection _sqliteConnection, string sql, string fieldNameNotNum)
    {
        List<string> result = new List<string>();
        using (IDbCommand dbcmd = _sqliteConnection.CreateCommand())
        {
            dbcmd.CommandText = sql;
			try {
				using (IDataReader reader = dbcmd.ExecuteReader())
					while (reader.Read())
						if (!reader.IsDBNull(reader.GetOrdinal(fieldNameNotNum)))
							result.Add(reader.GetString(reader.GetOrdinal(fieldNameNotNum)));
			} catch (SqliteException ex) {
				Debug.LogError(ex.Message);
			}
		}
        return result;
    }

    static List<int> intResultFromCommand(this SqliteConnection _sqliteConnection, string sql) {
		List<int> result = new List<int>();
		using (IDbCommand dbcmd = _sqliteConnection.CreateCommand()) {
			dbcmd.CommandText = sql;
			try {
				using (IDataReader reader = dbcmd.ExecuteReader())
					while (reader.Read())
						result.Add(reader.GetInt32(0));
			} catch (SqliteException ex) {
				Debug.LogError(ex.Message);
			}
		}
		return result;
	}

	static List<List<int>> allIntResultFromCommand(this SqliteConnection _sqliteConnection, string sql) {
		List<List<int>> result = new List<List<int>>();
		using (IDbCommand dbcmd = _sqliteConnection.CreateCommand()) {
			dbcmd.CommandText = sql;
			try {
				using (IDataReader reader = dbcmd.ExecuteReader()) {
					for (int i = 0; i < reader.FieldCount; i++)
						result.Add(new List<int>());
					while (reader.Read())
						for (int i = 0; i < reader.FieldCount; i++)
							result[i].Add(System.Convert.ToInt32(reader.GetInt32(i)));
				}
			} catch (SqliteException ex) {
				Debug.LogError(ex.Message);
			}

		}
		return result;
	}
}
#endif