using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class VehicleUserDetailCollection : ICollection<VehicleUserDetail>
    {

        List<VehicleUserDetail> m_VehicleUserDetail = new List<VehicleUserDetail>();

        private readonly TTISDbContext _context;

        public VehicleUserDetailCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_VehicleUserDetail.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(VehicleUserDetail item)
        {
            m_VehicleUserDetail.Add(item);
        }

        public void Clear()
        {
            m_VehicleUserDetail.Clear();
        }

        public bool Contains(VehicleUserDetail item)
        {
            return m_VehicleUserDetail.Contains(item);
        }

        public void CopyTo(VehicleUserDetail[] array, int arrayIndex)
        {
            m_VehicleUserDetail.CopyTo(array, arrayIndex);
        }

        public IEnumerator<VehicleUserDetail> GetEnumerator()
        {
            return m_VehicleUserDetail.GetEnumerator();
        }

        public bool Remove(VehicleUserDetail item)
        {
            return m_VehicleUserDetail.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_VehicleUserDetail.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQVehicleUserDetail = _context.VehicleUserDetail.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                                         || o.Plate.Contains(p_sKeyword));
            totalRecord = iQVehicleUserDetail.Count();
            m_VehicleUserDetail = iQVehicleUserDetail.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQVehicleUserDetail = _context.VehicleUserDetail.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                                         || o.Plate.Contains(p_sKeyword));
            totalRecord = iQVehicleUserDetail.Count();
            m_VehicleUserDetail = iQVehicleUserDetail.ToList();

            return bIsSuccess;
        }
    }
}
