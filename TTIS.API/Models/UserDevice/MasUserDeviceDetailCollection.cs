using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class MasUserDeviceDetailCollection : ICollection<MasUserDeviceDetail>
    {

        List<MasUserDeviceDetail> m_MasUserDeviceDetail = new List<MasUserDeviceDetail>();

        private readonly TTISDbContext _context;

        public MasUserDeviceDetailCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_MasUserDeviceDetail.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(MasUserDeviceDetail item)
        {
            m_MasUserDeviceDetail.Add(item);
        }

        public void Clear()
        {
            m_MasUserDeviceDetail.Clear();
        }

        public bool Contains(MasUserDeviceDetail item)
        {
            return m_MasUserDeviceDetail.Contains(item);
        }

        public void CopyTo(MasUserDeviceDetail[] array, int arrayIndex)
        {
            m_MasUserDeviceDetail.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MasUserDeviceDetail> GetEnumerator()
        {
            return m_MasUserDeviceDetail.GetEnumerator();
        }

        public bool Remove(MasUserDeviceDetail item)
        {
            return m_MasUserDeviceDetail.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_MasUserDeviceDetail.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQMasUserDeviceDetail = _context.MasUserDeviceDetail.Where(o => (o.Imei.Contains(p_sKeyword) || o.FullName.Contains(p_sKeyword) || o.NickName.Contains(p_sKeyword)) && o.IsActive);
            totalRecord = iQMasUserDeviceDetail.Count();
            m_MasUserDeviceDetail = iQMasUserDeviceDetail.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;

            var iQMasUserDeviceDetail = _context.MasUserDeviceDetail.Where(o => (o.Imei.Contains(p_sKeyword) || o.FullName.Contains(p_sKeyword) || o.NickName.Contains(p_sKeyword)) && o.IsActive);
            totalRecord = iQMasUserDeviceDetail.Count();
            m_MasUserDeviceDetail = iQMasUserDeviceDetail.ToList();

            return bIsSuccess;
        }
    }
}
