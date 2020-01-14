using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class TranWoCollection : ICollection<TranWo>
    {

        List<TranWo> m_TranWo = new List<TranWo>();

        private readonly TTISDbContext _context;

        public TranWoCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_TranWo.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(TranWo item)
        {
            m_TranWo.Add(item);
        }

        public void Clear()
        {
            m_TranWo.Clear();
        }

        public bool Contains(TranWo item)
        {
            return m_TranWo.Contains(item);
        }

        public void CopyTo(TranWo[] array, int arrayIndex)
        {
            m_TranWo.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TranWo> GetEnumerator()
        {
            return m_TranWo.GetEnumerator();
        }

        public bool Remove(TranWo item)
        {
            return m_TranWo.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_TranWo.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQTranWo = _context.TranWo.Where(o => o.Won.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQTranWo.Count();
            m_TranWo = iQTranWo.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQTranWo = _context.TranWo.Where(o => o.Won.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQTranWo.Count();
            m_TranWo = iQTranWo.ToList();

            return bIsSuccess;
        }
    }
}
