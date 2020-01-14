using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class SysApprovalDetailCollection : ICollection<SysApprovalDetail>
    {

        List<SysApprovalDetail> m_SysApprovalDetail = new List<SysApprovalDetail>();

        private readonly TTISDbContext _context;

        public SysApprovalDetailCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_SysApprovalDetail.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(SysApprovalDetail item)
        {
            m_SysApprovalDetail.Add(item);
        }

        public void Clear()
        {
            m_SysApprovalDetail.Clear();
        }

        public bool Contains(SysApprovalDetail item)
        {
            return m_SysApprovalDetail.Contains(item);
        }

        public void CopyTo(SysApprovalDetail[] array, int arrayIndex)
        {
            m_SysApprovalDetail.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SysApprovalDetail> GetEnumerator()
        {
            return m_SysApprovalDetail.GetEnumerator();
        }

        public bool Remove(SysApprovalDetail item)
        {
            return m_SysApprovalDetail.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_SysApprovalDetail.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQSysApprovalDetail = _context.SysApprovalDetail.Where(o => o.IsDeleted == false);
            iQSysApprovalDetail.OrderByDescending(o => o.CreateDate);
            totalRecord = iQSysApprovalDetail.Count();
            m_SysApprovalDetail = iQSysApprovalDetail.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQSysApprovalDetail = _context.SysApprovalDetail.Where(o => o.IsDeleted == false);
            iQSysApprovalDetail.OrderByDescending(o => o.CreateDate);
            totalRecord = iQSysApprovalDetail.Count();
            m_SysApprovalDetail = iQSysApprovalDetail.ToList();

            return bIsSuccess;
        }
    }
}
