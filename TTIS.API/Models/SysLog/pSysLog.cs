using System;
using System.Collections.Generic;


namespace TTIS.API.Models
{
    public partial class SysLog
    {

        private readonly TTISDbContext _dbContext;

        public SysLog()
        {
        }

        public SysLog(TTISDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
