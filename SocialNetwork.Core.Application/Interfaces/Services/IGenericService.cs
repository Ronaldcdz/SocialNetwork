using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Model>
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        Task<SaveViewModel> Add(SaveViewModel vm);

        Task Update (SaveViewModel entity, int id);

        Task Delete (int id);

        Task<List<ViewModel>> GetAllViewModel();

        Task<SaveViewModel> GetByIdSaveViewModel(int id);

    }
}
