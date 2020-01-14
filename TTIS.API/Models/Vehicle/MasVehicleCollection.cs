using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasVehicleCollection : ICollection<MasVehicle>
    {

        List<MasVehicle> m_MasVehicle = new List<MasVehicle>();

        private readonly TTISDbContext _context;

        public MasVehicleCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasVehicle.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasVehicle item)
        {
            m_MasVehicle.Add(item);
        }

        public void Clear()
        {
            m_MasVehicle.Clear();
        }

        public bool Contains(MasVehicle item)
        {
            return m_MasVehicle.Contains(item);
        }

        public void CopyTo(MasVehicle[] array, int arrayIndex)
        {
            m_MasVehicle.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasVehicle> GetEnumerator()
        {
            return m_MasVehicle.GetEnumerator();
        }

        public bool Remove(MasVehicle item)
        {
            return m_MasVehicle.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasVehicle.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasVehicle = _context.MasVehicle.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                           || o.Plate.Contains(p_sKeyword)
                                                           || o.IsDeleted == false);
            totalRecord = iQMasVehicle.Count();
            m_MasVehicle = iQMasVehicle.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasVehicle = _context.MasVehicle.Where(o => o.VehicleCode.Contains(p_sKeyword)
                                                           || o.Plate.Contains(p_sKeyword)
                                                           || o.IsDeleted == false);
            totalRecord = iQMasVehicle.Count();
            m_MasVehicle = iQMasVehicle.ToList();

            return bIsSuccess;
        }
    }
}
