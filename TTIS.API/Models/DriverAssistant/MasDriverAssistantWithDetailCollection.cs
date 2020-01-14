using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasDriverAssistantWithDetailCollection : ICollection<MasDriverAssistantWithDetail>
    {

        List<MasDriverAssistantWithDetail> m_MasDriverAssistantWithDetail = new List<MasDriverAssistantWithDetail>();

        private readonly TTISDbContext _context;

        public MasDriverAssistantWithDetailCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasDriverAssistantWithDetail.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasDriverAssistantWithDetail item)
        {
            m_MasDriverAssistantWithDetail.Add(item);
        }

        public void Clear()
        {
            m_MasDriverAssistantWithDetail.Clear();
        }

        public bool Contains(MasDriverAssistantWithDetail item)
        {
            return m_MasDriverAssistantWithDetail.Contains(item);
        }

        public void CopyTo(MasDriverAssistantWithDetail[] array, int arrayIndex)
        {
            m_MasDriverAssistantWithDetail.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasDriverAssistantWithDetail> GetEnumerator()
        {
            return m_MasDriverAssistantWithDetail.GetEnumerator();
        }

        public bool Remove(MasDriverAssistantWithDetail item)
        {
            return m_MasDriverAssistantWithDetail.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasDriverAssistantWithDetail.GetEnumerator();
        }

        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasDriverWithDetail = _context.Query<MasDriverAssistantWithDetail>().FromSql("EXEC [dbo].[uspDriverAssistantListWithDetail]");
            totalRecord = iQMasDriverWithDetail.Count();
            m_MasDriverAssistantWithDetail = iQMasDriverWithDetail.Skip(p_iSkip).Take(p_iLength).ToList();

            bIsSuccess = m_MasDriverAssistantWithDetail.Count > 1;

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasDriverWithDetail = _context.Query<MasDriverAssistantWithDetail>().FromSql("EXEC [dbo].[uspDriverAssistantListWithDetail]");
            totalRecord = iQMasDriverWithDetail.Count();
            m_MasDriverAssistantWithDetail = iQMasDriverWithDetail.ToList();

            bIsSuccess = m_MasDriverAssistantWithDetail.Count > 1;

            return bIsSuccess;
        }
    }
}
