using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class SysApprovalCollection : ICollection<SysApproval>
    {

        List<SysApproval> m_SysApproval = new List<SysApproval>();

        private readonly TTISDbContext _context;

        public SysApprovalCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_SysApproval.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(SysApproval item)
        {
            m_SysApproval.Add(item);
        }

        public void Clear()
        {
            m_SysApproval.Clear();
        }

        public bool Contains(SysApproval item)
        {
            return m_SysApproval.Contains(item);
        }

        public void CopyTo(SysApproval[] array, int arrayIndex)
        {
            m_SysApproval.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SysApproval> GetEnumerator()
        {
            return m_SysApproval.GetEnumerator();
        }

        public bool Remove(SysApproval item)
        {
            return m_SysApproval.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_SysApproval.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQSysApproval = _context.SysApproval.Where(o => o.IsDeleted == false);
            totalRecord = iQSysApproval.Count();
            m_SysApproval = iQSysApproval.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQSysApproval = _context.SysApproval.Where(o => o.IsDeleted == false);
            totalRecord = iQSysApproval.Count();
            m_SysApproval = iQSysApproval.ToList();

            return bIsSuccess;
        }
    }
}
