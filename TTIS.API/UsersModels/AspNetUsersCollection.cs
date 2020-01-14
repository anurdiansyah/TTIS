using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TTIS.API.UsersModels
{
    public partial class AspNetUsersCollection : ICollection<AspNetUsers>
    {

        List<AspNetUsers> m_AspNetUsers = new List<AspNetUsers>();

        private readonly IS4UsersContext _context;

        public AspNetUsersCollection(IS4UsersContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_AspNetUsers.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(AspNetUsers item)
        {
            m_AspNetUsers.Add(item);
        }

        public void Clear()
        {
            m_AspNetUsers.Clear();
        }

        public bool Contains(AspNetUsers item)
        {
            return m_AspNetUsers.Contains(item);
        }

        public void CopyTo(AspNetUsers[] array, int arrayIndex)
        {
            m_AspNetUsers.CopyTo(array, arrayIndex);
        }

        public IEnumerator<AspNetUsers> GetEnumerator()
        {
            return m_AspNetUsers.GetEnumerator();
        }

        public bool Remove(AspNetUsers item)
        {
            return m_AspNetUsers.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_AspNetUsers.GetEnumerator();
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            m_AspNetUsers = _context.AspNetUsers.Where(o => o.UserName.Contains(p_sKeyword)).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQAspNetUsers = _context.AspNetUsers.Where(o => o.UserName.Contains(p_sKeyword));
            totalRecord = iQAspNetUsers.Count();
            m_AspNetUsers = iQAspNetUsers.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }
    }
}
