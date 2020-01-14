using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasCustomerCollection : ICollection<MasCustomer>
    {

        List<MasCustomer> m_MasCustomer = new List<MasCustomer>();

        private readonly TTISDbContext _context;

        public MasCustomerCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasCustomer.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasCustomer item)
        {
            m_MasCustomer.Add(item);
        }

        public void Clear()
        {
            m_MasCustomer.Clear();
        }

        public bool Contains(MasCustomer item)
        {
            return m_MasCustomer.Contains(item);
        }

        public void CopyTo(MasCustomer[] array, int arrayIndex)
        {
            m_MasCustomer.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasCustomer> GetEnumerator()
        {
            return m_MasCustomer.GetEnumerator();
        }

        public bool Remove(MasCustomer item)
        {
            return m_MasCustomer.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasCustomer.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasCustomer = _context.MasCustomer.Where(o => (o.Name.Contains(p_sKeyword)
                                                        || o.Address.Contains(p_sKeyword)
                                                        || o.PhoneNumber.Contains(p_sKeyword)
                                                        || o.Email.Contains(p_sKeyword)
                                                        && o.IsDeleted == false));
            totalRecord = iQMasCustomer.Count();
            m_MasCustomer = iQMasCustomer.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasCustomer = _context.MasCustomer.Where(o => (o.Name.Contains(p_sKeyword)
                                                        || o.Address.Contains(p_sKeyword)
                                                        || o.PhoneNumber.Contains(p_sKeyword)
                                                        || o.Email.Contains(p_sKeyword)
                                                        && o.IsDeleted == false));
            totalRecord = iQMasCustomer.Count();
            m_MasCustomer = iQMasCustomer.ToList();

            return bIsSuccess;
        }
    }
}
