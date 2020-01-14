using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasUserDeviceCollection : ICollection<MasUserDevice>
    {

        List<MasUserDevice> m_MasUserDevice = new List<MasUserDevice>();

        private readonly TTISDbContext _context;

        public MasUserDeviceCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasUserDevice.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasUserDevice item)
        {
            m_MasUserDevice.Add(item);
        }

        public void Clear()
        {
            m_MasUserDevice.Clear();
        }

        public bool Contains(MasUserDevice item)
        {
            return m_MasUserDevice.Contains(item);
        }

        public void CopyTo(MasUserDevice[] array, int arrayIndex)
        {
            m_MasUserDevice.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasUserDevice> GetEnumerator()
        {
            return m_MasUserDevice.GetEnumerator();
        }

        public bool Remove(MasUserDevice item)
        {
            return m_MasUserDevice.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasUserDevice.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasUserDevice = _context.MasUserDevice.Where(o => o.Imei.Contains(p_sKeyword) && o.IsActive);
            totalRecord = iQMasUserDevice.Count();
            m_MasUserDevice = iQMasUserDevice.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasUserDevice = _context.MasUserDevice.Where(o => o.Imei.Contains(p_sKeyword) && o.IsActive);
            totalRecord = iQMasUserDevice.Count();
            m_MasUserDevice = iQMasUserDevice.ToList();

            return bIsSuccess;
        }
    }
}
