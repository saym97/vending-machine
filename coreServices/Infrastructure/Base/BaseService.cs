using AutoMapper;

namespace coreServices.Infrastructure.Base
{
    public class BaseService
    {
        private IMapper _mapper;

        public BaseService(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
