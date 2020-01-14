using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class CrewsCollection : ICollection<Crews>
    {

        List<Crews> m_Crews = new List<Crews>();

        private readonly TTISDbContext _context;

        public CrewsCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_Crews.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(Crews item)
        {
            m_Crews.Add(item);
        }

        public void Clear()
        {
            m_Crews.Clear();
        }

        public bool Contains(Crews item)
        {
            return m_Crews.Contains(item);
        }

        public void CopyTo(Crews[] array, int arrayIndex)
        {
            m_Crews.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Crews> GetEnumerator()
        {
            return m_Crews.GetEnumerator();
        }

        public bool Remove(Crews item)
        {
            return m_Crews.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Crews.GetEnumerator();
        }
        
        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQcrews = _context.VCrewDetail.Where(o => o.Name.Contains(p_sKeyword) 
                                                        || o.TagNo.Contains(p_sKeyword)
                                                        || o.Status.Contains(p_sKeyword));
            totalRecord = iQcrews.Count();
            m_Crews = iQcrews.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }
    }
}
