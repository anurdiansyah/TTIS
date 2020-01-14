using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasDriverAssistantCollection : ICollection<MasDriverAssistant>
    {

        List<MasDriverAssistant> m_MasDriverAssistant = new List<MasDriverAssistant>();

        private readonly TTISDbContext _context;

        public MasDriverAssistantCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasDriverAssistant.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasDriverAssistant item)
        {
            m_MasDriverAssistant.Add(item);
        }

        public void Clear()
        {
            m_MasDriverAssistant.Clear();
        }

        public bool Contains(MasDriverAssistant item)
        {
            return m_MasDriverAssistant.Contains(item);
        }

        public void CopyTo(MasDriverAssistant[] array, int arrayIndex)
        {
            m_MasDriverAssistant.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasDriverAssistant> GetEnumerator()
        {
            return m_MasDriverAssistant.GetEnumerator();
        }

        public bool Remove(MasDriverAssistant item)
        {
            return m_MasDriverAssistant.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasDriverAssistant.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasDriverAssistant = _context.MasDriverAssistant.Where(o => o.TagNumber.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasDriverAssistant.Count();
            m_MasDriverAssistant = iQMasDriverAssistant.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasDriverAssistant = _context.MasDriverAssistant.Where(o => o.TagNumber.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasDriverAssistant.Count();
            m_MasDriverAssistant = iQMasDriverAssistant.ToList();

            return bIsSuccess;
        }
    }
}
