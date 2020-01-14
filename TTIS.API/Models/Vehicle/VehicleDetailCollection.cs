using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class VehicleDetailCollection : ICollection<VehicleDetail>
    {

        List<VehicleDetail> m_VehicleDetail = new List<VehicleDetail>();

        private readonly TTISDbContext _context;

        public VehicleDetailCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_VehicleDetail.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(VehicleDetail item)
        {
            m_VehicleDetail.Add(item);
        }

        public void Clear()
        {
            m_VehicleDetail.Clear();
        }

        public bool Contains(VehicleDetail item)
        {
            return m_VehicleDetail.Contains(item);
        }

        public void CopyTo(VehicleDetail[] array, int arrayIndex)
        {
            m_VehicleDetail.CopyTo(array, arrayIndex);
        }

        public IEnumerator<VehicleDetail> GetEnumerator()
        {
            return m_VehicleDetail.GetEnumerator();
        }

        public bool Remove(VehicleDetail item)
        {
            return m_VehicleDetail.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_VehicleDetail.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQVehicleDetail = _context.VehicleDetail.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                           || o.Plate.Contains(p_sKeyword)
                                                           || o.IsDeleted == false);
            totalRecord = iQVehicleDetail.Count();
            m_VehicleDetail = iQVehicleDetail.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQVehicleDetail = _context.VehicleDetail.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                           || o.Plate.Contains(p_sKeyword)
                                                           || o.IsDeleted == false);
            totalRecord = iQVehicleDetail.Count();
            m_VehicleDetail = iQVehicleDetail.ToList();

            return bIsSuccess;
        }
    }
}
