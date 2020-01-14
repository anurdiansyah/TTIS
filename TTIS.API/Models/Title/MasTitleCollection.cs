using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasTitleCollection : ICollection<MasTitle>
    {

        List<MasTitle> m_MasTitle = new List<MasTitle>();

        private readonly TTISDbContext _context;

        public MasTitleCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasTitle.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasTitle item)
        {
            m_MasTitle.Add(item);
        }

        public void Clear()
        {
            m_MasTitle.Clear();
        }

        public bool Contains(MasTitle item)
        {
            return m_MasTitle.Contains(item);
        }

        public void CopyTo(MasTitle[] array, int arrayIndex)
        {
            m_MasTitle.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasTitle> GetEnumerator()
        {
            return m_MasTitle.GetEnumerator();
        }

        public bool Remove(MasTitle item)
        {
            return m_MasTitle.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasTitle.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasTitle = _context.MasTitle.Where(o => o.Name.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasTitle.Count();
            m_MasTitle = iQMasTitle.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasTitle = _context.MasTitle.Where(o => o.Name.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasTitle.Count();
            m_MasTitle = iQMasTitle.ToList();

            return bIsSuccess;
        }
    }
}
