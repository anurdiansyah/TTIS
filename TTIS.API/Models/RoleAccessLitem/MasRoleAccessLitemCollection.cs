using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasRoleAccessLitemCollection : ICollection<MasRoleAccessLitem>
    {

        List<MasRoleAccessLitem> m_MasRoleAccessLitem = new List<MasRoleAccessLitem>();

        private readonly TTISDbContext _context;

        public MasRoleAccessLitemCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasRoleAccessLitem.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasRoleAccessLitem item)
        {
            m_MasRoleAccessLitem.Add(item);
        }

        public void Clear()
        {
            m_MasRoleAccessLitem.Clear();
        }

        public bool Contains(MasRoleAccessLitem item)
        {
            return m_MasRoleAccessLitem.Contains(item);
        }

        public void CopyTo(MasRoleAccessLitem[] array, int arrayIndex)
        {
            m_MasRoleAccessLitem.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasRoleAccessLitem> GetEnumerator()
        {
            return m_MasRoleAccessLitem.GetEnumerator();
        }

        public bool Remove(MasRoleAccessLitem item)
        {
            return m_MasRoleAccessLitem.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasRoleAccessLitem.GetEnumerator();
        }
        
        public bool List(int p_iRoleAccessId, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasRoleAccessLitem = _context.MasRoleAccessLitem.Where(o => o.RoleAccessId == p_iRoleAccessId);
            totalRecord = iQMasRoleAccessLitem.Count();
            m_MasRoleAccessLitem = iQMasRoleAccessLitem.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }
    }
}
