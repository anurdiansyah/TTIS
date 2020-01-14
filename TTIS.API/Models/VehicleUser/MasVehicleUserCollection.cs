using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasVehicleUserCollection : ICollection<MasVehicleUser>
    {

        List<MasVehicleUser> m_MasVehicleUser = new List<MasVehicleUser>();

        private readonly TTISDbContext _context;

        public MasVehicleUserCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasVehicleUser.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasVehicleUser item)
        {
            m_MasVehicleUser.Add(item);
        }

        public void Clear()
        {
            m_MasVehicleUser.Clear();
        }

        public bool Contains(MasVehicleUser item)
        {
            return m_MasVehicleUser.Contains(item);
        }

        public void CopyTo(MasVehicleUser[] array, int arrayIndex)
        {
            m_MasVehicleUser.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasVehicleUser> GetEnumerator()
        {
            return m_MasVehicleUser.GetEnumerator();
        }

        public bool Remove(MasVehicleUser item)
        {
            return m_MasVehicleUser.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasVehicleUser.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasVehicleUser = _context.MasVehicleUser.Where(o => o.IsDeleted == false);
            totalRecord = iQMasVehicleUser.Count();
            m_MasVehicleUser = iQMasVehicleUser.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasVehicleUser = _context.MasVehicleUser.Where(o => o.IsDeleted == false);
            totalRecord = iQMasVehicleUser.Count();
            m_MasVehicleUser = iQMasVehicleUser.ToList();

            return bIsSuccess;
        }
    }
}
