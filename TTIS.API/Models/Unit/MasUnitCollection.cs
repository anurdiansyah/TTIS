using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasUnitCollection : ICollection<MasUnit>
    {

        List<MasUnit> m_MasUnit = new List<MasUnit>();

        private readonly TTISDbContext _context;

        public MasUnitCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasUnit.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasUnit item)
        {
            m_MasUnit.Add(item);
        }

        public void Clear()
        {
            m_MasUnit.Clear();
        }

        public bool Contains(MasUnit item)
        {
            return m_MasUnit.Contains(item);
        }

        public void CopyTo(MasUnit[] array, int arrayIndex)
        {
            m_MasUnit.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasUnit> GetEnumerator()
        {
            return m_MasUnit.GetEnumerator();
        }

        public bool Remove(MasUnit item)
        {
            return m_MasUnit.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasUnit.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasUnit = _context.MasUnit.Where(o => o.Name.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasUnit.Count();
            m_MasUnit = iQMasUnit.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasUnit = _context.MasUnit.Where(o => o.Name.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasUnit.Count();
            m_MasUnit = iQMasUnit.ToList();

            return bIsSuccess;
        }
    }
}
