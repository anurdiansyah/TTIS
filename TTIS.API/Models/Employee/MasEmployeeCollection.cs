using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasEmployeeCollection : ICollection<MasEmployee>
    {

        List<MasEmployee> m_MasEmployee = new List<MasEmployee>();

        private readonly TTISDbContext _context;

        public MasEmployeeCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasEmployee.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasEmployee item)
        {
            m_MasEmployee.Add(item);
        }

        public void Clear()
        {
            m_MasEmployee.Clear();
        }

        public bool Contains(MasEmployee item)
        {
            return m_MasEmployee.Contains(item);
        }

        public void CopyTo(MasEmployee[] array, int arrayIndex)
        {
            m_MasEmployee.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasEmployee> GetEnumerator()
        {
            return m_MasEmployee.GetEnumerator();
        }

        public bool Remove(MasEmployee item)
        {
            return m_MasEmployee.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasEmployee.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasEmployee = _context.MasEmployee.Where(o => (o.TagNumber.Contains(p_sKeyword) 
                                                        || o.FirstName.Contains(p_sKeyword) 
                                                        || o.MiddleName.Contains(p_sKeyword)
                                                        || o.LastName.Contains(p_sKeyword)
                                                        || o.NickName.Contains(p_sKeyword))
                                                        && o.IsDeleted == false);
            totalRecord = iQMasEmployee.Count();
            m_MasEmployee = iQMasEmployee.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasEmployee = _context.MasEmployee.Where(o => (o.TagNumber.Contains(p_sKeyword)
                                                        || o.FirstName.Contains(p_sKeyword)
                                                        || o.MiddleName.Contains(p_sKeyword)
                                                        || o.LastName.Contains(p_sKeyword)
                                                        || o.NickName.Contains(p_sKeyword))
                                                        && o.IsDeleted == false);
            totalRecord = iQMasEmployee.Count();
            m_MasEmployee = iQMasEmployee.ToList();

            return bIsSuccess;
        }
    }
}
