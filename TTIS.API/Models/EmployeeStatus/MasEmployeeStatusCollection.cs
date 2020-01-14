using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasEmployeeStatusCollection : ICollection<MasEmployeeStatus>
    {

        List<MasEmployeeStatus> m_MasEmployeeStatus = new List<MasEmployeeStatus>();

        private readonly TTISDbContext _context;

        public MasEmployeeStatusCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasEmployeeStatus.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasEmployeeStatus item)
        {
            m_MasEmployeeStatus.Add(item);
        }

        public void Clear()
        {
            m_MasEmployeeStatus.Clear();
        }

        public bool Contains(MasEmployeeStatus item)
        {
            return m_MasEmployeeStatus.Contains(item);
        }

        public void CopyTo(MasEmployeeStatus[] array, int arrayIndex)
        {
            m_MasEmployeeStatus.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasEmployeeStatus> GetEnumerator()
        {
            return m_MasEmployeeStatus.GetEnumerator();
        }

        public bool Remove(MasEmployeeStatus item)
        {
            return m_MasEmployeeStatus.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasEmployeeStatus.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasEmployeeStatus = _context.MasEmployeeStatus.Where(o => o.Name.Contains(p_sKeyword));
            totalRecord = iQMasEmployeeStatus.Count();
            m_MasEmployeeStatus = iQMasEmployeeStatus.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasEmployeeStatus = _context.MasEmployeeStatus.Where(o => o.Name.Contains(p_sKeyword));
            totalRecord = iQMasEmployeeStatus.Count();
            m_MasEmployeeStatus = iQMasEmployeeStatus.ToList();

            return bIsSuccess;
        }
    }
}
