using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class SysUserLogDetailCollection : ICollection<SysUserLogDetail>
    {

        List<SysUserLogDetail> m_SysUserLogDetail = new List<SysUserLogDetail>();

        private readonly TTISDbContext _context;

        public SysUserLogDetailCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_SysUserLogDetail.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(SysUserLogDetail item)
        {
            m_SysUserLogDetail.Add(item);
        }

        public void Clear()
        {
            m_SysUserLogDetail.Clear();
        }

        public bool Contains(SysUserLogDetail item)
        {
            return m_SysUserLogDetail.Contains(item);
        }

        public void CopyTo(SysUserLogDetail[] array, int arrayIndex)
        {
            m_SysUserLogDetail.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SysUserLogDetail> GetEnumerator()
        {
            return m_SysUserLogDetail.GetEnumerator();
        }

        public bool Remove(SysUserLogDetail item)
        {
            return m_SysUserLogDetail.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_SysUserLogDetail.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQSysUserLogDetail = _context.VSysUserLogDetail.Where(o => o.UserName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQSysUserLogDetail.Count();
            m_SysUserLogDetail = iQSysUserLogDetail.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQSysUserLogDetail = _context.VSysUserLogDetail.Where(o => o.UserName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQSysUserLogDetail.Count();
            m_SysUserLogDetail = iQSysUserLogDetail.OrderBy(o=>o.UserLogId).ToList();

            return bIsSuccess;
        }
    }
}
