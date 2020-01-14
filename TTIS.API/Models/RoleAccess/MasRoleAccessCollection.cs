using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasRoleAccessCollection : ICollection<MasRoleAccess>
    {

        List<MasRoleAccess> m_MasRoleAccess = new List<MasRoleAccess>();

        private readonly TTISDbContext _context;

        public MasRoleAccessCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasRoleAccess.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasRoleAccess item)
        {
            m_MasRoleAccess.Add(item);
        }

        public void Clear()
        {
            m_MasRoleAccess.Clear();
        }

        public bool Contains(MasRoleAccess item)
        {
            return m_MasRoleAccess.Contains(item);
        }

        public void CopyTo(MasRoleAccess[] array, int arrayIndex)
        {
            m_MasRoleAccess.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasRoleAccess> GetEnumerator()
        {
            return m_MasRoleAccess.GetEnumerator();
        }

        public bool Remove(MasRoleAccess item)
        {
            return m_MasRoleAccess.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasRoleAccess.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasRoleAccess = _context.MasRoleAccess.Where(o => o.RoleName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasRoleAccess.Count();
            m_MasRoleAccess = iQMasRoleAccess.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasRoleAccess = _context.MasRoleAccess.Where(o => o.RoleName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasRoleAccess.Count();
            m_MasRoleAccess = iQMasRoleAccess.ToList();

            return bIsSuccess;
        }
    }
}
