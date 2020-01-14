using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasUserRoleCollection : ICollection<MasUserRole>
    {

        List<MasUserRole> m_MasUserRole = new List<MasUserRole>();

        private readonly TTISDbContext _context;

        public MasUserRoleCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasUserRole.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasUserRole item)
        {
            m_MasUserRole.Add(item);
        }

        public void Clear()
        {
            m_MasUserRole.Clear();
        }

        public bool Contains(MasUserRole item)
        {
            return m_MasUserRole.Contains(item);
        }

        public void CopyTo(MasUserRole[] array, int arrayIndex)
        {
            m_MasUserRole.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasUserRole> GetEnumerator()
        {
            return m_MasUserRole.GetEnumerator();
        }

        public bool Remove(MasUserRole item)
        {
            return m_MasUserRole.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasUserRole.GetEnumerator();
        }
        
        public bool List(string p_iUserId, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasUserRole = _context.MasUserRole.Where(o => o.AspNetUserId.Contains(p_iUserId));
            totalRecord = iQMasUserRole.Count();
            m_MasUserRole = iQMasUserRole.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }
    }
}
