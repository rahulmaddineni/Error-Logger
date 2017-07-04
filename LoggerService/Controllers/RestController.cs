using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CommonCode;
using ErrorLoggerModel;
using System.Collections.Concurrent;
using System.Threading;
using LoadersandLogic;
using System.Threading.Tasks;

namespace LoggerService.Controllers
{
    public class RestController : ApiController
    {
        static BlockingCollection<LogModelView> bq;

        [HttpPost]
        public void AddLog(LogModelView log)
        {
            ErrorDataHandler errhan = new ErrorDataHandler();
            bool conn = errhan.checkDBConn();
            if (conn)
            {
                if (bq == null) bq = new BlockingCollection<LogModelView>();
                while (bq.Count > 0)
                {
                    errhan.AddLogsFromREST(bq.Take());
                }
                errhan.AddLogsFromREST(log);
            }
            else
            {
                if (bq == null) bq = new BlockingCollection<LogModelView>();
                bq.Add(log);
            }
        }
    }
}
