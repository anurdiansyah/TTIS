using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class SysUserLogCollection : ICollection<SysUserLog>
    {

        List<SysUserLog> m_SysUserLog = new List<SysUserLog>();

        private readonly TTISDbContext _context;

        public SysUserLogCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_SysUserLog.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(SysUserLog item)
        {
            m_SysUserLog.Add(item);
        }

        public void Clear()
        {
            m_SysUserLog.Clear();
        }

        public bool Contains(SysUserLog item)
        {
            return m_SysUserLog.Contains(item);
        }

        public void CopyTo(SysUserLog[] array, int arrayIndex)
        {
            m_SysUserLog.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SysUserLog> GetEnumerator()
        {
            return m_SysUserLog.GetEnumerator();
        }

        public bool Remove(SysUserLog item)
        {
            return m_SysUserLog.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_SysUserLog.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQSysUserLog = _context.SysUserLog.Where(o => o.UserName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQSysUserLog.Count();
            m_SysUserLog = iQSysUserLog.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQSysUserLog = _context.SysUserLog.Where(o => o.UserName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQSysUserLog.Count();
            m_SysUserLog = iQSysUserLog.ToList();

            return bIsSuccess;
        }
    }
}
