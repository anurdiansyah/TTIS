using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasDepartmentCollection : ICollection<MasDepartment>
    {

        List<MasDepartment> m_MasDepartment = new List<MasDepartment>();

        private readonly TTISDbContext _context;

        public MasDepartmentCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasDepartment.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasDepartment item)
        {
            m_MasDepartment.Add(item);
        }

        public void Clear()
        {
            m_MasDepartment.Clear();
        }

        public bool Contains(MasDepartment item)
        {
            return m_MasDepartment.Contains(item);
        }

        public void CopyTo(MasDepartment[] array, int arrayIndex)
        {
            m_MasDepartment.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasDepartment> GetEnumerator()
        {
            return m_MasDepartment.GetEnumerator();
        }

        public bool Remove(MasDepartment item)
        {
            return m_MasDepartment.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasDepartment.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasDepartment = _context.MasDepartment.Where(o => o.Name.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasDepartment.Count();
            m_MasDepartment = iQMasDepartment.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasDepartment = _context.MasDepartment.Where(o => o.Name.Contains(p_sKeyword) && o.IsDeleted == false);
            totalRecord = iQMasDepartment.Count();
            m_MasDepartment = iQMasDepartment.ToList();

            return bIsSuccess;
        }
    }
}
