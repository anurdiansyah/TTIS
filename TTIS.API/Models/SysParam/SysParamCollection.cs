using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class SysParamCollection : ICollection<SysParam>
    {

        List<SysParam> m_SysParam = new List<SysParam>();

        private readonly TTISDbContext _context;

        public SysParamCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_SysParam.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(SysParam item)
        {
            m_SysParam.Add(item);
        }

        public void Clear()
        {
            m_SysParam.Clear();
        }

        public bool Contains(SysParam item)
        {
            return m_SysParam.Contains(item);
        }

        public void CopyTo(SysParam[] array, int arrayIndex)
        {
            m_SysParam.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SysParam> GetEnumerator()
        {
            return m_SysParam.GetEnumerator();
        }

        public bool Remove(SysParam item)
        {
            return m_SysParam.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_SysParam.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQSysParam = _context.SysParam.Where(o => o.Name.Contains(p_sKeyword) && o.IsVisible == true);
            totalRecord = iQSysParam.Count();
            m_SysParam = iQSysParam.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQSysParam = _context.SysParam.Where(o => o.Name.Contains(p_sKeyword) && o.IsVisible == true);
            totalRecord = iQSysParam.Count();
            m_SysParam = iQSysParam.ToList();

            return bIsSuccess;
        }
    }
}
