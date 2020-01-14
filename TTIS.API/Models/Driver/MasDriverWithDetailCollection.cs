using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasDriverWithDetailCollection : ICollection<MasDriverWithDetail>
    {

        List<MasDriverWithDetail> m_MasDriverWithDetail = new List<MasDriverWithDetail>();

        private readonly TTISDbContext _context;

        public MasDriverWithDetailCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasDriverWithDetail.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasDriverWithDetail item)
        {
            m_MasDriverWithDetail.Add(item);
        }

        public void Clear()
        {
            m_MasDriverWithDetail.Clear();
        }

        public bool Contains(MasDriverWithDetail item)
        {
            return m_MasDriverWithDetail.Contains(item);
        }

        public void CopyTo(MasDriverWithDetail[] array, int arrayIndex)
        {
            m_MasDriverWithDetail.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasDriverWithDetail> GetEnumerator()
        {
            return m_MasDriverWithDetail.GetEnumerator();
        }

        public bool Remove(MasDriverWithDetail item)
        {
            return m_MasDriverWithDetail.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasDriverWithDetail.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasDriverWithDetail = _context.Query<MasDriverWithDetail>().FromSql("EXEC [dbo].[uspDriverListWithDetail]");
            totalRecord = iQMasDriverWithDetail.Count();
            m_MasDriverWithDetail = iQMasDriverWithDetail.Skip(p_iSkip).Take(p_iLength).ToList();

            bIsSuccess = m_MasDriverWithDetail.Count > 1;

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasDriverWithDetail = _context.Query<MasDriverWithDetail>().FromSql("EXEC [dbo].[uspDriverListWithDetail]");
            totalRecord = iQMasDriverWithDetail.Count();
            m_MasDriverWithDetail = iQMasDriverWithDetail.ToList();

            bIsSuccess = m_MasDriverWithDetail.Count > 1;

            return bIsSuccess;
        }
    }
}
