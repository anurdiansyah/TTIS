using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasClientCollection : ICollection<MasClient>
    {

        List<MasClient> m_MasClient = new List<MasClient>();

        private readonly TTISDbContext _context;

        public MasClientCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasClient.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasClient item)
        {
            m_MasClient.Add(item);
        }

        public void Clear()
        {
            m_MasClient.Clear();
        }

        public bool Contains(MasClient item)
        {
            return m_MasClient.Contains(item);
        }

        public void CopyTo(MasClient[] array, int arrayIndex)
        {
            m_MasClient.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasClient> GetEnumerator()
        {
            return m_MasClient.GetEnumerator();
        }

        public bool Remove(MasClient item)
        {
            return m_MasClient.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasClient.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasClient = _context.MasClient.Where(o => o.ClientName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasClient.Count();
            m_MasClient = iQMasClient.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasClient = _context.MasClient.Where(o => o.ClientName.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasClient.Count();
            m_MasClient = iQMasClient.ToList();

            return bIsSuccess;
        }
    }
}
