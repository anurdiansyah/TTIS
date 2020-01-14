using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasDriverCollection : ICollection<MasDriver>
    {

        List<MasDriver> m_MasDriver = new List<MasDriver>();

        private readonly TTISDbContext _context;

        public MasDriverCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasDriver.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasDriver item)
        {
            m_MasDriver.Add(item);
        }

        public void Clear()
        {
            m_MasDriver.Clear();
        }

        public bool Contains(MasDriver item)
        {
            return m_MasDriver.Contains(item);
        }

        public void CopyTo(MasDriver[] array, int arrayIndex)
        {
            m_MasDriver.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasDriver> GetEnumerator()
        {
            return m_MasDriver.GetEnumerator();
        }

        public bool Remove(MasDriver item)
        {
            return m_MasDriver.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasDriver.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasDriver = _context.MasDriver.Where(o => o.TagNumber.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasDriver.Count();
            m_MasDriver = iQMasDriver.Skip(p_iSkip).Take(p_iLength).ToList();

            bIsSuccess = m_MasDriver.Count > 1;

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasDriver = _context.MasDriver.Where(o => o.TagNumber.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasDriver.Count();
            m_MasDriver = iQMasDriver.ToList();

            bIsSuccess = m_MasDriver.Count > 1;

            return bIsSuccess;
        }

    }
}
