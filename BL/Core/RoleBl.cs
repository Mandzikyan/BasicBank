using BL.Repositories.Interfaceis;
using FCBankBasicHelper.Models;
using Models.BaseType;
using Models.DTO;
using Models.Mappers;

namespace BL.Core
{
    public class RoleBl
    {
        private readonly IRoleRepository roleRepository;
        public RoleBl(IRoleRepository repository)
        {
            roleRepository = repository;
        }
        public ResponseBase<UserRolesMappingModel> AddRoleForUser(UserRolesMappingModel model)
        {
            try
            {
                UserRolesMapping mapping = Mapper<UserRolesMappingModel, UserRolesMapping>.Map(model);                
                roleRepository.Add(mapping);
                return new ResponseBase<UserRolesMappingModel>(true, "Role added successfully", model);
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserRolesMappingModel>(false, ex.Message);
            }
        }
    }
}