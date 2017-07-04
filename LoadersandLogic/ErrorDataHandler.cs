using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ErrorLoggerModel;
using CommonCode;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Configuration;

namespace LoadersandLogic
{
    public class ErrorDataHandler
    {
        private BlockingCollection<ErrorLogModel> bq;
        public ErrorDataHandler()
        {
            if(bq == null) bq = new BlockingCollection<ErrorLogModel>();
        }

        public List<ErrorLogModel> getLogsById(int appID)
        {
            using (var err = new ErrorModel())
            {
                if (err.ErrorLogs.Any(x => x.Application.appId == appID))
                {
                    return err.ErrorLogs.Where(x => x.Application.appId == appID).ToList();
                }
                return new List<ErrorLogModel>();
            }
        }

        public List<ErrorLogModel> getAllLogs()
        {
            using(var err = new ErrorModel())
            {
                return err.ErrorLogs.ToList();
            }
        }
        
        public bool AddLogs(ErrorLogModel errToSave)
        {
            using (var err = new ErrorModel())
            {
                if (err.Applications.Any(x => x.appId == errToSave.ApplicationRefId))
                {
                    err.ErrorLogs.Add(errToSave);
                    err.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool AddLogsFromREST(LogModelView log)
        {
            ErrorLogModel errToSave = new ErrorLogModel();
            errToSave.errorDescription = log.errorDescription;
            errToSave.exception = log.exception;
            errToSave.logLevel = log.logLevel;
            errToSave.errorTime = log.errorTime;
            errToSave.ApplicationRefId = log.ApplicationRefId;
            return AddLogs(errToSave);
        }

        public List<String> getErrorDataForChart(List<ErrorLogModel> pagedData)
        {
            List<String> data = new List<String>();
            using (var err = new ErrorModel())
            {
                data.Add(pagedData.Where(x => x.exception == null).ToList().Count.ToString());
                data.Add(pagedData.Where(x => x.exception != null).ToList().Count.ToString());
            }
            return data;
        }

        public List<String> getErrorDataForBarChart(List<ErrorLogModel> pagedData)
        {
            List<String> data = new List<String>();
            using (var err = new ErrorModel())
            {
                data.Add(pagedData.Where(x => x.logLevel == 1).ToList().Count.ToString());
                data.Add(pagedData.Where(x => x.logLevel == 2).ToList().Count.ToString());
                data.Add(pagedData.Where(x => x.logLevel == 3).ToList().Count.ToString());
            }
            return data;
        }

        public bool checkDBConn()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ErrorModel"].ConnectionString + "Integrated Security=SSPI;Connection Timeout=5";

            using (var l_oConnection = new SqlConnection(connStr))
            {
                try
                {
                    l_oConnection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    Logger logger = new Logger();
                    logger.LogSystemError("DB Connection Lost at " + DateTime.Now);
                    return false;
                }
            }
        }

    }
}
