using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace TTIS.API.Models
{
    public partial class SysModuleCollection : ICollection<SysModule>
    {

        List<SysModule> m_SysModule = new List<SysModule>();

        private readonly TTISDbContext _context;

        public SysModuleCollection(TTISDbContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_SysModule.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(SysModule item)
        {
            m_SysModule.Add(item);
        }

        public void Clear()
        {
            m_SysModule.Clear();
        }

        public bool Contains(SysModule item)
        {
            return m_SysModule.Contains(item);
        }

        public void CopyTo(SysModule[] array, int arrayIndex)
        {
            m_SysModule.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SysModule> GetEnumerator()
        {
            return m_SysModule.GetEnumerator();
        }

        public bool Remove(SysModule item)
        {
            return m_SysModule.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_SysModule.GetEnumerator();
        }
        
        public bool List(int? p_iModuleId, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;
            IQueryable<SysModule> iQSysModule = _context.SysModule;

            if (p_iModuleId != null)
            {
                iQSysModule = _context.SysModule.Where(o => o.ModuleId == p_iModuleId && o.IsVisible == true);
            }

            foreach(SysModule oModule in iQSysModule)
            {
                oModule.SysModuleObject = _context.SysModuleObject.Where(o => o.ModuleId == oModule.ModuleId && o.IsVisible == true).ToList();
                foreach(SysModuleObject masModuleObject in oModule.SysModuleObject)
                {
                    masModuleObject.SysModuleObjectMember = _context.SysModuleObjectMember.Where(o => o.ModuleObjectId == masModuleObject.ModuleObjectId && o.IsVisible == true).ToList();
                }
            }

            totalRecord = iQSysModule.Count();
            m_SysModule = iQSysModule.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }
        public bool List(int? p_iModuleId)
        {
            bool bIsSuccess = false;
            IQueryable<SysModule> iQSysModule = _context.SysModule;

            if (p_iModuleId != null)
            {
                iQSysModule = _context.SysModule.Where(o => o.ModuleId == p_iModuleId && o.IsVisible == true);
            }

            foreach (SysModule oModule in iQSysModule)
            {
                oModule.SysModuleObject = _context.SysModuleObject.Where(o => o.ModuleId == oModule.ModuleId && o.IsVisible == true).ToList();
                foreach (SysModuleObject masModuleObject in oModule.SysModuleObject)
                {
                    masModuleObject.SysModuleObjectMember = _context.SysModuleObjectMember.Where(o => o.ModuleObjectId == masModuleObject.ModuleObjectId && o.IsVisible == true).ToList();
                }
            }

            totalRecord = iQSysModule.Count();
            m_SysModule = iQSysModule.ToList();

            return bIsSuccess;
        }
    }
}
