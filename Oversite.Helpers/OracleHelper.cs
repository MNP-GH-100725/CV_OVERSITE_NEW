using Dapper;
using Maibro.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Text;
namespace Oversite.Helpers
{
	public class OracleHelper 
	{
		private string strConnectionString = "";

		public OracleHelper()
		{
			
			strConnectionString = new  AppConfigManager().GetConnectionString + new EncryptDecryptUtil().Decrypt(new AppConfigManager().GetDbPasword);
		}

		public enum SQLMode
		{
			Query = 1,
			StoredProcedure = 2
		}

		public int ExecuteNonQuery(string query)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			OracleCommand cmd = new OracleCommand(query, cnn);
			if ((query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete") | query.StartsWith("exec")))
				cmd.CommandType = CommandType.Text;
			else
				cmd.CommandType = CommandType.StoredProcedure;
			int retval;
			try
			{
				cnn.Open();
				retval = cmd.ExecuteNonQuery();
			}
			catch (Exception exp)
			{
				throw exp;
			}
			// ExceptionHelper.ExceptionHelper exphelper = new ExceptionHelper.ExceptionHelper();
			// exphelper.PublishInDatabase(exp);
			// exphelper.PublishInEventLog(exp);
			// exphelper.PublishInEmail(exp);
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
				cnn.Dispose();
				cmd.Dispose();
			}
			return retval;
		}
		public int GetRecordsnew(string[] query, SQLMode sqlMode, DynamicParameters[] dyParam)
		{
			int retVal = -1;
			OracleConnection objConnection = new OracleConnection();
			try
			{
				CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
				OracleParameter oracleParameter = new OracleParameter();
				using (objConnection = new OracleConnection(strConnectionString))
				{
					objConnection.Open();
					using (var transaction = objConnection.BeginTransaction())
					{
                        foreach (string cmds in query)
						{
							var affectedRows = objConnection.Execute(cmds, dyParam, transaction: transaction, commandType: commandType);
						}
						transaction.Commit();
						retVal = 1;
					}
				}
				objConnection.Close();
				objConnection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (objConnection.State == ConnectionState.Open)
					objConnection.Close();
				objConnection.Dispose();
			}
			return retVal;
		}

		public int ExecuteNonQuerynew(string[] query, SQLMode sqlMode, DynamicParameters[] dyParam)
		{
			int retVal = -1;
			OracleConnection objConnection = new OracleConnection();
			try
			{
				CommandType commandType = sqlMode == SQLMode.Query ? CommandType.Text : CommandType.StoredProcedure;
				OracleParameter oracleParameter = new OracleParameter();
				using (objConnection = new OracleConnection(strConnectionString))
				{
					objConnection.Open();
					using (var transaction = objConnection.BeginTransaction())
					{
						foreach (string cmds in query)
						{
							var affectedRows = objConnection.Execute(cmds, dyParam, transaction: transaction, commandType: commandType);
						}
						transaction.Commit();
						retVal = 1;
					}
				}
				objConnection.Close();
				objConnection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (objConnection.State == ConnectionState.Open)
					objConnection.Close();
				objConnection.Dispose();
			}
			return retVal;
		}

		public int ExecuteNonQuery(string query, OracleParameter[] parameters_value)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			OracleCommand cmd = new OracleCommand(query, cnn);
			int retVal = -1;
			try
			{
				if ((query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete")))
					cmd.CommandType = CommandType.Text;
				else
					cmd.CommandType = CommandType.StoredProcedure;
				int i;
				for (i = 0; i <= parameters_value.Length - 1; i++)
					cmd.Parameters.Add(parameters_value[i]);
				cnn.Open();
				cmd.BindByName = true;
				retVal = cmd.ExecuteNonQuery();
				cnn.Close();
				cnn.Dispose();
			}
			catch (Exception ex)
			{
                
			}

			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();


				cnn.Dispose();
			}
			return retVal;
		}
		
		public T ExecuteScalar<T>(string query)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			OracleCommand cmd = new OracleCommand(query, cnn);
			T retval = Activator.CreateInstance<T>();
			try
			{
				if ((query.StartsWith("SELECT") | query.StartsWith("select")))
					cmd.CommandType = CommandType.Text;
				else
					cmd.CommandType = CommandType.StoredProcedure;
				cnn.Open();
				retval = (T)cmd.ExecuteScalar();

			}
			catch (Exception ex)
			{
				retval = Activator.CreateInstance<T>();
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
				cmd.Dispose();
				cnn.Dispose();
			}
			return retval;
		}

		public T ExecuteScalar<T>(string query, OracleParameter[] parameters_value)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			OracleCommand cmd = new OracleCommand(query, cnn);
			T retval = Activator.CreateInstance<T>();
			try
			{
				if ((query.StartsWith("SELECT") | query.StartsWith("select")))
					cmd.CommandType = CommandType.Text;
				else
					cmd.CommandType = CommandType.StoredProcedure;
				int i;
				for (i = 0; i <= parameters_value.Length - 1; i++)
					cmd.Parameters.Add(parameters_value[i]);
				cnn.Open();
				retval = (T)cmd.ExecuteScalar();

			}
			catch (Exception ex)
			{
				retval = Activator.CreateInstance<T>();
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
		
				cnn.Dispose();
			}
			return retval;
		}


		public object ExecuteScalar(string query, OracleParameter[] parameters_value)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			object retval = new object();
			OracleCommand cmd = new OracleCommand(query, cnn);
			try
			{
				if ((query.StartsWith("SELECT") | query.StartsWith("select")))
					cmd.CommandType = CommandType.Text;
				else
					cmd.CommandType = CommandType.StoredProcedure;
				int i;
				for (i = 0; i <= parameters_value.Length - 1; i++)
					cmd.Parameters.Add(parameters_value[i]);
				cnn.Open();
				retval = cmd.ExecuteScalar();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
				cnn.Dispose();
			}
			return retval;
		}
		public OracleDataReader ExecuteReader(string query)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			OracleCommand cmd = new OracleCommand(query, cnn);
			OracleDataReader retval = null;
			try
			{
				if ((query.StartsWith("SELECT") | query.StartsWith("select")))
				{
					cmd.CommandType = CommandType.Text;
					cnn.Open();
					retval = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				}
				else
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cnn.Open();
					retval = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				}
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
				cmd.Dispose();
				cnn.Dispose();
			}
			return retval;
		}
		public OracleDataReader ExecuteReader(string query, OracleParameter[] parameters_value)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			OracleCommand cmd = new OracleCommand(query, cnn);
			OracleDataReader retval = null;
			try
			{
				if ((query.StartsWith("SELECT") | query.StartsWith("select")))
					cmd.CommandType = CommandType.Text;
				else
					cmd.CommandType = CommandType.StoredProcedure;
				int i;
				for (i = 0; i <= parameters_value.Length - 1; i++)
					cmd.Parameters.Add(parameters_value[i]);
				cnn.Open();
				retval = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();

				cnn.Dispose();
			}
			return retval;
		}
		public DataSet ExecuteDataSet(string query)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			var cmd = new OracleCommand(query, cnn);
			DataSet ds = new DataSet();
			OracleDataAdapter da = new OracleDataAdapter();
			try
			{
				if ((query.ToUpper().StartsWith("SELECT") | query.ToLower().StartsWith("select")))
					cmd.CommandType = CommandType.Text;
				else
					cmd.CommandType = CommandType.StoredProcedure;

				da.SelectCommand = cmd;

				da.Fill(ds);
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
				cmd.Dispose();
				cnn.Dispose();
				da.Dispose();
			}
			return ds;
		}
		public DataSet ExecuteDataSet(string query, OracleParameter[] parameters_value)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			var cmd = new OracleCommand(query, cnn);
			OracleDataAdapter da = new OracleDataAdapter();
			DataSet ds = new DataSet();
			try
			{
				if ((query.StartsWith("SELECT") | query.StartsWith("select")))
					cmd.CommandType = CommandType.Text;
				else
					cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandTimeout = 3600;
				int i;
				for (i = 0; i <= parameters_value.Length - 1; i++)
					cmd.Parameters.Add(parameters_value[i]);
                cmd.BindByName = true;
                da.SelectCommand = cmd;
				da.Fill(ds);
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();

				cnn.Dispose();
				da.Dispose();

			}
			return ds;
		}
		public DataSet ExecuteMDataSet(string[] query, string[] tables)
		{
			OracleConnection cnn = new OracleConnection(strConnectionString);
			DataSet ds = new DataSet();
			OracleDataAdapter da = new OracleDataAdapter();
			try
			{
				int i;
				for (i = 0; i <= query.GetUpperBound(0); i++)
				{
					var cmd = new OracleCommand(query[i], cnn);
					cmd.CommandType = CommandType.Text;
					da.SelectCommand = cmd;
					da.Fill(ds, tables[i]);
					cmd.Dispose();
				}
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if ((cnn.State == ConnectionState.Open))
					cnn.Close();
				cnn.Dispose();
				da.Dispose();
			}
			return ds;
		}
		public List<T> GetRecords<T>(string query)
{
			List<T> obj = new List<T>();

			DataSet ds = ExecuteDataSet(query);

			if (ds != null)
			{
				if (ds.Tables != null&& ds.Tables.Count>0)
				{
					if (ds.Tables[0].Rows.Count > 0)
					{

						obj = ConvertDataTable<T>(ds.Tables[0]);

					}
					else
					{
						obj = new List<T>();
					}
				}
				else
				{
					obj = new List<T>();
				}
			}
			else
			{
				obj = new List<T>();
			}

			return obj;
		}
		public List<dynamic> GetRecordsToDynamicrpt(string query)
		{
			List<dynamic> obj = new List<dynamic>();

			DataSet ds = ExecuteDataSet(query);

			if (ds != null)
			{
				if (ds.Tables != null)
				{
					if (ds.Tables[0].Rows.Count > 0)
					{

						obj = ToDynamic(ds.Tables[0]);

					}
					else
					{
						obj = new List<dynamic>();
					}
				}
				else
				{
					obj = new List<dynamic>();
				}
			}
			else
			{
				obj = new List<dynamic>();
			}

			return obj;
		}

		public List<T> GetRecords<T>(string query, OracleParameter[] parameters_value)
		{
			List<T> obj = new List<T>();

			DataSet ds = ExecuteDataSet(query, parameters_value);

			if (ds != null)
			{
				if (ds.Tables != null && ds.Tables.Count > 0)
				{
					if (ds.Tables[0].Rows.Count > 0)
					{

						obj = ConvertDataTable<T>(ds.Tables[0]);

					}
					else
					{
						obj = new List<T>();
					}
				}
				else
				{
					obj = new List<T>();
				}
			}
			else
			{
				obj = new List<T>();
			}

			return obj;
		}
		public T GetRecord<T>(string query)
		{
            try
            {
				T Obj = Activator.CreateInstance<T>();

				DataSet ds = ExecuteDataSet(query);
				if (ds != null)
				{
					if (ds.Tables != null && ds.Tables.Count > 0)
					{
						if (ds.Tables[0].Rows.Count > 0)
						{
							Obj = ConvertClass<T>(ds.Tables[0]);
						}
						else
						{
							Obj = Activator.CreateInstance<T>();
						}
					}
					else
					{
						Obj = Activator.CreateInstance<T>();
					}
				}
				else
				{
					Obj = Activator.CreateInstance<T>();
				}

				return Obj;
			}
            catch (Exception ex)
            {

                throw ex;
            }

			
			
		}

		public T GetRecord<T>(string query, OracleParameter[] parameters_value)
		{

			T Obj = Activator.CreateInstance<T>();

			DataSet ds = ExecuteDataSet(query, parameters_value);
			if (ds != null)
			{
				if (ds.Tables != null && ds.Tables.Count > 0)
				{
					if (ds.Tables[0].Rows.Count > 0)
					{
						Obj = ConvertClass<T>(ds.Tables[0]);
					}
					else
					{
						Obj = Activator.CreateInstance<T>();
					}
				}
				else
				{
					Obj = Activator.CreateInstance<T>();
				}
			}
			else
			{
				Obj = Activator.CreateInstance<T>();
			}

			return Obj;
		}
		public List<T> ConvertDataTable<T>(DataTable dt)
		{
			List<T> data = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T item = GetItem<T>(row);
				data.Add(item);
			}
			return data;
		}
		public T ConvertClass<T>(DataTable dt)
		{
			T item = default(T);
			foreach (DataRow row in dt.Rows)
			{
				item = GetItem<T>(row);

			}
			return item;
		}
		public T GetItem<T>(DataRow dr)
		{
			Type temp = typeof(T);
			T obj = Activator.CreateInstance<T>();

			foreach (DataColumn column in dr.Table.Columns)
			{
				foreach (PropertyInfo pro in temp.GetProperties())
				{
					if (pro.Name.ToLower() == column.ColumnName.ToLower())
					{
						pro.SetValue(obj, (dr[column.ColumnName] == DBNull.Value) ? null : dr[column.ColumnName], null);
					}
					else
						continue;
				}
			}
			return obj;
		}
		protected bool isValidDataset(DataSet ds)
		{
			bool retVal = false;
			try
			{
				if (ds != null)
				{
					if (ds.Tables != null)
					{
						if (ds.Tables[0].Rows.Count > 0)
						{
							retVal = true;
						}
					}
				}
			}
			catch (Exception e) { }
			return retVal;
		}
		public void dispose()
		{
			this.dispose();
		}
		public List<dynamic> GetRecordsToDynamic(string query, OracleParameter[] parm_c)
		{
			List<dynamic> obj = new List<dynamic>();

			DataSet ds = ExecuteDataSet(query, parm_c);

			if (ds != null)
			{
				if (ds.Tables != null)
				{
					if (ds.Tables[0].Rows.Count > 0)
					{

						obj = ToDynamic(ds.Tables[0]);

					}
					else
					{
						obj = new List<dynamic>();
					}
				}
				else
				{
					obj = new List<dynamic>();
				}
			}
			else
			{
				obj = new List<dynamic>();
			}

			return obj;
		}

		public List<dynamic> ToDynamic(DataTable dt)
		{
			var dynamicDt = new List<dynamic>();
			foreach (DataRow row in dt.Rows)
			{
				dynamic dyn = new ExpandoObject();
				dynamicDt.Add(dyn);
				foreach (DataColumn column in dt.Columns)
				{
					var dic = (IDictionary<string, object>)dyn;
					dic[column.ColumnName] = row[column];
				}
			}
			return dynamicDt;
		}

	}
}
