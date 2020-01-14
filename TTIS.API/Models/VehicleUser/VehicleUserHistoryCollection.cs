using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class VehicleUserHistoryCollection : ICollection<VehicleUserHistory>
    {

        List<VehicleUserHistory> m_VehicleUserHistory = new List<VehicleUserHistory>();

        private readonly TTISDbContext _context;

        public VehicleUserHistoryCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_VehicleUserHistory.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(VehicleUserHistory item)
        {
            m_VehicleUserHistory.Add(item);
        }

        public void Clear()
        {
            m_VehicleUserHistory.Clear();
        }

        public bool Contains(VehicleUserHistory item)
        {
            return m_VehicleUserHistory.Contains(item);
        }

        public void CopyTo(VehicleUserHistory[] array, int arrayIndex)
        {
            m_VehicleUserHistory.CopyTo(array, arrayIndex);
        }

        public IEnumerator<VehicleUserHistory> GetEnumerator()
        {
            return m_VehicleUserHistory.GetEnumerator();
        }

        public bool Remove(VehicleUserHistory item)
        {
            return m_VehicleUserHistory.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_VehicleUserHistory.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQVehicleUserHistory = _context.VehicleUserHistory.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                                        || o.Plate.Contains(p_sKeyword));
            totalRecord = iQVehicleUserHistory.Count();
            m_VehicleUserHistory = iQVehicleUserHistory.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQVehicleUserHistory = _context.VehicleUserHistory.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                                         || o.Plate.Contains(p_sKeyword));
            totalRecord = iQVehicleUserHistory.Count();
            m_VehicleUserHistory = iQVehicleUserHistory.ToList();

            return bIsSuccess;
        }
    }
}
