//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp2.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskBanks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskBanks()
        {
            this.TasksIBanks = new HashSet<TasksIBanks>();
        }
    
        public int ID { get; set; }
        public string BankName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TasksIBanks> TasksIBanks { get; set; }

        public List<TaskNames> TaskList { get; set; }
    }
}
