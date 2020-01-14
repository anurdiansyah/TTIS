using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TTIS.WebUi.Data;

namespace TTIS.WebUi.Models
{
    public partial class SysParamCollection : ICollection<SysParam>
    {

        List<SysParam> m_SysParam = new List<SysParam>();

        private readonly TtisDbContext _context;

        public SysParamCollection(TtisDbContext context)
        {
            _context = context;
        }

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

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            List<SysParam> oSysParam = _context.SysParam.Where(o => o.Code.Contains(p_sKeyword) || o.Name.Contains(p_sKeyword)).ToList();
            foreach (SysParam SysParam in oSysParam)
            {
                this.Add(SysParam);
            }

            return bIsSuccess;
        }
    }
}
